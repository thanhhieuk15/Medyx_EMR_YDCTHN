function plugin0() {
    return document.getElementById('plugin0');
}

plugin = plugin0;
/************************************************************************/
/* jsbn.js                                                              */
/************************************************************************/

// Bits per digit
var dbits;
// JavaScript engine analysis
var canary = 0xdeadbeefcafe;
var j_lm = ((canary & 0xffffff) == 0xefcafe);

// (public) Constructor
function BigInteger(a, b, c) {
    if (a != null)
        if ("number" == typeof a) this.fromNumber(a, b, c);
        else if (b == null && "string" != typeof a) this.fromString(a, 256);
        else this.fromString(a, b);
}

// return new, unset BigInteger
function nbi() { return new BigInteger(null); }

// am: Compute w_j += (x*this_i), propagate carries,
// c is initial carry, returns final carry.
// c < 3*dvalue, x < 2*dvalue, this_i < dvalue
// We need to select the fastest one that works in this environment.

// am1: use a single mult and divide to get the high bits,
// max digit bits should be 26 because
// max internal value = 2*dvalue^2-2*dvalue (< 2^53)
function am1(i, x, w, j, c, n) {
    while (--n >= 0) {
        var v = x * this[i++] + w[j] + c;
        c = Math.floor(v / 0x4000000);
        w[j++] = v & 0x3ffffff;
    }
    return c;
}
// am2 avoids a big mult-and-extract completely.
// Max digit bits should be <= 30 because we do bitwise ops
// on values up to 2*hdvalue^2-hdvalue-1 (< 2^31)
function am2(i, x, w, j, c, n) {
    var xl = x & 0x7fff, xh = x >> 15;
    while (--n >= 0) {
        var l = this[i] & 0x7fff;
        var h = this[i++] >> 15;
        var m = xh * l + h * xl;
        l = xl * l + ((m & 0x7fff) << 15) + w[j] + (c & 0x3fffffff);
        c = (l >>> 30) + (m >>> 15) + xh * h + (c >>> 30);
        w[j++] = l & 0x3fffffff;
    }
    return c;
}
// Alternately, set max digit bits to 28 since some
// browsers slow down when dealing with 32-bit numbers.
function am3(i, x, w, j, c, n) {
    var xl = x & 0x3fff, xh = x >> 14;
    while (--n >= 0) {
        var l = this[i] & 0x3fff;
        var h = this[i++] >> 14;
        var m = xh * l + h * xl;
        l = xl * l + ((m & 0x3fff) << 14) + w[j] + c;
        c = (l >> 28) + (m >> 14) + xh * h;
        w[j++] = l & 0xfffffff;
    }
    return c;
}
if (j_lm && (navigator.appName == "Microsoft Internet Explorer")) {
    BigInteger.prototype.am = am2;
    dbits = 30;
}
else if (j_lm && (navigator.appName != "Netscape")) {
    BigInteger.prototype.am = am1;
    dbits = 26;
}
else { // Mozilla/Netscape seems to prefer am3
    BigInteger.prototype.am = am3;
    dbits = 28;
}

BigInteger.prototype.DB = dbits;
BigInteger.prototype.DM = ((1 << dbits) - 1);
BigInteger.prototype.DV = (1 << dbits);

var BI_FP = 52;
BigInteger.prototype.FV = Math.pow(2, BI_FP);
BigInteger.prototype.F1 = BI_FP - dbits;
BigInteger.prototype.F2 = 2 * dbits - BI_FP;

// Digit conversions
var BI_RM = "0123456789abcdefghijklmnopqrstuvwxyz";
var BI_RC = new Array();
var rr, vv;
rr = "0".charCodeAt(0);
for (vv = 0; vv <= 9; ++vv) BI_RC[rr++] = vv;
rr = "a".charCodeAt(0);
for (vv = 10; vv < 36; ++vv) BI_RC[rr++] = vv;
rr = "A".charCodeAt(0);
for (vv = 10; vv < 36; ++vv) BI_RC[rr++] = vv;

function int2char(n) { return BI_RM.charAt(n); }
function intAt(s, i) {
    var c = BI_RC[s.charCodeAt(i)];
    return (c == null) ? -1 : c;
}

// (protected) copy this to r
function bnpCopyTo(r) {
    for (var i = this.t - 1; i >= 0; --i) r[i] = this[i];
    r.t = this.t;
    r.s = this.s;
}

// (protected) set from integer value x, -DV <= x < DV
function bnpFromInt(x) {
    this.t = 1;
    this.s = (x < 0) ? -1 : 0;
    if (x > 0) this[0] = x;
    else if (x < -1) this[0] = x + DV;
    else this.t = 0;
}

// return bigint initialized to value
function nbv(i) { var r = nbi(); r.fromInt(i); return r; }

// (protected) set from string and radix
function bnpFromString(s, b) {
    var k;
    if (b == 16) k = 4;
    else if (b == 8) k = 3;
    else if (b == 256) k = 8; // byte array
    else if (b == 2) k = 1;
    else if (b == 32) k = 5;
    else if (b == 4) k = 2;
    else { this.fromRadix(s, b); return; }
    this.t = 0;
    this.s = 0;
    var i = s.length, mi = false, sh = 0;
    while (--i >= 0) {
        var x = (k == 8) ? s[i] & 0xff : intAt(s, i);
        if (x < 0) {
            if (s.charAt(i) == "-") mi = true;
            continue;
        }
        mi = false;
        if (sh == 0)
            this[this.t++] = x;
        else if (sh + k > this.DB) {
            this[this.t - 1] |= (x & ((1 << (this.DB - sh)) - 1)) << sh;
            this[this.t++] = (x >> (this.DB - sh));
        }
        else
            this[this.t - 1] |= x << sh;
        sh += k;
        if (sh >= this.DB) sh -= this.DB;
    }
    if (k == 8 && (s[0] & 0x80) != 0) {
        this.s = -1;
        if (sh > 0) this[this.t - 1] |= ((1 << (this.DB - sh)) - 1) << sh;
    }
    this.clamp();
    if (mi) BigInteger.ZERO.subTo(this, this);
}

// (protected) clamp off excess high words
function bnpClamp() {
    var c = this.s & this.DM;
    while (this.t > 0 && this[this.t - 1] == c)--this.t;
}

// (public) return string representation in given radix
function bnToString(b) {
    if (this.s < 0) return "-" + this.negate().toString(b);
    var k;
    if (b == 16) k = 4;
    else if (b == 8) k = 3;
    else if (b == 2) k = 1;
    else if (b == 32) k = 5;
    else if (b == 4) k = 2;
    else return this.toRadix(b);
    var km = (1 << k) - 1, d, m = false, r = "", i = this.t;
    var p = this.DB - (i * this.DB) % k;
    if (i-- > 0) {
        if (p < this.DB && (d = this[i] >> p) > 0) { m = true; r = int2char(d); }
        while (i >= 0) {
            if (p < k) {
                d = (this[i] & ((1 << p) - 1)) << (k - p);
                d |= this[--i] >> (p += this.DB - k);
            }
            else {
                d = (this[i] >> (p -= k)) & km;
                if (p <= 0) { p += this.DB; --i; }
            }
            if (d > 0) m = true;
            if (m) r += int2char(d);
        }
    }
    return m ? r : "0";
}

// (public) -this
function bnNegate() { var r = nbi(); BigInteger.ZERO.subTo(this, r); return r; }

// (public) |this|
function bnAbs() { return (this.s < 0) ? this.negate() : this; }

// (public) return + if this > a, - if this < a, 0 if equal
function bnCompareTo(a) {
    var r = this.s - a.s;
    if (r != 0) return r;
    var i = this.t;
    r = i - a.t;
    if (r != 0) return r;
    while (--i >= 0) if ((r = this[i] - a[i]) != 0) return r;
    return 0;
}

// returns bit length of the integer x
function nbits(x) {
    var r = 1, t;
    if ((t = x >>> 16) != 0) { x = t; r += 16; }
    if ((t = x >> 8) != 0) { x = t; r += 8; }
    if ((t = x >> 4) != 0) { x = t; r += 4; }
    if ((t = x >> 2) != 0) { x = t; r += 2; }
    if ((t = x >> 1) != 0) { x = t; r += 1; }
    return r;
}

// (public) return the number of bits in "this"
function bnBitLength() {
    if (this.t <= 0) return 0;
    return this.DB * (this.t - 1) + nbits(this[this.t - 1] ^ (this.s & this.DM));
}

// (protected) r = this << n*DB
function bnpDLShiftTo(n, r) {
    var i;
    for (i = this.t - 1; i >= 0; --i) r[i + n] = this[i];
    for (i = n - 1; i >= 0; --i) r[i] = 0;
    r.t = this.t + n;
    r.s = this.s;
}

// (protected) r = this >> n*DB
function bnpDRShiftTo(n, r) {
    for (var i = n; i < this.t; ++i) r[i - n] = this[i];
    r.t = Math.max(this.t - n, 0);
    r.s = this.s;
}

// (protected) r = this << n
function bnpLShiftTo(n, r) {
    var bs = n % this.DB;
    var cbs = this.DB - bs;
    var bm = (1 << cbs) - 1;
    var ds = Math.floor(n / this.DB), c = (this.s << bs) & this.DM, i;
    for (i = this.t - 1; i >= 0; --i) {
        r[i + ds + 1] = (this[i] >> cbs) | c;
        c = (this[i] & bm) << bs;
    }
    for (i = ds - 1; i >= 0; --i) r[i] = 0;
    r[ds] = c;
    r.t = this.t + ds + 1;
    r.s = this.s;
    r.clamp();
}

// (protected) r = this >> n
function bnpRShiftTo(n, r) {
    r.s = this.s;
    var ds = Math.floor(n / this.DB);
    if (ds >= this.t) { r.t = 0; return; }
    var bs = n % this.DB;
    var cbs = this.DB - bs;
    var bm = (1 << bs) - 1;
    r[0] = this[ds] >> bs;
    for (var i = ds + 1; i < this.t; ++i) {
        r[i - ds - 1] |= (this[i] & bm) << cbs;
        r[i - ds] = this[i] >> bs;
    }
    if (bs > 0) r[this.t - ds - 1] |= (this.s & bm) << cbs;
    r.t = this.t - ds;
    r.clamp();
}

// (protected) r = this - a
function bnpSubTo(a, r) {
    var i = 0, c = 0, m = Math.min(a.t, this.t);
    while (i < m) {
        c += this[i] - a[i];
        r[i++] = c & this.DM;
        c >>= this.DB;
    }
    if (a.t < this.t) {
        c -= a.s;
        while (i < this.t) {
            c += this[i];
            r[i++] = c & this.DM;
            c >>= this.DB;
        }
        c += this.s;
    }
    else {
        c += this.s;
        while (i < a.t) {
            c -= a[i];
            r[i++] = c & this.DM;
            c >>= this.DB;
        }
        c -= a.s;
    }
    r.s = (c < 0) ? -1 : 0;
    if (c < -1) r[i++] = this.DV + c;
    else if (c > 0) r[i++] = c;
    r.t = i;
    r.clamp();
}

// (protected) r = this * a, r != this,a (HAC 14.12)
// "this" should be the larger one if appropriate.
function bnpMultiplyTo(a, r) {
    var x = this.abs(), y = a.abs();
    var i = x.t;
    r.t = i + y.t;
    while (--i >= 0) r[i] = 0;
    for (i = 0; i < y.t; ++i) r[i + x.t] = x.am(0, y[i], r, i, 0, x.t);
    r.s = 0;
    r.clamp();
    if (this.s != a.s) BigInteger.ZERO.subTo(r, r);
}

// (protected) r = this^2, r != this (HAC 14.16)
function bnpSquareTo(r) {
    var x = this.abs();
    var i = r.t = 2 * x.t;
    while (--i >= 0) r[i] = 0;
    for (i = 0; i < x.t - 1; ++i) {
        var c = x.am(i, x[i], r, 2 * i, 0, 1);
        if ((r[i + x.t] += x.am(i + 1, 2 * x[i], r, 2 * i + 1, c, x.t - i - 1)) >= x.DV) {
            r[i + x.t] -= x.DV;
            r[i + x.t + 1] = 1;
        }
    }
    if (r.t > 0) r[r.t - 1] += x.am(i, x[i], r, 2 * i, 0, 1);
    r.s = 0;
    r.clamp();
}

// (protected) divide this by m, quotient and remainder to q, r (HAC 14.20)
// r != q, this != m.  q or r may be null.
function bnpDivRemTo(m, q, r) {
    var pm = m.abs();
    if (pm.t <= 0) return;
    var pt = this.abs();
    if (pt.t < pm.t) {
        if (q != null) q.fromInt(0);
        if (r != null) this.copyTo(r);
        return;
    }
    if (r == null) r = nbi();
    var y = nbi(), ts = this.s, ms = m.s;
    var nsh = this.DB - nbits(pm[pm.t - 1]); // normalize modulus
    if (nsh > 0) { pm.lShiftTo(nsh, y); pt.lShiftTo(nsh, r); }
    else { pm.copyTo(y); pt.copyTo(r); }
    var ys = y.t;
    var y0 = y[ys - 1];
    if (y0 == 0) return;
    var yt = y0 * (1 << this.F1) + ((ys > 1) ? y[ys - 2] >> this.F2 : 0);
    var d1 = this.FV / yt, d2 = (1 << this.F1) / yt, e = 1 << this.F2;
    var i = r.t, j = i - ys, t = (q == null) ? nbi() : q;
    y.dlShiftTo(j, t);
    if (r.compareTo(t) >= 0) {
        r[r.t++] = 1;
        r.subTo(t, r);
    }
    BigInteger.ONE.dlShiftTo(ys, t);
    t.subTo(y, y); // "negative" y so we can replace sub with am later
    while (y.t < ys) y[y.t++] = 0;
    while (--j >= 0) {
        // Estimate quotient digit
        var qd = (r[--i] == y0) ? this.DM : Math.floor(r[i] * d1 + (r[i - 1] + e) * d2);
        if ((r[i] += y.am(0, qd, r, j, 0, ys)) < qd) {  // Try it out
            y.dlShiftTo(j, t);
            r.subTo(t, r);
            while (r[i] < --qd) r.subTo(t, r);
        }
    }
    if (q != null) {
        r.drShiftTo(ys, q);
        if (ts != ms) BigInteger.ZERO.subTo(q, q);
    }
    r.t = ys;
    r.clamp();
    if (nsh > 0) r.rShiftTo(nsh, r); // Denormalize remainder
    if (ts < 0) BigInteger.ZERO.subTo(r, r);
}

// (public) this mod a
function bnMod(a) {
    var r = nbi();
    this.abs().divRemTo(a, null, r);
    if (this.s < 0 && r.compareTo(BigInteger.ZERO) > 0) a.subTo(r, r);
    return r;
}

// Modular reduction using "classic" algorithm
function Classic(m) { this.m = m; }
function cConvert(x) {
    if (x.s < 0 || x.compareTo(this.m) >= 0) return x.mod(this.m);
    else return x;
}
function cRevert(x) { return x; }
function cReduce(x) { x.divRemTo(this.m, null, x); }
function cMulTo(x, y, r) { x.multiplyTo(y, r); this.reduce(r); }
function cSqrTo(x, r) { x.squareTo(r); this.reduce(r); }

Classic.prototype.convert = cConvert;
Classic.prototype.revert = cRevert;
Classic.prototype.reduce = cReduce;
Classic.prototype.mulTo = cMulTo;
Classic.prototype.sqrTo = cSqrTo;

// (protected) return "-1/this % 2^DB"; useful for Mont. reduction
// justification:
//         xy == 1 (mod m)
//         xy =  1+km
//   xy(2-xy) = (1+km)(1-km)
// x[y(2-xy)] = 1-k^2m^2
// x[y(2-xy)] == 1 (mod m^2)
// if y is 1/x mod m, then y(2-xy) is 1/x mod m^2
// should reduce x and y(2-xy) by m^2 at each step to keep size bounded.
// JS multiply "overflows" differently from C/C++, so care is needed here.
function bnpInvDigit() {
    if (this.t < 1) return 0;
    var x = this[0];
    if ((x & 1) == 0) return 0;
    var y = x & 3;  // y == 1/x mod 2^2
    y = (y * (2 - (x & 0xf) * y)) & 0xf; // y == 1/x mod 2^4
    y = (y * (2 - (x & 0xff) * y)) & 0xff; // y == 1/x mod 2^8
    y = (y * (2 - (((x & 0xffff) * y) & 0xffff))) & 0xffff; // y == 1/x mod 2^16
    // last step - calculate inverse mod DV directly;
    // assumes 16 < DB <= 32 and assumes ability to handle 48-bit ints
    y = (y * (2 - x * y % this.DV)) % this.DV;  // y == 1/x mod 2^dbits
    // we really want the negative inverse, and -DV < y < DV
    return (y > 0) ? this.DV - y : -y;
}

// Montgomery reduction
function Montgomery(m) {
    this.m = m;
    this.mp = m.invDigit();
    this.mpl = this.mp & 0x7fff;
    this.mph = this.mp >> 15;
    this.um = (1 << (m.DB - 15)) - 1;
    this.mt2 = 2 * m.t;
}

// xR mod m
function montConvert(x) {
    var r = nbi();
    x.abs().dlShiftTo(this.m.t, r);
    r.divRemTo(this.m, null, r);
    if (x.s < 0 && r.compareTo(BigInteger.ZERO) > 0) this.m.subTo(r, r);
    return r;
}

// x/R mod m
function montRevert(x) {
    var r = nbi();
    x.copyTo(r);
    this.reduce(r);
    return r;
}

// x = x/R mod m (HAC 14.32)
function montReduce(x) {
    while (x.t <= this.mt2) // pad x so am has enough room later
        x[x.t++] = 0;
    for (var i = 0; i < this.m.t; ++i) {
        // faster way of calculating u0 = x[i]*mp mod DV
        var j = x[i] & 0x7fff;
        var u0 = (j * this.mpl + (((j * this.mph + (x[i] >> 15) * this.mpl) & this.um) << 15)) & x.DM;
        // use am to combine the multiply-shift-add into one call
        j = i + this.m.t;
        x[j] += this.m.am(0, u0, x, i, 0, this.m.t);
        // propagate carry
        while (x[j] >= x.DV) { x[j] -= x.DV; x[++j]++; }
    }
    x.clamp();
    x.drShiftTo(this.m.t, x);
    if (x.compareTo(this.m) >= 0) x.subTo(this.m, x);
}

// r = "x^2/R mod m"; x != r
function montSqrTo(x, r) { x.squareTo(r); this.reduce(r); }

// r = "xy/R mod m"; x,y != r
function montMulTo(x, y, r) { x.multiplyTo(y, r); this.reduce(r); }

Montgomery.prototype.convert = montConvert;
Montgomery.prototype.revert = montRevert;
Montgomery.prototype.reduce = montReduce;
Montgomery.prototype.mulTo = montMulTo;
Montgomery.prototype.sqrTo = montSqrTo;

// (protected) true iff this is even
function bnpIsEven() { return ((this.t > 0) ? (this[0] & 1) : this.s) == 0; }

// (protected) this^e, e < 2^32, doing sqr and mul with "r" (HAC 14.79)
function bnpExp(e, z) {
    if (e > 0xffffffff || e < 1) return BigInteger.ONE;
    var r = nbi(), r2 = nbi(), g = z.convert(this), i = nbits(e) - 1;
    g.copyTo(r);
    while (--i >= 0) {
        z.sqrTo(r, r2);
        if ((e & (1 << i)) > 0) z.mulTo(r2, g, r);
        else { var t = r; r = r2; r2 = t; }
    }
    return z.revert(r);
}

// (public) this^e % m, 0 <= e < 2^32
function bnModPowInt(e, m) {
    var z;
    if (e < 256 || m.isEven()) z = new Classic(m); else z = new Montgomery(m);
    return this.exp(e, z);
}

// protected
BigInteger.prototype.copyTo = bnpCopyTo;
BigInteger.prototype.fromInt = bnpFromInt;
BigInteger.prototype.fromString = bnpFromString;
BigInteger.prototype.clamp = bnpClamp;
BigInteger.prototype.dlShiftTo = bnpDLShiftTo;
BigInteger.prototype.drShiftTo = bnpDRShiftTo;
BigInteger.prototype.lShiftTo = bnpLShiftTo;
BigInteger.prototype.rShiftTo = bnpRShiftTo;
BigInteger.prototype.subTo = bnpSubTo;
BigInteger.prototype.multiplyTo = bnpMultiplyTo;
BigInteger.prototype.squareTo = bnpSquareTo;
BigInteger.prototype.divRemTo = bnpDivRemTo;
BigInteger.prototype.invDigit = bnpInvDigit;
BigInteger.prototype.isEven = bnpIsEven;
BigInteger.prototype.exp = bnpExp;

// public
BigInteger.prototype.toString = bnToString;
BigInteger.prototype.negate = bnNegate;
BigInteger.prototype.abs = bnAbs;
BigInteger.prototype.compareTo = bnCompareTo;
BigInteger.prototype.bitLength = bnBitLength;
BigInteger.prototype.mod = bnMod;
BigInteger.prototype.modPowInt = bnModPowInt;

// "constants"
BigInteger.ZERO = nbv(0);
BigInteger.ONE = nbv(1);

/************************************************************************/
/* JSBN2                                                                */
/************************************************************************/
// Copyright (c) 2005-2009  Tom Wu
// All Rights Reserved.
// See "LICENSE" for details.

// Extended JavaScript BN functions, required for RSA private ops.

// Version 1.1: new BigInteger("0", 10) returns "proper" zero

// (public)
function bnClone() { var r = nbi(); this.copyTo(r); return r; }

// (public) return value as integer
function bnIntValue() {
    if (this.s < 0) {
        if (this.t == 1) return this[0] - this.DV;
        else if (this.t == 0) return -1;
    }
    else if (this.t == 1) return this[0];
    else if (this.t == 0) return 0;
    // assumes 16 < DB < 32
    return ((this[1] & ((1 << (32 - this.DB)) - 1)) << this.DB) | this[0];
}

// (public) return value as byte
function bnByteValue() { return (this.t == 0) ? this.s : (this[0] << 24) >> 24; }

// (public) return value as short (assumes DB>=16)
function bnShortValue() { return (this.t == 0) ? this.s : (this[0] << 16) >> 16; }

// (protected) return x s.t. r^x < DV
function bnpChunkSize(r) { return Math.floor(Math.LN2 * this.DB / Math.log(r)); }

// (public) 0 if this == 0, 1 if this > 0
function bnSigNum() {
    if (this.s < 0) return -1;
    else if (this.t <= 0 || (this.t == 1 && this[0] <= 0)) return 0;
    else return 1;
}

// (protected) convert to radix string
function bnpToRadix(b) {
    if (b == null) b = 10;
    if (this.signum() == 0 || b < 2 || b > 36) return "0";
    var cs = this.chunkSize(b);
    var a = Math.pow(b, cs);
    var d = nbv(a), y = nbi(), z = nbi(), r = "";
    this.divRemTo(d, y, z);
    while (y.signum() > 0) {
        r = (a + z.intValue()).toString(b).substr(1) + r;
        y.divRemTo(d, y, z);
    }
    return z.intValue().toString(b) + r;
}

// (protected) convert from radix string
function bnpFromRadix(s, b) {
    this.fromInt(0);
    if (b == null) b = 10;
    var cs = this.chunkSize(b);
    var d = Math.pow(b, cs), mi = false, j = 0, w = 0;
    for (var i = 0; i < s.length; ++i) {
        var x = intAt(s, i);
        if (x < 0) {
            if (s.charAt(i) == "-" && this.signum() == 0) mi = true;
            continue;
        }
        w = b * w + x;
        if (++j >= cs) {
            this.dMultiply(d);
            this.dAddOffset(w, 0);
            j = 0;
            w = 0;
        }
    }
    if (j > 0) {
        this.dMultiply(Math.pow(b, j));
        this.dAddOffset(w, 0);
    }
    if (mi) BigInteger.ZERO.subTo(this, this);
}

// (protected) alternate constructor
function bnpFromNumber(a, b, c) {
    if ("number" == typeof b) {
        // new BigInteger(int,int,RNG)
        if (a < 2) this.fromInt(1);
        else {
            this.fromNumber(a, c);
            if (!this.testBit(a - 1))   // force MSB set
                this.bitwiseTo(BigInteger.ONE.shiftLeft(a - 1), op_or, this);
            if (this.isEven()) this.dAddOffset(1, 0); // force odd
            while (!this.isProbablePrime(b)) {
                this.dAddOffset(2, 0);
                if (this.bitLength() > a) this.subTo(BigInteger.ONE.shiftLeft(a - 1), this);
            }
        }
    }
    else {
        // new BigInteger(int,RNG)
        var x = new Array(), t = a & 7;
        x.length = (a >> 3) + 1;
        b.nextBytes(x);
        if (t > 0) x[0] &= ((1 << t) - 1); else x[0] = 0;
        this.fromString(x, 256);
    }
}

// (public) convert to bigendian byte array
function bnToByteArray() {
    var i = this.t, r = new Array();
    r[0] = this.s;
    var p = this.DB - (i * this.DB) % 8, d, k = 0;
    if (i-- > 0) {
        if (p < this.DB && (d = this[i] >> p) != (this.s & this.DM) >> p)
            r[k++] = d | (this.s << (this.DB - p));
        while (i >= 0) {
            if (p < 8) {
                d = (this[i] & ((1 << p) - 1)) << (8 - p);
                d |= this[--i] >> (p += this.DB - 8);
            }
            else {
                d = (this[i] >> (p -= 8)) & 0xff;
                if (p <= 0) { p += this.DB; --i; }
            }
            if ((d & 0x80) != 0) d |= -256;
            if (k == 0 && (this.s & 0x80) != (d & 0x80))++k;
            if (k > 0 || d != this.s) r[k++] = d;
        }
    }
    return r;
}

function bnEquals(a) { return (this.compareTo(a) == 0); }
function bnMin(a) { return (this.compareTo(a) < 0) ? this : a; }
function bnMax(a) { return (this.compareTo(a) > 0) ? this : a; }

// (protected) r = this op a (bitwise)
function bnpBitwiseTo(a, op, r) {
    var i, f, m = Math.min(a.t, this.t);
    for (i = 0; i < m; ++i) r[i] = op(this[i], a[i]);
    if (a.t < this.t) {
        f = a.s & this.DM;
        for (i = m; i < this.t; ++i) r[i] = op(this[i], f);
        r.t = this.t;
    }
    else {
        f = this.s & this.DM;
        for (i = m; i < a.t; ++i) r[i] = op(f, a[i]);
        r.t = a.t;
    }
    r.s = op(this.s, a.s);
    r.clamp();
}

// (public) this & a
function op_and(x, y) { return x & y; }
function bnAnd(a) { var r = nbi(); this.bitwiseTo(a, op_and, r); return r; }

// (public) this | a
function op_or(x, y) { return x | y; }
function bnOr(a) { var r = nbi(); this.bitwiseTo(a, op_or, r); return r; }

// (public) this ^ a
function op_xor(x, y) { return x ^ y; }
function bnXor(a) { var r = nbi(); this.bitwiseTo(a, op_xor, r); return r; }

// (public) this & ~a
function op_andnot(x, y) { return x & ~y; }
function bnAndNot(a) { var r = nbi(); this.bitwiseTo(a, op_andnot, r); return r; }

// (public) ~this
function bnNot() {
    var r = nbi();
    for (var i = 0; i < this.t; ++i) r[i] = this.DM & ~this[i];
    r.t = this.t;
    r.s = ~this.s;
    return r;
}

// (public) this << n
function bnShiftLeft(n) {
    var r = nbi();
    if (n < 0) this.rShiftTo(-n, r); else this.lShiftTo(n, r);
    return r;
}

// (public) this >> n
function bnShiftRight(n) {
    var r = nbi();
    if (n < 0) this.lShiftTo(-n, r); else this.rShiftTo(n, r);
    return r;
}

// return index of lowest 1-bit in x, x < 2^31
function lbit(x) {
    if (x == 0) return -1;
    var r = 0;
    if ((x & 0xffff) == 0) { x >>= 16; r += 16; }
    if ((x & 0xff) == 0) { x >>= 8; r += 8; }
    if ((x & 0xf) == 0) { x >>= 4; r += 4; }
    if ((x & 3) == 0) { x >>= 2; r += 2; }
    if ((x & 1) == 0)++r;
    return r;
}

// (public) returns index of lowest 1-bit (or -1 if none)
function bnGetLowestSetBit() {
    for (var i = 0; i < this.t; ++i)
        if (this[i] != 0) return i * this.DB + lbit(this[i]);
    if (this.s < 0) return this.t * this.DB;
    return -1;
}

// return number of 1 bits in x
function cbit(x) {
    var r = 0;
    while (x != 0) { x &= x - 1; ++r; }
    return r;
}

// (public) return number of set bits
function bnBitCount() {
    var r = 0, x = this.s & this.DM;
    for (var i = 0; i < this.t; ++i) r += cbit(this[i] ^ x);
    return r;
}

// (public) true iff nth bit is set
function bnTestBit(n) {
    var j = Math.floor(n / this.DB);
    if (j >= this.t) return (this.s != 0);
    return ((this[j] & (1 << (n % this.DB))) != 0);
}

// (protected) this op (1<<n)
function bnpChangeBit(n, op) {
    var r = BigInteger.ONE.shiftLeft(n);
    this.bitwiseTo(r, op, r);
    return r;
}

// (public) this | (1<<n)
function bnSetBit(n) { return this.changeBit(n, op_or); }

// (public) this & ~(1<<n)
function bnClearBit(n) { return this.changeBit(n, op_andnot); }

// (public) this ^ (1<<n)
function bnFlipBit(n) { return this.changeBit(n, op_xor); }

// (protected) r = this + a
function bnpAddTo(a, r) {
    var i = 0, c = 0, m = Math.min(a.t, this.t);
    while (i < m) {
        c += this[i] + a[i];
        r[i++] = c & this.DM;
        c >>= this.DB;
    }
    if (a.t < this.t) {
        c += a.s;
        while (i < this.t) {
            c += this[i];
            r[i++] = c & this.DM;
            c >>= this.DB;
        }
        c += this.s;
    }
    else {
        c += this.s;
        while (i < a.t) {
            c += a[i];
            r[i++] = c & this.DM;
            c >>= this.DB;
        }
        c += a.s;
    }
    r.s = (c < 0) ? -1 : 0;
    if (c > 0) r[i++] = c;
    else if (c < -1) r[i++] = this.DV + c;
    r.t = i;
    r.clamp();
}

// (public) this + a
function bnAdd(a) { var r = nbi(); this.addTo(a, r); return r; }

// (public) this - a
function bnSubtract(a) { var r = nbi(); this.subTo(a, r); return r; }

// (public) this * a
function bnMultiply(a) { var r = nbi(); this.multiplyTo(a, r); return r; }

// (public) this / a
function bnDivide(a) { var r = nbi(); this.divRemTo(a, r, null); return r; }

// (public) this % a
function bnRemainder(a) { var r = nbi(); this.divRemTo(a, null, r); return r; }

// (public) [this/a,this%a]
function bnDivideAndRemainder(a) {
    var q = nbi(), r = nbi();
    this.divRemTo(a, q, r);
    return new Array(q, r);
}

// (protected) this *= n, this >= 0, 1 < n < DV
function bnpDMultiply(n) {
    this[this.t] = this.am(0, n - 1, this, 0, 0, this.t);
    ++this.t;
    this.clamp();
}

// (protected) this += n << w words, this >= 0
function bnpDAddOffset(n, w) {
    if (n == 0) return;
    while (this.t <= w) this[this.t++] = 0;
    this[w] += n;
    while (this[w] >= this.DV) {
        this[w] -= this.DV;
        if (++w >= this.t) this[this.t++] = 0;
        ++this[w];
    }
}

// A "null" reducer
function NullExp() { }
function nNop(x) { return x; }
function nMulTo(x, y, r) { x.multiplyTo(y, r); }
function nSqrTo(x, r) { x.squareTo(r); }

NullExp.prototype.convert = nNop;
NullExp.prototype.revert = nNop;
NullExp.prototype.mulTo = nMulTo;
NullExp.prototype.sqrTo = nSqrTo;

// (public) this^e
function bnPow(e) { return this.exp(e, new NullExp()); }

// (protected) r = lower n words of "this * a", a.t <= n
// "this" should be the larger one if appropriate.
function bnpMultiplyLowerTo(a, n, r) {
    var i = Math.min(this.t + a.t, n);
    r.s = 0; // assumes a,this >= 0
    r.t = i;
    while (i > 0) r[--i] = 0;
    var j;
    for (j = r.t - this.t; i < j; ++i) r[i + this.t] = this.am(0, a[i], r, i, 0, this.t);
    for (j = Math.min(a.t, n) ; i < j; ++i) this.am(0, a[i], r, i, 0, n - i);
    r.clamp();
}

// (protected) r = "this * a" without lower n words, n > 0
// "this" should be the larger one if appropriate.
function bnpMultiplyUpperTo(a, n, r) {
    --n;
    var i = r.t = this.t + a.t - n;
    r.s = 0; // assumes a,this >= 0
    while (--i >= 0) r[i] = 0;
    for (i = Math.max(n - this.t, 0) ; i < a.t; ++i)
        r[this.t + i - n] = this.am(n - i, a[i], r, 0, 0, this.t + i - n);
    r.clamp();
    r.drShiftTo(1, r);
}

// Barrett modular reduction
function Barrett(m) {
    // setup Barrett
    this.r2 = nbi();
    this.q3 = nbi();
    BigInteger.ONE.dlShiftTo(2 * m.t, this.r2);
    this.mu = this.r2.divide(m);
    this.m = m;
}

function barrettConvert(x) {
    if (x.s < 0 || x.t > 2 * this.m.t) return x.mod(this.m);
    else if (x.compareTo(this.m) < 0) return x;
    else { var r = nbi(); x.copyTo(r); this.reduce(r); return r; }
}

function barrettRevert(x) { return x; }

// x = x mod m (HAC 14.42)
function barrettReduce(x) {
    x.drShiftTo(this.m.t - 1, this.r2);
    if (x.t > this.m.t + 1) { x.t = this.m.t + 1; x.clamp(); }
    this.mu.multiplyUpperTo(this.r2, this.m.t + 1, this.q3);
    this.m.multiplyLowerTo(this.q3, this.m.t + 1, this.r2);
    while (x.compareTo(this.r2) < 0) x.dAddOffset(1, this.m.t + 1);
    x.subTo(this.r2, x);
    while (x.compareTo(this.m) >= 0) x.subTo(this.m, x);
}

// r = x^2 mod m; x != r
function barrettSqrTo(x, r) { x.squareTo(r); this.reduce(r); }

// r = x*y mod m; x,y != r
function barrettMulTo(x, y, r) { x.multiplyTo(y, r); this.reduce(r); }

Barrett.prototype.convert = barrettConvert;
Barrett.prototype.revert = barrettRevert;
Barrett.prototype.reduce = barrettReduce;
Barrett.prototype.mulTo = barrettMulTo;
Barrett.prototype.sqrTo = barrettSqrTo;

// (public) this^e % m (HAC 14.85)
function bnModPow(e, m) {
    var i = e.bitLength(), k, r = nbv(1), z;
    if (i <= 0) return r;
    else if (i < 18) k = 1;
    else if (i < 48) k = 3;
    else if (i < 144) k = 4;
    else if (i < 768) k = 5;
    else k = 6;
    if (i < 8)
        z = new Classic(m);
    else if (m.isEven())
        z = new Barrett(m);
    else
        z = new Montgomery(m);

    // precomputation
    var g = new Array(), n = 3, k1 = k - 1, km = (1 << k) - 1;
    g[1] = z.convert(this);
    if (k > 1) {
        var g2 = nbi();
        z.sqrTo(g[1], g2);
        while (n <= km) {
            g[n] = nbi();
            z.mulTo(g2, g[n - 2], g[n]);
            n += 2;
        }
    }

    var j = e.t - 1, w, is1 = true, r2 = nbi(), t;
    i = nbits(e[j]) - 1;
    while (j >= 0) {
        if (i >= k1) w = (e[j] >> (i - k1)) & km;
        else {
            w = (e[j] & ((1 << (i + 1)) - 1)) << (k1 - i);
            if (j > 0) w |= e[j - 1] >> (this.DB + i - k1);
        }

        n = k;
        while ((w & 1) == 0) { w >>= 1; --n; }
        if ((i -= n) < 0) { i += this.DB; --j; }
        if (is1) {  // ret == 1, don't bother squaring or multiplying it
            g[w].copyTo(r);
            is1 = false;
        }
        else {
            while (n > 1) { z.sqrTo(r, r2); z.sqrTo(r2, r); n -= 2; }
            if (n > 0) z.sqrTo(r, r2); else { t = r; r = r2; r2 = t; }
            z.mulTo(r2, g[w], r);
        }

        while (j >= 0 && (e[j] & (1 << i)) == 0) {
            z.sqrTo(r, r2); t = r; r = r2; r2 = t;
            if (--i < 0) { i = this.DB - 1; --j; }
        }
    }
    return z.revert(r);
}

// (public) gcd(this,a) (HAC 14.54)
function bnGCD(a) {
    var x = (this.s < 0) ? this.negate() : this.clone();
    var y = (a.s < 0) ? a.negate() : a.clone();
    if (x.compareTo(y) < 0) { var t = x; x = y; y = t; }
    var i = x.getLowestSetBit(), g = y.getLowestSetBit();
    if (g < 0) return x;
    if (i < g) g = i;
    if (g > 0) {
        x.rShiftTo(g, x);
        y.rShiftTo(g, y);
    }
    while (x.signum() > 0) {
        if ((i = x.getLowestSetBit()) > 0) x.rShiftTo(i, x);
        if ((i = y.getLowestSetBit()) > 0) y.rShiftTo(i, y);
        if (x.compareTo(y) >= 0) {
            x.subTo(y, x);
            x.rShiftTo(1, x);
        }
        else {
            y.subTo(x, y);
            y.rShiftTo(1, y);
        }
    }
    if (g > 0) y.lShiftTo(g, y);
    return y;
}

// (protected) this % n, n < 2^26
function bnpModInt(n) {
    if (n <= 0) return 0;
    var d = this.DV % n, r = (this.s < 0) ? n - 1 : 0;
    if (this.t > 0)
        if (d == 0) r = this[0] % n;
        else for (var i = this.t - 1; i >= 0; --i) r = (d * r + this[i]) % n;
    return r;
}

// (public) 1/this % m (HAC 14.61)
function bnModInverse(m) {
    var ac = m.isEven();
    if ((this.isEven() && ac) || m.signum() == 0) return BigInteger.ZERO;
    var u = m.clone(), v = this.clone();
    var a = nbv(1), b = nbv(0), c = nbv(0), d = nbv(1);
    while (u.signum() != 0) {
        while (u.isEven()) {
            u.rShiftTo(1, u);
            if (ac) {
                if (!a.isEven() || !b.isEven()) { a.addTo(this, a); b.subTo(m, b); }
                a.rShiftTo(1, a);
            }
            else if (!b.isEven()) b.subTo(m, b);
            b.rShiftTo(1, b);
        }
        while (v.isEven()) {
            v.rShiftTo(1, v);
            if (ac) {
                if (!c.isEven() || !d.isEven()) { c.addTo(this, c); d.subTo(m, d); }
                c.rShiftTo(1, c);
            }
            else if (!d.isEven()) d.subTo(m, d);
            d.rShiftTo(1, d);
        }
        if (u.compareTo(v) >= 0) {
            u.subTo(v, u);
            if (ac) a.subTo(c, a);
            b.subTo(d, b);
        }
        else {
            v.subTo(u, v);
            if (ac) c.subTo(a, c);
            d.subTo(b, d);
        }
    }
    if (v.compareTo(BigInteger.ONE) != 0) return BigInteger.ZERO;
    if (d.compareTo(m) >= 0) return d.subtract(m);
    if (d.signum() < 0) d.addTo(m, d); else return d;
    if (d.signum() < 0) return d.add(m); else return d;
}

var lowprimes = [2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283, 293, 307, 311, 313, 317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397, 401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461, 463, 467, 479, 487, 491, 499, 503, 509];
var lplim = (1 << 26) / lowprimes[lowprimes.length - 1];

// (public) test primality with certainty >= 1-.5^t
function bnIsProbablePrime(t) {
    var i, x = this.abs();
    if (x.t == 1 && x[0] <= lowprimes[lowprimes.length - 1]) {
        for (i = 0; i < lowprimes.length; ++i)
            if (x[0] == lowprimes[i]) return true;
        return false;
    }
    if (x.isEven()) return false;
    i = 1;
    while (i < lowprimes.length) {
        var m = lowprimes[i], j = i + 1;
        while (j < lowprimes.length && m < lplim) m *= lowprimes[j++];
        m = x.modInt(m);
        while (i < j) if (m % lowprimes[i++] == 0) return false;
    }
    return x.millerRabin(t);
}

// (protected) true if probably prime (HAC 4.24, Miller-Rabin)
function bnpMillerRabin(t) {
    var n1 = this.subtract(BigInteger.ONE);
    var k = n1.getLowestSetBit();
    if (k <= 0) return false;
    var r = n1.shiftRight(k);
    t = (t + 1) >> 1;
    if (t > lowprimes.length) t = lowprimes.length;
    var a = nbi();
    for (var i = 0; i < t; ++i) {
        a.fromInt(lowprimes[i]);
        var y = a.modPow(r, this);
        if (y.compareTo(BigInteger.ONE) != 0 && y.compareTo(n1) != 0) {
            var j = 1;
            while (j++ < k && y.compareTo(n1) != 0) {
                y = y.modPowInt(2, this);
                if (y.compareTo(BigInteger.ONE) == 0) return false;
            }
            if (y.compareTo(n1) != 0) return false;
        }
    }
    return true;
}

// protected
BigInteger.prototype.chunkSize = bnpChunkSize;
BigInteger.prototype.toRadix = bnpToRadix;
BigInteger.prototype.fromRadix = bnpFromRadix;
BigInteger.prototype.fromNumber = bnpFromNumber;
BigInteger.prototype.bitwiseTo = bnpBitwiseTo;
BigInteger.prototype.changeBit = bnpChangeBit;
BigInteger.prototype.addTo = bnpAddTo;
BigInteger.prototype.dMultiply = bnpDMultiply;
BigInteger.prototype.dAddOffset = bnpDAddOffset;
BigInteger.prototype.multiplyLowerTo = bnpMultiplyLowerTo;
BigInteger.prototype.multiplyUpperTo = bnpMultiplyUpperTo;
BigInteger.prototype.modInt = bnpModInt;
BigInteger.prototype.millerRabin = bnpMillerRabin;

// public
BigInteger.prototype.clone = bnClone;
BigInteger.prototype.intValue = bnIntValue;
BigInteger.prototype.byteValue = bnByteValue;
BigInteger.prototype.shortValue = bnShortValue;
BigInteger.prototype.signum = bnSigNum;
BigInteger.prototype.toByteArray = bnToByteArray;
BigInteger.prototype.equals = bnEquals;
BigInteger.prototype.min = bnMin;
BigInteger.prototype.max = bnMax;
BigInteger.prototype.and = bnAnd;
BigInteger.prototype.or = bnOr;
BigInteger.prototype.xor = bnXor;
BigInteger.prototype.andNot = bnAndNot;
BigInteger.prototype.not = bnNot;
BigInteger.prototype.shiftLeft = bnShiftLeft;
BigInteger.prototype.shiftRight = bnShiftRight;
BigInteger.prototype.getLowestSetBit = bnGetLowestSetBit;
BigInteger.prototype.bitCount = bnBitCount;
BigInteger.prototype.testBit = bnTestBit;
BigInteger.prototype.setBit = bnSetBit;
BigInteger.prototype.clearBit = bnClearBit;
BigInteger.prototype.flipBit = bnFlipBit;
BigInteger.prototype.add = bnAdd;
BigInteger.prototype.subtract = bnSubtract;
BigInteger.prototype.multiply = bnMultiply;
BigInteger.prototype.divide = bnDivide;
BigInteger.prototype.remainder = bnRemainder;
BigInteger.prototype.divideAndRemainder = bnDivideAndRemainder;
BigInteger.prototype.modPow = bnModPow;
BigInteger.prototype.modInverse = bnModInverse;
BigInteger.prototype.pow = bnPow;
BigInteger.prototype.gcd = bnGCD;
BigInteger.prototype.isProbablePrime = bnIsProbablePrime;
/************************************************************************/
/*                                   rsa                                */
/************************************************************************/
// Depends on jsbn.js and rng.js

// Version 1.1: support utf-8 encoding in pkcs1pad2

// convert a (hex) string to a bignum object
function parseBigInt(str, r) {
    return new BigInteger(str, r);
}

function linebrk(s, n) {
    var ret = "";
    var i = 0;
    while (i + n < s.length) {
        ret += s.substring(i, i + n) + "\n";
        i += n;
    }
    return ret + s.substring(i, s.length);
}

function byte2Hex(b) {
    if (b < 0x10)
        return "0" + b.toString(16);
    else
        return b.toString(16);
}

// PKCS#1 (type 2, random) pad input string s to n bytes, and return a bigint
function pkcs1pad2(s, n) {
    if (n < s.length + 11) { // TODO: fix for utf-8
        console.log("Message too long for RSA");
        return null;
    }
    var ba = new Array();
    var i = s.length - 1;
    while (i >= 0 && n > 0) {
        var c = s.charCodeAt(i--);
        if (c < 128) { // encode using utf-8
            ba[--n] = c;
        }
        else if ((c > 127) && (c < 2048)) {
            ba[--n] = (c & 63) | 128;
            ba[--n] = (c >> 6) | 192;
        }
        else {
            ba[--n] = (c & 63) | 128;
            ba[--n] = ((c >> 6) & 63) | 128;
            ba[--n] = (c >> 12) | 224;
        }
    }
    ba[--n] = 0;
    var rng = new SecureRandom();
    var x = new Array();
    while (n > 2) { // random non-zero pad
        x[0] = 0;
        while (x[0] == 0) rng.nextBytes(x);
        ba[--n] = x[0];
    }
    ba[--n] = 2;
    ba[--n] = 0;
    return new BigInteger(ba);
}

// "empty" RSA key constructor
function RSAKey() {
    this.n = null;
    this.e = 0;
    this.d = null;
    this.p = null;
    this.q = null;
    this.dmp1 = null;
    this.dmq1 = null;
    this.coeff = null;
}

// Set the public key fields N and e from hex strings
function RSASetPublic(N, E) {
    if (N != null && E != null && N.length > 0 && E.length > 0) {
        this.n = parseBigInt(N, 16);
        this.e = parseInt(E, 16);
    }
    else
        console.log("Invalid RSA public key");
}

// Perform raw public operation on "x": return x^e (mod n)
function RSADoPublic(x) {
    return x.modPowInt(this.e, this.n);
}

// Return the PKCS#1 RSA encryption of "text" as an even-length hex string
function RSAEncrypt(text) {
    var m = pkcs1pad2(text, (this.n.bitLength() + 7) >> 3);
    if (m == null) return null;
    var c = this.doPublic(m);
    if (c == null) return null;
    var h = c.toString(16);
    if ((h.length & 1) == 0) return h; else return "0" + h;
}

// Return the PKCS#1 RSA encryption of "text" as a Base64-encoded string
//function RSAEncryptB64(text) {
//  var h = this.encrypt(text);
//  if(h) return hex2b64(h); else return null;
//}

// protected
RSAKey.prototype.doPublic = RSADoPublic;

// public
RSAKey.prototype.setPublic = RSASetPublic;
RSAKey.prototype.encrypt = RSAEncrypt;
//RSAKey.prototype.encrypt_b64 = RSAEncryptB64;

/************************************************************************/
/*                         SHA1                                         */
/************************************************************************/
sha1 = new function () {
    var blockLen = 64;
    var state = [0x67452301, 0xefcdab89, 0x98badcfe, 0x10325476, 0xc3d2e1f0];
    var sttLen = state.length;

    this.hex = function (_data) {
        return toHex(getMD(_data));
    }

    this.dec = function (_data) {
        return getMD(_data);
    }

    this.bin = function (_data) {
        return pack(getMD(_data));
    }

    var getMD = function (_data) {
        var datz = [];
        if (isAry(_data)) datz = _data;
        else if (isStr(_data)) datz = unpack(_data);
        else "unknown type";
        datz = paddingData(datz);
        return round(datz);
    }

    var isAry = function (_ary) {
        return _ary && _ary.constructor === [].constructor;
    }
    var isStr = function (_str) {
        return typeof (_str) == typeof ("string");
    }

    var rotl = function (_v, _s) { return (_v << _s) | (_v >>> (32 - _s)) };

    var round = function (_blk) {
        var stt = [];
        var tmpS = [];
        var i, j, tmp, x = [];
        for (j = 0; j < sttLen; j++) stt[j] = state[j];

        for (i = 0; i < _blk.length; i += blockLen) {
            for (j = 0; j < sttLen; j++) tmpS[j] = stt[j];
            x = toBigEndian32(_blk.slice(i, i + blockLen));
            for (j = 16; j < 80; j++)
                x[j] = rotl(x[j - 3] ^ x[j - 8] ^ x[j - 14] ^ x[j - 16], 1);

            for (j = 0; j < 80; j++) {
                if (j < 20)
                    tmp = ((stt[1] & stt[2]) ^ (~stt[1] & stt[3])) + K[0];
                else if (j < 40)
                    tmp = (stt[1] ^ stt[2] ^ stt[3]) + K[1];
                else if (j < 60)
                    tmp = ((stt[1] & stt[2]) ^ (stt[1] & stt[3]) ^ (stt[2] & stt[3])) + K[2];
                else
                    tmp = (stt[1] ^ stt[2] ^ stt[3]) + K[3];

                tmp += rotl(stt[0], 5) + x[j] + stt[4];
                stt[4] = stt[3];
                stt[3] = stt[2];
                stt[2] = rotl(stt[1], 30);
                stt[1] = stt[0];
                stt[0] = tmp;
            }
            for (j = 0; j < sttLen; j++) stt[j] += tmpS[j];
        }

        return fromBigEndian32(stt);
    }

    var paddingData = function (_datz) {
        var datLen = _datz.length;
        var n = datLen;
        _datz[n++] = 0x80;
        while (n % blockLen != 56) _datz[n++] = 0;
        datLen *= 8;
        return _datz.concat(0, 0, 0, 0, fromBigEndian32([datLen]));
    }

    var toHex = function (_decz) {
        var i, hex = "";

        for (i = 0; i < _decz.length; i++)
            hex += (_decz[i] > 0xf ? "" : "0") + _decz[i].toString(16);
        return hex;
    }

    var fromBigEndian32 = function (_blk) {
        var tmp = [];
        for (n = i = 0; i < _blk.length; i++) {
            tmp[n++] = (_blk[i] >>> 24) & 0xff;
            tmp[n++] = (_blk[i] >>> 16) & 0xff;
            tmp[n++] = (_blk[i] >>> 8) & 0xff;
            tmp[n++] = _blk[i] & 0xff;
        }
        return tmp;
    }

    var toBigEndian32 = function (_blk) {
        var tmp = [];
        var i, n;
        for (n = i = 0; i < _blk.length; i += 4, n++)
            tmp[n] = (_blk[i] << 24) | (_blk[i + 1] << 16) | (_blk[i + 2] << 8) | _blk[i + 3];
        return tmp;
    }

    var unpack = function (_dat) {
        var i, n, c, tmp = [];

        for (n = i = 0; i < _dat.length; i++) {
            c = _dat.charCodeAt(i);
            if (c <= 0xff) tmp[n++] = c;
            else {
                tmp[n++] = c >>> 8;
                tmp[n++] = c & 0xff;
            }
        }
        return tmp;
    }

    var pack = function (_ary) {
        var i, tmp = "";
        for (i in _ary) tmp += String.fromCharCode(_ary[i]);
        return tmp;
    }

    var K = [0x5a827999, 0x6ed9eba1, 0x8f1bbcdc, 0xca62c1d6];

}

/************************************************************************/
/*                  SHA256                                              */
/************************************************************************/
sha256 = new function () {
    var blockLen = 64;
    var state = [0x6a09e667, 0xbb67ae85, 0x3c6ef372, 0xa54ff53a,
                    0x510e527f, 0x9b05688c, 0x1f83d9ab, 0x5be0cd19];
    var sttLen = state.length;

    this.hex = function (_data) {
        return toHex(getMD(_data));
    }

    this.dec = function (_data) {
        return getMD(_data);
    }

    this.bin = function (_data) {
        return pack(getMD(_data));
    }

    var getMD = function (_data) {
        var datz = [];
        if (isAry(_data)) datz = _data;
        else if (isStr(_data)) datz = unpack(_data);
        else "unknown type";
        datz = paddingData(datz);
        return round(datz);
    }

    var isAry = function (_ary) {
        return _ary && _ary.constructor === [].constructor;
    }
    var isStr = function (_str) {
        return typeof (_str) == typeof ("string");
    }

    var rotr = function (_v, _s) { return (_v >>> _s) | (_v << (32 - _s)) };

    var S0 = function (_v) { return rotr(_v, 2) ^ rotr(_v, 13) ^ rotr(_v, 22) };
    var S1 = function (_v) { return rotr(_v, 6) ^ rotr(_v, 11) ^ rotr(_v, 25) };
    var s0 = function (_v) { return rotr(_v, 7) ^ rotr(_v, 18) ^ (_v >>> 3) };
    var s1 = function (_v) { return rotr(_v, 17) ^ rotr(_v, 19) ^ (_v >>> 10) };

    var ch = function (_b, _c, _d) { return (_b & _c) ^ (~_b & _d) };
    var maj = function (_b, _c, _d) { return (_b & _c) ^ (_b & _d) ^ (_c & _d) };

    var round = function (_blk) {
        var stt = [];
        var tmpS = [];
        var i, j, tmp1, tmp2, x = [];
        for (j = 0; j < sttLen; j++) stt[j] = state[j];

        for (i = 0; i < _blk.length; i += blockLen) {
            for (j = 0; j < sttLen; j++) tmpS[j] = stt[j];
            x = toBigEndian32(_blk.slice(i, i + blockLen));
            for (j = 16; j < 64; j++)
                x[j] = s1(x[j - 2]) + x[j - 7] + s0(x[j - 15]) + x[j - 16];

            for (j = 0; j < 64; j++) {
                tmp1 = stt[7] + S1(stt[4]) + ch(stt[4], stt[5], stt[6]) + K[j] + x[j];
                tmp2 = S0(stt[0]) + maj(stt[0], stt[1], stt[2]);

                stt[7] = stt[6];
                stt[6] = stt[5];
                stt[5] = stt[4];
                stt[4] = stt[3] + tmp1;
                stt[3] = stt[2];
                stt[2] = stt[1];
                stt[1] = stt[0];
                stt[0] = tmp1 + tmp2;
            }
            for (j = 0; j < sttLen; j++) stt[j] += tmpS[j];
        }

        return fromBigEndian32(stt);
    }

    var paddingData = function (_datz) {
        var datLen = _datz.length;
        var n = datLen;
        _datz[n++] = 0x80;
        while (n % blockLen != 56) _datz[n++] = 0;
        datLen *= 8;
        return _datz.concat(0, 0, 0, 0, fromBigEndian32([datLen]));
    }

    var toHex = function (_decz) {
        var i, hex = "";

        for (i = 0; i < _decz.length; i++)
            hex += (_decz[i] > 0xf ? "" : "0") + _decz[i].toString(16);
        return hex;
    }

    var fromBigEndian32 = function (_blk) {
        var tmp = [];
        for (n = i = 0; i < _blk.length; i++) {
            tmp[n++] = (_blk[i] >>> 24) & 0xff;
            tmp[n++] = (_blk[i] >>> 16) & 0xff;
            tmp[n++] = (_blk[i] >>> 8) & 0xff;
            tmp[n++] = _blk[i] & 0xff;
        }
        return tmp;
    }

    var toBigEndian32 = function (_blk) {
        var tmp = [];
        var i, n;
        for (n = i = 0; i < _blk.length; i += 4, n++)
            tmp[n] = (_blk[i] << 24) | (_blk[i + 1] << 16) | (_blk[i + 2] << 8) | _blk[i + 3];
        return tmp;
    }

    var unpack = function (_dat) {
        var i, n, c, tmp = [];

        for (n = i = 0; i < _dat.length; i++) {
            c = _dat.charCodeAt(i);
            if (c <= 0xff) tmp[n++] = c;
            else {
                tmp[n++] = c >>> 8;
                tmp[n++] = c & 0xff;
            }
        }
        return tmp;
    }

    var pack = function (_ary) {
        var i, tmp = "";
        for (i in _ary) tmp += String.fromCharCode(_ary[i]);
        return tmp;
    }


    var K = [
        0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5,
        0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5,
        0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3,
        0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,

        0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc,
        0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
        0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7,
        0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967,

        0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13,
        0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85,
        0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3,
        0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,

        0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5,
        0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
        0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208,
        0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
    ];
}

/************************************************************************/
/*                  RSA-SIGN                                            */
/************************************************************************/
// As for _RSASGIN_DIHEAD values for each hash algorithm, see PKCS#1 v2.1 spec (p38).
var _RSASIGN_DIHEAD = [];
_RSASIGN_DIHEAD['sha1'] = "3021300906052b0e03021a05000414";
_RSASIGN_DIHEAD['sha256'] = "3031300d060960864801650304020105000420";
//_RSASIGN_DIHEAD['md2'] = "3020300c06082a864886f70d020205000410";
//_RSASIGN_DIHEAD['md5'] = "3020300c06082a864886f70d020505000410";
//_RSASIGN_DIHEAD['sha384'] = "3041300d060960864801650304020205000430";
//_RSASIGN_DIHEAD['sha512'] = "3051300d060960864801650304020305000440";
var _RSASIGN_HASHHEXFUNC = [];
_RSASIGN_HASHHEXFUNC['sha1'] = sha1.hex;
_RSASIGN_HASHHEXFUNC['sha256'] = sha256.hex;

// ========================================================================
// Signature Generation
// ========================================================================

function _rsasign_getHexPaddedDigestInfoForString(s, keySize, hashAlg) {
    var pmStrLen = keySize / 4;
    var hashFunc = _RSASIGN_HASHHEXFUNC[hashAlg];
    var sHashHex = hashFunc(s);

    var sHead = "0001";
    var sTail = "00" + _RSASIGN_DIHEAD[hashAlg] + sHashHex;
    var sMid = "";
    var fLen = pmStrLen - sHead.length - sTail.length;
    for (var i = 0; i < fLen; i += 2) {
        sMid += "ff";
    }
    sPaddedMessageHex = sHead + sMid + sTail;
    return sPaddedMessageHex;
}

function _rsasign_signString(s, hashAlg) {
    var hPM = _rsasign_getHexPaddedDigestInfoForString(s, this.n.bitLength(), hashAlg);
    var biPaddedMessage = parseBigInt(hPM, 16);
    var biSign = this.doPrivate(biPaddedMessage);
    var hexSign = biSign.toString(16);
    return hexSign;
}

function _rsasign_signStringWithSHA1(s) {
    var hPM = _rsasign_getHexPaddedDigestInfoForString(s, this.n.bitLength(), 'sha1');
    var biPaddedMessage = parseBigInt(hPM, 16);
    var biSign = this.doPrivate(biPaddedMessage);
    var hexSign = biSign.toString(16);
    return hexSign;
}

function _rsasign_signStringWithSHA256(s) {
    var hPM = _rsasign_getHexPaddedDigestInfoForString(s, this.n.bitLength(), 'sha256');
    var biPaddedMessage = parseBigInt(hPM, 16);
    var biSign = this.doPrivate(biPaddedMessage);
    var hexSign = biSign.toString(16);
    return hexSign;
}

// ========================================================================
// Signature Verification
// ========================================================================

function _rsasign_getDecryptSignatureBI(biSig, hN, hE) {
    var rsa = new RSAKey();
    rsa.setPublic(hN, hE);
    var biDecryptedSig = rsa.doPublic(biSig);
    return biDecryptedSig;
}

function _rsasign_getHexDigestInfoFromSig(biSig, hN, hE) {
    var biDecryptedSig = _rsasign_getDecryptSignatureBI(biSig, hN, hE);
    var hDigestInfo = biDecryptedSig.toString(16).replace(/^1f+00/, '');
    return hDigestInfo;
}

function _rsasign_getAlgNameAndHashFromHexDisgestInfo(hDigestInfo) {
    for (var algName in _RSASIGN_DIHEAD) {
        var head = _RSASIGN_DIHEAD[algName];
        var len = head.length;
        if (hDigestInfo.substring(0, len) == head) {
            var a = [algName, hDigestInfo.substring(len)];
            return a;
        }
    }
    return [];
}

function _rsasign_verifySignatureWithArgs(sMsg, biSig, hN, hE) {
    var hDigestInfo = _rsasign_getHexDigestInfoFromSig(biSig, hN, hE);
    var digestInfoAry = _rsasign_getAlgNameAndHashFromHexDisgestInfo(hDigestInfo);
    if (digestInfoAry.length == 0) return false;
    var algName = digestInfoAry[0];
    var diHashValue = digestInfoAry[1];
    var ff = _RSASIGN_HASHHEXFUNC[algName];
    var msgHashValue = ff(sMsg);
    return (diHashValue == msgHashValue);
}

function _rsasign_verifyHexSignatureForMessage(hSig, sMsg) {
    var biSig = parseBigInt(hSig, 16);
    var result = _rsasign_verifySignatureWithArgs(sMsg, biSig,
                        this.n.toString(16),
                        this.e.toString(16));
    return result;
}

function _rsasign_verifyString(sMsg, hSig) {
    hSig = hSig.replace(/[ \n]+/g, "");
    var biSig = parseBigInt(hSig, 16);
    var biDecryptedSig = this.doPublic(biSig);
    var hDigestInfo = biDecryptedSig.toString(16).replace(/^1f+00/, '');
    var digestInfoAry = _rsasign_getAlgNameAndHashFromHexDisgestInfo(hDigestInfo);

    if (digestInfoAry.length == 0) return false;
    var algName = digestInfoAry[0];
    var diHashValue = digestInfoAry[1];
    var ff = _RSASIGN_HASHHEXFUNC[algName];
    var msgHashValue = ff(sMsg);
    return (diHashValue == msgHashValue);
}

RSAKey.prototype.signString = _rsasign_signString;
RSAKey.prototype.signStringWithSHA1 = _rsasign_signStringWithSHA1;
RSAKey.prototype.signStringWithSHA256 = _rsasign_signStringWithSHA256;

RSAKey.prototype.verifyString = _rsasign_verifyString;
RSAKey.prototype.verifyHexSignatureForMessage = _rsasign_verifyHexSignatureForMessage;

/************************************************************************/
/*                  BASE64                                              */
/************************************************************************/
var b64map = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
var b64pad = "=";

function hex2b64(h) {
    var i;
    var c;
    var ret = "";
    for (i = 0; i + 3 <= h.length; i += 3) {
        c = parseInt(h.substring(i, i + 3), 16);
        ret += b64map.charAt(c >> 6) + b64map.charAt(c & 63);
    }
    if (i + 1 == h.length) {
        c = parseInt(h.substring(i, i + 1), 16);
        ret += b64map.charAt(c << 2);
    }
    else if (i + 2 == h.length) {
        c = parseInt(h.substring(i, i + 2), 16);
        ret += b64map.charAt(c >> 2) + b64map.charAt((c & 3) << 4);
    }
    while ((ret.length & 3) > 0) ret += b64pad;
    return ret;
}

// convert a base64 string to hex
function b64tohex(s) {
    var ret = ""
    var i;
    var k = 0; // b64 state, 0-3
    var slop;
    for (i = 0; i < s.length; ++i) {
        if (s.charAt(i) == b64pad) break;
        v = b64map.indexOf(s.charAt(i));
        if (v < 0) continue;
        if (k == 0) {
            ret += int2char(v >> 2);
            slop = v & 3;
            k = 1;
        }
        else if (k == 1) {
            ret += int2char((slop << 2) | (v >> 4));
            slop = v & 0xf;
            k = 2;
        }
        else if (k == 2) {
            ret += int2char(slop);
            ret += int2char(v >> 2);
            slop = v & 3;
            k = 3;
        }
        else {
            ret += int2char((slop << 2) | (v >> 4));
            ret += int2char(v & 0xf);
            k = 0;
        }
    }
    if (k == 1)
        ret += int2char(slop << 2);
    return ret;
}

// convert a base64 string to a byte/number array
function b64toBA(s) {
    //piggyback on b64tohex for now, optimize later
    var h = b64tohex(s);
    var i;
    var a = new Array();
    for (i = 0; 2 * i < h.length; ++i) {
        a[i] = parseInt(h.substring(2 * i, 2 * i + 2), 16);
    }
    return a;
}

/************************************************************************/
/*                  ASN1HEX                                             */
/************************************************************************/
function _asnhex_getByteLengthOfL_AtObj(s, pos) {
    if (s.substring(pos + 2, pos + 3) != '8') return 1;
    var i = parseInt(s.substring(pos + 3, pos + 4));
    if (i == 0) return -1;      // length octet '80' indefinite length
    if (0 < i && i < 10) return i + 1; // including '8?' octet;
    return -2;          // malformed format
}

function _asnhex_getHexOfL_AtObj(s, pos) {
    var len = _asnhex_getByteLengthOfL_AtObj(s, pos);
    if (len < 1) return '';
    return s.substring(pos + 2, pos + 2 + len * 2);
}
function _asnhex_getIntOfL_AtObj(s, pos) {
    var hLength = _asnhex_getHexOfL_AtObj(s, pos);
    if (hLength == '') return -1;
    var bi;
    if (parseInt(hLength.substring(0, 1)) < 8) {
        bi = parseBigInt(hLength, 16);
    } else {
        bi = parseBigInt(hLength.substring(2), 16);
    }
    return bi.intValue();
}

//
// get ASN.1 value starting string position 
// for ASN.1 object refered by index 'idx'.
//
function _asnhex_getStartPosOfV_AtObj(s, pos) {
    var l_len = _asnhex_getByteLengthOfL_AtObj(s, pos);
    if (l_len < 0) return l_len;
    return pos + (l_len + 1) * 2;
}

function _asnhex_getHexOfV_AtObj(s, pos) {
    var pos1 = _asnhex_getStartPosOfV_AtObj(s, pos);
    var len = _asnhex_getIntOfL_AtObj(s, pos);
    return s.substring(pos1, pos1 + len * 2);
}

function _asnhex_getPosOfNextSibling_AtObj(s, pos) {
    var pos1 = _asnhex_getStartPosOfV_AtObj(s, pos);
    var len = _asnhex_getIntOfL_AtObj(s, pos);
    return pos1 + len * 2;
}

function _asnhex_getPosArrayOfChildren_AtObj(h, pos) {
    var a = new Array();
    var p0 = _asnhex_getStartPosOfV_AtObj(h, pos);
    a.push(p0);

    var len = _asnhex_getIntOfL_AtObj(h, pos);
    var p = p0;
    var k = 0;
    while (1) {
        var pNext = _asnhex_getPosOfNextSibling_AtObj(h, p);
        if (pNext == null || (pNext - p0 >= (len * 2))) break;
        if (k >= 200) break;

        a.push(pNext);
        p = pNext;

        k++;
    }

    return a;
}

/************************************************************************/
/*                  X509Cert                                            */
/************************************************************************/
function _x509_pemToBase64(sCertPEM) {
    var s = sCertPEM;
    if (s.indexOf("CERTIFICATE") > -1) {
        s = s.replace("-----BEGIN CERTIFICATE-----", "");
        s = s.replace("-----END CERTIFICATE-----", "");
    }
    if (s.indexOf("PUBLIC KEY") > -1) {
        s = s.replace("-----BEGIN PUBLIC KEY-----", "");
        s = s.replace("-----END PUBLIC KEY-----", "");
    }
    s = s.replace(/[ \n]+/g, "");
    return s;
}

function _x509_pemToHex(sCertPEM) {
    var b64Cert = _x509_pemToBase64(sCertPEM);
    var hCert = b64tohex(b64Cert);
    return hCert;
}

function _x509_getHexTbsCertificateFromCert(hCert) {
    var pTbsCert = _asnhex_getStartPosOfV_AtObj(hCert, 0);
    return pTbsCert;
}

// NOTE: privateKeyUsagePeriod field of X509v2 not supported.
// NOTE: v1 and v3 supported
function _x509_getSubjectPublicKeyInfoPosFromCertHex(hCert) {
    var pTbsCert = _asnhex_getStartPosOfV_AtObj(hCert, 0);
    var a = _asnhex_getPosArrayOfChildren_AtObj(hCert, pTbsCert);
    if (a.length < 1) return -1;
    if (hCert.substring(a[0], a[0] + 10) == "a003020102") { // v3
        if (a.length < 6) return -1;
        return a[6];
    } else {
        if (a.length < 5) return -1;
        return a[5];
    }
}

// NOTE: Without BITSTRING encapsulation.
function _x509_getSubjectPublicKeyPosFromCertHex(hCert) {
    var pInfo = _x509_getSubjectPublicKeyInfoPosFromCertHex(hCert);
    if (pInfo == -1) return -1;
    var a = _asnhex_getPosArrayOfChildren_AtObj(hCert, pInfo);
    if (a.length != 2) return -1;
    var pBitString = a[1];
    if (hCert.substring(pBitString, pBitString + 2) != '03') return -1;
    var pBitStringV = _asnhex_getStartPosOfV_AtObj(hCert, pBitString);

    if (hCert.substring(pBitStringV, pBitStringV + 2) != '00') return -1;
    return pBitStringV + 2;
}

function _x509_getPublicKeyHexArrayFromCertHex(hCert) {
    var p = _x509_getSubjectPublicKeyPosFromCertHex(hCert);
    var a = _asnhex_getPosArrayOfChildren_AtObj(hCert, p);
    if (a.length != 2) return [];
    var hN = _asnhex_getHexOfV_AtObj(hCert, a[0]);
    var hE = _asnhex_getHexOfV_AtObj(hCert, a[1]);
    if (hN != null && hE != null) {
        return [hN, hE];
    } else {
        return [];
    }
}

function _x509_getPublicKeyHexArrayFromCertPEM(sCertPEM) {
    var hCert = _x509_pemToHex(sCertPEM);
    var a = _x509_getPublicKeyHexArrayFromCertHex(hCert);
    return a;
}

function _x509_readCertPEM(sCertPEM) {
    var hCert = _x509_pemToHex(sCertPEM);
    var a = _x509_getPublicKeyHexArrayFromCertHex(hCert);
    var rsa = new RSAKey();
    rsa.setPublic(a[0], a[1]);
    this.subjectPublicKeyRSA = rsa;
    this.subjectPublicKeyRSA_hN = a[0];
    this.subjectPublicKeyRSA_hE = a[1];
}

function _x509_readCertPEMWithoutRSAInit(sCertPEM) {
    var hCert = _x509_pemToHex(sCertPEM);
    var a = _x509_getPublicKeyHexArrayFromCertHex(hCert);
    this.subjectPublicKeyRSA.setPublic(a[0], a[1]);
    this.subjectPublicKeyRSA_hN = a[0];
    this.subjectPublicKeyRSA_hE = a[1];
}

function X509() {
    this.subjectPublicKeyRSA = null;
    this.subjectPublicKeyRSA_hN = null;
    this.subjectPublicKeyRSA_hE = null;
}
X509.prototype.readCertPEM = _x509_readCertPEM;
X509.prototype.readCertPEMWithoutRSAInit = _x509_readCertPEMWithoutRSAInit;
/************************************************************************/
/*                  VERIFY                                              */
/************************************************************************/
//var x509Cert = "-----BEGIN CERTIFICATE-----"
//"MIIBvTCCASYCCQD55fNzc0WF7TANBgkqhkiG9w0BAQUFADAjMQswCQYDVQQGEwJK"
//"UDEUMBIGA1UEChMLMDAtVEVTVC1SU0EwHhcNMTAwNTI4MDIwODUxWhcNMjAwNTI1"
//"MDIwODUxWjAjMQswCQYDVQQGEwJKUDEUMBIGA1UEChMLMDAtVEVTVC1SU0EwgZ8w"
//"DQYJKoZIhvcNAQEBBQADgY0AMIGJAoGBANGEYXtfgDRlWUSDn3haY4NVVQiKI9Cz"
//"Thoua9+DxJuiseyzmBBe7Roh1RPqdvmtOHmEPbJ+kXZYhbozzPRbFGHCJyBfCLzQ"
//"fVos9/qUQ88u83b0SFA2MGmQWQAlRtLy66EkR4rDRwTj2DzR4EEXgEKpIvo8VBs/"
//"3+sHLF3ESgAhAgMBAAEwDQYJKoZIhvcNAQEFBQADgYEAEZ6mXFFq3AzfaqWHmCy1"
//"ARjlauYAa8ZmUFnLm0emg9dkVBJ63aEqARhtok6bDQDzSJxiLpCEF6G4b/Nv/M/M"
//"LyhP+OoOTmETMegAVQMq71choVJyOFE5BtQa6M/lCHEOya5QUfoRF2HF9EjRF44K"
//"3OK+u3ivTSj3zwjtpudY5Xo="
//"-----END CERTIFICATE-----";
var x509Certa = '-----BEGIN CERTIFICATE-----\n' +
'MIIBvTCCASYCCQD55fNzc0WF7TANBgkqhkiG9w0BAQUFADAjMQswCQYDVQQGEwJK\n' +
'UDEUMBIGA1UEChMLMDAtVEVTVC1SU0EwHhcNMTAwNTI4MDIwODUxWhcNMjAwNTI1\n' +
'MDIwODUxWjAjMQswCQYDVQQGEwJKUDEUMBIGA1UEChMLMDAtVEVTVC1SU0EwgZ8w\n' +
'DQYJKoZIhvcNAQEBBQADgY0AMIGJAoGBANGEYXtfgDRlWUSDn3haY4NVVQiKI9Cz\n' +
'Thoua9+DxJuiseyzmBBe7Roh1RPqdvmtOHmEPbJ+kXZYhbozzPRbFGHCJyBfCLzQ\n' +
'fVos9/qUQ88u83b0SFA2MGmQWQAlRtLy66EkR4rDRwTj2DzR4EEXgEKpIvo8VBs/\n' +
'3+sHLF3ESgAhAgMBAAEwDQYJKoZIhvcNAQEFBQADgYEAEZ6mXFFq3AzfaqWHmCy1\n' +
'ARjlauYAa8ZmUFnLm0emg9dkVBJ63aEqARhtok6bDQDzSJxiLpCEF6G4b/Nv/M/M\n' +
'LyhP+OoOTmETMegAVQMq71choVJyOFE5BtQa6M/lCHEOya5QUfoRF2HF9EjRF44K\n' +
'3OK+u3ivTSj3zwjtpudY5Xo=\n' +
'-----END CERTIFICATE-----\n';
var x509Cert = '-----BEGIN CERTIFICATE-----\n' +
'MIIEczCCA1ugAwIBAgIIIoXSY+C+wBowDQYJKoZIhvcNAQELBQAwSTEPMA0GA1UE\n' +
'AwwGQmthdkNBMRkwFwYDVQQKDBBCa2F2IENvcnBvcmF0aW9uMQ4wDAYDVQQHDAVI\n' +
'YW5vaTELMAkGA1UEBhMCVk4wHhcNMTYwNDEyMDQxODExWhcNMTgwNDEyMDQxODEx\n' +
'WjBmMREwDwYDVQQDDAh0dWFuZ3RhZzEPMA0GA1UECwwGQmthdkNBMQ8wDQYDVQQK\n' +
'DAZCa2F2Q0ExDzANBgNVBAcMBkhhIE5vaTERMA8GA1UECAwIQ2F1IEdpYXkxCzAJ\n' +
'BgNVBAYTAlZOMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAgCS9REWp\n' +
'B0k/qoXcPDLF8cJsAUT+s48P2rf4fujhor1POPpe7kTCJXInxlQDIXxAmzVEkKtr\n' +
'O6eRUOFOD8wTcwfn5DkRRyl+FQfHfPMH1kk55qmnJNZe/FCX+SnC6iKYaIH7erbu\n' +
'DULrqoF6S1ipjoBRHDbggauPdilen0OQRyP3QwvAaENZkCQufAcB57aqQ1RDCbHo\n' +
'o1JVlfVSw7KZeXc3ys7uv/09PPUsTOGCXBQir1Ta9LzyKbd3LVnGyNBqeMH0CoWv\n' +
'W5aFt13MLpH+mwg3JuUab3286FVqoH3/P1gBVnG6/RdWxZ0Z3lgIhphzefYGtafM\n' +
'ceJBgU/GWYJkNwIDAQABo4IBQDCCATwwHQYDVR0OBBYEFL1oJCXhGlJQLiTVNDfn\n' +
'A6YTHTTlMAwGA1UdEwEB/wQCMAAwHwYDVR0jBBgwFoAUuZJobyh6txIEmnUftu1K\n' +
'SsHNDqcwgaYGA1UdHwSBnjCBmzCBmKBhoF+GXWh0dHA6Ly9sb2NhbGhvc3Q6ODA4\n' +
'MC9lamJjYS9wdWJsaWN3ZWIvd2ViZGlzdC9jZXJ0ZGlzdD9jbWQ9Y3JsJmlzc3Vl\n' +
'cj1DTj1UZXN0Q0EsTz1BbmFUb20sQz1TRaIzpDEwLzEPMA0GA1UEAwwGVGVzdENB\n' +
'MQ8wDQYDVQQKDAZBbmFUb20xCzAJBgNVBAYTAlNFMA4GA1UdDwEB/wQEAwIDODAd\n' +
'BgNVHSUEFjAUBggrBgEFBQcDAQYIKwYBBQUHAwIwFAYDVR0RBA0wC4IJbG9jYWxo\n' +
'b3N0MA0GCSqGSIb3DQEBCwUAA4IBAQBE67S50sRmsSZOQxY/Cquhj3eRl0M8hcxF\n' +
'zYyxePfuoPmLRgc2p/O/uFs7dsO1U9QkbdELWG1iA/ctsbwaw5ev+7tgLlWD4nFG\n' +
'k0ClDSqMiQm/CQc/MaehJuJtkMMt0Gxv+AG/mfa4hZDSuQokS+rbZq8U/OQhLu32\n' +
'CAOXC93SB8r+BZQpGVKvAMY6LiyOMAgV+MGAQD03kDYaGzOrnpkfmA6yhJVxTaUt\n' +
'MOsi+9m4a764iOTb6w62b/IM/FN/yjGcC9J+CipwhveIgkC7wGuBASn2q8MCeEVc\n' +
'jUgc8RBqmSwsQuT2GhIuVc6TBS4AewTcfeoMZnZR0powB7h904P+\n' +
'-----END CERTIFICATE-----';


function doVerify(sMsg, hSig) {
    var x509 = new X509();
    x509.readCertPEM(x509Cert);
    var result = x509.subjectPublicKeyRSA.verifyString(sMsg, hSig);
    return result;
}


/************************************************************************/
/*                        BkavCA Signer Plugin                          */
/************************************************************************/
function ObjectBkavExtensionCallback() {
    this.FunctionCallback = null;
}

var objCallback = new ObjectBkavExtensionCallback();
var BkavExtensionCallback = 'BkavExtensionCallback';
var ExtensionIsValidEvent = 'ExtensionIsValid';
var ExtensionIsValidJSCallback = null;
var ExtensionIsValidRadomString = "";

var idExtension = 'iackjmmabodkopglcheffjfdcjakocgf';
var iCheckPluginValid = false;
var iCallback = false;   // callback cho ExtensionIsValid
// FIX Cung để hỗ trợ bản cũ
var iPluginCallback = true; // callback Plugin => dang set SignXML, SignPDF, SignOOXML, SignCMS, GetCertListByFilter
var iPluginVerified = true; // Xac thuc plugin

var test;
var BkavCAPluginJSCallback = BkavCASignerPluginCallback;

//vietpdb
$(document).ready(function () {
    CreatControl();
});

function CreatControl() {
    window.jQuery('#ExtensionPlaceHolder').append("<input type='image' id='holderDataInputToExtension' style='visibility:hidden' style='display:none;' ClientIDMode='Static' name='' ></input>");
    window.jQuery('#ExtensionPlaceHolder').append("<input type='image' id='hrSignedData' style='visibility:hidden' style='display:none;' ClientIDMode='Static' name=''></input>"); //ResultExtensionPlaceHolder
    window.jQuery('#ExtensionPlaceHolder').append("<button type='button' id='actionToExtensionProcess' style='visibility:hidden' style='display:none;' ClientIDMode='Static' name=''></button>"); //OnClientClick, onclick = 'WaitingDataresponse();' 
}
//end vietpdb

// Websocket
FUNCTION_ID = {
    SignXMLBase64ID: 0,
    SignXMLBase64XPathID: 1,
    SignXMLFileID: 2,
    SignPDFBase64ID: 3,
    SignPDFFileID: 4,
    SignOOXMLBase64ID: 5,
    SignOOXMLFileID: 6,
    PluginValidID: 7,
    VerifyXMLID: 8,
    VerifyPDFID: 9,
    VerifyOOXMLID: 10,
    GetCertIndexID: 11,
    GetAllCertID: 12,
    GetCertListByFilterID: 13,
    CheckOCSPBySerialID: 14,
    CheckOCSPID: 15,
    CheckCRLID: 16,
    CheckValidTimeID: 17,
    CheckTokenID: 18,
    ReadPDFBase64ToTextID: 19,
    ReadPDFFileToTextID: 20,
    ReadFormFieldsToTextID: 21,
    SetAESKeyID: 22,
    SetUsePKCS11ID: 23,
    ConvertFileToBase64ID: 24,
    SetDLLNameID: 25,
    SetLicenseKeyID: 26,
    GetAllExtensionsID: 27,
    GetSelfExtensionID: 28,
    ValidateCertificateID: 29,
    GetVersionID: 30,
    SetPINCacheID: 31,
    SetGetAttributesCertDefaultID: 32,
    DetectTokenID: 33,
    ImportCertID: 34,
    GetTokenInforID: 35,
    ChangePINTokenID: 36,
    CheckLoginID: 37,
    SignCMSBase64ID: 38,
    VerifyCMSID: 39,
    BkavCANativeAppValidateID: 40,
    SetCheckTokenDefaultID: 41,
    SetHashAlgorithmID: 42,
    SetAddCertChainID: 43,
    SetAddBase64CertID: 44,
    SignXMLDataListID: 45,
    SignOOXMLDataListID: 46,
    SignPDFDataListID: 47,
    SignDataListID: 48,
    FileBrowserID: 49
}

var timer;
var port = 443;
var webSocket;
var iCheckPluginS = 1;


function ProcessData(inputData) {
    try {

        if (iCheckPluginS == 1) {
            console.log("send length 0: " + inputData.length);
            var arr = inputData.split("*");
            var host = window.location.host;
            if (arr[0] == 26) {
                inputData = inputData + "*" + host;
            }
            inputData = inputData + "*end";
            var num = Math.floor(inputData.length / 114271);

            if (num == 0) {
                console.log(inputData);
                webSocket.send(inputData);
                inputData = "";
            }
            else {
                var dataSend = null;
                for (var i = 0; i < num; i++) {
                    dataSend = inputData.substring(114271 * i, 114271 * (i + 1));

                    webSocket.send(dataSend);
                }
                dataSend = inputData.substring((114271 * num), inputData.length);
                console.log("send length 3: " + dataSend);
                webSocket.send(dataSend);
            }
        }
        else {
            var strSub = inputData.split("*");
            if (iPluginVerified || strSub[0] == 'BkavCANativeAppValidate' || strSub[0] == 'ExtensionValidVer2') {
                document.getElementById("holderDataInputToExtension").value = inputData;
                var event = document.createEvent('Event');
                event.initEvent('SendToNativeApp', true, true);
                document.dispatchEvent(event);

            }
            else {

            }
        }

    } catch (e) {
        console.log("Error send: " + e);
    }
}

window.addEventListener("beforeunload", function (e) {
    sessionStorage.setItem("readyState", null);
});
var iCheckRef = 0;
function next(port, funcProcess) {
    tryConnect(port, funcProcess);
}
function tryConnect(port, funcProcess) {


    if (sessionStorage.getItem('readyState') != 1) {


        webSocket = new WebSocket("wss://localhost:" + 8443);
        timer = setTimeout(function () {

            //var s = webSocket;
            webSocket = null;
            //  s.close();
            port++;
            if (iCheckRef < 1) {
                iCheckRef++;

                next(port, funcProcess);
            } else {
                iCheckPluginS = 0;
                funcProcess("0");

            }


        }, 2 * 1000);

        webSocket.onopen = function () {
            sessionStorage.setItem('readyState', 1);

            clearTimeout(timer);
            funcProcess("1");

        }
        webSocket.onclose = function () {

        }

        webSocket.onmessage = function (message) {
            var arg = message.data;
            var sub = arg.split('*');


            if (sub[sub.length - 1] == "end") {
                if (sub[sub.length - 2] == BkavExtensionCallback) {
                    objCallback.FunctionCallback(arg.substr(0, arg.length - BkavExtensionCallback.length - 5));

                }
                else {
                    console.log("Message::" + arg);
                }
            }

            // kiem tra neu co Callback thi cat lay cai cuoi xong truyen vao ham nay 
        }
        webSocket.onerror = function () {

        }
    }
    else {

        funcProcess("1");
    }




}

function iBkavCANativeAppValidateResult(hSig) {
    iPluginVerified = doVerify(ExtensionIsValidRadomString, hSig);
    //iPluginVerified = doVerify("aaa", hSig);
    iCheckPluginValid = true;
    if (iPluginVerified) {
        document.getElementById('hrSignedData').value = '1'; // OK
    }
    else {
        document.getElementById('hrSignedData').value = '3'; // NOT OK 
    }
    if (ExtensionIsValidJSCallback != undefined && ExtensionIsValidJSCallback != null) { // callback hay k?
        // callback ham
        ExtensionIsValidJSCallback(document.getElementById('hrSignedData').value);
    }
    else {
        var eventExtensionValid2 = document.createEvent('Event');
        eventExtensionValid2.initEvent(ExtensionIsValidEvent, true, true);
        document.dispatchEvent(eventExtensionValid2);
    }
}

function GenerateString() {
    var text = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    for (var i = 0; i < 6; i++) {
        text += possible.charAt(Math.floor(Math.random() * possible.length));
    }
    return text;
}

function BkavCAPluginValidate() {
    if (plugin().valid) {
        var version = plugin().GetVersion();
        if (version > "1.0.15") {
            ExtensionIsValidRadomString = GenerateString();
            var hSig = plugin().PluginValidate(ExtensionIsValidRadomString);
            iPluginVerified = doVerify(ExtensionIsValidRadomString, hSig);
        }
        else {
            iPluginVerified = true;
        }
    }
    return iPluginVerified;
}
function BkavCANativeAppValidate() {
    // ExtensionIsValidEvent = "aaa"; // dang test
    ExtensionIsValidRadomString = GenerateString();
    var dataInput = 'BkavCANativeAppValidate*' + ExtensionIsValidRadomString + '*' + BkavExtensionCallback;
    //var dataInput = 'BkavCANativeAppValidate*aaa*' + BkavExtensionCallback; // dang test
    objCallback.FunctionCallback = iBkavCANativeAppValidateResult;
    return ProcessData(dataInput);
}
var BkavPluginSigner = {
    SetPlguinCallback: function (iCallback) {
        iPluginCallback = iCallback;
    },
    SignXML: function (objXmlSigner) {
        if (!iPluginVerified) {
            console.log("Co loi trong viec xac thuc Plugin")
            return;
        }
        var objXml = new ObjXmlSigner();
        objXml = objXmlSigner;
        var dsSignature = "1";
        if (!objXml.DsSignature) {
            dsSignature = "0";
        }
        switch (objXml.SigningType) {
            case XML_SIGNING_TYPE.SIGN_XML_FILE:
                return plugin().SignXMLFile(objXml.PathFileInput, objXml.PathFileOutput, objXml.TagSigning, objXml.NodeToSign, objXml.TagSaveResult, objXml.SigningTime, objXml.CertificateSerialNumber, dsSignature, objXml.sig);
            case XML_SIGNING_TYPE.SIGN_XML_XPATH_FILTER:
                if (objXml.FunctionCallback != null) {
                    objCallback.FunctionCallback = objXml.FunctionCallback;
                    plugin().SignXMLBase64XPathCallback(objXml.Base64String, objXml.NameXPathFilter, objXml.TagSigning, objXml.NameIDTimeSignature, objXml.SigningTime, objXml.CertificateSerialNumber, dsSignature, BkavCAPluginJSCallback);
                    return;
                }
                else {
                    return plugin().SignXMLBase64XPath(objXml.Base64String, objXml.NameXPathFilter, objXml.TagSigning, objXml.NameIDTimeSignature, objXml.SigningTime, objXml.CertificateSerialNumber, dsSignature);
                }
            case XML_SIGNING_TYPE.SIGN_XML_DATA_LIST:
                if (objXml.FunctionCallback != null) {
                    objCallback.FunctionCallback = objXml.FunctionCallback;
                    xmlOut = plugin().SignXMLDataListCallback(objXml.Base64String, objXml.TagSigning, objXml.NodeToSign, objXml.TagSaveResult, objXml.SigningTime, objXml.CertificateSerialNumber, dsSignature, BkavCAPluginJSCallback);
                    return;
                }
                else {
                    return plugin().SignXMLDataList(objXml.Base64String, objXml.TagSigning, objXml.NodeToSign, objXml.TagSaveResult, objXml.SigningTime, objXml.CertificateSerialNumber, dsSignature);
                }
            default:
                if (objXml.FunctionCallback != null) {
                    objCallback.FunctionCallback = objXml.FunctionCallback;
                    xmlOut = plugin().SignXMLBase64Callback(objXml.Base64String, objXml.TagSigning, objXml.NodeToSign, objXml.TagSaveResult, objXml.SigningTime, objXml.CertificateSerialNumber, dsSignature, BkavCAPluginJSCallback);
                    return;
                }
                else {
                    return plugin().SignXMLBase64(objXml.Base64String, objXml.TagSigning, objXml.NodeToSign, objXml.TagSaveResult, objXml.SigningTime, objXml.CertificateSerialNumber, dsSignature);
                }
        }
        return "";
    },

    SignPDF: function (objPdf) {
        if (!iPluginVerified) {
            console.log("Co loi trong viec xac thuc Plugin")
            return;
        }
        switch (objPdf.SigningType) {
            case PDF_SIGNING_TYPE.SIGN_PDF_FILE:
                if (objPdf.FunctionCallback != null) { // file
                    objCallback.FunctionCallback = objPdf.FunctionCallback;
                    plugin().SignPDFFileCallback(objPdf.PathFileInput, objPdf.PathFileInput, objPdf.SigningTime, objPdf.CertificateSerialNumber, objPdf.Signer, BkavCAPluginJSCallback);
                    return;
                }
                else {
                    return plugin().SignPDFFile(objPdf.PathFileInput, objPdf.PathFileInput, objPdf.SigningTime, objPdf.CertificateSerialNumber, objPdf.Signer);
                }
            case PDF_SIGNING_TYPE.SIGN_PDF_DATA_LIST:
                if (objPdf.FunctionCallback != null) { // base64
                    objCallback.FunctionCallback = objPdf.FunctionCallback;
                    plugin().SignPDFDataListCallback(objPdf.Base64String, objPdf.SigningTime, objPdf.CertificateSerialNumber, objPdf.Signer, BkavCAPluginJSCallback);
                    return;
                }
                else {
                    return plugin().SignPDFDataList(objPdf.Base64String, objPdf.SigningTime, objPdf.CertificateSerialNumber, objPdf.Signer);
                }
            default:
                if (objPdf.FunctionCallback != null) { // base64
                    objCallback.FunctionCallback = objPdf.FunctionCallback;
                    plugin().SignPDFBase64Callback(objPdf.Base64String, objPdf.SigningTime, objPdf.CertificateSerialNumber, objPdf.Signer, BkavCAPluginJSCallback);
                    return;
                }
                else {
                    return plugin().SignPDFBase64(objPdf.Base64String, objPdf.SigningTime, objPdf.CertificateSerialNumber, objPdf.Signer);
                }
        }
    },

    SignOOXML: function (objOOXml) {
        if (!iPluginVerified) {
            console.log("Co loi trong viec xac thuc Plugin")
            return;
        }

        switch (objOOXml.SigningType) {
            case OOXML_SIGNING_TYPE.SIGN_OOXML_FILE:
                if (objOOXml.FunctionCallback != null) { // file
                    objCallback.FunctionCallback = objOOXml.FunctionCallback;
                    plugin().SignOOXMLFileCallback(objOOXml.PathFileInput, objOOXml.PathFileOut, objOOXml.CertificateSerialNumber, BkavCAPluginJSCallback);
                    return;
                }
                else {
                    return plugin().SignOOXMLFile(objOOXml.PathFileInput, objOOXml.PathFileOut, objOOXml.CertificateSerialNumber);
                }
            case OOXML_SIGNING_TYPE.SIGN_OOXML_DATA_LIST:
                if (objOOXml.FunctionCallback != null) { // base64
                    objCallback.FunctionCallback = objOOXml.FunctionCallback;
                    plugin().SignOOXMLDataListCallback(objOOXml.Base64String, objOOXml.CertificateSerialNumber, BkavCAPluginJSCallback);
                    return;
                }
                else {
                    return plugin().SignOOXMLDataList(objOOXml.Base64String, objOOXml.CertificateSerialNumber);
                }
            default:
                if (objOOXml.FunctionCallback != null) { // base64
                    objCallback.FunctionCallback = objOOXml.FunctionCallback;
                    plugin().SignOOXMLBase64Callback(objOOXml.Base64String, objOOXml.CertificateSerialNumber, BkavCAPluginJSCallback);
                    return;
                }
                else {
                    return plugin().SignOOXMLBase64(objOOXml.Base64String, objOOXml.CertificateSerialNumber);
                }
        }
    },

    VerifyXML: function (objVerifier) {
        if (objVerifier.VerifyType = VERIFY_TYPE.VERYFY_BASE64) {
            if (objVerifier.FunctionCallback != null) {
                objCallback.FunctionCallback = objVerifier.FunctionCallback;
                plugin().VerifyXMLCallback(objVerifier.Base64Signed, objVerifier.TimeCheck, BkavCAPluginJSCallback);
                return;
            }
            else {
                return plugin().VerifyXML(objVerifier.Base64Signed, objVerifier.TimeCheck);
            }
        } else {
            if (objVerifier.FunctionCallback != null) {
                objCallback.FunctionCallback = objVerifier.FunctionCallback;
                plugin().VerifyXMLCallback(objVerifier.PathFileInput, objVerifier.TimeCheck, BkavCAPluginJSCallback);
                return;
            }
            else {
                return plugin().VerifyXML(objVerifier.PathFileInput, objVerifier.TimeCheck);
            }
        }
    },

    VerifyOOXML: function (objVerifier) {
        if (objVerifier.VerifyType = VERIFY_TYPE.VERYFY_BASE64) {
            if (objVerifier.FunctionCallback != null) {
                objCallback.FunctionCallback = objVerifier.FunctionCallback;
                plugin().VerifyOOXMLCallback(objVerifier.Base64Signed, objVerifier.TimeCheck, BkavCAPluginJSCallback);
                return;
            }
            else {
                return plugin().VerifyOOXML(objVerifier.Base64Signed, objVerifier.TimeCheck);
            }
        } else {
            if (objVerifier.FunctionCallback != null) {
                objCallback.FunctionCallback = objVerifier.FunctionCallback;
                plugin().VerifyOOXMLCallback(objVerifier.PathFileInput, objVerifier.TimeCheck, BkavCAPluginJSCallback);
                return;
            }
            else {
                return plugin().VerifyOOXML(objVerifier.PathFileInput, objVerifier.TimeCheck);
            }
        }
    },
    VerifyPDF: function (objVerifier) {
        if (objVerifier.VerifyType = VERIFY_TYPE.VERYFY_BASE64) {
            if (objVerifier.FunctionCallback != null) {
                objCallback.FunctionCallback = objVerifier.FunctionCallback;
                plugin().VerifyPDFCallback(objVerifier.Base64Signed, objVerifier.TimeCheck, BkavCAPluginJSCallback);
                return;
            }
            else {
                return plugin().VerifyPDF(objVerifier.Base64Signed, objVerifier.TimeCheck);
            }
        } else {
            if (objVerifier.FunctionCallback != null) {
                objCallback.FunctionCallback = objVerifier.FunctionCallback;
                plugin().VerifyPDFCallback(objVerifier.PathFileInput, objVerifier.TimeCheck, BkavCAPluginJSCallback);
                return;
            }
            else {
                return plugin().VerifyPDF(objVerifier.PathFileInput, objVerifier.TimeCheck);
            }
        }
    },
    ReadPDFBase64ToText: function (pdfBase64) {
        try {
            return plugin().ReadPDFBase64(pdfBase64);
        } catch (e) {
            return "";
        }
    },

    GetCertIndex: function (CertificateSerialNumber) {
        return plugin().GetCertIndex(CertificateSerialNumber);
    },

    GetCertListByFilter: function (objFilter) {
        try {
            var objFilterCert = new ObjFilter();
            objFilterCert = objFilter;
            var isOnlyToken = '0', isPKCS11 = '0';
            if (objFilterCert.isOnlyCertFromToken) {
                isOnlyToken = '1';
            }
            if (objFilterCert.UsePKCS11) {
                isPKCS11 = '1';
            }

            if (objFilterCert.FunctionCallback != null) { // check callback
                objCallback.FunctionCallback = objFilterCert.FunctionCallback;
                if (objFilterCert.Filter == INFO_CERT_FILTER.CERTIFICATE_SERIAL_NUMBER) {
                    plugin().GetCertListByFilterCallback("SerialNumber", objFilterCert.Value, isPKCS11, isOnlyToken, BkavCAPluginJSCallback);
                }
                else if (objFilterCert.Filter == INFO_CERT_FILTER.CERTIFICATE_SUBJECT_CN) {
                    plugin().GetCertListByFilterCallback("SubjectCN", objFilterCert.Value, isPKCS11, isOnlyToken, BkavCAPluginJSCallback);
                }
                else if (objFilterCert.Filter == INFO_CERT_FILTER.CERTIFICATE_ISSUER_CN) {
                    plugin().GetCertListByFilterCallback("IssuerCN", objFilterCert.Value, isPKCS11, isOnlyToken, BkavCAPluginJSCallback);
                }
                else if (objFilterCert.Filter == INFO_CERT_FILTER.CERTIFICATE_VALIDTO) {
                    plugin().GetCertListByFilterCallback("ValidTo", objFilterCert.Value, isPKCS11, isOnlyToken, BkavCAPluginJSCallback);
                }
                else if (objFilterCert.Filter == INFO_CERT_FILTER.CERTIFICATE_VALIDFROM) {
                    plugin().GetCertListByFilterCallback("ValidFrom", objFilterCert.Value, isPKCS11, isOnlyToken, BkavCAPluginJSCallback);
                }
                else if (objFilterCert.Filter == INFO_CERT_FILTER.CERTIFICATE_TIMEVALID) {
                    plugin().GetCertListByFilterCallback("TimeValid", objFilterCert.Value, isPKCS11, isOnlyToken, BkavCAPluginJSCallback);
                }
                else {
                    plugin().GetCertListByFilterCallback("SerialNumber", "", isPKCS11, isOnlyToken, BkavCAPluginJSCallback);
                }
                return;
            }
            else {
                if (objFilterCert.Filter == INFO_CERT_FILTER.CERTIFICATE_SERIAL_NUMBER) {
                    return plugin().GetCertListByFilter("SerialNumber", objFilterCert.Value, isPKCS11, isOnlyToken);
                }
                else if (objFilterCert.Filter == INFO_CERT_FILTER.CERTIFICATE_SUBJECT_CN) {
                    return plugin().GetCertListByFilter("SubjectCN", objFilterCert.Value, isPKCS11, isOnlyToken);
                }
                else if (objFilterCert.Filter == INFO_CERT_FILTER.CERTIFICATE_ISSUER_CN) {
                    return plugin().GetCertListByFilter("IssuerCN", objFilterCert.Value, isPKCS11, isOnlyToken);
                }
                else if (objFilterCert.Filter == INFO_CERT_FILTER.CERTIFICATE_VALIDTO) {
                    return plugin().GetCertListByFilter("ValidTo", objFilterCert.Value, isPKCS11, isOnlyToken);
                }
                else if (objFilterCert.Filter == INFO_CERT_FILTER.CERTIFICATE_VALIDFROM) {
                    return plugin().GetCertListByFilter("ValidFrom", objFilterCert.Value, isPKCS11, isOnlyToken);
                }
                else if (objFilterCert.Filter == INFO_CERT_FILTER.CERTIFICATE_TIMEVALID) {
                    return plugin().GetCertListByFilter("TimeValid", objFilterCert.Value, isPKCS11, isOnlyToken);
                }
                else {

                    return plugin().GetCertListByFilter("SerialNumber", "", isPKCS11, isOnlyToken);
                }
            }

        } catch (e) {
            if (objFilterCert.FunctionCallback != null) {
                objCallback.FunctionCallback(e);
                return;
            }
            return e;
        }
    },

    ReadPDFFileToText: function (ptrPDF) {
        try {
            return plugin().ReadPDFFileToText(ptrPDF);
        } catch (e) {
            return "";
        }
    },

    ReadFormFieldsToText: function (ptrPDF) {
        try {
            return plugin().ReadFormFieldsToText(ptrPDF);
        } catch (e) {
            return "";
        }
    },

    FileBrowser: function (OPEN_FILE_FILTER) {
        try {
            if (OPEN_FILE_FILTER == 0) {
                return plugin().FileBrowser("XML");
            }
            else if (OPEN_FILE_FILTER == 1) {
                return plugin().FileBrowser("PDF");
            }
            else if (OPEN_FILE_FILTER == 2) {
                return plugin().FileBrowser("WORD");
            }
            else if (OPEN_FILE_FILTER == 3) {
                return plugin().FileBrowser("EXCEL");
            }
            else
                return plugin().FileBrowser("");
        } catch (e) {
            return "";
        }
    },

    FileBrowser: function (OPEN_FILE_FILTER, jsCallback) {
        try {
            if (jsCallback == undefined) {
                if (OPEN_FILE_FILTER == 0) {
                    return plugin().FileBrowser("XML");
                }
                else if (OPEN_FILE_FILTER == 1) {
                    return plugin().FileBrowser("PDF");
                }
                else if (OPEN_FILE_FILTER == 2) {
                    return plugin().FileBrowser("WORD");
                }
                else if (OPEN_FILE_FILTER == 3) {
                    return plugin().FileBrowser("EXCEL");
                }
                else
                    return plugin().FileBrowser("");
            }
            else {
                objCallback.FunctionCallback = jsCallback;
                if (OPEN_FILE_FILTER == 0) {
                    return plugin().FileBrowserCallback("XML", BkavCAPluginJSCallback);
                }
                else if (OPEN_FILE_FILTER == 1) {
                    return plugin().FileBrowserCallback("PDF", BkavCAPluginJSCallback);
                }
                else if (OPEN_FILE_FILTER == 2) {
                    return plugin().FileBrowserCallback("WORD", BkavCAPluginJSCallback);
                }
                else if (OPEN_FILE_FILTER == 3) {
                    return plugin().FileBrowserCallback("EXCEL", BkavCAPluginJSCallback);
                }
                else
                    return plugin().FileBrowserCallback("", BkavCAPluginJSCallback);
            }

        } catch (e) {
            if (jsCallback != undefined) {
                objCallback.FunctionCallback(e);
                return;
            }
            return e;
        }
    },

    CheckOCSP: function (objCert) {
        //var objCert=new ObjCertificate();
        try {
            if (objCert.CertificateBase64 != null && objCert.CertificateBase64.trim().length > 0) {
                if (objCert.FunctionCallback != null) {
                    objCallback.FunctionCallback = objCert.FunctionCallback;
                    plugin().CheckOCSPCallback(objCert.CertificateBase64, objCert.OcspUrl, objCert.TimeCheck, BkavCAPluginJSCallback);
                    return;
                }
                else {
                    return plugin().CheckOCSP(objCert.CertificateBase64, objCert.OcspUrl, objCert.TimeCheck);
                }
            }
            else {
                if (objCert.FunctionCallback != null) {
                    objCallback.FunctionCallback = objCert.FunctionCallback;
                    plugin().CheckOCSPBySerialCallback(objCert.CertificateSerialNumber, objCert.OcspUrl, objCert.TimeCheck, BkavCAPluginJSCallback);
                    return;
                }
                else {
                    return plugin().CheckOCSPBySerial(objCert.CertificateSerialNumber, objCert.OcspUrl, objCert.TimeCheck);
                }
            }
        } catch (e) {
            return "";
        }
    },
    CheckToken: function (serial, JSCallback) {
        if (JSCallback == undefined) {
            return plugin().CheckToken(serial);
        }
        else {
            objCallback.FunctionCallback = JSCallback;
            return plugin().CheckTokenCallback(serial, BkavCAPluginJSCallback);
        }
    },

    CheckCRL: function (objCert) {
        try {
            if (objCert.CertificateBase64 != null && objCert.CertificateBase64.trim().length > 0) {
                if (objCert.FunctionCallback != null) {
                    objCallback.FunctionCallback = objCert.FunctionCallback;
                    plugin().CheckCRLCallback(objCert.CertificateBase64, objCert.TimeCheck, BkavCAPluginJSCallback);
                    return;
                }
                else {
                    return plugin().CheckCRL(objCert.CertificateBase64, objCert.TimeCheck);
                }
            }
            else {
                if (objCert.FunctionCallback != null) {
                    objCallback.FunctionCallback = objCert.FunctionCallback;
                    plugin().CheckCRLCallback(objCert.CertificateSerialNumber, objCert.TimeCheck, BkavCAPluginJSCallback);
                    return;
                }
                else {
                    return plugin().CheckCRL(objCert.CertificateSerialNumber, objCert.TimeCheck);
                }
            }
        } catch (e) {
            return "";
        }
    },

    CheckValidTime: function (objCert) {
        //var objCert = new ObjCertificate();
        try {
            if (objCert.CertificateBase64 != null && objCert.CertificateBase64.trim().length > 0) {
                if (objCert.FunctionCallback != null) {
                    objCallback.FunctionCallback = objCert.FunctionCallback;
                    plugin().CheckValidTimeCallback(objCert.CertificateBase64, objCert.TimeCheck, BkavCAPluginJSCallback);
                    return;
                }
                else {
                    return plugin().CheckValidTime(objCert.CertificateBase64, objCert.TimeCheck);
                }
            }
            else {
                if (objCert.FunctionCallback != null) {
                    objCallback.FunctionCallback = objCert.FunctionCallback;
                    plugin().CheckValidTimeCallback(objCert.CertificateSerialNumber, objCert.TimeCheck, BkavCAPluginJSCallback);
                    return;
                }
                else {
                    return plugin().CheckValidTime(objCert.CertificateSerialNumber, objCert.TimeCheck);
                }
            }
        } catch (e) {
            return "";
        }
    },

    ValidateCertificate: function (objCert) {
        //var objCert=new ObjCertificate();
        var usePKCS11 = '0';
        if (objCert.TimeCheck == null || objCert.TimeCheck.trim().length == 0) {
            objCert.TimeCheck = "1";
        }
        if (objCert.CertificateBase64 != null && objCert.CertificateBase64.trim().length > 0) {
            if (objCert.FunctionCallback != null) {
                objCallback.FunctionCallback = objCert.FunctionCallback;
                plugin().ValidateCertificateCallback(objCert.CertificateBase64, objCert.TimeCheck, BkavCAPluginJSCallback);
                return;
            }
            else {
                return plugin().ValidateCertificate(objCert.CertificateBase64, objCert.TimeCheck);
            }
        }
        else {
            if (objCert.FunctionCallback != null) {
                objCallback.FunctionCallback = objCert.FunctionCallback;
                plugin().ValidateCertificateBySerialCallback(objCert.CertificateSerialNumber, objCert.TimeCheck, BkavCAPluginJSCallback);
                return;
            }
            else {
                return plugin().ValidateCertificateBySerial(objCert.CertificateSerialNumber, objCert.TimeCheck);
            }
        }
        return "";
    },

    SetAESKey: function (keyAES) {
        if (!iPluginVerified) {
            console.log("Error in verify the Plugin")
            return;
        }
        keyAES = keyAES + '*' + CreateKeyAES();
        plugin().SetAESKey(keyAES);
    },
    SetLicenseKey: function (license) {
        if (!iPluginVerified) {
            console.log("Error in verify the Plugin")
            return;
        }
        if (license != null && license.length > 0) {
            return plugin().SetLicenseKey(license);
        }
        else {
            console.log("The license is NULL, please check again!");
        }
    },
    SetUsePKCS11: function (SET_USE_PKCS11) {
        if (SET_USE_PKCS11 == 0) {
            plugin().SetUsePKCS11("0");
        }
        else {
            plugin().SetUsePKCS11("1");
        }
    },

    SetPINCache: function (oneSessiosPINCache, sessionsPINCache, secondPINCache) {
        var strOneSessionPINCache = '0';
        var strSessionsPINCache = '0';

        if (oneSessiosPINCache) {
            strOneSessionPINCache = '1';
        }
        if (sessionsPINCache) {
            strSessionsPINCache = '1';
        }
        return plugin().SetPINCache(strOneSessionPINCache, strSessionsPINCache, secondPINCache);
    },
    GetVersion: function () {
        return plugin().GetVersion();
    },
    SetDLLName: function (dllNameList) {
        plugin().SetDLLName(dllNameList);
    },
    PluginValid: function () {
        return iPluginVerified;
    },
    ConvertFileToBase64: function (pathFile) {
        if (pathFile == null || pathFile.trim().length == 0) {
            pathFile = "1";
        }
        return plugin().ConvertFileToBase64(pathFile);
    },
    ConvertFileToBase64: function (pathFile, jsCallback) {
        if (pathFile == null || pathFile.trim().length == 0) {
            pathFile = "1";
        }
        if (jsCallback == undefined) {
            return plugin().ConvertFileToBase64(pathFile);
        }
        objCallback.FunctionCallback = jsCallback;
        plugin().ConvertFileToBase64Callback(pathFile, BkavCAPluginJSCallback)
        return;
    },
    ChooserCertFromWindowStore: function (JSCallback) {
        if (JSCallback == undefined) {
            return plugin().ChooserCertFromWindowStore();
        }
        else {
            objCallback.FunctionCallback = JSCallback;
            plugin().ChooserCertFromWindowStoreCallback(BkavCAPluginJSCallback);
            return;
        }
    },
    SetGetAttributesCertDefault: function (iDefault) { // set get default
        return plugin().SetGetAttributesCertDefault(iDefault);
    },
    // 1-6-2016: TuanTAg CMS
    SignCMS: function (objCMS) {
        if (!iPluginVerified) {
            console.log("Co loi trong viec xac thuc Plugin")
            return;
        }
        if (objCMS.FunctionCallback != null) {
            objCallback.FunctionCallback = objCMS.FunctionCallback;
            plugin().SignCMSBase64Callback(objCMS.Base64String, objCMS.CertificateSerialNumber, BkavCAPluginJSCallback);
            return;
        }
        else {
            return plugin().SignCMSBase64(objCMS.Base64String, objCMS.CertificateSerialNumber);
        }
    },
    VerifyCMS: function (objVerifier) {
        if (!iPluginVerified) {
            console.log("Co loi trong viec xac thuc Plugin")
            return;
        }
        if (objVerifier.FunctionCallback != null) {
            objCallback.FunctionCallback = objVerifier.FunctionCallback;
            plugin().VerifyCMSBase64Callback(objVerifier.Base64Signed, objVerifier.TimeCheck, BkavCAPluginJSCallback);
            return;
        }
        else {
            return plugin().VerifyCMSBase64(objVerifier.Base64Signed, objVerifier.TimeCheck);
        }
    },

    // Ho tro version cu <1.0.10
    GetAllCert: function (filter, value, jsCallback) {
        if (jsCallback == undefined) {
            return plugin().GetAllSerial(filter, value);
        }
        objCallback.FunctionCallback = jsCallback;
        plugin().GetAllSerialCallback(filter, value, BkavCAPluginJSCallback);
        return;
    },
    ReadFileToBase64: function (pathFile) {
        if (pathFile == null || pathFile.trim().length == 0) {
            pathFile = "1";
        }
        return plugin().ConvertFileToBase64(pathFile);
    },
    ReadFileToBase64: function (pathFile, jsCallback) {
        if (pathFile == null || pathFile.trim().length == 0) {
            pathFile = "1";
        }
        if (jsCallback == undefined) {
            return plugin().ConvertFileToBase64(pathFile);
        }
        objCallback.FunctionCallback = jsCallback;
        plugin().ConvertFileToBase64Callback(pathFile, BkavCAPluginJSCallback);
        return;
    },
    SetCheckTokenDefault: function (iDefault) {
        return plugin().SetCheckTokenDefault(iDefault);
    },
    // 29-07
    SetHashAlgorithm: function (HASH_ALGORITHM) { //hash algorithm
        var dataInput = 0;
        if (HASH_ALGORITHM == 1) {
            dataInput = 1;
        }
        return plugin().SetHashAlgorithm(dataInput);
    },
    SetAddCertChain: function (ADD_CERTCHAIN) { //hash algorithm
        var dataInput = 0;
        if (ADD_CERTCHAIN == 1) {
            dataInput = 1;
        }
        return plugin().SetAddCertChain(dataInput);
    },
    SetAddBase64Cert: function (ADD_BASE64CERT) { //hash algorithm
        var dataInput = 0;
        if (ADD_BASE64CERT == 1) {
            dataInput = 1;
        }
        return plugin().SetAddBase64Cert(dataInput);
    },
    // 8-8
    //int BkavCAExtension::DetectToken (string serialNumber)
    DetectToken: function () {
        return plugin().DetectToken();
    },
    ImportCert: function (Base64P12, Password, UserPin) {
        if (UserPin == '' || UserPin == undefined) {
            return -1;
        }
        if (Password == '' || Password == undefined) {
            return -1;
        }
        if (Base64P12 == '' || Base64P12 == undefined) {
            return -1;
        }
        return plugin().ImportCert(Base64P12, Password, UserPin);
    },
    ChangePINToken: function (PIN_TYPE, OldPIN, NewPIN) {
        if (OldPIN == '' || OldPIN == undefined) {
            OldPIN = "1";
        }
        if (NewPIN == undefined) {
            NewPIN = "1";
        }
        var pinType = "0";
        if (PIN_TYPE == 1) {
            pinType = "1";
        }
        return plugin().ChangePINToken(pinType, OldPIN, NewPIN);
    },
    CheckLogin: function (PIN_TYPE, PIN) {
        if (PIN == '' || PIN == undefined) {
            PIN = "1";
        }
        var pinType = "0";
        if (PIN_TYPE == 1) {
            pinType = "1";
        }
        return plugin().CheckLogin(pinType, PIN);
    },
    GetTokenInfor: function () {
        return plugin().GetTokenInfor();
    },
    // 12/10
    SignDataList: function (objDataList) { // 11/10
        var dataInput = "";
        if (objDataList == null || objDataList == undefined)
            return "";
        var dsSignature = "1";
        var objXml = objDataList.objXmlSigner;
        var objPdf = objDataList.objPdfSiger;
        var objOOXml = objDataList.objOOxmlSigner;
        if (objXml == null || objXml == undefined || objPdf == null || objPdf == undefined || objOOXml == null || objOOXml == undefined)
            return "";
        if (!objXml.DsSignature) {
            dsSignature = "0";
        }
        if (objDataList.FunctionCallback != null) {
            objCallback.FunctionCallback = objDataList.FunctionCallback;
            xmlOut = plugin().SignDataListCallback(objDataList.Base64String, objXml.TagSigning + "*" + objXml.NodeToSign, objXml.TagSaveResult, objDataList.SigningTime, objDataList.CertificateSerialNumber, dsSignature, objPdf.Signer, BkavCAPluginJSCallback);
            return;
        }
        else {
            return plugin().SignDataList(objDataList.Base64String, objXml.TagSigning, objXml.NodeToSign, objXml.TagSaveResult, objDataList.SigningTime, objDataList.CertificateSerialNumber, dsSignature, objPdf.Signer);
        }
    },
    CallUpdateProcess: function () {
        return plugin().CallUpdateProcess();
    },
    GetDllPKCS11ListFound: function (jsCallback) {
        if (jsCallback != undefined && jsCallback != null) {
            objCallback.FunctionCallback = jsCallback;
            plugin().GetDllPKCS11ListFoundCallback(BkavCAPluginJSCallback);
        }
        return;
    }
};


/************************************************************************/
/* BkavExtension                                                        */
/************************************************************************/


// websoket vietpdb

var BkavCAPlugin = {
    /*
    * Sign XML Data
    */
    SignXML: function (objXml) {
        var dataInput = "";
        var dsSignature = "1";
        var fileToSign = objXml.PathFileInput;
        var fileSigned = objXml.PathFileOutput;
        var tagSigning = objXml.TagSigning;
        var nodeToSign = objXml.NodeToSign;
        var tagSaveResult = objXml.TagSaveResult;
        var timeSign = objXml.SigningTime;
        var serialToken = objXml.CertificateSerialNumber;
        var b64Xml = objXml.Base64String;
        var nameXPathFilter = objXml.NameXPathFilter;
        var nameIDTimeSignature = objXml.NameIDTimeSignature;
        var enableTimeStamp = objXml.EnableTimeStamp;

        //PhongNQ thêm signatureID
        var signatureId = objXml.SignatureID;
        if (objXml == null || objXml == undefined)
            return "";
        if (!objXml.DsSignature) {
            dsSignature = "0";
        }
        switch (objXml.SigningType) {
            case XML_SIGNING_TYPE.SIGN_XML_FILE:

                dataInput = FUNCTION_ID.SignXMLFileID + '*' + objXml.PathFileInput + '*' + objXml.PathFileOutput + '*' + objXml.TagSigning + '*' + objXml.NodeToSign + '*' +
                objXml.TagSaveResult + '*' + objXml.SigningTime + '*' + objXml.CertificateSerialNumber + '*' + dsSignature + '*' + objXml.SignatureID;

                break;
            case XML_SIGNING_TYPE.SIGN_XML_XPATH_FILTER:
                dataInput = FUNCTION_ID.SignXMLBase64XPathID + '*' + b64Xml + '*' + nameXPathFilter + '*' + tagSaveResult + '*' + nameIDTimeSignature + '*' + timeSign + '*' + serialToken + '*' + dsSignature;
                break;
            case XML_SIGNING_TYPE.SIGN_XML_DATA_LIST:
                dataInput = FUNCTION_ID.SignXMLDataListID + '*' + b64Xml + '*' + tagSigning + '*' + nodeToSign + '*' + tagSaveResult + '*' + timeSign + '*' + serialToken + '*' + dsSignature;
                break;
            default:
                dataInput = FUNCTION_ID.SignXMLBase64ID + '*' + b64Xml + '*' + tagSigning + '*' + nodeToSign + '*' + tagSaveResult + '*' + timeSign + '*' + serialToken + '*' + dsSignature;
                break;
        }
        if (objXml.FunctionCallback != null) {
            dataInput = dataInput + '*' + BkavExtensionCallback;
            objCallback.FunctionCallback = objXml.FunctionCallback;
        }
        else {
            return;
        }
        return ProcessData(dataInput);
    },
    //trong code sẽ tự động kiểm tra xem dữ liệu vào là base64 hay file và gọi đến các hàm xử lý tương ứng, không cần đặt ra 2 tên hàm xử lý base64 và file riêng.
    // Sign PDF Data
    SignPDF: function (objPdf) {
        var dataInput = "";
        if (objPdf == null || objPdf == undefined)
            return "";
        if (objPdf.CertificateSerialNumber == null || objPdf.CertificateSerialNumber.trim().length == 0) {
            objPdf.CertificateSerialNumber = "1";
        }
        switch (objPdf.SigningType) {
            case PDF_SIGNING_TYPE.SIGN_PDF_FILE:
                dataInput = FUNCTION_ID.SignPDFFileID + '*' + objPdf.PathFileInput + '*' + objPdf.PathFileOutput + '*' + objPdf.SigningTime + '*' + objPdf.CertificateSerialNumber + '*' + objPdf.Signer;
                break;
            case PDF_SIGNING_TYPE.SIGN_PDF_DATA_LIST:
                dataInput = FUNCTION_ID.SignPDFDataListID + '*' + objPdf.Base64String + '*' + objPdf.SigningTime + '*' + objPdf.CertificateSerialNumber + '*' + objPdf.Signer;
                break;
            default:
                dataInput = FUNCTION_ID.SignPDFBase64ID + '*' + objPdf.Base64String + '*' + objPdf.SigningTime + '*' + objPdf.CertificateSerialNumber + '*' + objPdf.Signer;
                break;
        }
        if (objPdf.FunctionCallback != null) {
            dataInput = dataInput + '*' + BkavExtensionCallback;
            objCallback.FunctionCallback = objPdf.FunctionCallback;
        }
        return ProcessData(dataInput);
    },

    //Sign Office Data
    SignOOXML: function (objOOXml) {
        var dataInput = "";
        if (objOOXml == null || objOOXml == undefined)
            return "";
        if (objOOXml.CertificateSerialNumber == null || objOOXml.CertificateSerialNumber.trim().length == 0) {
            objOOXml.CertificateSerialNumber = "1";
        }
        switch (objOOXml.SigningType) {
            case OOXML_SIGNING_TYPE.SIGN_OOXML_FILE:
                dataInput = FUNCTION_ID.SignOOXMLFileID + '*' + objOOXml.PathFileInput + '*' + objOOXml.PathFileOutput + '*' + objOOXml.CertificateSerialNumber;
                break;
            case OOXML_SIGNING_TYPE.SIGN_OOXML_DATA_LIST:
                dataInput = FUNCTION_ID.SignOOXMLDataListID + '*' + objOOXml.Base64String + '*' + objOOXml.CertificateSerialNumber;
                break;
            default:
                dataInput = FUNCTION_ID.SignOOXMLBase64ID + '*' + objOOXml.Base64String + '*' + objOOXml.CertificateSerialNumber;
                break;
        }
        if (objOOXml.FunctionCallback != null) {
            dataInput = dataInput + '*' + BkavExtensionCallback;
            objCallback.FunctionCallback = objOOXml.FunctionCallback;
        }
        return ProcessData(dataInput);
    },

    VerifyXML: function (objVerifier) {
        var dataInput = "";
        if (objVerifier.TimeCheck == null || objVerifier.TimeCheck.trim().length == 0) {
            objVerifier.TimeCheck = "1";
        }
        if (objVerifier.VerifyType = VERIFY_TYPE.VERYFY_BASE64) {
            dataInput = FUNCTION_ID.VerifyXMLID + '*' + objVerifier.Base64Signed + '*' + objVerifier.TimeCheck;
        } else {
            dataInput = FUNCTION_ID.VerifyXMLID + '*' + objVerifier.PathFileInput + '*' + objVerifier.TimeCheck;
        }
        if (objVerifier.FunctionCallback != null) {
            dataInput = dataInput + '*' + BkavExtensionCallback;
            objCallback.FunctionCallback = objVerifier.FunctionCallback;
        }
        return ProcessData(dataInput);
    },

    VerifyOOXML: function (objVerifier) {
        var dataInput = "";
        if (objVerifier.TimeCheck == null || objVerifier.TimeCheck.trim().length == 0) {
            objVerifier.TimeCheck = "1";
        }
        if (objVerifier.VerifyType = VERIFY_TYPE.VERYFY_BASE64) {
            dataInput = FUNCTION_ID.VerifyOOXMLID + '*' + objVerifier.Base64Signed + '*' + objVerifier.TimeCheck;
        }
        else {
            dataInput = FUNCTION_ID.VerifyOOXMLID + '*' + objVerifier.PathFileInput + '*' + objVerifier.TimeCheck;
        }
        if (objVerifier.FunctionCallback != null) {
            dataInput = dataInput + '*' + BkavExtensionCallback;
            objCallback.FunctionCallback = objVerifier.FunctionCallback;
        }
        return ProcessData(dataInput);
    },
    VerifyPDF: function (objVerifier) {
        var dataInput = "";
        if (objVerifier.TimeCheck == null || objVerifier.TimeCheck.trim().length == 0) {
            objVerifier.TimeCheck = "1";
        }
        if (objVerifier.VerifyType = VERIFY_TYPE.VERYFY_BASE64) {
            dataInput = FUNCTION_ID.VerifyPDFID + '*' + objVerifier.Base64Signed + '*' + objVerifier.TimeCheck;
        }
        else {
            dataInput = FUNCTION_ID.VerifyPDFID + '*' + objVerifier.PathFileInput + '*' + objVerifier.TimeCheck;
        }
        if (objVerifier.FunctionCallback != null) {
            dataInput = dataInput + '*' + BkavExtensionCallback;
            objCallback.FunctionCallback = objVerifier.FunctionCallback;
        }
        return ProcessData(dataInput);
    },

    //Hàm đọc nội dung base64pdf ra text
    ReadPDFBase64ToText: function (pdfBase64) {
        if (pdfBase64 == null || pdfBase64.trim().length == 0) {
            pdfBase64 = "1";
        }
        var dataInput = "";
        dataInput = FUNCTION_ID.ReadPDFBase64ToTextID + '*' + pdfBase64;
        return ProcessData(dataInput);
    },

    // Take cert infos based on Filter and Value parameter
    GetCertListByFilter: function (objFilter) {
        try {
            var dataInput = "";
            var usePKCS11 = '0'; isOnlyToken = '0';
            var objFilterCert = new ObjFilter();
            objFilterCert = objFilter;
            if (objFilterCert.Value == "") {
                objFilterCert.Value = "1";
            }
            if (objFilterCert.UsePKCS11) {
                usePKCS11 = '1';
            }
            if (objFilterCert.isOnlyCertFromToken) {
                isOnlyToken = '1';
            }

            if (objFilterCert.Filter == INFO_CERT_FILTER.CERTIFICATE_SERIAL_NUMBER) {
                dataInput = FUNCTION_ID.GetCertListByFilterID + '*SerialNumber*' + objFilterCert.Value + '*' + usePKCS11 + '*' + isOnlyToken;
            }
            else if (objFilterCert.Filter == INFO_CERT_FILTER.CERTIFICATE_SUBJECT_CN) {
                dataInput = FUNCTION_ID.GetCertListByFilterID + '*SubjectCN*' + objFilterCert.Value + '*' + usePKCS11 + '*' + isOnlyToken;
            }
            else if (objFilterCert.Filter == INFO_CERT_FILTER.CERTIFICATE_ISSUER_CN) {
                dataInput = FUNCTION_ID.GetCertListByFilterID + '*IssuerCN*' + objFilterCert.Value + '*' + usePKCS11 + '*' + isOnlyToken;
            }
            else if (objFilterCert.Filter == INFO_CERT_FILTER.CERTIFICATE_VALIDTO) {
                dataInput = FUNCTION_ID.GetCertListByFilterID + '*ValidTo*' + objFilterCert.Value + '*' + usePKCS11 + '*' + isOnlyToken;
            }
            else if (objFilterCert.Filter == INFO_CERT_FILTER.CERTIFICATE_VALIDFROM) {
                dataInput = FUNCTION_ID.GetCertListByFilterID + '*ValidFrom*' + objFilterCert.Value + '*' + usePKCS11 + '*' + isOnlyToken;
            }
            else if (objFilterCert.Filter == INFO_CERT_FILTER.CERTIFICATE_TIMEVALID) {
                dataInput = FUNCTION_ID.GetCertListByFilterID + '*TimeValid*' + objFilterCert.Value + '*' + usePKCS11 + '*' + isOnlyToken;
            }
            else {
                dataInput = FUNCTION_ID.GetCertListByFilterID + '*SerialNumber*1' + '*' + usePKCS11 + '*' + isOnlyToken;
            }
            if (objFilterCert.FunctionCallback != null) {
                dataInput = dataInput + '*' + BkavExtensionCallback;
                objCallback.FunctionCallback = objFilterCert.FunctionCallback;
            }
            return ProcessData(dataInput);
        } catch (e) {
            console.log(e);
        }
    },

    ReadPDFFileToText: function (pdfFile) {
        if (pdfFile == null || pdfFile.trim().length == 0) {
            pdfFile = "1";
        }
        var dataInput = "";
        dataInput = FUNCTION_ID.ReadPDFFileToTextID + '*' + pdfFile;
        return ProcessData(dataInput);
    },

    ReadFormFieldsToText: function (pdfFile) {
        if (pdfFile == null || pdfFile.trim().length == 0) {
            pdfFile = "1";
        }
        var dataInput = "";
        dataInput = FUNCTION_ID.ReadFormFieldsToTextID + '*' + pdfFile;
        return ProcessData(dataInput);
    },
    // Open a dialog and choose files
    FileBrowser: function (OPEN_FILE_FILTER, jsCallback) {
        var dataInput = "";
        dataInput = FUNCTION_ID.FileBrowserID + '*';
        if (jsCallback == undefined) {
            if (OPEN_FILE_FILTER == 0) {
                dataInput = dataInput + 'XML';
            }
            else if (OPEN_FILE_FILTER == 1) {
                dataInput = dataInput + 'PDF';
            }
            else if (OPEN_FILE_FILTER == 2) {
                dataInput = dataInput + 'WORD';
            }
            else if (OPEN_FILE_FILTER == 3) {
                dataInput = dataInput + 'EXCEL';
            }
            else
                dataInput = dataInput + '1';
        }
        else {
            objCallback.FunctionCallback = jsCallback;
            if (OPEN_FILE_FILTER == 0) {
                dataInput = dataInput + 'XML*' + BkavExtensionCallback;
            }
            else if (OPEN_FILE_FILTER == 1) {
                dataInput = dataInput + 'PDF*' + BkavExtensionCallback;
            }
            else if (OPEN_FILE_FILTER == 2) {
                dataInput = dataInput + 'WORD*' + BkavExtensionCallback;
            }
            else if (OPEN_FILE_FILTER == 3) {
                dataInput = dataInput + 'EXCEL*' + BkavExtensionCallback;
            }
            else
                dataInput = dataInput + '1*' + BkavExtensionCallback;
        }
        return ProcessData(dataInput);
    },
    // Check revoke status of cert through OCSP
    CheckOCSP: function (objCert) {
        //var objCert=new ObjCertificate();
        if (objCert.TimeCheck == null || objCert.TimeCheck.trim().length == 0) {
            objCert.TimeCheck = "1";
        }
        var dataInput = "";
        if (objCert.CertificateBase64 != null && objCert.CertificateBase64.trim().length > 0) {
            dataInput = FUNCTION_ID.CheckOCSPID + '*' + objCert.CertificateBase64 + '*' + objCert.OcspUrl + '*' + objCert.TimeCheck;
        }
        else {
            dataInput = FUNCTION_ID.CheckOCSPBySerialID + '*' + objCert.CertificateSerialNumber + '*' + objCert.OcspUrl + '*' + objCert.TimeCheck;
        }
        if (objCert.FunctionCallback != null) {
            dataInput = dataInput + '*' + BkavExtensionCallback;
            objCallback.FunctionCallback = objCert.FunctionCallback;
        }
        return ProcessData(dataInput);
    },

    // Check revoke status of cert
    CheckCRL: function (objCert) {
        var dataInput = "";
        if (objCert.TimeCheck == null || objCert.TimeCheck.trim().length == 0) {
            objCert.TimeCheck = "1";
        }
        if (objCert.CertificateBase64 != null && objCert.CertificateBase64.trim().length > 0) {
            dataInput = FUNCTION_ID.CheckCRLID + '*' + objCert.CertificateBase64 + '*' + objCert.TimeCheck;
        }
        else {
            dataInput = FUNCTION_ID.CheckCRLID + '*' + objCert.CertificateSerialNumber + '*' + objCert.TimeCheck;
        }
        if (objCert.FunctionCallback != null) {
            dataInput = dataInput + '*' + BkavExtensionCallback;
            objCallback.FunctionCallback = objCert.FunctionCallback;
        }
        return ProcessData(dataInput);
    },

    // Check valid time of cert
    CheckValidTime: function (objCert) {
        //var objCert = new ObjCertificate();
        if (objCert.TimeCheck == null || objCert.TimeCheck.trim().length == 0) {
            objCert.TimeCheck = "1";
        }
        var dataInput = "";
        if (objCert.CertificateBase64 != null && objCert.CertificateBase64.trim().length > 0) {
            dataInput = FUNCTION_ID.CheckValidTimeID + '*' + objCert.CertificateBase64 + '*' + objCert.TimeCheck;
        }
        else {
            dataInput = FUNCTION_ID.CheckValidTimeID + '*' + objCert.CertificateSerialNumber + '*' + objCert.TimeCheck;
        }
        if (objCert.FunctionCallback != null) {
            dataInput = dataInput + '*' + BkavExtensionCallback;
            objCallback.FunctionCallback = objCert.FunctionCallback;
        }
        return ProcessData(dataInput);
    },

    // Check if user log-ins by token or not, token has truth cert or not
    CheckToken: function (CertificateSerialNumber, JSCallback) {
        var dataInput = "";
        if (CertificateSerialNumber == null || CertificateSerialNumber.trim().length == 0) {
            CertificateSerialNumber = "1";
        }

        if (JSCallback == undefined) {
            alert("Not found function callback");
            return;
        }
        else {
            dataInput = FUNCTION_ID.CheckTokenID + '*' + CertificateSerialNumber + '*' + BkavExtensionCallback;
            objCallback.FunctionCallback = JSCallback;
        }
        return ProcessData(dataInput);
    },

    //PdfUtils
    //install AES key to encrypt and decrypt PIN memorized by user
    SetAESKey: function (keyAES) {
        keyAES = keyAES + '*' + CreateKeyAES();
        var dataInput = "";
        dataInput = FUNCTION_ID.SetAESKeyID + '*' + keyAES;
        return ProcessData(dataInput);
    },
    // install and sign pkcs11
    SetUsePKCS11: function (SET_USE_PKCS11) {
        var dataInput = "";
        if (SET_USE_PKCS11 == 0) {
            dataInput = FUNCTION_ID.SetUsePKCS11ID + '*0';
        }
        else {
            dataInput = FUNCTION_ID.SetUsePKCS11ID + '*1';
        }
        return ProcessData(dataInput);
    },
    // install dll pkcs11 list of providers. For instance: BkavCA.dll  ... 
    // Use this function before CheckToken and SetUsePKCS11 functions
    SetDLLName: function (dllNameList) {
        var dataInput = "";
        if (dllNameList == null || dllNameList.trim().length == 0) {
            dllNameList = "1";
        }
        var dataInput = FUNCTION_ID.SetDLLNameID + '*' + dllNameList;
        return ProcessData(dataInput);
    },
    ConvertFileToBase64: function (pathFile, jsCallback) {
        if (pathFile == null || pathFile.trim().length == 0) {
            pathFile = "1";
        }
        var dataInput = "";
        dataInput = FUNCTION_ID.ConvertFileToBase64ID + '*' + pathFile;
        if (jsCallback != undefined) {
            objCallback.FunctionCallback = jsCallback;
            dataInput = dataInput + '*' + BkavExtensionCallback;
        }
        else {

        }
        return ProcessData(dataInput);
    },
    GetCertIndex: function (CertificateSerialNumber) {
        if (CertificateSerialNumber == null || CertificateSerialNumber.trim().length == 0) {
            CertificateSerialNumber = "1";
        }
        var dataInput = "";
        dataInput = FUNCTION_ID.GetCertIndexID + '*' + CertificateSerialNumber;
        return ProcessData(dataInput);
    },

    // setup license to use software 
    SetLicenseKey: function (license) {
        var dataInput = "";
        dataInput = FUNCTION_ID.SetLicenseKeyID + '*' + license;
        return ProcessData(dataInput);
    },

    // check activity status of plugin on chrome browser
    ExtensionValid: function () {
        var dataInput = "";
        dataInput = FUNCTION_ID.ExtensionValidID + '*1';
        return ProcessData(dataInput);
    },

    //GetAllExtensions, GetSelfExtension, GetExtensionWithID

    // Cac ham ho tro version cu 1.0:
    GetAllCert: function (filter, value, jsCallback) {
        if (jsCallback == undefined) {
            return;
        }
        else {
            var dataInput = "";
            dataInput = FUNCTION_ID.GetAllCertID + '*' + filter + '*' + value + "*" + BkavExtensionCallback;
            objCallback.FunctionCallback = jsCallback;
        }

        return ProcessData(dataInput);

    },

    //ReadFileToBase64, SignOffice, SignOfficeBase64, SignXMLBase64

    // New: Sign CMS Data
    SignCMS: function (objCMS) {
        var dataInput = "";
        if (objCMS == null || objCMS == undefined)
            return "";
        if (objCMS.CertificateSerialNumber == null || objCMS.CertificateSerialNumber.trim().length == 0) {
            objCMS.CertificateSerialNumber = "1";
        }
        dataInput = FUNCTION_ID.SignCMSBase64ID + '*' + objCMS.Base64String + '*' + objCMS.CertificateSerialNumber;
        if (objCMS.FunctionCallback != null) {
            dataInput = dataInput + '*' + BkavExtensionCallback;
            objCallback.FunctionCallback = objCMS.FunctionCallback;
        }
        return ProcessData(dataInput);
    },
    // New: Verify XML data with parameter path of file or xml string encoded by base64
    VerifyCMS: function (objVerifier) {
        var dataInput = "";
        if (objVerifier.TimeCheck == null || objVerifier.TimeCheck.trim().length == 0) {
            objVerifier.TimeCheck = "1";
        }
        dataInput = FUNCTION_ID.VerifyCMSID + '*' + objVerifier.Base64Signed + '*' + objVerifier.TimeCheck;
        if (objVerifier.FunctionCallback != null) {
            dataInput = dataInput + '*' + BkavExtensionCallback;
            objCallback.FunctionCallback = objVerifier.FunctionCallback;
        }
        return ProcessData(dataInput);
    },

    //New: Check validity of cert
    ValidateCertificate: function (objCert) {
        //var objCert=new ObjCertificate();
        var usePKCS11 = '0';
        if (objCert.TimeCheck == null || objCert.TimeCheck.trim().length == 0) {
            objCert.TimeCheck = "1";
        }
        var dataInput = "";
        dataInput = FUNCTION_ID.ValidateCertificateID + '*' + objCert.CertificateBase64 + '*' + objCert.CertificateSerialNumber + '*' + objCert.TimeCheck;
        if (objCert.FunctionCallback != null) {
            dataInput = dataInput + '*' + BkavExtensionCallback;
            objCallback.FunctionCallback = objCert.FunctionCallback;
        }
        return ProcessData(dataInput);
    },

    //New: Set PIN Cache of Token
    SetPINCache: function (oneSessiosPINCache, sessionsPINCache, secondPINCache) {
        var strOneSessionPINCache = '0';
        var strSessionsPINCache = '0';

        if (oneSessiosPINCache) {
            strOneSessionPINCache = '1';
        }
        if (sessionsPINCache) {
            strSessionsPINCache = '1';
        }
        var dataInput = "";
        dataInput = FUNCTION_ID.SetPINCacheID + '*' + strOneSessionPINCache + '*' + strSessionsPINCache + '*' + secondPINCache;
        return ProcessData(dataInput);
    },

    //New: Get version of software
    GetVersion: function () {
        var dataInput = "";
        dataInput = FUNCTION_ID.GetVersionID + '*1';
        return ProcessData(dataInput)
    },

    // New:
    SetGetAttributesCertDefault: function (iDefault) {
        var dataInput = "";
        dataInput = FUNCTION_ID.SetGetAttributesCertDefaultID + '*' + iDefault;
        return ProcessData(dataInput);
    },

    //New: display cert lists in windows store for user
    ChooserCertFromWindowStore: function (JSCallback) {
        var dataInput = "";
        if (JSCallback == undefined) {
            return;
        }
        else {
            dataInput = FUNCTION_ID.ChooserCertFromWindowStoreID + '*' + BkavExtensionCallback;
            objCallback.FunctionCallback = JSCallback;
        }
        return ProcessData(dataInput);
    },

    // New:
    SetCheckTokenDefault: function (iDefault) {
        var dataInput = "";
        dataInput = FUNCTION_ID.SetCheckTokenDefaultID + '*0';
        if (iDefault == 1) {
            dataInput = FUNCTION_ID.SetCheckTokenDefaultID + '*1';
        }
        return ProcessData(dataInput);
    },
    // New:
    SetHashAlgorithm: function (HASH_ALGORITHM) { //hash algorithm
        var dataInput = "";
        dataInput = FUNCTION_ID.SetHashAlgorithmID + '*0';
        if (HASH_ALGORITHM == 1) {
            dataInput = FUNCTION_ID.SetHashAlgorithmID + '*1';
        }
        return ProcessData(dataInput);
    },

    // New:
    SetAddCertChain: function (ADD_CERTCHAIN) { //hash algorithm
        var dataInput = "";
        dataInput = FUNCTION_ID.SetAddCertChainID + '*0';
        if (ADD_CERTCHAIN == 1) {
            dataInput = FUNCTION_ID.SetAddCertChainID + '*1'
        }
        return ProcessData(dataInput);
    },

    // New:
    SetAddBase64Cert: function (ADD_BASE64CERT) { //hash algorithm
        var dataInput = "";
        dataInput = FUNCTION_ID.SetAddBase64CertID + '*0'
        if (ADD_BASE64CERT == 1) {
            dataInput = FUNCTION_ID.SetAddBase64CertID + '*0'
        }
        return ProcessData(dataInput);
    },
    // 8-8
    //int BkavCAExtension::DetectToken (string serialNumber)
    DetectToken: function (JSCallback) {
        var dataInput = "";
        dataInput = FUNCTION_ID.DetectTokenID + '*0'
        if (JSCallback != undefined && JSCallback != "") {
            dataInput = dataInput + '*' + BkavExtensionCallback;
            objCallback.FunctionCallback = JSCallback;
        }
        else {
            return;
        }
        return ProcessData(dataInput);
    },
    ImportCert: function (Base64P12, Password, UserPin, JSCallback) {
        var dataInput = "";
        dataInput = FUNCTION_ID.ImportCertID;
        if (UserPin == '' || UserPin == undefined) {
            UserPin = "1";
        }
        if (Password == '' || Password == undefined) {
            Password = "1";
        }
        if (Base64P12 == '' || Base64P12 == undefined) {
            Base64P12 = "1";
        }
        dataInput = dataInput + '*' + Base64P12 + '*' + Password + '*' + UserPin;
        if (JSCallback != undefined) {
            dataInput = dataInput + '*' + BkavExtensionCallback;
            objCallback.FunctionCallback = JSCallback;
        }
        else {
            return;
        }
        return ProcessData(dataInput);
    },
    ChangePINToken: function (PIN_TYPE, OldPIN, NewPIN, JSCallback) {
        var dataInput = "";
        dataInput = FUNCTION_ID.ChangePINTokenID;
        if (OldPIN == '' || OldPIN == undefined) {
            OldPIN = "1";
        }
        var pinType = "0";
        if (PIN_TYPE == 1) {
            pinType = "1";
        }
        dataInput = dataInput + '*' + pinType + '*' + OldPIN + '*' + NewPIN;
        if (JSCallback != undefined) {
            dataInput = dataInput + '*' + BkavExtensionCallback;
            objCallback.FunctionCallback = JSCallback;
        }
        else {
            return;
        }
        return ProcessData(dataInput);
    },
    CheckLogin: function (PIN_TYPE, PIN, JSCallback) {
        var dataInput = "";
        dataInput = FUNCTION_ID.CheckLoginID;
        if (PIN == '' || PIN == undefined) {
            PIN = "1";
        }
        var pinType = "0";
        if (PIN_TYPE == 1) {
            pinType = "1";
        }
        dataInput = dataInput + '*' + pinType + '*' + PIN;
        if (JSCallback != undefined) {
            dataInput = dataInput + '*' + BkavExtensionCallback;
            objCallback.FunctionCallback = JSCallback;
        }
        return ProcessData(dataInput);
    },
    GetTokenInfor: function (JSCallback) {
        var dataInput = "";
        dataInput = FUNCTION_ID.GetTokenInforID + '*1';
        if (JSCallback != undefined) {
            dataInput = dataInput + '*' + BkavExtensionCallback;
            objCallback.FunctionCallback = JSCallback;
        }
        else
            return;
        return ProcessData(dataInput);
    },
    SignDataList: function (objDataList) { // 11/10
        var dataInput = "";
        if (objDataList == null || objDataList == undefined)
            return "";
        var dsSignature = "1";
        var objXml = objDataList.objXmlSigner;
        var objPdf = objDataList.objPdfSiger;
        var objOOXml = objDataList.objOOxmlSigner;
        if (objXml == null || objXml == undefined || objPdf == null || objPdf == undefined || objOOXml == null || objOOXml == undefined)
            return "";
        if (!objXml.DsSignature) {
            dsSignature = "0";
        }
        dataInput = FUNCTION_ID.GetTokenInforID + '*' + objDataList.Base64String + '*' + objXml.TagSigning + '*' + objXml.NodeToSign + '*' + objXml.TagSaveResult + '*' + objDataList.SigningTime + '*' + objDataList.CertificateSerialNumber + '*' + dsSignature + '*' + objPdf.Signer;
        if (objDataList.FunctionCallback != null) {
            dataInput = dataInput + '*' + BkavExtensionCallback;
            objCallback.FunctionCallback = objDataList.FunctionCallback;
        }
        return ProcessData(dataInput);
    },
    Connect: function (funcProcessCallback) {

        tryConnect(port, funcProcessCallback);

    },
    ExtensionIsValid: function (jsCallback) {
        var dataInput = 'ExtensionValidVer2*1'; // chua co ham nay trong core
        iCheckPluginValid = false;
        iCallback = false;
        var eventExtensionValid = document.createEvent('Event');
        document.getElementById('hrSignedData').value = '';
        var checkExtension = document.getElementById(idExtension);
        if (checkExtension == null) { // chua cai Extension
            if (jsCallback != undefined) { // callback hay k?
                jsCallback("0");
            }
            else {
                document.getElementById('hrSignedData').value = '0';
                eventExtensionValid.initEvent(ExtensionIsValidEvent, true, true);
                document.dispatchEvent(eventExtensionValid);
            }
        }
        else { // da cai Ex => check tiep App
            if (jsCallback != undefined) { // callback hay k?
                ExtensionIsValidJSCallback = jsCallback;
            }
            setTimeout(BkavCASignerPluginValidCallBack, 10000); // sau 5.5s goi lai ham callback: can check lai thoi gian thuc te
            return ProcessData(dataInput);
        }
    }
};
/*---------------------------------------------------------*/



var BkavExtensionSigner = {
    SetPlguinCallback: function (iCallback) {
        iPluginCallback = iCallback;
    },
    Connect: function (funcProcessCallback) {
        if (iCheckPluginS == 1) {
            tryConnect(port, funcProcessCallback);
        } else {
            funcProcessCallback("0");
        }
    },
    SignXML: function (objXml) {
        if (iCheckPluginS == 1) {
            BkavPlugin.SignXML(objXml);

        }
        else {
            var dataInput = "";
            var dsSignature = "";
            var fileToSign = objXml.PathFileInput;
            var fileSigned = objXml.PathFileOutput;
            var tagSigning = objXml.TagSigning;
            var nodeToSign = objXml.NodeToSign;
            var tagSaveResult = objXml.TagSaveResult;
            var timeSign = objXml.SigningTime;
            var serialToken = objXml.CertificateSerialNumber;
            var b64Xml = objXml.Base64String;
            var nameXPathFilter = objXml.NameXPathFilter;
            var nameIDTimeSignature = objXml.NameIDTimeSignature;
            var enableTimeStamp = objXml.EnableTimeStamp;

            //PhongNQ thêm signatureID
            var signatureId = objXml.SignatureID;

            if (objXml == null || objXml == undefined)
                return "";
            if (serialToken == null || serialToken == 0) {
                serialToken = "1";
            }
            if (!objXml.DsSignature) {
                dsSignature = "0";
            }
            switch (objXml.SigningType) {
                case XML_SIGNING_TYPE.SIGN_XML_FILE:
                    dataInput = 'SignXMLFile*' + fileToSign + '*' + fileSigned + '*' + tagSigning + '*' + nodeToSign + '*' + tagSaveResult + '*' + timeSign + '*' + serialToken + '*' + dsSignature + '*' + signatureId + '*' + enableTimeStamp;
                    break;
                case XML_SIGNING_TYPE.SIGN_XML_XPATH_FILTER:
                    dataInput = 'SignXMLBase64XPath*' + b64Xml + '*' + nameXPathFilter + '*' + tagSaveResult + '*' + nameIDTimeSignature + '*' + timeSign + '*' + serialToken + '*' + dsSignature;
                    break;
                case XML_SIGNING_TYPE.SIGN_XML_DATA_LIST:
                    dataInput = 'SignXMLDataList*' + b64Xml + '*' + tagSigning + '*' + nodeToSign + '*' + tagSaveResult + '*' + timeSign + '*' + serialToken + '*' + dsSignature;
                    break;
                default:
                    dataInput = 'SignXMLBase64*' + b64Xml + '*' + tagSigning + '*' + nodeToSign + '*' + tagSaveResult + '*' + timeSign + '*' + serialToken + '*' + dsSignature;
                    break;
            }
            if (objXml.FunctionCallback != null) {
                dataInput = dataInput + '*' + BkavExtensionCallback;
                objCallback.FunctionCallback = objXml.FunctionCallback;
            }

            console.log(dataInput);
            return ProcessData(dataInput);
        }

    },
    //trong code sẽ tự động kiểm tra xem dữ liệu vào là base64 hay file và gọi đến các hàm xử lý tương ứng, không cần đặt ra 2 tên hàm xử lý base64 và file riêng.
    SignPDF: function (objPdf) {
        if (iCheckPluginS == 1) {
            BkavPlugin.SignPDF(objPdf);
        }
        else {
            var dataInput = "";
            if (objPdf == null || objPdf == undefined)
                return "";
            if (objPdf.CertificateSerialNumber == null || objPdf.CertificateSerialNumber.trim().length == 0) {
                objPdf.CertificateSerialNumber = "1";
            }
            switch (objPdf.SigningType) {
                case PDF_SIGNING_TYPE.SIGN_PDF_FILE:
                    dataInput = 'SignPDFFile*' + objPdf.PathFileInput + '*' + objPdf.PathFileOutput + '*' + objPdf.SigningTime + '*' + objPdf.CertificateSerialNumber + '*' + objPdf.Signer;
                    break;
                case PDF_SIGNING_TYPE.SIGN_PDF_DATA_LIST:
                    dataInput = "SignPDFDataList*" + objPdf.Base64String + '*' + objPdf.SigningTime + '*' + objPdf.CertificateSerialNumber + '*' + objPdf.Signer;
                    break;
                default:
                    dataInput = "SignPDFBase64*" + objPdf.Base64String + '*' + objPdf.SigningTime + '*' + objPdf.CertificateSerialNumber + '*' + objPdf.Signer;
                    break;
            }
            if (objPdf.FunctionCallback != null) {
                dataInput = dataInput + '*' + BkavExtensionCallback;
                objCallback.FunctionCallback = objPdf.FunctionCallback;
            }
            return ProcessData(dataInput);
        }

    },

    SignOOXML: function (objOOXml) {
        if (iCheckPluginS == 1) {
            BkavPlugin.SignOOXML(objOOXml);
        }
        else {
            var dataInput = "";
            if (objOOXml == null || objOOXml == undefined)
                return "";
            if (objOOXml.CertificateSerialNumber == null || objOOXml.CertificateSerialNumber.trim().length == 0) {
                objOOXml.CertificateSerialNumber = "1";
            }
            switch (objOOXml.SigningType) {
                case OOXML_SIGNING_TYPE.SIGN_OOXML_FILE:
                    dataInput = "SignOOXMLFile*" + objOOXml.PathFileInput + '*' + objOOXml.PathFileOutput + '*' + objOOXml.CertificateSerialNumber;
                    break;
                case OOXML_SIGNING_TYPE.SIGN_OOXML_DATA_LIST:
                    dataInput = "SignOOXMLDataList*" + objOOXml.Base64String + '*' + objOOXml.CertificateSerialNumber;
                    break;
                default:
                    dataInput = "SignOOXMLBase64*" + objOOXml.Base64String + '*' + objOOXml.CertificateSerialNumber;
                    break;
            }
            if (objOOXml.FunctionCallback != null) {
                dataInput = dataInput + '*' + BkavExtensionCallback;
                objCallback.FunctionCallback = objOOXml.FunctionCallback;
            }
            return ProcessData(dataInput);
        }

    },
    //TuanTAg: 1-6-2016: CMS
    SignCMS: function (objCMS) {
        if (iCheckPluginS == 1) {
            BkavPlugin.SignCMS(objCMS);
        }
        else {
            var dataInput = "";
            if (objCMS == null || objCMS == undefined)
                return "";
            if (objCMS.CertificateSerialNumber == null || objCMS.CertificateSerialNumber.trim().length == 0) {
                objCMS.CertificateSerialNumber = "1";
            }
            dataInput = "SignCMSBase64*" + objCMS.Base64String + '*' + objCMS.CertificateSerialNumber;
            if (objCMS.FunctionCallback != null) {
                dataInput = dataInput + '*' + BkavExtensionCallback;
                objCallback.FunctionCallback = objCMS.FunctionCallback;
            }
            return ProcessData(dataInput);
        }

    },
    VerifyCMS: function (objVerifier) {
        if (iCheckPluginS == 1) {
            BkavPlugin.VerifyCMS(objVerifier);
        }
        else {
            var dataInput = "";
            if (objVerifier.TimeCheck == null || objVerifier.TimeCheck.trim().length == 0) {
                objVerifier.TimeCheck = "1";
            }
            dataInput = "VerifyCMS*" + objVerifier.Base64Signed + '*' + objVerifier.TimeCheck;
            if (objVerifier.FunctionCallback != null) {
                dataInput = dataInput + '*' + BkavExtensionCallback;
                objCallback.FunctionCallback = objVerifier.FunctionCallback;
            }
            return ProcessData(dataInput);
        }

    },
    VerifyXML: function (objVerifier) {
        if (iCheckPluginS == 1) {
            BkavPlugin.VerifyXML(objVerifier);
        }
        else {
            var dataInput = "";
            if (objVerifier.TimeCheck == null || objVerifier.TimeCheck.trim().length == 0) {
                objVerifier.TimeCheck = "1";
            }
            if (objVerifier.VerifyType = VERIFY_TYPE.VERYFY_BASE64) {
                dataInput = "VerifyXML*" + objVerifier.Base64Signed + '*' + objVerifier.TimeCheck;
            } else {
                dataInput = "VerifyXML*" + objVerifier.PathFileInput + '*' + objVerifier.TimeCheck;
            }
            if (objVerifier.FunctionCallback != null) {
                dataInput = dataInput + '*' + BkavExtensionCallback;
                objCallback.FunctionCallback = objVerifier.FunctionCallback;
            }
            return ProcessData(dataInput);
        }

    },

    VerifyOOXML: function (objVerifier) {
        if (iCheckPluginS == 1) {
            BkavPlugin.VerifyOOXML(objVerifier);
        }
        else {
            var dataInput = "";
            if (objVerifier.TimeCheck == null || objVerifier.TimeCheck.trim().length == 0) {
                objVerifier.TimeCheck = "1";
            }
            if (objVerifier.VerifyType = VERIFY_TYPE.VERYFY_BASE64) {
                dataInput = "VerifyOOXML*" + objVerifier.Base64Signed + '*' + objVerifier.TimeCheck;
            }
            else {
                dataInput = "VerifyOOXML*" + objVerifier.PathFileInput + '*' + objVerifier.TimeCheck;
            }
            if (objVerifier.FunctionCallback != null) {
                dataInput = dataInput + '*' + BkavExtensionCallback;
                objCallback.FunctionCallback = objVerifier.FunctionCallback;
            }
            return ProcessData(dataInput);
        }

    },
    VerifyPDF: function (objVerifier) {
        if (iCheckPluginS == 1) {
            BkavPlugin.VerifyPDF(objVerifier);
        }
        else {
            var dataInput = "";
            if (objVerifier.TimeCheck == null || objVerifier.TimeCheck.trim().length == 0) {
                objVerifier.TimeCheck = "1";
            }
            if (objVerifier.VerifyType = VERIFY_TYPE.VERYFY_BASE64) {
                dataInput = "VerifyPDF*" + objVerifier.Base64Signed + '*' + objVerifier.TimeCheck;
            }
            else {
                dataInput = "VerifyPDF*" + objVerifier.PathFileInput + '*' + objVerifier.TimeCheck;
            }
            if (objVerifier.FunctionCallback != null) {
                dataInput = dataInput + '*' + BkavExtensionCallback;
                objCallback.FunctionCallback = objVerifier.FunctionCallback;
            }
            return ProcessData(dataInput);
        }

    },

    //Utils function

    ReadPDFBase64ToText: function (pdfBase64) {
        if (iCheckPluginS == 1) {
            BkavPlugin.ReadPDFBase64ToText(pdfBase64);
        }
        else {
            if (pdfBase64 == null || pdfBase64.trim().length == 0) {
                pdfBase64 = "1";
            }
            var dataInput = 'ReadPDFBase64ToText*' + pdfBase64;
            return ProcessData(dataInput);
        }

    },

    GetCertListByFilter: function (objFilter) {
        try {
            if (iCheckPluginS == 1) {
                BkavPlugin.GetCertListByFilter(objFilter);
            }
            else {
                var dataInput, usePKCS11 = '0';
                var isOnlyToken = '0';
                var objFilterCert = new ObjFilter();
                objFilterCert = objFilter;
                if (objFilterCert.Value == "") {
                    objFilterCert.Value = "1";
                }
                if (objFilterCert.UsePKCS11) {
                    usePKCS11 = '1';
                }
                if (objFilterCert.isOnlyCertFromToken) {
                    isOnlyToken = '1';
                }

                if (objFilterCert.Filter == INFO_CERT_FILTER.CERTIFICATE_SERIAL_NUMBER) {
                    dataInput = "GetCertListByFilter*SerialNumber*" + objFilterCert.Value + '*' + usePKCS11 + '*' + isOnlyToken;
                }
                else if (objFilterCert.Filter == INFO_CERT_FILTER.CERTIFICATE_SUBJECT_CN) {
                    dataInput = "GetCertListByFilter*SubjectCN*" + objFilterCert.Value + '*' + usePKCS11 + '*' + isOnlyToken;
                }
                else if (objFilterCert.Filter == INFO_CERT_FILTER.CERTIFICATE_ISSUER_CN) {
                    dataInput = "GetCertListByFilter*IssuerCN*" + objFilterCert.Value + '*' + usePKCS11 + '*' + isOnlyToken;
                }
                else if (objFilterCert.Filter == INFO_CERT_FILTER.CERTIFICATE_VALIDTO) {
                    dataInput = "GetCertListByFilter*ValidTo*" + objFilterCert.Value + '*' + usePKCS11 + '*' + isOnlyToken;
                }
                else if (objFilterCert.Filter == INFO_CERT_FILTER.CERTIFICATE_VALIDFROM) {
                    dataInput = "GetCertListByFilter*ValidFrom*" + objFilterCert.Value + '*' + usePKCS11 + '*' + isOnlyToken;
                }
                else if (objFilterCert.Filter == INFO_CERT_FILTER.CERTIFICATE_TIMEVALID) {
                    dataInput = "GetCertListByFilter*TimeValid*" + objFilterCert.Value + '*' + usePKCS11 + '*' + isOnlyToken;
                }
                else {
                    dataInput = "GetCertListByFilter*SerialNumber*1" + '*' + usePKCS11 + '*' + isOnlyToken;
                }
                if (objFilterCert.FunctionCallback != null) {
                    dataInput = dataInput + '*' + BkavExtensionCallback;
                    objCallback.FunctionCallback = objFilterCert.FunctionCallback;
                }
                return ProcessData(dataInput);
            }

        } catch (e) {
            console.log(e);
        }
    },

    ReadPDFFileToText: function (pdfFile) {
        if (iCheckPluginS == 1) {
            BkavPlugin.ReadPDFFileToText(pdfFile);
        }
        else {
            if (pdfFile == null || pdfFile.trim().length == 0) {
                pdfFile = "1";
            }
            var dataInput = 'ReadPDFFileToText*' + pdfFile;
            return ProcessData(dataInput);
        }

    },

    ReadFormFieldsToText: function (pdfFile) {
        if (iCheckPluginS == 1) {
            BkavPlugin.ReadFormFieldsToText(pdfFile);
        }
        else {
            if (pdfFile == null || pdfFile.trim().length == 0) {
                pdfFile = "1";
            }
            var dataInput = 'ReadFormFieldsToText*' + pdfFile;
            return ProcessData(dataInput);
        }

    },

    FileBrowser: function (OPEN_FILE_FILTER) {
        if (iCheckPluginS == 1) {
            BkavPlugin.FileBrowser(OPEN_FILE_FILTER);
        }
        else {
            if (OPEN_FILE_FILTER == 0) {
                var dataInput = 'FileBrowser*XML';
            }
            else if (OPEN_FILE_FILTER == 1) {
                var dataInput = 'FileBrowser*PDF';
            }
            else if (OPEN_FILE_FILTER == 2) {
                var dataInput = 'FileBrowser*WORD';
            }
            else if (OPEN_FILE_FILTER == 3) {
                var dataInput = 'FileBrowser*EXCEL';
            }
            else
                var dataInput = 'FileBrowser*1';
            return ProcessData(dataInput);
        }

    },
    FileBrowser: function (OPEN_FILE_FILTER, jsCallback) {
        if (iCheckPluginS == 1) {
            BkavPlugin.FileBrowser(OPEN_FILE_FILTER, jsCallback);
        }
        else {
            if (jsCallback == undefined) {


                if (OPEN_FILE_FILTER == 0) {
                    var dataInput = 'FileBrowser*XML';
                }
                else if (OPEN_FILE_FILTER == 1) {
                    var dataInput = 'FileBrowser*PDF';
                }
                else if (OPEN_FILE_FILTER == 2) {
                    var dataInput = 'FileBrowser*WORD';
                }
                else if (OPEN_FILE_FILTER == 3) {
                    var dataInput = 'FileBrowser*EXCEL';
                }
                else
                    var dataInput = 'FileBrowser*1';
            }
            else {
                objCallback.FunctionCallback = jsCallback;
                if (OPEN_FILE_FILTER == 0) {
                    var dataInput = 'FileBrowser*XML*' + BkavExtensionCallback;
                }
                else if (OPEN_FILE_FILTER == 1) {
                    var dataInput = 'FileBrowser*PDF*' + BkavExtensionCallback;
                }
                else if (OPEN_FILE_FILTER == 2) {
                    var dataInput = 'FileBrowser*WORD*' + BkavExtensionCallback;
                }
                else if (OPEN_FILE_FILTER == 3) {
                    var dataInput = 'FileBrowser*EXCEL*' + BkavExtensionCallback;
                }
                else
                    var dataInput = 'FileBrowser*1*' + BkavExtensionCallback;
            }
            return ProcessData(dataInput);
        }

    },
    CheckOCSP: function (objCert) {
        if (iCheckPluginS == 1) {
            BkavPlugin.CheckOCSP(objCert);
        }
        else {
            //var objCert=new ObjCertificate();
            if (objCert.TimeCheck == null || objCert.TimeCheck.trim().length == 0) {
                objCert.TimeCheck = "1";
            }
            var dataInput = "";
            if (objCert.CertificateBase64 != null && objCert.CertificateBase64.trim().length > 0) {
                dataInput = 'CheckOCSP*' + objCert.CertificateBase64 + '*' + objCert.OcspUrl + '*' + objCert.TimeCheck;
            }
            else {
                dataInput = 'CheckOCSPBySerial*' + objCert.CertificateSerialNumber + '*' + objCert.OcspUrl + '*' + objCert.TimeCheck;
            }
            if (objCert.FunctionCallback != null) {
                dataInput = dataInput + '*' + BkavExtensionCallback;
                objCallback.FunctionCallback = objCert.FunctionCallback;
            }
            return ProcessData(dataInput);
        }

    },

    CheckCRL: function (objCert) {
        if (iCheckPluginS == 1) {
            BkavPlugin.CheckCRL(objCert);
        }
        else {
            var dataInput = "";
            if (objCert.TimeCheck == null || objCert.TimeCheck.trim().length == 0) {
                objCert.TimeCheck = "1";
            }
            if (objCert.CertificateBase64 != null && objCert.CertificateBase64.trim().length > 0) {
                dataInput = 'CheckCRL*' + objCert.CertificateBase64 + '*' + objCert.TimeCheck;
            }
            else {
                dataInput = 'CheckCRL*' + objCert.CertificateSerialNumber + '*' + objCert.TimeCheck;
            }
            if (objCert.FunctionCallback != null) {
                dataInput = dataInput + '*' + BkavExtensionCallback;
                objCallback.FunctionCallback = objCert.FunctionCallback;
            }
            return ProcessData(dataInput);
        }

    },

    CheckValidTime: function (objCert) {
        if (iCheckPluginS == 1) {
            BkavPlugin.CheckValidTime(objCert);
        }
        else {
            //var objCert = new ObjCertificate();
            if (objCert.TimeCheck == null || objCert.TimeCheck.trim().length == 0) {
                objCert.TimeCheck = "1";
            }
            var dataInput = "";
            if (objCert.CertificateBase64 != null && objCert.CertificateBase64.trim().length > 0) {
                dataInput = 'CheckValidTime*' + objCert.CertificateBase64 + '*' + objCert.TimeCheck;
            }
            else {
                dataInput = 'CheckValidTime*' + objCert.CertificateSerialNumber + '*' + objCert.TimeCheck;
            }
            if (objCert.FunctionCallback != null) {
                dataInput = dataInput + '*' + BkavExtensionCallback;
                objCallback.FunctionCallback = objCert.FunctionCallback;
            }
            return ProcessData(dataInput);
        }


    },

    CheckToken: function (CertificateSerialNumber, JSCallback) {
        if (iCheckPluginS == 1) {
            BkavPlugin.CheckToken(CertificateSerialNumber, JSCallback);
        }
        else {
            var dataInput;
            if (CertificateSerialNumber == null || CertificateSerialNumber.trim().length == 0) {
                CertificateSerialNumber = "1";
            }

            if (JSCallback == undefined) {
                dataInput = 'CheckToken*' + CertificateSerialNumber;
            }
            else {
                dataInput = 'CheckToken*' + CertificateSerialNumber + '*' + BkavExtensionCallback;
                objCallback.FunctionCallback = JSCallback;
            }
            return ProcessData(dataInput);
        }

    },
    ValidateCertificate: function (objCert) {
        if (iCheckPluginS == 1) {
            BkavPlugin.ValidateCertificate(objCert);
        }
        else {
            //var objCert=new ObjCertificate();
            var usePKCS11 = '0';
            if (objCert.TimeCheck == null || objCert.TimeCheck.trim().length == 0) {
                objCert.TimeCheck = "1";
            }
            var dataInput = "";
            dataInput = 'ValidateCertificate*' + objCert.CertificateBase64 + '*' + objCert.CertificateSerialNumber + '*' + objCert.TimeCheck;
            if (objCert.FunctionCallback != null) {
                dataInput = dataInput + '*' + BkavExtensionCallback;
                objCallback.FunctionCallback = objCert.FunctionCallback;
            }
            return ProcessData(dataInput);
        }

    },
    //PdfUtils
    SetAESKey: function (keyAES) {
        if (iCheckPluginS == 1) {
            BkavPlugin.SetAESKey(keyAES);
        }
        else {
            keyAES = keyAES + '*' + CreateKeyAES();
            var dataInput = 'SetAESKey*' + keyAES;
            return ProcessData(dataInput);
        }

    },
    SetUsePKCS11: function (SET_USE_PKCS11) {
        if (iCheckPluginS == 1) {
            BkavPlugin.SetUsePKCS11(SET_USE_PKCS11);
        }
        else {
            var dataInput;
            if (SET_USE_PKCS11 == 0) {
                dataInput = 'SetUsePKCS11*0';
            }
            else {
                dataInput = 'SetUsePKCS11*1';
            }
            return ProcessData(dataInput);
        }

    },
    SetDLLName: function (dllNameList) {
        if (iCheckPluginS == 1) {
            BkavPlugin.SetDLLName(dllNameList);
        }
        else {
            var dataInput;
            if (dllNameList == null || dllNameList.trim().length == 0) {
                dllNameList = "1";
            }
            var dataInput = 'SetDLLName*' + dllNameList;
            return ProcessData(dataInput);
        }

    },
    ConvertFileToBase64: function (pathFile) {
        if (iCheckPluginS == 1) {
            BkavPlugin.ConvertFileToBase64(pathFile);
        }
        else {
            if (pathFile == null || pathFile.trim().length == 0) {
                pathFile = "1";
            }
            var dataInput = 'ConvertFileToBase64*' + pathFile;
            return ProcessData(dataInput);
        }

    },
    ConvertFileToBase64: function (pathFile, jsCallback) {
        if (iCheckPluginS == 1) {
            BkavPlugin.ConvertFileToBase64(pathFile, jsCallback);
        }
        else {
            if (pathFile == null || pathFile.trim().length == 0) {
                pathFile = "1";
            }
            var dataInput = 'ConvertFileToBase64*' + pathFile;
            if (jsCallback != undefined) {
                objCallback.FunctionCallback = jsCallback;
                dataInput = dataInput + '*' + BkavExtensionCallback;
            }
            return ProcessData(dataInput);
        }

    },
    GetCertIndex: function (CertificateSerialNumber) {
        if (iCheckPluginS == 1) {
            BkavPlugin.GetCertIndex(CertificateSerialNumber);
        }
        else {
            if (CertificateSerialNumber == null || CertificateSerialNumber.trim().length == 0) {
                CertificateSerialNumber = "1";
            }
            var dataInput = 'GetCertIndex*' + CertificateSerialNumber;
            return ProcessData(dataInput);
        }

    },
    SetLicenseKey: function (license) {
        if (iCheckPluginS == 1) {
            BkavPlugin.SetLicenseKey(license);
        }
        else {
            var dataInput = 'SetLicenseKey*' + license;
            return ProcessData(dataInput);
        }

    },
    ExtensionValid: function () {
        if (iCheckPluginS == 1) {
            BkavPlugin.ExtensionValid();
        }
        else {
            var dataInput = 'ExtensionValid*1';
            return ProcessData(dataInput);
        }

    },
    ExtensionIsValid: function (jsCallback) {
        var dataInput = 'ExtensionValidVer2*1'; // chua co ham nay trong core
        iCheckPluginValid = false;
        iCallback = false;
        var eventExtensionValid = document.createEvent('Event');
        document.getElementById('hrSignedData').value = '';
        var checkExtension = document.getElementById(idExtension);
        if (checkExtension == null) { // chua cai Extension
            if (jsCallback != undefined) { // callback hay k?
                jsCallback("0");
            }
            else {
                document.getElementById('hrSignedData').value = '0';
                eventExtensionValid.initEvent(ExtensionIsValidEvent, true, true);
                document.dispatchEvent(eventExtensionValid);
            }
        }
        else { // da cai Ex => check tiep App
            if (jsCallback != undefined) { // callback hay k?
                ExtensionIsValidJSCallback = jsCallback;
            }
            setTimeout(BkavCASignerPluginValidCallBack, 10000); // sau 5.5s goi lai ham callback: can check lai thoi gian thuc te
            return ProcessData(dataInput);
        }
    },
    GetAllExtensions: function () {

        var dataInput = 'GetAllExtensions*';
        return ProcessData(dataInput);
    },
    GetSelfExtension: function () {
        var dataInput = 'GetSelfExtension*';
        return ProcessData(dataInput);
    },
    GetExtensionWithID: function (id) {
        var dataInput = 'GetExtensionWithID*' + id;
        return ProcessData(dataInput);
    },
    /************************************************************************/
    /* Set PIN Cache                                                        */
    /************************************************************************/
    SetPINCache: function (oneSessiosPINCache, sessionsPINCache, secondPINCache) {
        if (iCheckPluginS == 1) {
            BkavPlugin.SetPINCache(oneSessiosPINCache, sessionsPINCache, secondPINCache)
        }
        else {
            var strOneSessionPINCache = '0';
            var strSessionsPINCache = '0';

            if (oneSessiosPINCache) {
                strOneSessionPINCache = '1';
            }
            if (sessionsPINCache) {
                strSessionsPINCache = '1';
            }
            var dataInput = 'SetPINCache*' + strOneSessionPINCache + '*' + strSessionsPINCache + '*' + secondPINCache;
            return ProcessData(dataInput);
        }

    },
    GetVersion: function () {
        if (iCheckPluginS == 1) {
            BkavPlugin.GetVersion();
        }
        else {
            return ProcessData('GetVersion*1')
        }

    },

    // Cac ham ho tro version cu 1.0:
    ReadFileToBase64: function (pathFile) {
        if (iCheckPluginS == 1) {
            BkavPlugin.ReadFileToBase64(pathFile);
        } else {
            var dataInput = 'ReadFileToBase64*' + pathFile;
            return ProcessData(dataInput);
        }

    },
    ReadFileToBase64: function (pathFile, jsCallback) {
        if (iCheckPluginS == 1) {
            BkavPlugin.ReadFileToBase64(pathFile, jsCallback);
        }
        else {
            if (pathFile == null || pathFile.trim().length == 0) {
                pathFile = "1";
            }
            if (jsCallback == undefined) {
                var dataInput = 'ReadFileToBase64*' + pathFile;
            }
            else {
                objCallback.FunctionCallback = jsCallback;
                var dataInput = 'ReadFileToBase64*' + pathFile + "*" + BkavExtensionCallback;
            }
            return ProcessData(dataInput);
        }

    },
    GetAllCert: function (filter, value) {
        if (iCheckPluginS == 1) {
            BkavPlugin.GetAllCert(filter, value);
        }
        else {
            var dataInput = "GetAllCert*" + filter + '*' + value;
            return ProcessData(dataInput);
        }

    },
    GetAllCert: function (filter, value, jsCallback) {
        if (iCheckPluginS == 1) {
            BkavPlugin.GetAllCert(filter, value, jsCallback);
        }
        else {
            if (jsCallback == undefined) {
                var dataInput = "GetAllCert*" + filter + '*' + value;
            }
            else {
                var dataInput = "GetAllCert*" + filter + '*' + value + "*" + BkavExtensionCallback;
                objCallback.FunctionCallback = jsCallback;
            }

            return ProcessData(dataInput);
        }


    },
    SignOffice: function (fileIn, fileOut, serialCert) {

        var dataInput = "SignOffice*" + fileIn + '*' + fileOut + '*' + serialCert;
        return ProcessData(dataInput);
    },
    SignOfficeBase64: function (b64Office, serialCert) {
        var dataInput = "SignOffice*" + b64Office + '*' + serialCert;
        return ProcessData(dataInput);
    },
    SignXMLBase64: function (b64Xml, tagSigning, nodeToSign, tagSaveResult, timeSign, serialNumber, dsSignature) {
        dataInput = 'SignXMLBase64*' + b64Xml + '*' + tagSigning + '*' + nodeToSign + '*' + tagSaveResult + '*' + timeSign + '*' + serialNumber + '*' + dsSignature;
        return ProcessData(dataInput);
    },
    SetGetAttributesCertDefault: function (iDefault) {
        if (iCheckPluginS == 1) {
            BkavPlugin.SetGetAttributesCertDefault(iDefault);
        }
        else {
            var dataInput = 'SetGetAttributesCertDefault*' + iDefault;
            return ProcessData(dataInput);
        }


    },
    ChooserCertFromWindowStore: function (JSCallback) {
        if (iCheckPluginS == 1) {
            BkavPlugin.ChooserCertFromWindowStore(JSCallback);
        }
        else {
            var dataInput;
            if (JSCallback == undefined) {
                dataInput = 'ChooserCertFromWindowStore*1';
            }
            else {
                dataInput = 'ChooserCertFromWindowStore*1*' + BkavExtensionCallback;
                objCallback.FunctionCallback = JSCallback;
            }
            return ProcessData(dataInput);
        }

    },
    Test: function (objXml) {
        var dataInput, nameEventCallback;
        //objCallback = new ObjectBkavExtensionCallback();
        if (objXml.FunctionCallback != null) {
            dataInput = 'Test*1*BkavExtensionCallback';
            nameEventCallback = 'BkavExtensionCallbackTest';
            objCallback.FunctionCallback = objXml.FunctionCallback;
        }
        else {
            dataInput = 'Test*1';
        }
        return ProcessData(dataInput);
    },
    SetCheckTokenDefault: function (iDefault) {
        if (iCheckPluginS == 1) {
            BkavPlugin.SetCheckTokenDefault(iDefault);
        }
        else {
            var dataInput = "SetCheckTokenDefault*0";
            if (iDefault == 1) {
                dataInput = "SetCheckTokenDefault*1";
            }
            return ProcessData(dataInput);
        }

    },
    // 29-07
    SetHashAlgorithm: function (HASH_ALGORITHM) { //hash algorithm
        if (iCheckPluginS == 1) {
            BkavPlugin.SetHashAlgorithm(HASH_ALGORITHM);
        }
        else {
            var dataInput = "SetHashAlgorithm*0";
            if (HASH_ALGORITHM == 1) {
                dataInput = "SetHashAlgorithm*1";
            }
            return ProcessData(dataInput);
        }

    },
    SetAddCertChain: function (ADD_CERTCHAIN) { //hash algorithm
        if (iCheckPluginS == 1) {
            BkavPlugin.SetAddCertChain(ADD_CERTCHAIN);

        }
        else {
            var dataInput = "SetAddCertChain*0";
            if (ADD_CERTCHAIN == 1) {
                dataInput = "SetAddCertChain*1";
            }
            return ProcessData(dataInput);
        }

    },
    SetAddBase64Cert: function (ADD_BASE64CERT) { //hash algorithm
        if (iCheckPluginS == 1) {
            var dataInput = "SetAddBase64Cert*0";
            if (ADD_BASE64CERT == 1) {
                dataInput = "SetAddBase64Cert*1";
            }
            return ProcessData(dataInput);
        }

    },
    // 8-8
    //int BkavCAExtension::DetectToken (string serialNumber)
    DetectToken: function (JSCallback) {
        if (iCheckPluginS == 1) {
            BkavPlugin.DetectToken(JSCallback);
        }
        else {
            var dataInput = "DetectToken*1";
            if (JSCallback != undefined && JSCallback != "") {
                dataInput = dataInput + '*' + BkavExtensionCallback;
                objCallback.FunctionCallback = JSCallback;
            }
            return ProcessData(dataInput);
        }

    },
    ImportCert: function (Base64P12, Password, UserPin, JSCallback) {
        if (iCheckPluginS == 1) {
            var dataInput = "ImportCert";
            if (UserPin == '' || UserPin == undefined) {
                UserPin = "1";
            }
            if (Password == '' || Password == undefined) {
                Password = "1";
            }
            if (Base64P12 == '' || Base64P12 == undefined) {
                Base64P12 = "1";
            }
            dataInput = dataInput + '*' + Base64P12 + '*' + Password + '*' + UserPin;
            if (JSCallback != undefined) {
                dataInput = dataInput + '*' + BkavExtensionCallback;
                objCallback.FunctionCallback = JSCallback;
            }
            return ProcessData(dataInput);
        }

    },
    ChangePINToken: function (PIN_TYPE, OldPIN, NewPIN, JSCallback) {
        if (iCheckPluginS == 1) {
            BkavPlugin.ChangePINToken(PIN_TYPE, OldPIN, NewPIN
                , JSCallback);
        }
        else {
            var dataInput = "ChangePINToken";
            if (OldPIN == '' || OldPIN == undefined) {
                OldPIN = "1";
            }
            var pinType = "0";
            if (PIN_TYPE == 1) {
                pinType = "1";
            }
            dataInput = dataInput + '*' + pinType + '*' + OldPIN + '*' + NewPIN;
            if (JSCallback != undefined) {
                dataInput = dataInput + '*' + BkavExtensionCallback;
                objCallback.FunctionCallback = JSCallback;
            }
            return ProcessData(dataInput);
        }

    },
    CheckLogin: function (PIN_TYPE, PIN, JSCallback) {
        if (iCheckPluginS == 1) {
            BkavPlugin.CheckLogin(PIN_TYPE, PIN, JSCallback);
        }
        else {
            var dataInput = "CheckLogin";
            if (PIN == '' || PIN == undefined) {
                PIN = "1";
            }
            var pinType = "0";
            if (PIN_TYPE == 1) {
                pinType = "1";
            }
            dataInput = dataInput + '*' + pinType + '*' + PIN;
            if (JSCallback != undefined) {
                dataInput = dataInput + '*' + BkavExtensionCallback;
                objCallback.FunctionCallback = JSCallback;
            }
            return ProcessData(dataInput);
        }

    },
    GetTokenInfor: function (JSCallback) {
        if (iCheckPluginS == 1) {
            BkavPlugin.GetTokenInfor(JSCallback);

        } else {
            var dataInput = "GetTokenInfor*1";
            if (JSCallback != undefined) {
                dataInput = dataInput + '*' + BkavExtensionCallback;
                objCallback.FunctionCallback = JSCallback;
            }
            return ProcessData(dataInput);
        }

    },
    SignDataList: function (objDataList) { // 11/10
        if (iCheckPluginS == 1) {
            BkavPlugin.SignDataList(objDataList);
        }
        else {
            var dataInput = "";
            if (objDataList == null || objDataList == undefined)
                return "";
            var dsSignature = "1";
            var objXml = objDataList.objXmlSigner;
            var objPdf = objDataList.objPdfSiger;
            var objOOXml = objDataList.objOOxmlSigner;
            if (objXml == null || objXml == undefined || objPdf == null || objPdf == undefined || objOOXml == null || objOOXml == undefined)
                return "";
            if (!objXml.DsSignature) {
                dsSignature = "0";
            }
            dataInput = 'SignDataList*' + objDataList.Base64String + '*' + objXml.TagSigning + '*' + objXml.NodeToSign + '*' + objXml.TagSaveResult + '*' + objDataList.SigningTime + '*' + objDataList.CertificateSerialNumber + '*' + dsSignature + '*' + objPdf.Signer;
            if (objDataList.FunctionCallback != null) {
                dataInput = dataInput + '*' + BkavExtensionCallback;
                objCallback.FunctionCallback = objDataList.FunctionCallback;
            }
            return ProcessData(dataInput);
        }

    },
    CallUpdateProcess: function () {
        var dataInput = 'CallUpdateProcess*1';
        return ProcessData(dataInput);
    },
    GetDllPKCS11ListFound: function (jsCallback) {
        var dataInput = 'GetDllPKCS11ListFound*1';
        if (jsCallback != undefined) {
            dataInput = dataInput + '*' + BkavExtensionCallback;
            objCallback.FunctionCallback = jsCallback;
        }
        return ProcessData(dataInput);
    }
};


/**
* Đây là hàm demo dạng định nghĩa danh sách kiểu enum.
*/
VERIFY_STATUS = {
    GOOD: 0,
    DATA_INVALID: 1,
    CERTIFICATE_EXPIRE: 2,
    CERTIFICATE_REVOKED: 3,
    CERTIFICATE_HOLD: 4,
    CERTIFICATE_NOT_TRUST: 5
};

XML_SIGNING_TYPE = {
    SIGN_XML_FILE: 0,
    SIGN_XML_BASE64: 1,
    SIGN_XML_XPATH_FILTER: 2,
    SIGN_XML_DATA_LIST: 3
};
PDF_SIGNING_TYPE = {
    SIGN_PDF_FILE: 0,
    SIGN_PDF_BASE64: 1,
    SIGN_PDF_DATA_LIST: 2
};
OOXML_SIGNING_TYPE = {
    SIGN_OOXML_FILE: 0,
    SIGN_OOXML_BASE64: 1,
    SIGN_OOXML_DATA_LIST: 2
};
INFO_CERT_FILTER = {
    CERTIFICATE_SERIAL_NUMBER: 0,
    CERTIFICATE_SUBJECT_CN: 1,
    CERTIFICATE_ISSUER_CN: 2,
    CERTIFICATE_VALIDTO: 3,
    CERTIFICATE_VALIDFROM: 4,
    CERTIFICATE_TIMEVALID: 5
};
OPEN_FILE_FILTER = {
    XML: 0,
    PDF: 1,
    DOCX: 2,
    XLSX: 3//
};
SET_USE_PKCS11 = {
    YES: 1,
    NO: 0
};
VERIFY_TYPE = {
    VERYFY_BASE64: 0,
    VERYFY_FILE: 1
};
HASH_ALGORITHM = {
    SHA1: 0,
    SHA256: 1
};
ADD_CERTCHAIN = {
    NO: 0,
    YES: 1
};
ADD_BASE64CERT = {
    NO: 0,
    YES: 1
};
PIN_TYPE = {
    USER_PIN: 0,
    SO_PIN: 1
};
//EVENT_EXTENSION = {
//    OOXMLFile: 0,
//    OOXMLBase64: 1,
//};
/**
* Đây là đối tượng ký xml.
* @FileIn Dữ liệu dạng đường dẫn đến tập tin Xml cần ký.
* @Base64In Dữ liệu dạng xml Base64 String cần ký.
* @FileOut Đây là kết quả trả về sau khi ký file dạng đưa vào đường dẫn, nếu là ký Base64Xml thì để null trường này.
* @TagSigning Đây là thẻ dữ liệu cần ký trong tài liệu. Nếu để null thì mặc định hệ thống sẽ ký toàn bộ tài liệu.
* @NodeToSign .
* @TagSaveResult Đây là thẻ lưu chữ ký.
* @SigningTime Thời gian ký.
* @CertificateSerialNumber serial number của cert dùng để ký.
* @NameXPathFilter Thẻ dữ liệu cần ký theo chuẩn XPath Filter 2.0.
* @NameIDTimeSignature ID của thẻ thời gian (Ký theo chuẩn XPath Filter 2.0).
* @DsSignature Tiền tố ds:.
* @SigningType Kiểu ký XML.
*/

function ObjXmlSigner() {
    this.PathFileInput = "";
    this.Base64String = "";
    this.PathFileOutput = "";
    this.TagSigning = "";
    this.NodeToSign = "";
    this.TagSaveResult = "";
    this.SigningTime = "";
    this.CertificateSerialNumber = "";
    this.NameXPathFilter = "";
    this.NameIDTimeSignature = "";
    this.DsSignature = true;
    this.SigningType = XML_SIGNING_TYPE.SIGN_XML_BASE64;
    this.FunctionCallback = null

    //PhongNQ thêm signature ID
    this.SignatureID = "";
}

function ObjOOXmlSigner() {
    this.PathFileInput = "";
    this.Base64String = "";
    this.PathFileOut = "";
    this.CertificateSerialNumber = "";
    this.SigningType = OOXML_SIGNING_TYPE.SIGN_OOXML_BASE64;
    this.FunctionCallback = null;
}

//TuanTAg: 
function ObjPdfSigner() {
    this.PathFileInput = "";
    this.Base64String = "";
    this.PathFileInput = "";
    this.SigningTime = "";
    this.CertificateSerialNumber = "";
    this.Signer = "";
    this.SigningType = PDF_SIGNING_TYPE.SIGN_PDF_BASE64;
    this.FunctionCallback = null;
}
function ObjDataListSigner() {
    this.objXmlSigner;
    this.objPdfSiger;
    this.objOOxmlSigner;
    this.Base64String = "";
    this.SigningTime = "";
    this.CertificateSerialNumber = "";
    this.FunctionCallback = null;
}

function ObjCMSSigner() {
    this.Base64String = "";
    this.CertificateSerialNumber = "";
    this.FunctionCallback = null;
}

function ObjVerifier() {
    this.OriginalData = "";
    this.PathFileInput = "";
    this.Base64Signed = "";
    this.TimeCheck = "";
    this.VerifyType = VERIFY_TYPE.VERYFY_BASE64;
    this.FunctionCallback = null;
}

function ObjCertificate() {
    this.CertificateBase64 = "";
    this.CertificateSerialNumber = "";
    this.OcspUrl = "";
    this.TimeCheck = "";
    this.FunctionCallback = null;
}

function ObjFilter() {
    this.Filter = INFO_CERT_FILTER.SerialNumber;
    this.Value = "";
    this.UsePKCS11 = false;
    this.isOnlyCertFromToken = false;
    this.FunctionCallback = null;
}

function CreateKeyAES() {
    var key1 = window.location.host;
    //var key1 = 'demo'; 
    var key2 = 2015 << 2;
    var key3 = (key1.length) << 4;
    var key = key1 + '*' + key3 + '*' + key2;
    return key;
}

function stringToDate(_date, _format, _delimiter) {
    var formatLowerCase = _format.toLowerCase();
    var formatItems = formatLowerCase.split(_delimiter);
    var dateItems = _date.split(_delimiter);
    var monthIndex = formatItems.indexOf("mm");
    var dayIndex = formatItems.indexOf("dd");
    var yearIndex = formatItems.indexOf("yyyy");
    var month = parseInt(dateItems[monthIndex]);
    month -= 1;
    var formatedDate = new Date(dateItems[yearIndex], month, dateItems[dayIndex]);
    return formatedDate;
}

/*
*   Event Name
*/
EXTENSION_EVENT_NAME = {
    // Sign XMl
    SIGN_XML_FILE: 'SignXMLFile',
    SIGN_XML_BASE64: 'SignXMLBase64',
    SIGN_XML_XPATH_FILTER: 'SignXMLBase64XPath',
    SIGN_XML_DATA_LIST: 'SignXMLDataList',
    // Sign PDF
    SIGN_PDF_FILE: 'SignPDFFile',
    SIGN_PDF_BASE64: 'SignPDFBase64',
    SIGN_PDF_DATA_LIST: 'SignPDFDataList',
    // SIGN OOXML
    SIGN_OOXML_FILE: 'SignOOXMLFile',
    SIGN_OOXML_BASE64: 'SignOOXMLBase64',
    SIGN_OOXML_DATA_LIST: 'SignOOXMLDataList',
    SIGN_CMS: 'SignCMSBase64',
    SIGN_DATA_LIST: 'SignDataList',
    // VERIFY
    VERIFY_XML: 'VerifyXML',
    VERIFY_PDF: 'VerifyPDF',
    VERIFY_OOXML: 'VerifyOOXML',
    VERIFY_CMS: 'VerifyCMS',
    // CHECK CERT
    CHECK_OCSP: 'CheckOCSP',
    CHECK_OCSP_BY_SERIAL: 'CheckOCSPBySerial',
    CHECK_CRL: 'CheckCRL',
    CHECK_VALID_TIME: 'CheckValidTime',
    CHECK_TOKEN: 'CheckToken',
    VALIDATE_CERTIFICATE: 'ValidateCertificate',
    // GET INFO CERT
    GET_CERTLIST_BY_FILTER: 'GetCertListByFilter',
    GET_CERT_INDEX: 'GetCertIndex',
    // READ FILE
    READ_PDF_BASE64_TO_TEXT: 'ReadPDFBase64ToText',
    READ_PDF_FILE_TO_TEXT: 'ReadPDFFileToText',
    READ_FORM_FIELS_TO_TEXT: 'ReadFormFieldsToText',
    CONVERT_FILE_TO_BASE64: 'ConvertFileToBase64',
    SET_LICENSE_KEY: 'SetLicenseKey',
    // FILE BROWSER
    FILE_BROWSER: 'FileBrowser',
    ERROR_MESSAGE: 'ExtensionError',
    GET_SELF_EXTENSION: 'GetSelfExtension',
    GET_EXTENSION_WITH_ID: 'GetExtensionWithID',
    GET_ALL_EXTENSION: 'GetAllExtensions',

    //
    CHOOSER_CERT_FROM_WINDOWSTORE: 'ChooserCertFromWindowStore',

    // GEt version
    GET_VERSION: 'GetVersion',
    SET_PIN_CACHE: 'SetPINCache',

    // ExtensionIsValid
    EXTENSION_VALID: 'ExtensionIsValid'
};

/************************************************************************/
/* Utils                                                                */
/************************************************************************/

// Chuc nang callback ap dung tu ver 1.0.12
document.addEventListener('BkavExtensionCallback', function (data) {
    var result = document.getElementById('hrSignedData').value;
    //objXml.FunctionCallback(result);
    objCallback.FunctionCallback(result);
});


// Chuc nang callback cua plugin tu ver 1.0.12
function BkavCASignerPluginCallback(plugin, result) {
    objCallback.FunctionCallback(result);
}

function CheckNativeGetVersionCallback(result) {
    if (result != null && result.trim().length > 0 && iCallback == false) { // check xem co du lieu hay k? callback chua
        if (result < "1.0.15") {
            iCheckPluginValid = true;
            // FIX Cung để hỗ trợ bản cũ
            iPluginVerified = true;
            document.getElementById('hrSignedData').value = '1'; // OK
            if (ExtensionIsValidJSCallback != undefined && ExtensionIsValidJSCallback != null) { // callback hay k?
                // callback ham
                ExtensionIsValidJSCallback(document.getElementById('hrSignedData').value);
            }
            else {
                var eventExtensionValid2 = document.createEvent('Event');
                eventExtensionValid2.initEvent(ExtensionIsValidEvent, true, true);
                document.dispatchEvent(eventExtensionValid2);
            }
        }
        else {
            BkavCANativeAppValidate();
        }
    }
}

// dung cho ham ExtensionValidVer2 moi cua BkavExtension
document.addEventListener('ExtensionValidVer2', function (data) {
    var result = document.getElementById('hrSignedData').value;
    if (result != null && result.trim().length > 0 && iCallback == false) { // check xem co du lieu hay k? callback chua
        //BkavCANativeAppValidate();
        objCallback.FunctionCallback = CheckNativeGetVersionCallback;
        var dataInput = 'GetVersion*1*' + BkavExtensionCallback;
        return ProcessData(dataInput);
    }
});

//document.addEventListener('GetVersion', function (data) {
//    var result = document.getElementById('hrSignedData').value;

//});

function BkavCASignerPluginValidCallBack() {
    var eventExtensionValid3 = document.createEvent('Event');
    if (!iCheckPluginValid) {
        iCallback = true;
        document.getElementById('hrSignedData').value = '2'; // chua cai native app
        if (ExtensionIsValidJSCallback != undefined) { // callback hay k?
            ExtensionIsValidJSCallback("2");
        }
        else {
            eventExtensionValid3.initEvent(ExtensionIsValidEvent, true, true);
            document.dispatchEvent(eventExtensionValid3);
        }
    }
}

// khoi tao plugin
var initPluginRet = false;
function initPlugin() {

    eval("var x = document.getElementsByTagName('body')[0];");
    var checkPlugin = document.getElementById("plugin0");
    if (checkPlugin == null) {
        var node = document.createElement("object");
        node.id = "plugin0";
        try {
            node.height = "10";
            node.width = "10";
        }
        catch (e) {
            console.log(e);
        }
        node.type = "application/x-bkavcaplugin";
        x.appendChild(node);
        plugin = plugin0;
    }
    return BkavCAPluginValidate();
    //return ;
}
//<object id="plugin0" type="application/x-bkavcaplugin" width="0" height="0"/>