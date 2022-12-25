using System.Drawing;
using System.Windows.Forms;

namespace CodeeloUI.SupportClasses.Animation.Effects
{
    public static class ControlExtensions
    {
        public static Bitmap GetSnapshot(this Control control)
        {
            if (control.Width <= 0 || control.Height <= 0)
                return null;

            var image = new Bitmap(control.Width, control.Height);
            var targetBounds = new Rectangle(0, 0, control.Width, control.Height);

            control.DrawToBitmap(image, targetBounds);
            return image;
        }
        public static Bitmap GetFormBorderlessSnapshot(this Form window)
        {
            using (var bmp = new Bitmap(window.Width, window.Height))
            {
                window.DrawToBitmap(bmp, new Rectangle(0, 0, window.Width, window.Height));

                Point point = window.PointToScreen(Point.Empty);

                Bitmap target = new Bitmap(window.ClientSize.Width, window.ClientSize.Height);
                using (var graphics = System.Drawing.Graphics.FromImage(target))
                {
                    var srcRect = new Rectangle(point.X - window.Location.X,
                        point.Y - window.Location.Y, target.Width, target.Height);

                    graphics.DrawImage(bmp, 0, 0, srcRect, GraphicsUnit.Pixel);
                }

                return target;
            }
        }
    }
}
