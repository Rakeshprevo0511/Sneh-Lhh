using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace SnehBLL
{
    public class ApiSms_Bll
    {
        DbHelper.SqlDb db;

        public ApiSms_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public SnehDLL.ApiSms_Dll Get()
        {
            SnehDLL.ApiSms_Dll D = null;
            SqlCommand cmd = new SqlCommand("ApiSms_Get"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.ApiSms_Dll();
                D.SmsID = int.Parse(dt.Rows[0]["SmsID"].ToString());
                D.SmsApi = dt.Rows[0]["SmsApi"].ToString();
            }
            return D;
        }

        public int Set(SnehDLL.ApiSms_Dll D)
        {
            SqlCommand cmd = new SqlCommand("ApiSms_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@SmsApi", SqlDbType.VarChar, 4000).Value = D.SmsApi;

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

        public bool Send(string _mobile, string _msg)
        {
            SnehDLL.ApiSms_Dll AD = Get();
            if (AD != null)
            {
                _msg = _msg.Replace("&", "and");
                try
                {
                    string _api = AD.SmsApi.Replace(DbHelper.Configuration.mobileString, _mobile.Trim());
                    _api = _api.Replace(DbHelper.Configuration.messageString, _msg.Trim());
                    HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(_api);
                    HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
                    System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
                    string responseString = respStreamReader.ReadToEnd();
                    respStreamReader.Close();
                    myResp.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
        
        public bool Send(string[] _mobiles, string _msg)
        {
            SnehDLL.ApiSms_Dll AD = Get();
            if (AD != null)
            {
                _msg = _msg.Replace("&", "and");
                try
                {
                    string _mobile = "";
                    for (int i = 0; i < _mobiles.Length; i++)
                    {
                        if (_mobiles[i].Length == 10)
                        {
                            _mobile += _mobiles[i] + ",";
                        }
                    }
                    if (_mobile.Length > 0) { _mobile = _mobile.Substring(0, _mobile.Length - 1); }

                    string _api = AD.SmsApi.Replace(DbHelper.Configuration.mobileString, _mobile.Trim());
                    _api = _api.Replace(DbHelper.Configuration.messageString, _msg.Trim());

                    HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(_api);
                    HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
                    System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
                    string responseString = respStreamReader.ReadToEnd();
                    respStreamReader.Close();
                    myResp.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public bool Send(string _mobile, string _msg, bool _usePromotional)
        {
            _msg = _msg.Replace("&", "and");
            try
            {
                string _api = "http://xpresssms.co.in/new/api/api_http.php?username=bingomagic&password=bingo@123&senderid=BingoM&to=" + DbHelper.Configuration.mobileString + "&text=" + DbHelper.Configuration.messageString + "&route=Enterprise&type=text&datetime=1";
                _api = _api.Replace(DbHelper.Configuration.mobileString, _mobile.Trim());
                _api = _api.Replace(DbHelper.Configuration.messageString, _msg.Trim());
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(_api);
                HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
                System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
                string responseString = respStreamReader.ReadToEnd();
                respStreamReader.Close();
                myResp.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
