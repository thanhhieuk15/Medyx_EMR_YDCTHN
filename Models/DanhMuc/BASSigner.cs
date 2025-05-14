using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System;
using Newtonsoft.Json.Linq;

namespace Medyx_EMR.Models.DanhMuc
{
    class BASSigner
    {
        string _url = "https://103.237.99.15/EAServer/services/EAService?wsdl";
        X509Certificate2Collection certificates;

        //public void initWebservice()
        //{
        //    certificates = new X509Certificate2Collection();
        //    certificates.Import(@".\SSL_Goline.p12", "123456a@A", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
        //    System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072;
        //    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        //}
        public void initWebservice(string url, string path, string pass)
        {
            _url = url;
            certificates = new X509Certificate2Collection();
            //certificates.Import(@".\SSL_Goline.p12", "123456a@A", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
            certificates.Import(path, pass, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
            System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }
        public string GetCertificate(string id, string partnerName, string serial)
        {
            return DoGetCertificate(id, partnerName, serial);
        }


        public string DoGetCertificate(string id, string partnerName, string serial)
        {

            try
            {
                var _action = "getCertificate";

                string request = "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ser='http://service.easerver.bkavca.com'>" +
                                  "   <soapenv:Header/>" +
                                  "   <soapenv:Body>" +
                                  "      <ser:getCertificate>   " +
                                  "         <ser:UID>" + id + "</ser:UID> " +
                                  "         <ser:partnerName>" + partnerName + "</ser:partnerName>" +
                                  "         <ser:serialOrCardNumber>" + serial + "</ser:serialOrCardNumber>" +
                                  "      </ser:getCertificate>" +
                                  "   </soapenv:Body>" +
                                  "</soapenv:Envelope>";


                XmlDocument soapEnvelopeDocument = new XmlDocument();
                soapEnvelopeDocument.LoadXml(request);
                XmlDocument soapEnvelopeXml = soapEnvelopeDocument;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                HttpWebRequest webRequest = CreateWebRequest(_url, _action);
                webRequest.ClientCertificates = certificates;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.Expect100Continue = false;
                string soapResult;
                using (WebResponse webResponse = webRequest.EndGetResponse(webRequest.BeginGetResponse(null, null)))
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        soapResult = rd.ReadToEnd();
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(soapResult);
                        return doc.InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                return "[DoGetCertificate] => Excetion: " + ex.ToString();
            }


        }

        public string SignHash(string hexHash, string id, string partnerName, string serial)
        {

            try
            {
                var _action = "signHash";
                string request = "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ser='http://service.easerver.bkavca.com'>" +
                                 "   <soapenv:Header/>" +
                                 "   <soapenv:Body>" +
                                 "      <ser:signHash>" +
                                 "         <ser:hashData>" + hexHash + "</ser:hashData>" +
                                 "         <ser:UID>" + id + "</ser:UID>" +
                                 "         <ser:partnerName>" + partnerName + "</ser:partnerName>" +
                                 "         <ser:serialOrCardNumber>" + serial + "</ser:serialOrCardNumber>" +
                                 "      </ser:signHash>" +
                                 "   </soapenv:Body>" +
                                 "</soapenv:Envelope>";

                XmlDocument soapEnvelopeDocument = new XmlDocument();
                soapEnvelopeDocument.LoadXml(request);

                XmlDocument soapEnvelopeXml = soapEnvelopeDocument;
                HttpWebRequest webRequest = CreateWebRequest(_url, _action);
                webRequest.ClientCertificates = certificates;
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

                string soapResult;
                using (WebResponse webResponse = webRequest.EndGetResponse(webRequest.BeginGetResponse(null, null)))
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        soapResult = rd.ReadToEnd();
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(soapResult);

                        var obj = JObject.Parse(doc.InnerText);
                        var code = (string)obj["code"];
                        if (Int32.Parse(code) == 10) return (string)obj["dataResponse"];
                    }

                }

            }
            catch (Exception ex)
            {
                return "[SignHash] => Excetion: " + ex.ToString();
            }

            return "";
        }

        public string SignHash2(string hexHash, string id, string partnerName, string serial)
        {

            try
            {
                var _action = "signHash2";
                string request = "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ser='http://service.easerver.bkavca.com'>" +
                                 "   <soapenv:Header/>" +
                                 "   <soapenv:Body>" +
                                 "      <ser:signHash>" +
                                 "         <ser:hashData>" + hexHash + "</ser:hashData>" +
                                 "         <ser:UID>" + id + "</ser:UID>" +
                                 "         <ser:partnerName>" + partnerName + "</ser:partnerName>" +
                                 "         <ser:serialOrCardNumber>" + serial + "</ser:serialOrCardNumber>" +
                                 "      </ser:signHash>" +
                                 "   </soapenv:Body>" +
                                 "</soapenv:Envelope>";

                XmlDocument soapEnvelopeDocument = new XmlDocument();
                soapEnvelopeDocument.LoadXml(request);

                XmlDocument soapEnvelopeXml = soapEnvelopeDocument;
                HttpWebRequest webRequest = CreateWebRequest(_url, _action);
                webRequest.ClientCertificates = certificates;
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

                string soapResult;
                using (WebResponse webResponse = webRequest.EndGetResponse(webRequest.BeginGetResponse(null, null)))
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        soapResult = rd.ReadToEnd();
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(soapResult);

                        var obj = JObject.Parse(doc.InnerText);
                        var code = (string)obj["code"];
                        if (Int32.Parse(code) == 10) return (string)obj["dataResponse"];
                    }

                }

            }
            catch (Exception ex)
            {
                return "[SignHash] => Excetion: " + ex.ToString();
            }

            return "";
        }

        public string SignCMS(string hexHash, string id, string partnerName, string serial)
        {
            try
            {
                var _action = "sign0";
                string request = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ser=\"http://service.easerver.bkavca.com\">  " +
                                 "<soapenv:Header/>   <soapenv:Body>      <ser:sign0>         <!--Optional:-->        " +
                                 " <ser:input>{  \"dataBase64\": \"" + hexHash + "\",  " +
                                 "\"serialNumber\": \"" + serial + "\",  " +
                                 "\"partnerName\": \"" + partnerName + "\", " +
                                 "\"MST\": \"" + id + "\",  " +
                                 "\"extensionFile\": \"cms\",  }" +
                                 "</ser:input>      " +
                                 "</ser:sign0>  " +
                                 " </soapenv:Body></soapenv:Envelope>";

                XmlDocument soapEnvelopeDocument = new XmlDocument();
                soapEnvelopeDocument.LoadXml(request);

                XmlDocument soapEnvelopeXml = soapEnvelopeDocument;
                HttpWebRequest webRequest = CreateWebRequest(_url, _action);
                webRequest.ClientCertificates = certificates;
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

                string soapResult;
                using (WebResponse webResponse = webRequest.EndGetResponse(webRequest.BeginGetResponse(null, null)))
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        soapResult = rd.ReadToEnd();
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(soapResult);

                        var obj = JObject.Parse(doc.InnerText);
                        var code = (string)obj["code"];
                        if (Int32.Parse(code) == 10) return (string)obj["dataResponse"];
                    }

                }

            }
            catch (Exception ex)
            {
                return "[SignHash] => Excetion: " + ex.ToString();
            }

            return "";
        }
        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }
    }

}
