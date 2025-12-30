using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Globalization;
using System.Web.UI;

namespace DbHelper
{
    public class Configuration
    {
        public static string dateFormat = "yyyy-MM-dd HH:mm:ss";
        public static string dateFormattt = "yyyy-MM-dd hh:mm:ss tt";
        public static string timeFormat = "HH:mm:ss";
        public static string showDateFormat = "dd/MM/yyyy";
        public static string showTimeFormat = "hh:mm tt";
        public static string showMonthFormat = "MM/yyyy";
        public static string loginFullName = "3cac2111-7335-410f-ae18-1eb9211f6863";
        public static string loginUserID = "13cfbe23-e7d7-4b8a-b510-2dd72287a995";
        public static string loginName = "d9f548f8-b68e-4e56-96cd-c8492334e5b3";
        public static string loginUserCat = "105f9a1e-a9de-416a-afe2-11f23e25a52e";
        public static string cookieUserID = "044552da-8dc8-43cb-8395-4c9aed3774f4";
        public static string cookieCompID = "105f9a1e-b68e-416a-afe2-1eb9211f6863";

        public static string LogoutURL = "~/";
        public static string SessionOutURL = "~/login.aspx";

        public static string FileFolder = "../Files/";
        public static string messageTextSession = "messageTextSession";
        public static string messageTypeSession = "messageTypeSession";

        public static string mobileString = "**********";
        public static string messageString = "##########";

        public static string serverAddress = "http://localhost:23970/";
        //public static string serverAddress = "http://lhh.snehrehab.in/";
        public static int managerLoginId = 13;


        private static Regex isGuid = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);

        public static bool IsGuid(string candidate)
        {
            bool isValid = false;
            if (candidate != null)
            {
                if (isGuid.IsMatch(candidate))
                {
                    isValid = true;
                }
            }
            return isValid;
        }

        public static bool isValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

        static string Passphrase = "This is my new snehrehab in";

        public static string Encrypt(string Message)
        {
            try
            {
                byte[] Results;
                System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

                // Step 1. We hash the passphrase using MD5
                // We use the MD5 hash generator as the result is a 128 bit byte array
                // which is a valid length for the TripleDES encoder we use below

                MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
                byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

                // Step 2. Create a new TripleDESCryptoServiceProvider object
                TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

                // Step 3. Setup the encoder
                TDESAlgorithm.Key = TDESKey;
                TDESAlgorithm.Mode = CipherMode.ECB;
                TDESAlgorithm.Padding = PaddingMode.PKCS7;

                // Step 4. Convert the input string to a byte[]
                byte[] DataToEncrypt = UTF8.GetBytes(Message);

                // Step 5. Attempt to encrypt the string
                try
                {
                    ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                    Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
                }
                finally
                {
                    // Clear the TripleDes and Hashprovider services of any sensitive information
                    TDESAlgorithm.Clear();
                    HashProvider.Clear();
                }

                // Step 6. Return the encrypted string as a base64 encoded string
                return Convert.ToBase64String(Results);
            }
            catch
            {
                return "";
            }
        }

        public static string Decrypt(string Message)
        {
            try
            {
                byte[] Results;
                System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

                // Step 1. We hash the passphrase using MD5
                // We use the MD5 hash generator as the result is a 128 bit byte array
                // which is a valid length for the TripleDES encoder we use below

                MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
                byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

                // Step 2. Create a new TripleDESCryptoServiceProvider object
                TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

                // Step 3. Setup the decoder
                TDESAlgorithm.Key = TDESKey;
                TDESAlgorithm.Mode = CipherMode.ECB;
                TDESAlgorithm.Padding = PaddingMode.PKCS7;

                // Step 4. Convert the input string to a byte[]
                byte[] DataToDecrypt = Convert.FromBase64String(Message.Replace(' ', '+'));

                // Step 5. Attempt to decrypt the string
                try
                {
                    ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                    Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
                }
                finally
                {
                    // Clear the TripleDes and Hashprovider services of any sensitive information
                    TDESAlgorithm.Clear();
                    HashProvider.Clear();
                }

                // Step 6. Return the decrypted string in UTF8 format
                return UTF8.GetString(Results);
            }
            catch
            {
                return "";
            }

        }

        public static string ReceiptNo(long ReceiptNo)
        {
            string receiptNo = ReceiptNo.ToString();
            int minNoLength = 4; if (receiptNo.Length < minNoLength) { int loopTo = minNoLength - receiptNo.Length; for (int i = 0; i < loopTo; i++) { receiptNo = ("0" + receiptNo); } }
            return receiptNo;
        }

        public static string RandomString(System.Int32 length)
        {
            string possibles = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            char[] passwords = new char[length];
            Random rd = new Random();

            for (int i = 0; i < length; i++)
            {
                passwords[i] = possibles[rd.Next(0, possibles.Length)];
            }
            return new string(passwords);
        }

        public static string RandomNumber(System.Int32 length)
        {
            string possibles = "0123456789";
            char[] passwords = new char[length];
            Random rd = new Random();

            for (int i = 0; i < length; i++)
            {
                passwords[i] = possibles[rd.Next(0, possibles.Length)];
            }
            return new string(passwords);
        }

        public static string FORMATDATE(string _str)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            if (_test > DateTime.MinValue)
                return _test.ToString(DbHelper.Configuration.showDateFormat);
            return "- - -";
        }

        public static string FORMATLONGDATE(string _str)
        {
            DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
            if (_test > DateTime.MinValue)
                return _test.ToString("dd/MM/yyyy hh:mm tt");
            return "- - -";
        }

        public static void ClearPlaceHolder(Control ctrls)
        {
            foreach (Control ctrl in ctrls.Controls)
            {
                if (ctrl is LiteralControl)
                    ctrls.Controls.Remove(ctrl);
            }
        }


        /// <summary>
        /// 1 Success, 2 Error 3 Information 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="_alertMsg"></param>
        /// <param name="_type"></param>
        public static void setAlert(System.Web.UI.Page p, string _alertMsg, int _type)
        {
            System.Web.UI.WebControls.PlaceHolder MsgPlace = p.Master.FindControl("MsgPlaceHolder") as System.Web.UI.WebControls.PlaceHolder;
            string _html = "";
            if (MsgPlace != null)
            {
                switch (_type)
                {
                    case 1: _html = "<div class=\"alert alert-success alert-dismissible\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span></button><strong>Success !</strong> " + _alertMsg + "</div>";
                        break;
                    case 2: _html = "<div class=\"alert alert-danger alert-dismissible\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span></button><strong>Error !</strong> " + _alertMsg + "</div>";
                        break;
                    case 3: _html = "<div class=\"alert alert-info alert-dismissible\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span></button><strong>Info !</strong> " + _alertMsg + "</div>";
                        break;
                    default: _html = "";
                        break;
                }

                MsgPlace.Controls.Add(new System.Web.UI.LiteralControl(_html));
            }
        }

        public static void setAlert(System.Web.UI.WebControls.PlaceHolder MsgPlace, string _alertMsg, int _type)
        {
            string _html = "";
            if (MsgPlace != null)
            {
                switch (_type)
                {
                    case 1: _html = "<div class=\"alert alert-success alert-dismissible\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span></button><strong>Success !</strong> " + _alertMsg + "</div>";
                        break;
                    case 2: _html = "<div class=\"alert alert-danger alert-dismissible\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span></button><strong>Error !</strong> " + _alertMsg + "</div>";
                        break;
                    case 3: _html = "<div class=\"alert alert-info alert-dismissible\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span></button><strong>Info !</strong> " + _alertMsg + "</div>";
                        break;
                    default: _html = "";
                        break;
                }

                MsgPlace.Controls.Add(new System.Web.UI.LiteralControl(_html));
            }
        }

        public static string setAlert(string _alertMsg, int _type)
        {
            string _html = "";
            switch (_type)
            {
                case 1: _html = "<div class=\"alert alert-success alert-dismissible\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span></button><strong>Success !</strong> " + _alertMsg + "</div>";
                    break;
                case 2: _html = "<div class=\"alert alert-danger alert-dismissible\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span></button><strong>Error !</strong> " + _alertMsg + "</div>";
                    break;
                case 3: _html = "<div class=\"alert alert-info alert-dismissible\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">&times;</span></button><strong>Info !</strong> " + _alertMsg + "</div>";
                    break;
                default: _html = "";
                    break;
            }
            return _html;
        }

        public static string MakeValidFilename(string text)
        {
            text = text.Replace('\'', '’'); // U+2019 right single quotation mark
            text = text.Replace('"', '”'); // U+201D right double quotation mark
            text = text.Replace('/', '⁄');  // U+2044 fraction slash
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                text = text.Replace(c, '_');
            }
            return text;
        }
    }
}
