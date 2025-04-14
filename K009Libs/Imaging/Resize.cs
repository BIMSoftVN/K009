using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;


namespace K009Libs.Imaging
{
    public class Resize
    {
        public static byte[] ResizeImage(string inputPath, int width, int height)
        {
            byte[] imageBytes = File.ReadAllBytes(inputPath);

            using (var inputStream = new MemoryStream(imageBytes))
            using (var originalImage = Image.FromStream(inputStream))
            using (var resizedImage = new Bitmap(width, height))
            {
                resizedImage.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);

                using (var graphics = Graphics.FromImage(resizedImage))
                {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    var destRect = new Rectangle(0, 0, width, height);
                    using (var wrapMode = new ImageAttributes())
                    {
                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                        graphics.DrawImage(originalImage, destRect, 0, 0, originalImage.Width, originalImage.Height, GraphicsUnit.Pixel, wrapMode);
                    }
                }

                using (var outputStream = new MemoryStream())
                {
                    resizedImage.Save(outputStream, originalImage.RawFormat);
                    return outputStream.ToArray();
                }
            }
        }
    }
}
