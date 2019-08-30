using System.Drawing;
using System.IO;

namespace System.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class ImageExtensions
    {
        public static Stream ToStream(this Image image, Drawing.Imaging.ImageFormat format = null)
        {
            if (format == null) format = Drawing.Imaging.ImageFormat.Png;
            var ms = new MemoryStream();
            image.Save(ms, format);
            return ms;
        }
        public static byte[] ToBytes(this Image image, Drawing.Imaging.ImageFormat format = null)
        {
            if (format == null) format = Drawing.Imaging.ImageFormat.Png;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }
        public static Image ToImage(this byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            Image image = System.Drawing.Image.FromStream(ms);
            return image;
        }
    }
}
