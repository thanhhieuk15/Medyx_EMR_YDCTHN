using Medyx_EMR_BCA.ApiAssets.Repository;
using Medyx_EMR_BCA.Models.DBConText;
using Medyx_EMR_BCA.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Doc.Reporting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using System.Windows.Interop;

namespace Medyx_EMR_BCA.ApiAssets.Helpers
{
    public static class PrintHelper
    {
        public const string DELETE_ROW_KEY = "delete_row";

        // print with field, values, and list data
        public static string PrintFile<T>(IHostingEnvironment hostingEnvironment, string template, List<string> fields = null, List<string> values = null, List<T> dataGroup = null, string keyGroup = null)
        {
            setLicense();
            Document document = new Document(hostingEnvironment.WebRootPath + $"/Template/{template}", FileFormat.Auto);
            document.MailMerge.MergeImageField += new MergeImageFieldEventHandler(MailMerge_MergeImageField);
            document.MailMerge.MergeField += MailMerge_MergeHTMLField;
            if (!String.IsNullOrEmpty(keyGroup))
            {
                MailMergeDataTable table = new MailMergeDataTable(keyGroup, dataGroup);
                document.MailMerge.ExecuteGroup(table);
            }
            if (fields != null && values != null && fields.Count > 0 && values.Count > 0)
            {
                document.MailMerge.Execute(fields.ToArray(), values.ToArray());
            }
            InsertHtml();
            return HanlderSaveFile(document, null, template);
        }
        private static void setLicense()
        {
            Spire.License.LicenseProvider.SetLicenseFileName("Spire.License.dll");
            Spire.License.LicenseProvider.SetLicenseKey("CtOzJs2BlzPokWgBAKMfmNxjRwLa3eqzrAvKtn54UDB/dWjIyGokcs+UQuYuvMY03wX56Ox75KV+U1r5H0PR++c1zc6i8e0QIOVuhMp9Qbg5A9bJJA7e7KvC4KMINTr4jnJy/yTGFwT1aEusw144kml/6oAttwEUoXBkDPLWGOsvNgH1iTYkTGWMXEV8Or4p4t4doNsl0Z7V5qWDKwB6sD/ZiH7l/Jum27FWevOlKIa2VG1rEKjtURYukbWXeSH54IKtmn7nmr0wKwnRgdu3q60aC/PdkxC0zX75EnbU5M6fa3pplU40f3LGOWcgZ2f+8oI7qpPXJ8/s7LrsxBqpQ2YGKfKuqx5ex9ALrXgjnwjcslmXPYun7flHGIkbvBsCjCpo4Ed+M658sZTGATak6gLmftEqhJ1ZZJJKFgXE5qa/TyCY7wIq1ll+z1VNhnSBZUc1RA4TwSBcFKvrZEHlj9o1WFZ1+QqNAcnzh/n+tG48B0wHLCl6D4hroCfWMoaw/23DRxx1WuWqfkazuz2H8ga1RC2XPs83nB7CHPFNs0sT5lsKbfA3P9jgtza5CEhfjAN/3TiwEP/tvnTZY+VABK97veB77h4LEiVMfQXzKfhm9cNW4ft/ofVU2OfqZ8GjtntoZdPxp1bIwTvI98SnQi/H81w19aHwUqNECTeJBjqqHMxdVKVSBAKJL0TM7RyzoOPKS19OfURAxlEgRUqJF/BM8eU0R+UicIM2h36sTuBKO4g3H6woDMlnx0QG0nqthauTB7oK6QFTwk44UQ1kTAu8LeOJwM2xNu5MLsPmoWwDvmIaTuZIW6VUX8C285c9KkrYAf79YKA3e3yxx6SSQdN/jLbtR7MaeGpxRzX0iEbqL9sG1m5USuYVByvVKQ4ntvfCMlLmUN9UCvJ/m63K27Z2dm6fTXIe/g0smYmnvEQ3JQVnldWOi1TKOMK8RbuU5un5mQZ96pLq0Q7g0NLQZh50UMT+OjAzXHPxmXfV6/deHeE8Gbb3ZYJSg7UXW2sty86uXwkj89x5yJTaMNtm6Kh2QQugn/Vd9n8C8QReNewYxjF827FBpMp9yf+vLf2FSyA50wiA9o9luoXYgRmGuUh+g9+KMWgMK5fxQ2h3cHqADzPcwsDhVfG6HuAgt81vH/M5hFLdQztXdvRKVuYOyyTOnQz9K93LZ2EvbeWz0YByRkGxnve+K8UNo3pyNgaPGRQWr5RbeURNJ4PhmM3dB2oMkwE//+s39ccgADdEJS8s35cjRrVEGs8JicRu6mDNqJfdHUNfLmiySMjG/ePwhYkiB2WhJ9AqpY9N7eQ3TBsAMkr34olS6eSNpaE1BjgJsljB27GDnmMAXNZeifyIYpBcqu6H9SLN5pGBF9WHcPVivjdNpMUrKQ==");
            Spire.License.LicenseProvider.LoadLicense();
        }
        public static string ConvertDecimalToInt(decimal? number)
        {
            if (!number.HasValue)
            {
                return "";
            }
            return (Convert.ToInt32(number)).ToString();
        }

        public static void HanlderBooleanNumberTypeOption(ref List<string> fields, ref List<string> values, byte? value, string key, byte true_value = 1, byte false_value = 2)
        {
            fields.Add($"{key}_0");
            fields.Add($"{key}_1");
            values.Add(value == true_value ? "x" : "");
            values.Add(value == false_value ? "x" : "");
        }


        public static void HanlderBooleanTypeOption(ref List<string> fields, ref List<string> values, byte? value, string key)
        {
            fields.Add($"{key}_0");
            fields.Add($"{key}_1");
            values.Add(Convert.ToBoolean(value) ? "x" : "");
            values.Add(Convert.ToBoolean(value) ? "" : "x");
        }
        // save file to pdf
        private static string HanlderSaveFile(Document document, string idba_stt, string template, bool removeCellInTable = false)
        {
            string mss = "";
            try
            {
                mss += "removeCellInTable:" + removeCellInTable;

                // Xử lý việc xóa các ô trong bảng nếu cần thiết
                if (removeCellInTable)
                {
                    foreach (Section s in document.Sections)
                    {
                        foreach (Table t in s.Tables)
                        {
                            for (int i = t.Rows.Count - 1; i >= 0; i--)
                            {
                                if (t.Rows[i].Cells[0].Paragraphs[0].Text.Contains(DELETE_ROW_KEY))
                                {
                                    t.Rows.RemoveAt(i);
                                }
                            }
                        }
                    }
                }

                // Lấy định dạng tệp và tên tệp
                string time = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                string ext = Path.GetExtension(template);
                mss += "\next:" + ext;
                string path = $"Storage/Print/{time}-{idba_stt}";
                FileFormat format = ext == ".doc" ? FileFormat.Doc : FileFormat.Docx;
                mss += "\nformat:" + format;

                // Sử dụng using để đảm bảo tài nguyên được giải phóng sau khi xử lý
                using (document)
                {
                    document.SaveToFile(path, format);
                }
                if (File.Exists(path))
                {
                    // Chuyển đổi tệp Word sang PDF nếu cần
                    if (ext == ".doc" || ext == ".docx")
                    {
                        using (Document documentPdf = new Document())
                        {
                            documentPdf.LoadFromFile(path);
                            ToPdfParameterList ps = new ToPdfParameterList
                            {
                                UsePSCoversion = true,
                                IsEmbeddedAllFonts = true
                            };
                            documentPdf.SaveToFile($"Storage/Print/{time}-{idba_stt}.pdf", ps);
                        }
                    }
                    else
                    {
                        mss += "\t 142:";
                        throw new InvalidOperationException($"Unsupported file format: {ext}");
                    }
                }
                else
                {
                    mss += "\t 148:" + $"Word file not found for conversion: {path}";
                    throw new FileNotFoundException($"Word file not found for conversion: {path}");
                }
                // Xóa tệp Word tạm sau khi chuyển sang PDF
                var uploadFileResponse = new UploadFileRespository();
                uploadFileResponse.RemoveFile(path, true);

                mss += "\nuploadFileResponse:" + uploadFileResponse;
                return $"Storage/Print/{time}-{idba_stt}.pdf";
            }
            catch (Exception ex)
            {
                WriteLog("\nLỗi HanlderSaveFile: " + ex.Message + "\nmss:" + mss);
                Console.WriteLine($"Error in HanlderSaveFile: {ex.Message}");
                throw;
            }
        }

        //private static string HanlderSaveFile(Document document, string template, bool removeCellInTable = false)
        //{
        //    try
        //    {
        //        if (removeCellInTable)
        //        {
        //            foreach (Section s in document.Sections)
        //            {
        //                foreach (Table t in s.Tables)
        //                {
        //                    for (int i = t.Rows.Count - 1; i >= 0; i--)
        //                    {
        //                        //delete all the lines which contain “delete_row” in the first column.
        //                        if (t.Rows[i].Cells[0].Paragraphs[0].Text.Contains(DELETE_ROW_KEY))
        //                        {
        //                            t.Rows.RemoveAt(i);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        var time = DateTime.Now.ToString("yyyyMMddHHmmssffff");
        //        var ext = Path.GetExtension(template);
        //        var path = $"Storage/Print/{time}{ext}";
        //        var format = ext == "doc" ? FileFormat.Doc : FileFormat.Docx;
        //        document.SaveToFile(path, format);

        //        // convert to pdf
        //        string FileExt = Path.GetExtension(path);
        //        if (FileExt == ".doc" || FileExt == ".docx")
        //        {
        //            Document documentPdf = new Document();
        //            //var fullpath = Path.GetFullPath($"Storage/Print");
        //            documentPdf.LoadFromFile($"Storage/Print/{time}{ext}");
        //            ToPdfParameterList ps = new ToPdfParameterList
        //            {
        //                UsePSCoversion = true,
        //                IsEmbeddedAllFonts = true
        //            };
        //            document.SaveToFile($"Storage/Print/{time}.pdf", ps);
        //        }
        //        //remove file word
        //        var uploadFileResponse = new UploadFileRespository();
        //        uploadFileResponse.RemoveFile(path, true);
        //        document.Close();
        //        return $"Storage/Print/{time}.pdf";
        //    }
        //    catch (Exception ex)
        //    {

        //        Console.WriteLine($"Error in HanlderSaveFile: {ex.Message}");
        //        throw ex;

        //    }


        //}
        //merge pdf file
        public static string MergePdfFile(List<string> paths)
        {
            var time = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            string[] files = paths.Select(x => Path.GetFullPath(x)).ToArray();
            Spire.Pdf.PdfDocumentBase doc = Spire.Pdf.PdfDocument.MergeFiles(files);
            var path = $"Storage/Print/{time}.pdf";
            doc.Save(path, Spire.Pdf.FileFormat.PDF);
            var uploadFileResponse = new UploadFileRespository();
            foreach (var file in files)
            {
                uploadFileResponse.RemoveFile(file, true);
            }
            return path;
        }

        public static void WriteLog(String log)
        {
            #region ghi log dang nhap
            IHttpContextAccessor _context = new HttpContextAccessor();
            var session = _context.HttpContext.Session;
            var u = ApiAssets.Helpers.SessionHelper.GetObjectFromJson<UserProfileSessionData>(
                 session, "UserProfileSessionData");
            //string constr = ConfigurationManager.AppSettings["MongoDBconnectionString"];
            //string constr = _config.GetValue<string>("Medyx_BCADatabase:ConnectionString");
            string constr = u.MongoDBConnectionString;
            var client = new MongoClient(constr);
            //IMongoDatabase db = client.GetDatabase("Medyx_BCA");
            //IMongoDatabase dbm = client.GetDatabase(ConfigurationManager.AppSettings["MongoDBDataBase"]);
            //IMongoDatabase dbm = client.GetDatabase(_config.GetValue<string>("Medyx_BCADatabase:DatabaseName"));
            IMongoDatabase dbm = client.GetDatabase(u.MongoDBDataBaseName);
            IMongoCollection<TraceLogMongo> collection = dbm.GetCollection<TraceLogMongo>("TraceLog");
            string MaMay = "11111";
            TraceLogMongo emp = new TraceLogMongo();
            emp.NgaySD = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            emp.TenBang = "Log";
            emp.KieuTacDong = "Error";
            emp.NguoiSD = u.Pub_sNguoiSD;
            emp.MaMay = MaMay;
            emp.NoiDungSD = "Loi print 1: " + log;
            collection.InsertOne(emp);
            #endregion
        }

        // print with table have relationship inside, fields, values
        public static string PrintFileWithTable(string idba_stt, IHostingEnvironment hostingEnvironment, string template, DataSet dataset, List<DictionaryEntry> list, List<string> fields = null, List<string> values = null, bool removeCellInTable = false)
        {
            string mss = "";
            try
            {
                mss += idba_stt;
                mss += "\ndataset:" + dataset;
                mss += "\nlist:" + list;
                setLicense();

                // Kiểm tra các tham số đầu vào
                if (hostingEnvironment == null) throw new ArgumentNullException(nameof(hostingEnvironment));
                if (string.IsNullOrWhiteSpace(template)) throw new ArgumentException("Template cannot be null or empty.", nameof(template));

                string templatePath = Path.Combine(hostingEnvironment.WebRootPath, "Template", template);
                mss += templatePath;

                // Sử dụng using để tự động giải phóng tài nguyên
                using (Document document = new Document(templatePath, FileFormat.Auto))
                {
                    if (document == null)
                    {
                        mss += "\ndocument is null: " + (document == null).ToString();
                        throw new InvalidOperationException("Failed to load the document.");
                    }
                    document.MailMerge.MergeImageField += new MergeImageFieldEventHandler(MailMerge_MergeImageField);
                    mss += "\n278:" ;
                    document.MailMerge.MergeField += MailMerge_MergeHTMLField;
                    mss += "\n280:"+document;

                    if (dataset != null && list != null && list.Count > 0)
                    {
                        mss += "\n284:";
                        document.MailMerge.ExecuteWidthNestedRegion(dataset, list);
                        mss += "\n286:";
                    }

                    mss += "\nfields:" + fields;
                    if (fields != null && values != null && fields.Count > 0 && values.Count > 0)
                    {
                        document.MailMerge.Execute(fields.ToArray(), values.ToArray());
                    }

                    InsertHtml();

                    return HanlderSaveFile(document, idba_stt, template, removeCellInTable);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PrintFileWithTable: {ex.Message}");
                WriteLog("\nLỗi PrintFileWithTable: " + ex.Message + "\nmss:" + mss);
                throw;
            }
        }

        //public static string PrintFileWithTable(IHostingEnvironment hostingEnvironment, string template, DataSet dataset, List<DictionaryEntry> list, List<string> fields = null, List<string> values = null, bool removeCellInTable = false)
        //{
        //    try
        //    {
        //        setLicense();
        //        Document document = new Document(hostingEnvironment.WebRootPath + $"/Template/{template}", FileFormat.Auto);
        //        document.MailMerge.MergeImageField += new MergeImageFieldEventHandler(MailMerge_MergeImageField);
        //        document.MailMerge.MergeField += MailMerge_MergeHTMLField;
        //        if (dataset != null && list != null && list.Count > 0)
        //        {
        //            document.MailMerge.ExecuteWidthNestedRegion(dataset, list);
        //        }
        //        if (fields != null && values != null && fields.Count > 0 && values.Count > 0)
        //        {
        //            document.MailMerge.Execute(fields.ToArray(), values.ToArray());
        //        }
        //        InsertHtml();
        //        document.Dispose();
        //        return HanlderSaveFile(document, template, removeCellInTable);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }


        //}

        public class DatasetTable
        {
            public DataSet DataSet { get; set; }
            public List<DictionaryEntry> List { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        /// <param name="template"></param>
        /// <param name="datasets"></param>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <returns>return path file pdf</returns>
        public static string PrintFileWithTable(IHostingEnvironment hostingEnvironment, string template, List<DatasetTable> datasets, List<string> fields = null, List<string> values = null)
        {
            setLicense();
            string templatePath = hostingEnvironment.WebRootPath + $"/Template/{template}";
            using (Document document = new Document(templatePath, FileFormat.Auto))
            {
                document.MailMerge.MergeImageField += new MergeImageFieldEventHandler(MailMerge_MergeImageField);
                document.MailMerge.MergeField += MailMerge_MergeHTMLField;
                if (datasets != null)
                {
                    foreach (var item in datasets)
                    {
                        document.MailMerge.ExecuteWidthNestedRegion(item.DataSet, item.List);
                    }
                }
                if (fields != null && values != null && fields.Count > 0 && values.Count > 0)
                {
                    document.MailMerge.Execute(fields.ToArray(), values.ToArray());
                }
                InsertHtml();
                return HanlderSaveFile(document, null, template);
            }
            //Document document = new Document(hostingEnvironment.WebRootPath + $"/Template/{template}", FileFormat.Auto);

        }
        // (v1, v2, v3) => "v1, v2, v3"
        public static string ConcatStringArr(params string[] values)
        {
            return ConcatStringArr((object)", ", values);
        }
        public static string ConcatStringArrChamPhay(params string[] values)
        {
            return ConcatStringArr((object)"; ", values);
        }
        public static string ConcatStringArr(object separator, params string[] values)
        {
            return string.Join(separator.ToString(), Array.FindAll(values.ToArray(), x => !string.IsNullOrEmpty(x)));
        }
        // handler nullable type to string v => v || ""
        public static string HanlderUnknowsType<T>(Nullable<T> value) where T : struct
        {
            if (value.HasValue)
            {
                return value.ToString();
            }
            return "";
        }
        /// <summary>
        /// Hanlder boolean
        /// </summary>
        /// <param name="value"></param>
        /// <param name="addValue"></param>
        /// <returns>string</returns>
        public static string HanlderBooleanType(byte? value, byte addValue = 0)
        {
            if (value.HasValue)
            {
                return (value + addValue).ToString();
            }
            return "";
        }

        /// <summary>
        /// Dia chi 
        /// </summary>
        /// <param name="soNha"></param>
        /// <param name="thonPho"></param>
        /// <param name="xaPhuong"></param>
        /// <param name="quanHuyen"></param>
        /// <param name="tinh"></param>
        /// <returns> số nhà ... thôn .. xã phường .. </returns>
        public static string HanlderDiaChiText(string soNha, string thonPho, string xaPhuong, string quanHuyen, string tinh)
        {
            var items = new List<string>(){
                soNha,
                thonPho,
                xaPhuong,
                quanHuyen,
                tinh,
            };
            return string.Join(", ", items.Where(x => !String.IsNullOrEmpty(x)).ToArray());
        }

        /// <summary>
        ///  [] CBCS [] BHYT [x] Tự nguyện
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="maDt"></param>
        /// <param name="key"></param>
        public static void HanlderDoiTuongSpecail(ref List<string> fields, ref List<string> values, string maDt, string key = "DoiTuong")
        {
            const string CBCS = "3";
            const string BHYT = "1";
            const string TU_NGUYEN = "2";
            var options = new string[] { CBCS, BHYT, TU_NGUYEN };
            for (int i = 0; i < options.Length; i++)
            {
                fields.Add($"{key}_{i}");
                values.Add(maDt == options[i] ? "x" : "");
            }
        }
        /// <summary>
        /// Convert date
        /// </summary>
        /// <param name="date"></param>
        /// <returns>string ( Ngày ... Tháng ... Năm)</returns>
        public static string DateText(DateTime? date)
        {
            return date.HasValue ? $"Ngày {date.Value.ToString("dd")} tháng {date.Value.ToString("MM")} năm {date.Value.ToString("yyyy")}" : "Ngày...Tháng...Năm...";
        }
        /// <summary>
        /// Convert date 
        /// </summary>
        /// <param name="date"></param>
        /// <returns>String (Ngày dd/MM/yyyy) or empty string</returns>
        public static string DateTextShort(DateTime? date)
        {
            return date.HasValue ? $"ngày {date.Value.ToString("dd")}/{date.Value.ToString("MM")}/{date.Value.ToString("yyyy")}" : "";
        }

        /// <summary>
        /// Convert date
        /// </summary>
        /// <param name="date"></param>
        /// <returns>string (dd/MM/yyyy) or empty string</returns>
        public static string DateTextShortest(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("dd/MM/yyyy") : "";
        }
        /// <summary>
        /// Convert date
        /// </summary>
        /// <param name="date"></param>
        /// <returns>string(... giờ ... phút, Ngày ..... Tháng ..... Năm ....)</returns>
        public static string DateTimeText(DateTime? date)
        {
            return date.HasValue ? $"{date.Value.ToString("HH")} giờ {date.Value.ToString("mm")} phút, Ngày {date.Value.ToString("dd")} tháng {date.Value.ToString("MM")} năm {date.Value.ToString("yyyy")}" : "... giờ ... phút, Ngày ..... Tháng ..... Năm ....";
        }
        /// <summary>
        /// Convert date
        /// </summary>
        /// <param name="date"></param>
        /// <returns>string (giờ ...phút, dd/MM/yyyy)</returns>
        public static string DateShortTimeText(DateTime? date)
        {
            return date.HasValue ? $"{date.Value.ToString("HH")} giờ {date.Value.ToString("mm")} phút, {date.Value.ToString("dd/MM/yyyy")}" : "... giờ ... phút,.../.../....";
        }
        /// <summary>
        /// Convert Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns>string(dd/MM/yyyy) or empty string</returns>
        public static string DateTimeFullTextShort(DateTime? date)
        {
            if (!date.HasValue)
            {
                return "";
            }
            return date.Value.ToString("dd/MM/yyyy HH:mm");
        }
        /// <summary>
        /// Convert Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns>string(dd/MM HH:mm) or empty string</returns>
        public static string DateTimeTextShort(DateTime? date)
        {
            if (!date.HasValue)
            {
                return "";
            }
            return date.Value.ToString("dd/MM HH:mm");
        }
        /// <summary>
        /// Convert time
        /// </summary>
        /// <param name="date"></param>
        /// <returns>string(...giờ ... phút ) or empty string</returns>
        public static string TimeText(DateTime? date)
        {
            return date.HasValue ? $"{date.Value.ToString("HH")} giờ {date.Value.ToString("mm")} phút" : "";
        }
        /// <summary>
        /// format datetime birthday
        /// </summary>
        /// <param name="date"></param>
        /// <returns>string(ddMMyyyy)</returns>
        public static string BirthText(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("ddMMyyyy") : "";
        }
        /// <summary>
        /// Remove unwanted spaces from string
        /// </summary>
        /// <param name="s"></param>
        /// <returns>string</returns>
        public static string Trimmer(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return s;
            }
            var trimmer = new Regex(@"\s\s+");
            return trimmer.Replace(s, " ");
        }
        /// <summary>
        /// remove \t\n\r from string
        /// </summary>
        /// <param name="text"></param>
        /// <param name="rx"></param>
        /// <param name="replacement"></param>
        /// <returns>string or empty string</returns>
        public static string RegexStringReplace(string text, string rx = "[ ]{2,}", string replacement = " ")
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(rx, options);
            if (text == null)
            {
                return "";
            }
            return Regex.Replace(regex.Replace(text, replacement), "[\t\n\r]", "");
        }
        /// <summary>
        /// fill checkbox with option, key much start with Html(ex: HtmlKey)
        /// </summary>
        /// <param name="label"></param>
        /// <param name="values"></param>
        /// <param name="options"></param>
        /// <param name="optionsExtra"></param>
        /// <returns>☐ or ☑</returns>
        public static string StringOptionHanlder(string label, string values, OptionsBAComboDs[] options, List<OptionExtra> optionsExtra = null)
        {
            var dataValues = values != null ? values.Replace(" ", "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries) : new string[] { };
            string result = "";
            string charCheck = "<img src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAA5UlEQVQ4jZ3TMUrFQBgE4C8PPYGHsNDCQrHRQlCws/UIYmnlBSxtPYitaKOdpYIooqQWQbSweEXkhQnER+SZDCw//2RnmH+zCyWqgassIrzGhX7YwaYYHPYUi6YapVkYYFBrRrP3/cISFttEH4NV3OM4/biPwTKu8IGzcHNtgyp1DRtT4nXc4Svfb8MXbYNx6kF+6Xb6FVziO9zzdLTGYD71BE+5E0e4wWdGeOiarTEoUl8ywiTyaWJvhe9E1yG+YQ/n2MXjX2LNSXbgNSYz0SR4/8/mKdSaJsH+gOs8eUw1hj9nyh+E3UU2kf+J9wAAAABJRU5ErkJggg=='/>";
            string emptyChar = "<img src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAWUlEQVQ4jWNgYGB4wMDA8J9M/IARqnE9AwPDbgbSgCsDA0MgA9SAABI1M0D1/GeCciTJMACsh4mwOvxg1IBRA5ANeE6GXrAeFignjozkDMpMYEB+dmZgeAAAAwQn2d8tEdMAAAAASUVORK5CYII='/>";

            foreach (var item in options)
            {
                // var charFill = dataValues.Contains(item.Ma) ? charCheck : emptyChar;
                // result = result != "" ? $"{result},  {item.Ten} {charFill}" : $"{item.Ten} {charFill}";
                if (dataValues.Contains(item.Ma))
                {
                    result = result != "" ? $"{result}, {item.Ten} {charCheck}" : $"{label} {item.Ten} {charCheck}";
                    if (optionsExtra != null && optionsExtra.Count > 0)
                    {
                        for (int i = 0; i < optionsExtra.Count; i++)
                        {
                            if (item.Ten.ToLower() == optionsExtra[i].Ten.ToLower())
                            {
                                result = $"{result}{optionsExtra[i].Content}";
                                optionsExtra.RemoveAt(i);
                            }
                        }
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// fill checkbox key much start with Html(ex: HtmlKey)
        /// </summary>
        /// <param name="value"></param>
        /// <returns>☐ or ☑</returns>
        public static string StringOptionCheckBoxHander(byte? value)
        {
            string emptyChar = "<img src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAWUlEQVQ4jWNgYGB4wMDA8J9M/IARqnE9AwPDbgbSgCsDA0MgA9SAABI1M0D1/GeCciTJMACsh4mwOvxg1IBRA5ANeE6GXrAeFignjozkDMpMYEB+dmZgeAAAAwQn2d8tEdMAAAAASUVORK5CYII='/>";
            string charCheck = "<img src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAA5UlEQVQ4jZ3TMUrFQBgE4C8PPYGHsNDCQrHRQlCws/UIYmnlBSxtPYitaKOdpYIooqQWQbSweEXkhQnER+SZDCw//2RnmH+zCyWqgassIrzGhX7YwaYYHPYUi6YapVkYYFBrRrP3/cISFttEH4NV3OM4/biPwTKu8IGzcHNtgyp1DRtT4nXc4Svfb8MXbYNx6kF+6Xb6FVziO9zzdLTGYD71BE+5E0e4wWdGeOiarTEoUl8ywiTyaWJvhe9E1yG+YQ/n2MXjX2LNSXbgNSYz0SR4/8/mKdSaJsH+gOs8eUw1hj9nyh+E3UU2kf+J9wAAAABJRU5ErkJggg=='/>";
            return Convert.ToBoolean(value) ? $" {charCheck} " : $" {emptyChar} ";
        }
        /// <summary>
        /// fill text "abcd" -> [a][b][c][d]
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <param name="lengthTextNull"></param>
        /// <param name="emptyChar"></param>
        /// <param name="reverse"></param>
        public static void TexboxFieldHanlder(ref List<string> fields, ref List<string> values, string text, string key, int lengthTextNull, char emptyChar, bool reverse = false)
        {
            string result;
            text = text == null ? "" : text;
            if (reverse)
            {
                text = Reverse(text);
            }
            //if (!String.IsNullOrEmpty(text))
            //{
            //    text = Regex.Replace(text, @"[^a-zA-Z0-9.]", "");
            //}
            int length = text != null ? text.Length : 0;
            if (lengthTextNull - length < 0)
            {
                result = text.Substring(0, lengthTextNull);
            }
            else
            {
                string textNull = new String(emptyChar, lengthTextNull - length);
                result = textNull + text;
            }
            if (reverse)
            {
                result = Reverse(result);
            }
            for (int i = 0; i < result.Length; i++)
            {
                if (result.Length == 1)
                {
                    fields.Add($"{key}_{i}");
                    values.Add($"{text}");
                }
                else
                {
                    fields.Add($"{key}_{i}");
                    values.Add($"{result[i]}");
                }

            }
        }
        public static void TexboxFieldHanlderChamPhay(ref List<string> fields, ref List<string> values, string text, string key, int lengthTextNull, char emptyChar, bool reverse = false)
        {
            string result;
            text = text == null ? "" : text;
            if (reverse)
            {
                text = Reverse(text);
            }
            //if (!String.IsNullOrEmpty(text))
            //{
            //    text = Regex.Replace(text, @"[^a-zA-Z0-9.]", "");
            //}
            int length = text != null ? text.Length : 0;
            if (lengthTextNull - length < 0)
            {
                result = text.Substring(0, lengthTextNull);
            }
            else
            {
                string textNull = new String(emptyChar, lengthTextNull - length);
                result = textNull + text;
            }
            if (reverse)
            {
                result = Reverse(result);
            }
            for (int i = 0; i < result.Length; i++)
            {
                if (result.Length == 1)
                {
                    fields.Add($"{key}_{i}");
                    if (text.Length > 0)
                    {
                        values.Add($"{text}" + ";");
                    }
                    else
                    {
                        values.Add($"{text}");
                    }

                }
                else
                {
                    fields.Add($"{key}_{i}");
                    values.Add($"{result[i]}");
                }

            }
        }
        /// <summary>
        /// fill text box with key from option
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="conditionValues"></param>
        /// <param name="activeText"></param>
        public static void OptionFieldHanlder(ref List<string> fields, ref List<string> values, string key, object value, object[] conditionValues, string activeText = "x")
        {
            for (int i = 0; i < conditionValues.Length; i++)
            {
                fields.Add($"{key}_{i}");
                values.Add(value?.ToString() == conditionValues[i]?.ToString() ? "x" : "");
            }
        }

        /// <summary>
        /// Cắt object truyền lên lấy ra số
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="conditionValues"></param>
        /// <param name="activeText"></param>
        public static void OptionFieldValueHanlder(ref List<string> fields, ref List<string> values, string key, object value, object[] conditionValues, string activeText = "x")
        {
            string numberOnlyString = "";
            foreach (char c in value.ToString())
            {
                if (char.IsDigit(c))
                {
                    numberOnlyString += c;
                }
            }
            //for (int i = 0; i < conditionValues.Length; i++)
            //{
            //    if(numberOnlyString == conditionValues[i]?.ToString())
            //    {
            fields.Add($"{key}");
            values.Add(numberOnlyString);
            //    }

            //}
        }
        /// <summary>
        /// fill key with options
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="amount"></param>
        /// <param name="options"></param>
        public static void OptionFieldSyncHanlder(ref List<string> fields, ref List<string> values, string key, string value, int amount, string[] options)
        {
            var data = value != null ? value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();
            var result = new List<int>();
            foreach (var item in data)
            {
                var index = options.ToList().FindIndex(o => o == item);
                result.Add(index);
            }
            result = result.Distinct().ToList();
            for (int i = 0; i <= amount - 1; i++)
            {
                fields.Add($"{key}_{i}");
                values.Add(result.FindIndex(x => x == i) != -1 ? "x" : "");
            }
        }
        /// <summary>
        /// fill data with key_0 key_1 ... with options
        /// Lấy ra dấu x
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="amount"></param>
        /// <param name="options"></param>
        public static void OptionFieldSyncMultipleHanlder(ref List<string> fields, ref List<string> values, string key, string value, int amount, string[] options)
        {
            var data = value != null ? value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();
            var result = new List<string>();
            var temp = new List<int>();
            foreach (var item in data)
            {
                var index = options.ToList().FindIndex(o => o == item);
                if (index != -1)
                {
                    temp.Add(index + 1);
                }
            }
            result = temp.Distinct().ToList().OrderByDescending(x => x).Select(x => x.ToString()).ToList();
            for (int i = amount - 1; i >= 0; i--)
            {
                if (amount == 1)
                {
                    fields.Add($"{key}");
                }
                else
                {
                    fields.Add($"{key}_{i}");
                }
                values.Add(result.Count() != 0 ? "x" : "");
                if (result.Count() != 0)
                {
                    result.RemoveAt(0);
                }
            }
        }

        /// <summary>
        /// fill data with key_0 key_1 ... with options
        /// lấy ra số
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="amount"></param>
        /// <param name="options"></param>
        public static void OptionFieldTextMultipleHanlder(ref List<string> fields, ref List<string> values, string key, string value, int amount, string[] options)
        {
            var data = value != null ? value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();
            var result = new List<string>();
            var temp = new List<int>();

            foreach (var item in data)
            {
                var index = Array.IndexOf(options, item);
                if (index != -1)
                {
                    temp.Add(index + 1);
                }
            }

            result = temp.Distinct().OrderBy(x => x).Select(x => x.ToString()).ToList();

            for (int i = 0; i < amount; i++)
            {
                if (i < result.Count)
                {
                    fields.Add($"{key}_{i}");
                    values.Add($"{result[i]}");
                }
                else
                {
                    fields.Add($"{key}_{i}");
                    values.Add("");
                }
            }
        }

        /// <summary>
        /// compare 2 date
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns>number of days</returns>
        public static string CompareTwoDate(DateTime? t1, DateTime? t2, int numberIncrease = 0)
        {
            if (t1 == null || t2 == null)
            {
                return numberIncrease.ToString();
            }
            else
            {
                TimeSpan ts = t2.Value - t1.Value;
                var totalDay = ts.TotalDays;
                return ((int)Math.Floor(ts.TotalDays + numberIncrease)).ToString();
            }
        }
        /// <summary>
        /// 1. option 1 2. option 2  ->> [][1]
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="amount"></param>
        public static void OptionFieldMultipleHanlder(ref List<string> fields, ref List<string> values, string key, string value, int amount)
        {
            var data = value != null ? value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();

            for (int i = amount - 1; i >= 0; i--)
            {
                if (amount == 1)
                {
                    fields.Add($"{key}");
                }
                else
                {
                    fields.Add($"{key}_{i}");
                }
                values.Add(data.Count() != 0 ? data[0] : "");
                if (data.Count() != 0)
                {
                    data.RemoveAt(0);
                }
            }
        }

        /// <summary>
        /// add field and values [] bhyt [] thu phi [] mien [] khac 
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <param name="length"></param>
        /// <param name="active"></param>
        public static void TextboxFieldDoiTuongHanlder(ref List<string> fields, ref List<string> values, string text, string key = "doituong", int length = 4, char active = 'x')
        {
            string result = new String(' ', length);
            int index;
            if (!int.TryParse(text, out index))
            {
                index = 0;
            }
            for (int i = 1; i <= result.Length; i++)
            {
                fields.Add($"{key}_{i}");
                if (i == index)
                {
                    values.Add($"{active}");
                    continue;
                }
                values.Add($"{result[i - 1]}");
            }
        }
        /// <summary>
        /// change string order ex: abc -> cba
        /// </summary>
        /// <param name="s"></param>
        /// <returns>change string order</returns>
        private static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        public static void TextboxFieldBHYTHanlder(ref List<string> fields, ref List<string> values, string text, string key = "bhyt", bool isNotStartZero = false)
        {
            string[] bhytValues = {
                "",
                "",
                "",
                "",
                ""
            };
            text = text ?? "";
            if (text.Length == 10)
            {
                bhytValues[bhytValues.Length - 1] = text;
            }
            else if (text.Length == 15)
            {
                bhytValues[1] = text.Substring(0, 2);
                bhytValues[2] = text.Substring(2, 1);
                bhytValues[3] = text.Substring(3, 2);
                bhytValues[4] = text.Substring(5, 10);
            }
            for (int i = 0; i < bhytValues.Length; i++)
            {
                if (isNotStartZero)
                {
                    fields.Add($"{key}_{i + 1}");
                }
                else
                {
                    fields.Add($"{key}_{i}");
                }
                values.Add($"{bhytValues[i]}");
            }
        }
        public static void MailMergeValueAddHanlder(ref List<string> fields, ref List<string> values, string value, string key)
        {
            fields.Add(key);
            values.Add(value);
        }
        // public static void ImageHanlder(Document document, ref List<string> fields, ref List<string> values, string absolutePath, string key){
        // 	fields.Add(key);
        // 	values.Add(absolutePath);
        // 	document.MailMerge.MergeImageField += new MergeImageFieldEventHandler(MailMerge_MergeImageField);
        // }
        public static string[] CreateArrayIncreate(int end, int start = 1, int increment = 1)
        {
            return Enumerable
                .Repeat(start, (int)((end - start) / increment) + 1)
                .Select((tr, ti) => (tr + (increment * ti)).ToString())
                .ToArray();
        }
        private static void MailMerge_MergeImageField(object sender, MergeImageFieldEventArgs field)
        {
            try
            {
                string filePath = field.ImageFileName;

                if (string.IsNullOrEmpty(filePath) && field.Image == null)
                {
                    Console.WriteLine($"Field {field.FieldName} has no valid image data.");
                    return;
                }
                // Kiểm tra filePath hoặc field.Image có giá trị
                if (!string.IsNullOrEmpty(filePath) || field.Image != null)
                {
                    // Chỉ tải hình ảnh từ file nếu filePath không rỗng
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        // Sử dụng `using` để tự động giải phóng tài nguyên hình ảnh sau khi xử lý
                        using (var image = Image.FromFile(filePath))
                        {
                            field.Image = (Image)image.Clone();
                        }
                    }
                    if (field.CurrentMergeField?.OwnerParagraph?.OwnerTextBody?.Owner != null)
                    {
                        var textBoxFormat = field.CurrentMergeField.OwnerParagraph.OwnerTextBody.Owner as TextBox;
                        var rowTableFormat = field.CurrentMergeField.OwnerParagraph.OwnerTextBody.Owner as TableRow;

                        if (textBoxFormat != null)
                        {
                            // Điều chỉnh kích thước hình ảnh để vừa với TextBox
                            float width = field.Image.Size.Width;
                            float height = field.Image.Size.Height;

                            if (width != textBoxFormat.Width)
                            {
                                float scalePercentage = (float)(textBoxFormat.Width / width * 100);
                                width *= scalePercentage / 100;
                            }

                            if (height != textBoxFormat.Height)
                            {
                                float scalePercentage = (float)(textBoxFormat.Height / height * 100);
                                height *= scalePercentage / 100;
                            }

                            field.PictureSize = new System.Drawing.SizeF(width, height);
                        }
                        else if (rowTableFormat != null && rowTableFormat.HeightType == TableRowHeightType.Exactly)
                        {
                            // Điều chỉnh kích thước hình ảnh để vừa với TableRow
                            float width = field.Image.Size.Width;
                            float height = field.Image.Size.Height;
                            var cell = rowTableFormat.FirstChild as TableCell;

                            if (cell != null)
                            {
                                if (width != cell.Width)
                                {
                                    float scalePercentage = (float)(cell.Width / width * 100);
                                    width *= scalePercentage / 100;
                                }

                                if (height != rowTableFormat.Height)
                                {
                                    float scalePercentage = (float)(rowTableFormat.Height / height * 100);
                                    height *= scalePercentage / 100;
                                }

                                field.PictureSize = new System.Drawing.SizeF(width, height);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MailMerge_MergeImageField: {ex.Message}");
                throw; // Sử dụng `throw` để giữ nguyên stack trace
            }
        }

        //private static void MailMerge_MergeImageField(object sender, MergeImageFieldEventArgs field)
        //{
        //    try
        //    {
        //        string filePath = field.ImageFileName;
        //        if (!String.IsNullOrEmpty(filePath) || field.Image != null)
        //        {
        //            if (!String.IsNullOrEmpty(filePath))
        //            {
        //                field.Image = Image.FromFile(filePath);
        //            }
        //            var textBoxFormat = (field.CurrentMergeField.OwnerParagraph.OwnerTextBody.Owner as TextBox);
        //            var rowTableFormat = (field.CurrentMergeField.OwnerParagraph.OwnerTextBody.Owner as TableRow);
        //            if (textBoxFormat != null)
        //            {
        //                //Resizes the picture to fit within text box
        //                float scalePercentage = 100;
        //                float width = field.Image.Size.Width;
        //                float height = field.Image.Size.Height;
        //                if (width != textBoxFormat.Width)
        //                {
        //                    //Calculates value for width scale factor
        //                    scalePercentage = (float)(textBoxFormat.Width / width * 100);
        //                    //This will resize the width
        //                    width *= scalePercentage / 100;
        //                }
        //                scalePercentage = 100;
        //                if (height != textBoxFormat.Height)
        //                {
        //                    //Calculates value for height scale factor
        //                    scalePercentage = (float)(textBoxFormat.Height / height * 100);
        //                    //This will resize the height
        //                    height *= scalePercentage / 100;
        //                }
        //                field.PictureSize = new System.Drawing.SizeF(width, height);
        //            }
        //            else if (rowTableFormat != null && rowTableFormat.HeightType == TableRowHeightType.Exactly)
        //            {
        //                float scalePercentage = 100;
        //                float width = field.Image.Size.Width;
        //                float height = field.Image.Size.Height;
        //                var cell = rowTableFormat.FirstChild as TableCell;
        //                if (cell == null)
        //                {
        //                    return;
        //                }
        //                if (width != cell.Width)
        //                {
        //                    //Calculates value for width scale factor
        //                    scalePercentage = (float)(cell.Width / width * 100);
        //                    //This will resize the width
        //                    width *= scalePercentage / 100;
        //                }
        //                scalePercentage = 100;
        //                if (height != rowTableFormat.Height)
        //                {
        //                    //Calculates value for height scale factor
        //                    scalePercentage = (float)(rowTableFormat.Height / height * 100);
        //                    //This will resize the height
        //                    height *= scalePercentage / 100;
        //                }
        //                field.PictureSize = new System.Drawing.SizeF(width, height);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        private static Dictionary<Paragraph, Dictionary<int, string>> paraToInsertHTML = new Dictionary<Paragraph, Dictionary<int, string>>();

        private static void MailMerge_MergeHTMLField(object sender, MergeFieldEventArgs args)
        {
            try
            {
                if (args.FieldName.StartsWith("Html", StringComparison.OrdinalIgnoreCase))
                {
                    if (args.CurrentMergeField != null && args.CurrentMergeField.OwnerParagraph != null)
                    {
                        Paragraph paragraph = args.CurrentMergeField.OwnerParagraph;

                        int mergeFieldIndex = paragraph.ChildObjects.IndexOf(args.CurrentMergeField);

                        if (!string.IsNullOrEmpty(args.FieldValue?.ToString()))
                        {
                            Dictionary<int, string> fieldValues = new Dictionary<int, string>
                {
                    { mergeFieldIndex, args.FieldValue.ToString() }
                };

                            if (!paraToInsertHTML.ContainsKey(paragraph))
                            {
                                paraToInsertHTML.Add(paragraph, fieldValues);
                            }
                            else
                            {
                                paraToInsertHTML[paragraph].Add(mergeFieldIndex, args.FieldValue.ToString());
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Field {args.FieldName} has no valid value.");
                        }

                        args.Text = string.Empty;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MailMerge_MergeHTMLField: FieldName = {args.FieldName}, Error: {ex.Message}");
                WriteLog($"\nLỗi MailMerge_MergeHTMLField:FieldName = {args.FieldName}, Error: {ex.Message}");
                throw;
            }
        }
        //private static void MailMerge_MergeHTMLField(object sender, MergeFieldEventArgs args)
        //{
        //    try
        //    {

        //        //The merge fields that expect HTML data should be marked with some prefix, for instance,'Html'.
        //        if (args.FieldName.StartsWith("Html"))
        //        {
        //            //Gets the current merge field owner paragraph.
        //            Paragraph paragraph = args.CurrentMergeField.OwnerParagraph;
        //            //Gets the current merge field index in the current paragraph.
        //            int mergeFieldIndex = paragraph.ChildObjects.IndexOf(args.CurrentMergeField);
        //            //Maintain HTML in collection.
        //            Dictionary<int, string> fieldValues = new Dictionary<int, string>();
        //            fieldValues.Add(mergeFieldIndex, args.FieldValue.ToString());
        //            //Maintain paragraph in collection.
        //            paraToInsertHTML.Add(paragraph, fieldValues);
        //            //Set field value as empty.
        //            args.Text = string.Empty;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error in MailMerge_MergeImageField: {ex.Message}");
        //        throw;
        //    }
        //}
        private static void InsertHtml()
        {
            //Iterates through each item in the dictionary.
            foreach (KeyValuePair<Paragraph, Dictionary<int, string>> dictionaryItems in paraToInsertHTML)
            {
                Paragraph paragraph = dictionaryItems.Key as Paragraph;
                Dictionary<int, string> values = dictionaryItems.Value as Dictionary<int, string>;
                //Iterates through each value in the dictionary.
                foreach (KeyValuePair<int, string> valuePair in values)
                {
                    int index = valuePair.Key;
                    string fieldValue = valuePair.Value;
                    //Inserts an HTML string at the same position of mergefield in a Word document.
                    paragraph.OwnerTextBody.InsertXHTML(fieldValue, paragraph.OwnerTextBody.ChildObjects.IndexOf(paragraph), index);
                }
            }
        }
        public class OptionsBAComboDs
        {
            public string Ten { get; set; }
            public string Ma { get; set; }
        }

        public class OptionExtra
        {
            public string Ten { get; set; }
            public string Content { get; set; }
        }
    }
}
