using System.Drawing;
using System.Drawing.Imaging;

namespace CodeeloUI.SupportClasses.Animation.Effects
{
    public static class ImageExtensions
    {
        public static Image ChangeOpacity(this Image image, int opacity)
        {
            opacity = opacity < 0 ? 0 : opacity;
            opacity = opacity > 100 ? 100 : opacity;

            Bitmap bmp = new Bitmap(image.Width, image.Height);

            using (var graphics = Graphics.FromImage(bmp))
            {
                var matrix = new ColorMatrix();

                matrix.Matrix33 = ((float)opacity) / 100;

                var attributes = new ImageAttributes();

                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                graphics.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            }

            return bmp;
        }
    }
}
