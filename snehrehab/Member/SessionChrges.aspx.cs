using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Globalization;
using System.Data.SqlClient;

public partial class Member_SessionChrges : System.Web.UI.Page
{
    int _loginID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        _loginID = SnehBLL.UserAccount_Bll.IsLogin();
        if (_loginID <= 0)
        {
            Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
        }
        if (!IsPostBack)
        {
            txtSession.Items.Clear(); txtSession.Items.Add(new ListItem("Select Session", "-1"));
            SnehBLL.SessionMast_Bll PTB = new SnehBLL.SessionMast_Bll();
            foreach (SnehDLL.SessionMast_Dll PTD in PTB.GetList())
            {
                txtSession.Items.Add(new ListItem(PTD.SessionName, PTD.SessionID.ToString()));
            }

            LoadData();
        }
    }
    protected void txtChargeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //txtTimeWise.Checked = false; pnlTime.Visible = false;
        //int _type = 0; if (txtChargeType.SelectedItem != null) { int.TryParse(txtChargeType.SelectedItem.Value, out _type); }
        //if (_type == 2)
        //{
        //    int _sessionID = 0; if (txtSession.SelectedItem != null) { int.TryParse(txtSession.SelectedItem.Value, out _sessionID); }
        //    SnehBLL.SessionMast_Bll SMB = new SnehBLL.SessionMast_Bll();
        //    SnehDLL.SessionMast_Dll SMD = SMB.Get(_sessionID);
        //    if (SMD != null)
        //    {
        //        if (SMD.TimeWise)
        //        {
        //            pnlTime.Visible = true; txtTimeWise.Checked = true;
        //            SqlCommand cmd = new SqlCommand("SessionMst_TimeCharges"); cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = _sessionID;

        //            DbHelper.SqlDb db = new DbHelper.SqlDb(); DataSet ds = db.DbFetch(cmd);

        //            DataTable dt1 = new DataTable(); if (txtTimeWise.Checked) { if (ds.Tables.Count > 0) { dt1 = ds.Tables[0]; } }
        //            DataTable dt2 = new DataTable(); if (txtTimeWise.Checked) { if (ds.Tables.Count > 1) { dt2 = ds.Tables[1]; } }
        //            txtTime1.DataSource = dt1;
        //            txtTime1.DataBind();
        //            txtTime2.DataSource = dt2;
        //            txtTime2.DataBind();
        //            foreach (RepeaterItem item in txtTime2.Items)
        //            {
        //                TextBox txtAmount = item.FindControl("txtAmount") as TextBox;
        //                if (!SMD.MultipleDoctor)
        //                {
        //                    txtAmount.ReadOnly = true; txtAmount.Text = "";
        //                }
        //                else
        //                {
        //                    txtAmount.ReadOnly = false;
        //                }
        //            }
        //        }
        //    }
        //}
    }
    protected void txtSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtDoctor2.Enabled = false; txtDoctor2.Text = "";
        int _sessionID = 0; if (txtSession.SelectedItem != null) { int.TryParse(txtSession.SelectedItem.Value, out _sessionID); }
        SnehBLL.SessionMast_Bll SMB = new SnehBLL.SessionMast_Bll();
        SnehDLL.SessionMast_Dll SMD = SMB.Get(_sessionID);
        if (SMD != null)
        {
            if (SMD.MultipleDoctor)
            {
                txtDoctor2.Enabled = true;
            }
            if (txtChargeType.Items.FindByValue(SMD.ChargeType.ToString()) != null)
            {
                txtChargeType.SelectedValue = SMD.ChargeType.ToString();
            }
            //if (SMD.TimeWise)
            //{
            //    if (SMD.ChargeType == 2)
            //    {
            //        pnlTime.Visible = true; txtTimeWise.Checked = true;
            //        SqlCommand cmd = new SqlCommand("SessionMst_TimeCharges"); cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = _sessionID;

            //        DbHelper.SqlDb db = new DbHelper.SqlDb(); DataSet ds = db.DbFetch(cmd);

            //        DataTable dt1 = new DataTable(); if (txtTimeWise.Checked) { if (ds.Tables.Count > 0) { dt1 = ds.Tables[0]; } }
            //        DataTable dt2 = new DataTable(); if (txtTimeWise.Checked) { if (ds.Tables.Count > 1) { dt2 = ds.Tables[1]; } }
            //        txtTime1.DataSource = dt1;
            //        txtTime1.DataBind();
            //        txtTime2.DataSource = dt2;
            //        txtTime2.DataBind();
            //        foreach (RepeaterItem item in txtTime2.Items)
            //        {
            //            TextBox txtAmount = item.FindControl("txtAmount") as TextBox;
            //            if (!SMD.MultipleDoctor)
            //            {
            //                txtAmount.ReadOnly = true; txtAmount.Text = "";
            //            }
            //            else
            //            {
            //                txtAmount.ReadOnly = false;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        txtTimeWise.Checked = false; pnlTime.Visible = false;
            //    }
            //}
            //else
            //{
            //    txtTimeWise.Checked = false; pnlTime.Visible = false;
            //}
        }
    }

    protected void ChargesGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ChargesGV.PageIndex = e.NewPageIndex; LoadData();
    }

    private void LoadData()
    {
        if (txtSession.Items.Count > 0) { txtSession.SelectedIndex = 0; }
        if (txtChargeType.Items.Count > 0) { txtChargeType.SelectedIndex = 0; }
        txtDoctor1.Text = ""; txtDoctor2.Text = "";
        SnehBLL.SessionMast_Bll DB = new SnehBLL.SessionMast_Bll();
        ChargesGV.DataSource = DB.getCharges();
        ChargesGV.DataBind(); 
        if (ChargesGV.HeaderRow != null) { ChargesGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
        btn_update_charges.Visible = false;
        ChargesGV.Columns[5].Visible = false;
        if(_loginID == DbHelper.Configuration.managerLoginId) 
        {
            btn_update_charges.Visible = true;
            ChargesGV.Columns[5].Visible = true; 
        }
    }

    public string FORMATDATE(string _str)
    {
        DateTime _test = new DateTime(); DateTime.TryParseExact(_str, DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _test);
        if (_test > DateTime.MinValue)
            return _test.ToString(DbHelper.Configuration.showDateFormat);
        return "- - -";
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        int _sessionID = 0; if (txtSession.SelectedItem != null) { int.TryParse(txtSession.SelectedItem.Value, out _sessionID); }
        if (_sessionID <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select session...", 2); return;
        }
        int _type = 0; if (txtChargeType.SelectedItem != null) { int.TryParse(txtChargeType.SelectedItem.Value, out _type); }

        float _amount1 = 0; float.TryParse(txtDoctor1.Text.Trim(), out _amount1);
        float _amount2 = 0; float.TryParse(txtDoctor2.Text.Trim(), out _amount2);

        SnehBLL.SessionMast_Bll SMB = new SnehBLL.SessionMast_Bll();
        SnehDLL.SessionMast_Dll SMD = SMB.Get(_sessionID);
        if (SMD != null)
        {
            int i = SMB.setCharges(_sessionID, _type, _amount1, _amount2);
            if (i > 0)
            {
                //if (SMD.TimeWise)
                //{
                //    float amount = 0; int duration = 0; float amount1 = 0; int duration1 = 0;
                //    SqlCommand cmd1 = new SqlCommand("SessionMst_TimeCharges_Delete"); cmd1.CommandType = CommandType.StoredProcedure;
                //    cmd1.Parameters.Add("@SessionID", SqlDbType.Int).Value = _sessionID;
                //    DbHelper.SqlDb db1 = new DbHelper.SqlDb(); db1.DbRead(cmd1);

                //    if (!SMD.MultipleDoctor)
                //    {
                //        foreach (RepeaterItem item in txtTime1.Items)
                //        {
                //            TextBox txtAmount = item.FindControl("txtAmount") as TextBox;
                //            float.TryParse(txtAmount.Text, out amount);
                //            HiddenField txtduration = item.FindControl("hidDuration") as HiddenField;
                //            int.TryParse(txtduration.Value, out duration);

                //            if (amount > 0)
                //            {
                //                SqlCommand cmd = new SqlCommand("SessionMst_TimeCharges_Set"); cmd.CommandType = CommandType.StoredProcedure;
                //                cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = _sessionID;
                //                cmd.Parameters.Add("@DoctorNo", SqlDbType.Int).Value = 1;
                //                cmd.Parameters.Add("@Duration", SqlDbType.Int).Value = duration;
                //                cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = amount;

                //                DbHelper.SqlDb db = new DbHelper.SqlDb(); db.DbRead(cmd);
                //            }
                //        }
                //    }
                //    else
                //    {
                //        foreach (RepeaterItem item in txtTime1.Items)
                //        {
                //            TextBox txtAmount = item.FindControl("txtAmount") as TextBox;
                //            float.TryParse(txtAmount.Text, out amount);
                //            HiddenField txtduration = item.FindControl("hidDuration") as HiddenField;
                //            int.TryParse(txtduration.Value, out duration);

                //            if (amount > 0)
                //            {
                //                SqlCommand cmd = new SqlCommand("SessionMst_TimeCharges_Set"); cmd.CommandType = CommandType.StoredProcedure;
                //                cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = _sessionID;
                //                cmd.Parameters.Add("@DoctorNo", SqlDbType.Int).Value = 1;
                //                cmd.Parameters.Add("@Duration", SqlDbType.Int).Value = duration;
                //                cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = amount;

                //                DbHelper.SqlDb db = new DbHelper.SqlDb(); db.DbRead(cmd);
                //            }
                //        }
                //        foreach (RepeaterItem item in txtTime2.Items)
                //        {
                //            TextBox txtAmount = item.FindControl("txtAmount") as TextBox;
                //            float.TryParse(txtAmount.Text, out amount1);
                //            HiddenField txtduration = item.FindControl("hidDuration") as HiddenField;
                //            int.TryParse(txtduration.Value, out duration1);

                //            if (amount1 > 0)
                //            {
                //                SqlCommand cmd = new SqlCommand("SessionMst_TimeCharges_Set"); cmd.CommandType = CommandType.StoredProcedure;
                //                cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = _sessionID;
                //                cmd.Parameters.Add("@DoctorNo", SqlDbType.Int).Value = 2;
                //                cmd.Parameters.Add("@Duration", SqlDbType.Int).Value = duration1;
                //                cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = amount1;

                //                DbHelper.SqlDb db = new DbHelper.SqlDb(); db.DbRead(cmd);
                //            }
                //        }
                //    }
                //    txtTimeWise.Checked = false; pnlTime.Visible = false;
                //}
                Session[DbHelper.Configuration.messageTextSession] = "Session charges for doctor's saved successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";
                Response.Redirect(ResolveClientUrl("~/Member/SessionChrges.aspx"), true);
               
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
            }
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int _sessionID = 0; LinkButton lk = (LinkButton)sender;
        if (lk != null) { int.TryParse(lk.CommandArgument, out _sessionID); }
        if (_sessionID > 0)
        {
            SnehBLL.SessionMast_Bll SMB = new SnehBLL.SessionMast_Bll();
            int i = SMB.setCharges(_sessionID, 0, 0, 0);
            if (i > 0)
            {
                LoadData();
                DbHelper.Configuration.setAlert(Page, "session charges for doctor's removed successfully...", 1);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
            }
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
        }
    }
}
