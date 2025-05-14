using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System.IO;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System;
using Org.BouncyCastle.X509;
using iTextSharp.text.pdf.security;
using System.Security.Cryptography;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using iTextSharp.text.pdf.parser;
using System.Drawing;
using System.Drawing.Imaging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Collections.Generic;
using System.Text;


namespace Medyx_EMR.Models.DanhMuc
{
    public class PDFSigning
    {


        byte[] content;
        PdfReader pdfReader;
        MemoryStream ms;
        PdfSignatureAppearance appearance;
        private const int csize = 8912;
        string userName;
        BASSigner basSigner;
        Org.BouncyCastle.X509.X509Certificate[] chain;
        public string id { get; set; }
        public string serial { get; set; }
        public string partnerName { get; set; }
        public string cerurl { get; set; }
        public string cerpath { get; set; }
        public string cerpass { get; set; }
        public PDFSigning(byte[] Content, string url, string path, string pass)
        {
            content = Content;
            pdfReader = new iTextSharp.text.pdf.PdfReader(content);
            cerurl = url;
            cerpath = path;
            cerpass = pass;
            ms = new MemoryStream();
            basSigner = new BASSigner();
            basSigner.initWebservice(cerurl, cerpath, cerpass);
        }

        public void Sign(string filename, string SignatureCreator, string Reason, string Location, string find, string filegoc)
        {

            try
            {
                //xác định vị trí hiện thị chữ ký
                float x = 0, y = 0;
                int page = pdfReader.NumberOfPages;
                string searchText = find; // Chuỗi cần tìm
                bool found = false;
                for (int i = 1; i <= pdfReader.NumberOfPages; i++)
                {
                    var strategy = new CustomTextExtractionStrategy(searchText);
                    PdfTextExtractor.GetTextFromPage(pdfReader, i, strategy);
                    strategy.GetResultantText();
                    if (strategy != null && strategy.LastFoundPosition != null)
                    {
                        x = strategy.LastFoundPosition.Left - 50;
                        y = strategy.LastFoundPosition.Top + 5;
                        page = i;
                        found = true;
                        break; 
                    }
                }
                if (!found)
                {
                    throw new Exception($"Không tìm thấy chuỗi cần ký: \"{searchText}\" trong file PDF.");
                }
                string base64Cert = basSigner.GetCertificate(id, partnerName, serial);
                if (string.IsNullOrWhiteSpace(base64Cert))
                    throw new Exception("Certificate không tồn tại hoặc trả về rỗng.");

                byte[] rawCert = Convert.FromBase64String(base64Cert);
                X509Certificate2 x509Cert = new X509Certificate2(rawCert);

                // Parse sang BouncyCastle cert
                X509CertificateParser certParse = new X509CertificateParser();
                Org.BouncyCastle.X509.X509Certificate cert = certParse.ReadCertificate(x509Cert.RawData);
                Org.BouncyCastle.X509.X509Certificate[] chain = new Org.BouncyCastle.X509.X509Certificate[] { cert };

                // Kiểm tra filename và pdfReader
                if (string.IsNullOrEmpty(filename))
                    throw new Exception("Đường dẫn file PDF không hợp lệ.");
               
                if (pdfReader == null)
                    throw new Exception("pdfReader chưa được khởi tạo.");

                using (Stream signedPdf = new FileStream(filename, FileMode.Create))
                {
                    PdfStamper stamper = null;
                    stamper = PdfStamper.CreateSignature(pdfReader, signedPdf, '\0', null, false);
                    // Appearance
                    PdfSignatureAppearance appearance = stamper.SignatureAppearance;
                    appearance.SignatureCreator = SignatureCreator;
                    appearance.Reason = Reason;
                    appearance.Location = Location;
                    appearance.SetVisibleSignature(new iTextSharp.text.Rectangle(x, y, x + 150, y + 50), page, "Signature");
                    //appearance.SetVisibleSignature(new iTextSharp.text.Rectangle(x, y, x + 50, y + 50), page, "Signature");
                    //appearance.SetVisibleSignature(new iTextSharp.text.Rectangle(50, 50, 250, 100), pdfReader.NumberOfPages, "Signature");
                    // appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC_AND_DESCRIPTION;
                    // Digital signature
                    IExternalSignature externalSignature = new MyExternalSignature2("SHA-1", id, serial, partnerName, cerurl, cerpath, cerpass);
                    MakeSignature.SignDetached(appearance, externalSignature, chain, null, null, null, 0, CryptoStandard.CMS);
                    stamper.Close();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

    }

    class MyExternalSignature2 : IExternalSignature
    {
        private String hashAlgorithm;
        private String encryptionAlgorithm;
        BASSigner basSigner;
        public string id { get; set; }
        public string serial { get; set; }
        public string partnerName { get; set; }
        public string cerurl { get; set; }
        public string cerpath { get; set; }
        public string cerpass { get; set; }
        public MyExternalSignature2(String hashAlgorithm, string id, string serial, string partnerName, string url, string path, string pass)
        {
            this.encryptionAlgorithm = "RSA";
            this.hashAlgorithm = DigestAlgorithms.GetDigest(DigestAlgorithms.GetAllowedDigests(hashAlgorithm));
            this.id = id;
            this.serial = serial;
            this.partnerName = partnerName;
            this.cerurl = url;
            this.cerpath = path;
            this.cerpass = pass;
            basSigner = new BASSigner();
            basSigner.initWebservice(cerurl, cerpath, cerpass);
        }

        public virtual byte[] Sign(byte[] message)
        {

            byte[] hash = null;
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                hash = sha1.ComputeHash(message);
            }

            byte[] sig = Convert.FromBase64String(basSigner.SignHash(ByteArrayToHexString(hash), id, partnerName, serial));

            return sig;
        }


        public static string ByteArrayToHexString(byte[] bytes)
        {
            char[] c = new char[bytes.Length * 2];

            byte b;

            for (int bx = 0, cx = 0; bx < bytes.Length; ++bx, ++cx)
            {
                b = ((byte)(bytes[bx] >> 4));
                c[cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);

                b = ((byte)(bytes[bx] & 0x0F));
                c[++cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);
            }

            return new string(c);
        }

        public virtual String GetHashAlgorithm()
        {
            return hashAlgorithm;
        }

        public virtual String GetEncryptionAlgorithm()
        {
            return encryptionAlgorithm;
        }
    }

    // Lớp tùy chỉnh để lấy vị trí văn bản
    class CustomTextExtractionStrategy : ITextExtractionStrategy
    {
        private readonly string _searchText;
        private readonly List<Rectangle> _foundLocations = new List<Rectangle>();
        private readonly List<(string Text, Vector Start, Vector End, LineSegment Baseline, LineSegment AscentLine)> _textChunks
            = new List<(string, Vector, Vector, LineSegment, LineSegment)>();

        public Rectangle LastFoundPosition { get; private set; }
        public List<Rectangle> FoundLocations => _foundLocations;

        public CustomTextExtractionStrategy(string searchText)
        {
            _searchText = searchText;
        }

        public void RenderText(TextRenderInfo renderInfo)
        {
            string text = renderInfo.GetText();
            if (string.IsNullOrEmpty(text)) return;

            LineSegment baseline = renderInfo.GetBaseline();
            LineSegment ascentLine = renderInfo.GetAscentLine();

            if (baseline != null && ascentLine != null)
            {
                Vector start = baseline.GetStartPoint();
                Vector end = baseline.GetEndPoint();

                _textChunks.Add((text, start, end, baseline, ascentLine));
            }
        }

        public string GetResultantText()
        {
            var fullText = new StringBuilder();

            for (int i = 0; i < _textChunks.Count; i++)
            {
                fullText.Append(_textChunks[i].Text);
            }

            string full = fullText.ToString();

            int idx = full.IndexOf(_searchText, StringComparison.OrdinalIgnoreCase);
            if (idx >= 0)
            {
                // Tìm vị trí bắt đầu và kết thúc của đoạn chứa từ cần tìm
                int charCount = 0;
                for (int i = 0; i < _textChunks.Count; i++)
                {
                    charCount += _textChunks[i].Text.Length;
                    if (charCount >= idx + _searchText.Length)
                    {
                        var chunk = _textChunks[i];

                        var rect = new Rectangle
                        {
                            Left = chunk.Start[0],
                            Bottom = chunk.Start[1],
                            Right = chunk.End[0],
                            Top = chunk.AscentLine.GetEndPoint()[1]
                        };

                        LastFoundPosition = rect;
                        _foundLocations.Add(rect);
                        break;
                    }
                }
            }

            return fullText.ToString();
        }

        public void BeginTextBlock() { }
        public void EndTextBlock() { }
        public void RenderImage(ImageRenderInfo renderInfo) { }
    }



    //class CustomTextExtractionStrategy : ITextExtractionStrategy
    //{
    //    private List<Rectangle> _foundLocations;
    //    public List<Rectangle> FoundLocations
    //    {
    //        get { return _foundLocations; }
    //        private set { _foundLocations = value; }
    //    }
    //    //public Rectangle LastFoundPosition { get; private set; } = new Rectangle(); // Khởi tạo trước
    //    private Rectangle _LastFoundPosition;
    //    public Rectangle LastFoundPosition
    //    {
    //        get { return _LastFoundPosition; }
    //        private set { _LastFoundPosition = value; }
    //    }
    //    public CustomTextExtractionStrategy()
    //    {
    //        _foundLocations = new List<Rectangle>();
    //        _LastFoundPosition = new Rectangle();
    //    }
    //    private string _searchText;

    //    public CustomTextExtractionStrategy(string searchText)
    //    {
    //        _searchText = searchText;
    //    }
    //    public void RenderText(TextRenderInfo renderInfo)
    //    {
    //        string text = renderInfo.GetText();
    //        if (string.IsNullOrEmpty(text)) return;

    //        // Nếu đoạn văn bản chứa từ cần tìm
    //        if (text.Contains(_searchText))
    //        {
    //            LineSegment baseline = renderInfo.GetBaseline();
    //            LineSegment ascentLine = renderInfo.GetAscentLine();

    //            if (baseline != null && ascentLine != null)
    //            {
    //                Vector start = baseline.GetStartPoint();
    //                Vector end = baseline.GetEndPoint();
    //                Vector top = ascentLine.GetEndPoint();

    //                var rect = new Rectangle
    //                {
    //                    Left = start[0],
    //                    Bottom = start[1],
    //                    Right = end[0],
    //                    Top = top[1]
    //                };

    //                LastFoundPosition = rect;
    //                _foundLocations.Add(rect);
    //            }
    //        }
    //    }
    //    //public void RenderText(TextRenderInfo renderInfo)
    //    //{
    //    //    string text = renderInfo.GetText();
    //    //    if (text.Contains(_searchText))
    //    //    {
    //    //        LineSegment baseline = renderInfo.GetBaseline();
    //    //        if (baseline != null) // Kiểm tra trước khi dùng
    //    //        {
    //    //            Vector start = baseline.GetStartPoint();
    //    //            LastFoundPosition = new Rectangle
    //    //            {
    //    //                Left = start[0],
    //    //                Bottom = start[1]
    //    //            };
    //    //        }
    //    //    }
    //    //}
    //    public string GetResultantText()
    //    {
    //        return "";
    //    }
    //    // Không cần trích xuất toàn bộ văn bản
    //    public void BeginTextBlock() { }
    //    public void EndTextBlock() { }
    //    public void RenderImage(ImageRenderInfo renderInfo) { }
    //}
    // Lớp hỗ trợ để lưu tọa độ
    class Rectangle
    {
        public float Left { get; set; }
        public float Bottom { get; set; }
        public float Right { get; set; }
        public float Top { get; set; }
    }
}
