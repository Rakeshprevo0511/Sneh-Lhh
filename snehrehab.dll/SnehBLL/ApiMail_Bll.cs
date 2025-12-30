using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.IO;
using System.Web;

namespace SnehBLL
{
    public class ApiMail_Bll
    {
        DbHelper.SqlDb db;

        public ApiMail_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public SnehDLL.ApiMail_Dll Get()
        {
            SnehDLL.ApiMail_Dll D = null;
            SqlCommand cmd = new SqlCommand("ApiMail_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@MailID", SqlDbType.Int).Value = 1;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.ApiMail_Dll();
                D.MailID = int.Parse(dt.Rows[0]["MailID"].ToString());
                D.SenderName = dt.Rows[0]["SenderName"].ToString();
                D.Smtp = dt.Rows[0]["Smtp"].ToString();
                D.EmailAddress = dt.Rows[0]["EmailAddress"].ToString();
                D.Password = dt.Rows[0]["Password"].ToString();
                int PortNo = 0; int.TryParse(dt.Rows[0]["PortNo"].ToString(), out PortNo);
                D.PortNo = PortNo;
                bool SslEnabled = false; bool.TryParse(dt.Rows[0]["SslEnabled"].ToString(), out SslEnabled);
                D.SslEnabled = SslEnabled;

            }
            return D;
        }

        public int Set(SnehDLL.ApiMail_Dll D)
        {
            SqlCommand cmd = new SqlCommand("ApiMail_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@MailID", SqlDbType.Int).Value = D.MailID;
            cmd.Parameters.Add("@SenderName", SqlDbType.VarChar, 50).Value = D.SenderName;
            cmd.Parameters.Add("@Smtp", SqlDbType.VarChar, 50).Value = D.Smtp;
            cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 50).Value = D.EmailAddress;
            cmd.Parameters.Add("@Password", SqlDbType.VarChar, 50).Value = D.Password;
            cmd.Parameters.Add("@PortNo", SqlDbType.Int).Value = D.PortNo;
            cmd.Parameters.Add("@SslEnabled", SqlDbType.Bit).Value = D.SslEnabled;

            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@RetVal";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);

            db.DbUpdate(cmd);

            int i = 0;
            if (cmd.Parameters["@RetVal"].Value != null)
            {
                int.TryParse(cmd.Parameters["@RetVal"].Value.ToString(), out i);
            }
            return i;
        }

        public bool send(string _mailto, string _body, string _subject)
        {
            SnehDLL.ApiMail_Dll D = Get();
            if (D != null)
            {
                if (DbHelper.Configuration.isValidEmail(_mailto))
                {
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(D.EmailAddress);
                    mail.To.Add(_mailto);
                    mail.Subject = _subject;
                    mail.Body = _body;
                    mail.IsBodyHtml = true;

                    SmtpClient SmtpServer = new SmtpClient(D.Smtp);
                    SmtpServer.Port = D.PortNo;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(D.EmailAddress, D.Password);
                    SmtpServer.EnableSsl = D.SslEnabled;
                    try
                    {
                        SmtpServer.Send(mail);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        public bool send(string[] _mailto, string _body, string _subject)
        {
            SnehDLL.ApiMail_Dll D = Get();
            if (D != null)
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(D.EmailAddress);
                for (int i = 0; i < _mailto.Length; i++)
                {
                    if (DbHelper.Configuration.isValidEmail(_mailto[i]))
                    {
                        mail.To.Add(_mailto[i]);
                    }
                }
                mail.Subject = _subject;
                mail.Body = _body;
                mail.IsBodyHtml = true;
                SmtpClient SmtpServer = new SmtpClient(D.Smtp);
                SmtpServer.Port = D.PortNo;
                SmtpServer.Credentials = new System.Net.NetworkCredential(D.EmailAddress, D.Password);
                SmtpServer.EnableSsl = D.SslEnabled;
                try
                {
                    SmtpServer.Send(mail);
                    return true;
                }
                catch
                {
                    return false;
                }

            }
            return false;
        }

        public bool send(string[] _mailto, string[] _bccto, string _body, string _subject)
        {
            SnehDLL.ApiMail_Dll D = Get();
            if (D != null)
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(D.EmailAddress);
                for (int i = 0; i < _mailto.Length; i++)
                {
                    if (DbHelper.Configuration.isValidEmail(_mailto[i]))
                    {
                        mail.To.Add(_mailto[i]);
                    }
                }
                for (int i = 0; i < _bccto.Length; i++)
                {
                    if (DbHelper.Configuration.isValidEmail(_bccto[i]))
                    {
                        mail.Bcc.Add(_bccto[i]);
                    }
                }
                mail.Subject = _subject;
                mail.Body = _body;
                mail.IsBodyHtml = true;
                SmtpClient SmtpServer = new SmtpClient(D.Smtp);
                SmtpServer.Port = D.PortNo;
                SmtpServer.Credentials = new System.Net.NetworkCredential(D.EmailAddress, D.Password);
                SmtpServer.EnableSsl = D.SslEnabled;
                try
                {
                    SmtpServer.Send(mail);
                    return true;
                }
                catch
                {
                    return false;
                }

            }
            return false;
        }

        public bool sendAttachment(string _mailto, string _body, string _subject, string _file)
        {

            try
            {

                SnehDLL.ApiMail_Dll D = Get();
                if (D != null)
                {
                    if (DbHelper.Configuration.isValidEmail(_mailto))
                    {
                        MailMessage mail = new MailMessage();
                        mail.From = new MailAddress(D.EmailAddress, D.SenderName);
                        mail.To.Add(_mailto);
                        mail.Subject = _subject;
                        mail.Body = _body;
                        mail.IsBodyHtml = true;
                        FileStream stream = null;
                        if (File.Exists(_file))
                        {
                            FileInfo fi = new FileInfo(_file);
                            stream = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read);
                            Attachment data = new Attachment(stream, fi.Name, DbHelper.MIMEType.Get(fi.Extension));
                            mail.Attachments.Add(data);
                        }
                        //Attachment data = new Attachment(_file, MediaTypeNames.Application.Octet);
                        //mail.Attachments.Add(data);//Attach the file  

                        SmtpClient client = new SmtpClient();
                        client.UseDefaultCredentials = true;
                        client.Credentials = new System.Net.NetworkCredential(D.EmailAddress, D.Password);
                        client.Port = D.PortNo; // You can use Port 25 if 587 is blocked (mine is!)
                        client.Host = D.Smtp;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.EnableSsl = D.SslEnabled;

                        //client.UseDefaultCredentials = true;
                        //client.Credentials = new System.Net.NetworkCredential("snehrehab002@gmail.com", "brgplzwdplhgajjl");
                        //client.Port = 587; // You can use Port 25 if 587 is blocked (mine is!)
                        //client.Host = "smtp.gmail.com";
                        //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        //client.EnableSsl = true;

                        client.Send(mail);
                        if (stream != null)
                        {
                            try { stream.Close(); stream.Dispose(); }
                            catch (Exception ex)
                            {
                                SnehBLL.ApiMail_Bll AM = new SnehBLL.ApiMail_Bll();
                                AM.Mail_LOG_SET(1, "Send Mail apimailbal 227", "", ex.StackTrace, ex.Message);
                            }
                        }
                        return true;

                    }
                }
            }
            catch (Exception ex)
            {
                ////if (stream != null) { try { stream.Close(); stream.Dispose(); } catch { } }

                //Console.WriteLine("Catch clause caught : {0} \n", ex.Message);

                //string LogValue = ex.ToString();
                //string PageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                SnehBLL.ApiMail_Bll AM = new SnehBLL.ApiMail_Bll();
                AM.Mail_LOG_SET(1, "Send Mail apimailbal 240", "", ex.StackTrace, ex.Message);
                return false;

                // Log the exception details to a .log file
                //LogExceptionToFile(ex);
            }
            return false;
        }


        static void LogExceptionToFile(Exception ex)
        {
            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;
            string logFilePath = HttpContext.Current.Server.MapPath("error.log");

            // Append the exception details to the log file
            using (StreamWriter sw = File.AppendText(logFilePath))
            {
                sw.WriteLine($"Timestamp: {DateTime.Now}");
                sw.WriteLine($"Exception Type: {ex.GetType().FullName}");
                sw.WriteLine($"Message: {ex.Message}");
                sw.WriteLine($"Stack Trace: {ex.StackTrace}");
                sw.WriteLine(new string('-', 50));
            }

            Console.WriteLine($"Exception details logged to {logFilePath}");
        }
        public long Mail_LOG_SET(int MailID, string PageTitle, string PageUrl, string LogMsg, string LogValue)
        {
            SqlCommand cmd = new SqlCommand("Mail_LOG_SET"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@MailID", SqlDbType.BigInt).Value = MailID;
            cmd.Parameters.Add("@PageTitle", SqlDbType.NVarChar, 500).Value = PageTitle;
            cmd.Parameters.Add("@PageUrl", SqlDbType.NVarChar, 4000).Value = PageUrl;
            cmd.Parameters.Add("@LogMsg", SqlDbType.NVarChar, 500).Value = LogMsg;
            cmd.Parameters.Add("@LogValue", SqlDbType.NVarChar, 2000).Value = LogValue;

            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@RetVal";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);

            db.DbUpdate(cmd);

            long i = 0;

            if (cmd.Parameters["@RetVal"].Value != null)
            {
                long.TryParse(cmd.Parameters["@RetVal"].Value.ToString(), out i);
            }
            return i;
        }
    }
}
