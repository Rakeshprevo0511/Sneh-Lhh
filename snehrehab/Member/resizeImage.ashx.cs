using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace snehrehab.Member
{
    /// <summary>
    /// Summary description for resizeImage
    /// </summary>
    public class resizeImage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int width = 0; int height = 0;
            if (context.Request.QueryString["w"] != null) { int.TryParse(context.Request.QueryString["w"].ToString(), out width); }
            if (context.Request.QueryString["h"] != null) { int.TryParse(context.Request.QueryString["h"].ToString(), out height); }
            string file = string.Empty; if (context.Request.QueryString["record"] != null) { file = context.Request.QueryString["record"].ToString(); }
            if (width > 0 && height > 0 && !string.IsNullOrEmpty(file))
            {
                string fileName = width.ToString() + "x" + height.ToString() + "_" + file;
                string path = context.Server.MapPath("~/Files/") + fileName;
                if (File.Exists(path))
                {
                    context.Response.Clear();
                    context.Response.ContentType = getContentType(path);
                    context.Response.WriteFile(path);
                    context.Response.End();
                }
                else
                {
                    string OldPath = context.Server.MapPath("~/Files/") + file;
                    if (File.Exists(OldPath))
                    {
                        try
                        {
                            using (System.Drawing.Image img = System.Drawing.Image.FromFile(OldPath))
                            {
                                SnehBLL.ImageToolBLL IT = new SnehBLL.ImageToolBLL();
                                System.Drawing.Image thumb = IT.ResizeImage_New(img, width, height);
                                if (thumb != null)
                                {
                                    thumb.Save(context.Server.MapPath("~/Files/") + fileName);
                                }
                            }
                            path = context.Server.MapPath("~/Files/") + fileName;
                            context.Response.Clear();
                            context.Response.ContentType = getContentType(path);
                            context.Response.WriteFile(path);
                            context.Response.End();
                        }
                        catch
                        {
                            context.Response.Clear();
                            context.Response.ContentType = getContentType(OldPath);
                            context.Response.WriteFile(path);
                            context.Response.End();
                        }
                    }
                    else
                    {

                    }
                }
            }
            else
            {
                string path = context.Server.MapPath("~/Files/") + file;
                if (File.Exists(path))
                {
                    context.Response.Clear();
                    context.Response.ContentType = getContentType(path);
                    context.Response.WriteFile(path);
                    context.Response.End();
                }
                else
                {

                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private string getContentType(String path)
        {
            switch (Path.GetExtension(path))
            {
                case ".bmp": return "Image/bmp";
                case ".gif": return "Image/gif";
                case ".jpg": return "Image/jpeg";
                case ".jpeg": return "Image/jpeg";
                case ".png": return "Image/png";
                default: break;
            }
            return "";
        }
    }
}