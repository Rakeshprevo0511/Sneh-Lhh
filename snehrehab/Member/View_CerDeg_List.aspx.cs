using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.Member
{
    public partial class View_CerDeg_List : System.Web.UI.Page
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
                LoadData();
            }
        }

        private void LoadData()
        {
            int type = 0; if (txtType.SelectedItem != null) { int.TryParse(txtType.SelectedItem.Value, out type); }
            int id = 0; if (txtid.SelectedItem != null) { int.TryParse(txtid.SelectedItem.Value, out id); }
            SnehBLL.Degree_Bll RB = new SnehBLL.Degree_Bll(); DataTable dt = new DataTable();

            if (type == 1)
            {
                dt = RB.Get_CerImage(0, 0, id);
            }
            else if (type == 2)
            {
                dt = RB.Get_CerImage(0, id, 0);
            }
            else if (type == 3)
            {
                dt = RB.Get_CerImage(id, 0, 0);
            }
            else
            {
                dt = RB.Get_CerImage(0, 0, 0);
            }
            if (dt.Rows.Count > 0)
            {
                //rptCer.DataSource = dt;
                //rptCer.DataBind();

                txtMain.DataSource = dt;
                txtMain.DataBind();

                //PopulatePager(dt.Rows.Count, pageIndex);
            }
            else
            {
                txtMain.DataSource = dt;
                txtMain.DataBind();

                //rptCer.DataSource = null;
                //rptCer.DataBind();
            }

        }

        //protected void rptCer_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        HiddenField recid = e.Item.FindControl("hidrecid") as HiddenField;
        //        HiddenField docid = e.Item.FindControl("hiddocid") as HiddenField;
        //        HiddenField manid = e.Item.FindControl("hidmanid") as HiddenField;
        //        HiddenField hidnew = e.Item.FindControl("hidnewid") as HiddenField;
        //        Repeater rptrInner = e.Item.FindControl("rptinner") as Repeater;

        //        int reciptionid = 0; int.TryParse(recid.Value, out reciptionid);
        //        int doctorid = 0; int.TryParse(docid.Value, out doctorid);
        //        int managerid = 0; int.TryParse(manid.Value, out managerid);
        //        int idnew = 0; int.TryParse(hidnew.Value, out idnew);
        //        SnehBLL.Degree_Bll RB = new SnehBLL.Degree_Bll();
        //        List<SnehDLL.Degree_Dll> DD = new List<SnehDLL.Degree_Dll>();
        //        DataTable dt = RB.Get_CerData(idnew,reciptionid,managerid,doctorid);
        //        if (dt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                SnehDLL.Degree_Dll D = new SnehDLL.Degree_Dll();
        //                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
        //                D.Image_Path = (dt.Rows[i]["Image_Path"].ToString().Length > 0 ? "<a><img src=\"/Files/" + dt.Rows[i]["Image_Path"].ToString() + "\"style=\"width: 100%;\"/></a>" : string.Empty);
        //                D.Image = dt.Rows[i]["Image_Path"].ToString();
        //                DD.Add(D);
        //            }
        //            rptrInner.DataSource = DD;
        //            rptrInner.DataBind();
        //        }

        //    }
        //}

        protected void txtType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int type = 0; if (txtType.SelectedItem != null) { int.TryParse(txtType.SelectedItem.Value, out type); }
            DataTable dt = new DataTable();
            if (type == 1)
            {
                SnehBLL.Receiption_Bll RB = new SnehBLL.Receiption_Bll();
                dt = RB.GetListNew(); txtid.Items.Clear(); txtid.Items.Add(new ListItem("Select Receiptionist", "-1"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    txtid.Items.Add(new ListItem(dt.Rows[i]["FullName"].ToString(), dt.Rows[i]["ReceiptionID"].ToString()));
                }
            }
            else if (type == 2)
            {
                SnehBLL.Management_Bll MB = new SnehBLL.Management_Bll();
                dt = MB.GetListNew(); txtid.Items.Clear(); txtid.Items.Add(new ListItem("Select Management", "-1"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    txtid.Items.Add(new ListItem(dt.Rows[i]["FullName"].ToString(), dt.Rows[i]["ManagerID"].ToString()));
                }
            }
            else if (type == 3)
            {
                txtid.Items.Clear(); txtid.Items.Add(new ListItem("Select Doctor", "-1"));
                SnehBLL.DoctorMast_Bll DMB = new SnehBLL.DoctorMast_Bll();
                foreach (SnehDLL.DoctorMast_Dll DMD in DMB.GetForDropdown())
                {
                    txtid.Items.Add(new ListItem(DMD.PreFix + " " + DMD.FullName, DMD.DoctorID.ToString()));
                }
            }
            else
            {
                txtid.Items.Clear();
            }
        }

        //protected void btnSaveRefer_Click(object sender, EventArgs e)
        //{
        //    string uniqueid = string.Empty; int i = 0;

        //    if (txtdegcer.HasFile)
        //    {
        //        uniqueid = txtuniqueid.Value;
        //        if (uniqueid != null)
        //        {
        //            string _filePath = "d_" + DateTime.UtcNow.AddMinutes(330).Ticks.ToString() + txtdegcer.FileName.Substring(txtdegcer.FileName.LastIndexOf('.'));
        //            string _filename = txtdegcer.FileName;
        //            SnehBLL.Receiption_Bll RB = new SnehBLL.Receiption_Bll(); SnehBLL.Degree_Bll DB = new SnehBLL.Degree_Bll();
        //            if (RB.SaveFile(ref txtdegcer, _filePath))
        //            {
        //                i = DB.Update(uniqueid, _filePath,_filename);
        //            }

        //        }
        //        else
        //        {
        //            DbHelper.Configuration.setAlert(msgmodal, "Unable to process.", 2);
        //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script>$('#edit_cer_modal').modal('show').off('hidden.bs.modal');</script>", false);
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        DbHelper.Configuration.setAlert(msgmodal, "Please select file.", 2);
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script>$('#edit_cer_modal').modal('show').off('hidden.bs.modal');</script>", false);
        //        return;
        //    }

        //    if (i > 0)
        //    {
        //        Session[DbHelper.Configuration.messageTextSession] =  "Certificate updated successfully..";
        //        Session[DbHelper.Configuration.messageTypeSession] = "1";
        //        Response.Redirect(ResolveClientUrl("/Member/View_CerDeg_List.aspx"), true);
        //    }
        //}

        //private void PopulatePager(int recordCount, int currentPage)
        //{
        //    double dblPageCount = (double)((decimal)recordCount / Convert.ToDecimal(PageSize));
        //    int pageCount = (int)Math.Ceiling(dblPageCount);
        //    List<ListItem> pages = new List<ListItem>();
        //    if (pageCount > 0)
        //    {
        //        for (int i = 1; i <= pageCount; i++)
        //        {
        //            pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
        //        }
        //    }
        //    rptPager.DataSource = pages;
        //    rptPager.DataBind();
        //}

        //protected void Page_Changed(object sender, EventArgs e)
        //{
        //    int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        //    LoadData(pageIndex);
        //}

        protected void btnSearch__Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void txtMain_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Checking the RowType of the Row  
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField recid = e.Row.FindControl("hidrecid") as HiddenField;
                HiddenField docid = e.Row.FindControl("hiddocid") as HiddenField;
                HiddenField manid = e.Row.FindControl("hidmanid") as HiddenField;
                HiddenField hidnew = e.Row.FindControl("hidnewid") as HiddenField;
                Repeater rptrInner = e.Row.FindControl("rptinner") as Repeater;

                int reciptionid = 0; int.TryParse(recid.Value, out reciptionid);
                int doctorid = 0; int.TryParse(docid.Value, out doctorid);
                int managerid = 0; int.TryParse(manid.Value, out managerid);
                int idnew = 0; int.TryParse(hidnew.Value, out idnew);
                SnehBLL.Degree_Bll RB = new SnehBLL.Degree_Bll();
                List<SnehDLL.Degree_Dll> DD = new List<SnehDLL.Degree_Dll>();
                DataTable dt = RB.Get_CerData(idnew, reciptionid, managerid, doctorid);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SnehDLL.Degree_Dll D = new SnehDLL.Degree_Dll();
                        D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                        //D.Image_Path = (dt.Rows[i]["Image_Path"].ToString().Length > 0 ? "<a><img src=\"/Files/" + dt.Rows[i]["Image_Path"].ToString() + "\"style=\"width: 100%;\"/></a>" : string.Empty);
                        //D.Image_Path = dt.Rows[i]["Image_Path"].ToString();
                        D.Image = dt.Rows[i]["Image_Path"].ToString();
                        DD.Add(D);
                    }
                    rptrInner.DataSource = DD;
                    rptrInner.DataBind();
                }
            }
        }

        protected void txtMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            txtMain.PageIndex = e.NewPageIndex; LoadData();
        }
    }
}