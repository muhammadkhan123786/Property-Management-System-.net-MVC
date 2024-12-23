using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace _360PropertyManagement.Models
{
    public static class Extensions
    {
        private static string[] patternsToIgnore = new[] { "#", "$", "%", "2$", ","," " }; //expand it as you like;
        public static string Encrypt(string str)
        {
            string EncrptKey = "2013;[pnuLIT)WebCodeExpert";
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byKey = System.Text.Encoding.UTF8.GetBytes(EncrptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(str);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }

        public static Image ResizeImage(Image image)
        {
            int newWidth = 127;
            int newHeight = 127;
            Size size = new Size();
            size.Width = newWidth;
            size.Height = newHeight;
            Image newImage = new Bitmap(newWidth, newHeight);
            using (Graphics graphicsHandle = Graphics.FromImage(newImage))
            {
                graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

        public static Image ResizeCategoryImage(Image image)
        {
            int newWidth = 80;
            int newHeight = 65;
            Size size = new Size();
            size.Width = newWidth;
            size.Height = newHeight;
            Image newImage = new Bitmap(newWidth, newHeight);
            using (Graphics graphicsHandle = Graphics.FromImage(newImage))
            {
                graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

            public static string RemoveSpecialCharacters(this string str)
            {
                var v1 = str;
                foreach (var p in patternsToIgnore)
                {
                    v1 = v1.Replace(p, String.Empty);
                    
                }
                return v1;
            }
        
       
        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public static Image ResizeLogo(Image image)
        {
            int newWidth = 200;
            int newHeight = 150;
            Size size = new Size();
            size.Width = newWidth;
            size.Height = newHeight;
            Image newImage = new Bitmap(newWidth, newHeight);
            using (Graphics graphicsHandle = Graphics.FromImage(newImage))
            {
                graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

        public static Image ResizeSliderImage(Image image)
        {
            int newWidth = 550;
            int newHeight = 400;
            Size size = new Size();
            size.Width = newWidth;
            size.Height = newHeight;
            Image newImage = new Bitmap(newWidth, newHeight);
            using (Graphics graphicsHandle = Graphics.FromImage(newImage))
            {
                graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
            
        }

        //Custom Html Displayer
        public static MvcHtmlString Image(this HtmlHelper html, byte[] image)
        {
            var img = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(image));
            
            return new MvcHtmlString("<img src='" + img + "' />");

        }

            
        public static IHtmlString DisplayFormattedData(this HtmlHelper htmlHelper, string data)
            {
                if (string.IsNullOrEmpty(data))
                {
                    return MvcHtmlString.Empty;
                }

                string myString = data;
                myString = myString.Replace("\n", "<br>");

                return new HtmlString(myString);
            
        }


      
    }
}