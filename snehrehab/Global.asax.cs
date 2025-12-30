using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml.Linq;
using System.Collections.Generic;
using snehrehab.App_Start;
using System.Web.Mvc;
using System.Web.Http;

namespace snehrehab
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            //System.Timers.Timer timer = new System.Timers.Timer();
            ////Set interval of repeated execution in millisecond
            //timer.Interval = 900000;//15 MINUTE
            ////set name of the method to be called
            //timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            //timer.Start();
            //Application.Add("timer", timer);

            string _siteKey = ConfigurationManager.AppSettings.Get("SessionPrefix");
            DbHelper.Configuration.loginFullName = _siteKey + DbHelper.Configuration.loginFullName;
            DbHelper.Configuration.loginUserID = _siteKey + DbHelper.Configuration.loginUserID;
            DbHelper.Configuration.loginName = _siteKey + DbHelper.Configuration.loginName;
            DbHelper.Configuration.loginUserCat = _siteKey + DbHelper.Configuration.loginUserCat;
            DbHelper.Configuration.cookieUserID = _siteKey + DbHelper.Configuration.cookieUserID;
            DbHelper.Configuration.cookieCompID = _siteKey + DbHelper.Configuration.cookieCompID;
            DbHelper.Configuration.messageTextSession = _siteKey + DbHelper.Configuration.messageTextSession;
            DbHelper.Configuration.messageTypeSession = _siteKey + DbHelper.Configuration.messageTypeSession;

        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime _lastSentDate = SnehBLL.SettingsMst_Bll.GetRptSentDate().AddMinutes(600);
            if (_lastSentDate < DateTime.UtcNow.AddMinutes(330))
            {
                if (SnehBLL.SettingsMst_Bll.SendReport(_lastSentDate))
                {

                }
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 525600;                        
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}