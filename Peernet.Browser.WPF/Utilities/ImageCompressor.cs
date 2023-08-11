using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Peernet.Browser.WPF.Utilities
{
    internal class ImageCompressor
    {
        public static Image CompressImage(Image image, double widthCompressionRate, double heightCompressionRate, int newQuality)
        {
            using (Image resizedImage = new Bitmap(image, (int)Math.Round(image.Width * widthCompressionRate), (int)Math.Round(image.Height * heightCompressionRate)))
            {
                var imageCodecInfo = GetEncoderInfo("image/jpeg");
                var encoderParameter = new EncoderParameter(Encoder.Quality, newQuality);
                var encoderParameters = new EncoderParameters(1) { Param = new[] { encoderParameter } };

                var compressedImageMemoryStream = new MemoryStream();
                resizedImage.Save(compressedImageMemoryStream, imageCodecInfo, encoderParameters);
                return Image.FromStream(compressedImageMemoryStream);
            }
        }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codecInfo in encoders)
            {
                if (codecInfo.MimeType == mimeType)
                {
                    return codecInfo;
                }
            }

            return null;
        }
    }
}