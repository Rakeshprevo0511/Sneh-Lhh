using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace SnehBLL
{
    public class ImageToolBLL
    {
        public System.Drawing.Image RezizeImage(System.Drawing.Image img, int maxWidth, int maxHeight)
        {
            if (img.Height < maxHeight && img.Width < maxWidth) return img;
            using (img)
            {
                Double xRatio = (double)img.Width / maxWidth;
                Double yRatio = (double)img.Height / maxHeight;
                Double ratio = Math.Max(xRatio, yRatio);
                int nnx = (int)Math.Floor(img.Width / ratio);
                int nny = (int)Math.Floor(img.Height / ratio);
                System.Drawing.Bitmap cpy = new System.Drawing.Bitmap(nnx, nny, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(cpy))
                {
                    gr.Clear(System.Drawing.Color.Transparent);

                    // This is said to give best quality when resizing images
                    gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                    gr.DrawImage(img,
                        new System.Drawing.Rectangle(0, 0, nnx, nny),
                        new System.Drawing.Rectangle(0, 0, img.Width, img.Height),
                        System.Drawing.GraphicsUnit.Pixel);
                }
                return cpy;
            }
        }

        public System.Drawing.Bitmap ResizeImage_New(System.Drawing.Image image, int width, int height)
        {
            //a holder for the result
            Bitmap result = new Bitmap(width, height);
            //set the resolutions the same to avoid cropping due to resolution differences
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //use a graphics object to draw the resized image into the bitmap
            using (Graphics graphics = Graphics.FromImage(result))
            {
                //set the resize quality modes to high quality
                graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Default;

                //draw the image into the target bitmap
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }

            //return the resulting bitmap
            return result;
        }

        public string ImageToBase64(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public string ImageToBase64(string path)
        {
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    return Convert.ToBase64String(imageBytes);
                }
            }
        }

        public string ImageToBase64(string path, System.Drawing.Imaging.ImageFormat format, int maxWidth, int maxHeight)
        {
            using (System.Drawing.Image img = System.Drawing.Image.FromFile(path))
            {
                using (img)
                {
                    int sourceWidth = img.Width;
                    int sourceHeight = img.Height;
                    float nPercent = ((float)maxWidth / (float)sourceWidth);
                    float hnPercent = ((float)maxHeight / (float)sourceHeight);

                    int destWidth = (int)(sourceWidth * nPercent);
                    int destHeight = (int)(sourceHeight * hnPercent);

                    System.Drawing.Bitmap cpy = new System.Drawing.Bitmap(destWidth, destHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    using (System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(cpy))
                    {
                        gr.Clear(System.Drawing.Color.Transparent);
                        gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        gr.DrawImage(img, 0, 0, destWidth, destHeight);
                    }
                    /*Double xRatio = (double)img.Width / maxWidth;
                    Double yRatio = (double)img.Height / maxHeight;
                    Double ratio = Math.Max(xRatio, yRatio);
                    int nnx = (int)Math.Floor(img.Width / ratio);
                    int nny = (int)Math.Floor(img.Height / ratio);
                    System.Drawing.Bitmap cpy = new System.Drawing.Bitmap(nnx, nny, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    using (System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(cpy))
                    {
                        gr.Clear(System.Drawing.Color.Transparent);

                        // This is said to give best quality when resizing images
                        gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                        gr.DrawImage(img,
                            new System.Drawing.Rectangle(0, 0, nnx, nny),
                            new System.Drawing.Rectangle(0, 0, img.Width, img.Height),
                            System.Drawing.GraphicsUnit.Pixel);
                    }*/
                    using (MemoryStream m = new MemoryStream())
                    {
                        cpy.Save(m, format);
                        byte[] imageBytes = m.ToArray();
                        return Convert.ToBase64String(imageBytes);
                    }
                }
            }
        }

        public System.Drawing.Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            System.IO.MemoryStream ms = new System.IO.MemoryStream(imageBytes, 0, imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            return image;
        }
    }
}
