using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace ZigBlog.Common.Validations
{
    public class ValidateImageFileAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;

            if (file == null)
                return false;

            if (!file.ContentType.Contains("image/"))
                return false;

            if (file.ContentLength <= 5120)
                return false;

            try
            {
                using (var image = Image.FromStream(file.InputStream))
                {
                    return image.RawFormat.Equals(ImageFormat.Jpeg)
                        || image.RawFormat.Equals(ImageFormat.Png)
                        || image.RawFormat.Equals(ImageFormat.Gif);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}