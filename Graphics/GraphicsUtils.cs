using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using CodeeloUI.Controls;

namespace CodeeloUI.Graphics
{
    internal static class GraphicsUtils
    {
        #region [ Win32 API ]

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        #endregion

        #region [ DropShadow Класс]
        private static class DropShadow
        {
            const int CHANNELS = 4;
            const int InflateMultiple = 2;
            public static Rectangle GetBounds(GraphicsPath path, int radius, out Rectangle pathBounds, out int inflate)
            {
                var bounds = pathBounds = Rectangle.Ceiling(path.GetBounds());
                inflate = radius * InflateMultiple;
                bounds.Inflate(inflate, inflate);
                return bounds;
            }
            public static Rectangle GetBounds(Rectangle source, int radius)
            {
                var inflate = radius * InflateMultiple;
                source.Inflate(inflate, inflate);
                return source;
            }
            public static Bitmap Create(GraphicsPath path, Color color, int radius = 5)
            {
                var bounds = GetBounds(path, radius, out Rectangle pathBounds, out int inflate);
                var shadow = new Bitmap(bounds.Width, bounds.Height);

                if (color.A == 0)
                {
                    return shadow;
                }

                System.Drawing.Graphics g = null;
                GraphicsPath pathCopy = null;
                Matrix matrix = null;
                SolidBrush brush = null;
                try
                {
                    matrix = new Matrix();
                    matrix.Translate(-pathBounds.X + inflate, -pathBounds.Y + inflate);
                    pathCopy = (GraphicsPath)path.Clone();
                    pathCopy.Transform(matrix);

                    brush = new SolidBrush(color);

                    g = System.Drawing.Graphics.FromImage(shadow);
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.FillPath(brush, pathCopy);
                }
                finally
                {
                    g?.Dispose();
                    brush?.Dispose();
                    pathCopy?.Dispose();
                    matrix?.Dispose();
                }

                if (radius <= 0)
                {
                    return shadow;
                }

                BitmapData data = null;
                try
                {
                    data = shadow.LockBits(new Rectangle(0, 0, shadow.Width, shadow.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                    BoxBlur(data, radius, color);
                    BoxBlur(data, radius, color);

                    return shadow;
                }
                finally
                {
                    shadow.UnlockBits(data);
                }
            }

#if UNSAFE
            private static unsafe void BoxBlur(BitmapData data, int radius, Color color)
#else
            private static void BoxBlur(BitmapData data, int radius, Color color)
#endif
            {
#if UNSAFE 
                IntPtr p1 = data1.Scan0;
#else
                byte[] p1 = new byte[data.Stride * data.Height];
                Marshal.Copy(data.Scan0, p1, 0, p1.Length);
#endif
                byte R = color.R, G = color.G, B = color.B;
                for (int i = 3; i < p1.Length; i += 4)
                {
                    if (p1[i] == 0)
                    {
                        p1[i - 1] = R;
                        p1[i - 2] = G;
                        p1[i - 3] = B;
                    }
                }

                byte[] p2 = new byte[p1.Length];
                int radius2 = 2 * radius + 1;
                int First, Last, Sum;
                int stride = data.Stride,
                    width = data.Width,
                    height = data.Height;

                for (int r = 0; r < height; r++)
                {
                    int start = r * stride;
                    int left = start;
                    int right = start + radius * CHANNELS;

                    First = p1[start + 3];
                    Last = p1[start + stride - 1];
                    Sum = (radius + 1) * First;

                    for (int column = 0; column < radius; column++)
                    {
                        Sum += p1[start + column * CHANNELS + 3];
                    }
                    for (var column = 0; column <= radius; column++, right += CHANNELS, start += CHANNELS)
                    {
                        Sum += p1[right + 3] - First;
                        p2[start + 3] = (byte)(Sum / radius2);
                    }
                    for (var column = radius + 1; column < width - radius; column++, left += CHANNELS, right += CHANNELS, start += CHANNELS)
                    {
                        Sum += p1[right + 3] - p1[left + 3];
                        p2[start + 3] = (byte)(Sum / radius2);
                    }
                    for (var column = width - radius; column < width; column++, left += CHANNELS, start += CHANNELS)
                    {
                        Sum += Last - p1[left + 3];
                        p2[start + 3] = (byte)(Sum / radius2);
                    }
                }

                for (int column = 0; column < width; column++)
                {
                    int start = column * CHANNELS;
                    int top = start;
                    int bottom = start + radius * stride;

                    First = p2[start + 3];
                    Last = p2[start + (height - 1) * stride + 3];
                    Sum = (radius + 1) * First;

                    for (int row = 0; row < radius; row++)
                    {
                        Sum += p2[start + row * stride + 3];
                    }
                    for (int row = 0; row <= radius; row++, bottom += stride, start += stride)
                    {
                        Sum += p2[bottom + 3] - First;
                        p1[start + 3] = (byte)(Sum / radius2);
                    }
                    for (int row = radius + 1; row < height - radius; row++, top += stride, bottom += stride, start += stride)
                    {
                        Sum += p2[bottom + 3] - p2[top + 3];
                        p1[start + 3] = (byte)(Sum / radius2);
                    }
                    for (int row = height - radius; row < height; row++, top += stride, start += stride)
                    {
                        Sum += Last - p2[top + 3];
                        p1[start + 3] = (byte)(Sum / radius2);
                    }
                }
#if !UNSAFE
                Marshal.Copy(p1, 0, data.Scan0, p1.Length);
#endif
            }
        }
        #endregion

        internal static GraphicsPath GetTopSeparatorPart(RectangleF rect)
        {
            var thickness = rect.Width;
            var areaHeight = rect.Height;
            var arcHeight = rect.Height / 10;
            RectangleF topArc = new RectangleF(rect.X + thickness / 4, rect.Y, thickness / 2, 2 * arcHeight);
            GraphicsPath path = new GraphicsPath();

            path.StartFigure();
            path.AddArc(topArc, 180, 180);
            path.AddLine(rect.X + thickness / 4, rect.Y + arcHeight, rect.X + 3 * thickness / 4, rect.Y + arcHeight);
            path.AddLine(rect.X + 3 * thickness / 4, rect.Y + arcHeight, rect.X + thickness, rect.Y + areaHeight);
            path.AddLine(rect.X + thickness, rect.Y + areaHeight, rect.X, rect.Y + areaHeight);
            path.AddLine(rect.X, rect.Y + areaHeight, rect.X + thickness / 4, rect.Y + arcHeight);
            path.CloseFigure();

            return path;
        }
        internal static GraphicsPath GetBottomSeparatorPart(RectangleF rect)
        {
            var thickness = rect.Width;
            var areaHeight = 9 * rect.Height / 10;
            var arcHeight = rect.Height / 10;
            RectangleF topArc = new RectangleF(rect.X + thickness / 4, rect.Y + areaHeight - arcHeight, thickness / 2, arcHeight * 2);
            GraphicsPath path = new GraphicsPath();

            path.StartFigure();
            path.AddLine(rect.X + thickness / 4, rect.Y + areaHeight, rect.X, rect.Y);
            path.AddLine(rect.X, rect.Y, rect.X + thickness, rect.Y);
            path.AddLine(rect.X + thickness, rect.Y, rect.X + 3 * thickness / 4, rect.Y + areaHeight);
            path.AddLine(rect.X + 3 * thickness / 4, rect.Y + areaHeight, rect.X + thickness / 4, rect.Y + areaHeight);
            path.AddArc(topArc, 0, 180);
            path.CloseFigure();

            return path;
        }
        internal static GraphicsPath GetRightSeparatorPart(RectangleF rect)
        {
            var thickness = rect.Height;
            var areaWidth = 9 * rect.Width / 10;
            var arcWidth = rect.Width / 10;
            RectangleF topArc = new RectangleF(rect.X + areaWidth - arcWidth, rect.Y + thickness / 4, 2 * arcWidth, thickness / 2);
            GraphicsPath path = new GraphicsPath();

            PointF p1 = new PointF(rect.X, rect.Y);
            PointF p2 = new PointF(rect.X, rect.Y + thickness);
            PointF p3 = new PointF(rect.X + areaWidth, rect.Y + 3 * thickness / 4);
            PointF p4 = new PointF(rect.X + areaWidth, rect.Y + thickness / 4);
            
            path.StartFigure();
            path.AddLine(p4, p1);
            path.AddLine(p1, p2);
            path.AddLine(p2, p3);
            path.AddLine(p3, p4);
            path.AddArc(topArc, 270, 180);
            path.CloseFigure();

            return path;
        }
        internal static GraphicsPath GetLeftSeparatorPart(RectangleF rect)
        {
            var thickness = rect.Height;
            var areaWidth = rect.Width;
            var arcWidth = rect.Width / 10;
            RectangleF topArc = new RectangleF(rect.X, rect.Y + thickness / 4, 2 * arcWidth, thickness / 2);
            GraphicsPath path = new GraphicsPath();

            PointF p1 = new PointF(rect.X+ arcWidth, rect.Y + 3 * thickness / 4);
            PointF p2 = new PointF(rect.X+ arcWidth, rect.Y + thickness / 4);
            PointF p3 = new PointF(rect.X + areaWidth, rect.Y);
            PointF p4 = new PointF(rect.X + areaWidth, rect.Y + thickness);

            path.StartFigure();
            path.AddLine(p1, p2);
            path.AddLine(p2, p3);
            path.AddLine(p3, p4);
            path.AddLine(p4, p1);
            path.AddArc(topArc,90, 180);
            path.CloseFigure();

            return path;
        }
        internal static GraphicsPath GetFigurePath(Rectangle rect)
        {
            var heightPart = rect.Height / 10F;
            RectangleF topArc = new RectangleF(rect.X, rect.Y, rect.Width - 1F, heightPart);
            RectangleF area = new RectangleF(rect.X, rect.Y + heightPart / 2F, rect.Width - 1F, 9F * heightPart);
            RectangleF bottomArc = new RectangleF(rect.X, rect.Y + 9F * heightPart, rect.Width - 1F, heightPart);
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(topArc, 180, 180);
            path.AddRectangle(area);
            path.AddArc(bottomArc, 0, 180);
            path.CloseFigure();
            return path;
        }
        internal static GraphicsPath GetFigurePath(CodeeloToggleButton toggleButton)
        {
            int arcSize = toggleButton.Height - 1;
            Rectangle leftArc = new Rectangle(0, 0, arcSize, arcSize);
            Rectangle rightArc = new Rectangle(toggleButton.Width - arcSize - 2, 0, arcSize, arcSize);
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(leftArc, 90, 180);
            path.AddArc(rightArc, 270, 180);
            path.CloseFigure();
            return path;
        }
        internal static GraphicsPath GetFigurePath(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float diameter = radius * 2F;
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
        internal static GraphicsPath GetTopSideTriangle(float side, float X, float Y)
        {
            GraphicsPath path = new GraphicsPath();
            float h = (float)(side * Math.Sqrt(3) / 2);

            float startX = X;
            float middleX = startX + 0.5f * side;
            float endX = startX + side;

            float startY = Y;
            float middleY = Y + h;
            float endY = Y;

            path.StartFigure();
            path.AddLine(startX, startY, middleX, middleY);
            path.AddLine(middleX, middleY, endX, endY);
            path.AddLine(endX, endY, startX, startY);
            path.CloseFigure();
            return path;
        }
        internal static GraphicsPath GetBottomSideTriangle(float side, float X, float Y)
        {
            GraphicsPath path = new GraphicsPath();
            float h = (float)(side * Math.Sqrt(3) / 2);

            float startX = X;
            float middleX = startX + 0.5f * side;
            float endX = startX + side;

            float startY = Y + h;
            float middleY = Y;
            float endY = Y + h;

            path.StartFigure();
            path.AddLine(startX, startY, middleX, middleY);
            path.AddLine(middleX, middleY, endX, endY);
            path.AddLine(endX, endY, startX, startY);
            path.CloseFigure();
            return path;
        }
        public static Rectangle GetBounds(Rectangle rectangle, CodeeloBorder border = null, int shadowRadius = 0, int offsetX = 0, int offsetY = 0)
        {
            if (border != null)
            {
                rectangle = border.GetBounds(rectangle);
            }
            var boundsShadow = DropShadow.GetBounds(rectangle, shadowRadius);
            boundsShadow.Offset(offsetX, offsetY);
            return Rectangle.Union(rectangle, boundsShadow);
        }

        public static SizeF MeasureString(string text, Font font, int width = 0, StringFormat stringFormat = null) =>
            MeasureString(text, font, new SizeF(width, width > 0 ? 999999 : 0), stringFormat);

        public static SizeF MeasureString(string text, Font font, SizeF area, StringFormat stringFormat = null)
        {
            IntPtr dcScreen = IntPtr.Zero;
            System.Drawing.Graphics g = null;
            try
            {
                dcScreen = GetDC(IntPtr.Zero);
                g = System.Drawing.Graphics.FromHdc(dcScreen);
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                return g.MeasureString(text, font, area, stringFormat);
            }
            finally
            {
                g?.Dispose();
                if (dcScreen != IntPtr.Zero)
                {
                    ReleaseDC(IntPtr.Zero, dcScreen);
                }
            }
        }

        private static GraphicsPath GetRoundedRectangle(Rectangle rectangle, int radius)
        {
            radius = Math.Min(radius, Math.Min(rectangle.Width, rectangle.Height) / 2);

            var path = new GraphicsPath();
            if (radius < 1)
            {
                path.AddRectangle(rectangle);
                return path;
            }

            int d = radius * 2;
            var arc = new Rectangle(rectangle.X, rectangle.Y, d, d);
            path.AddArc(arc, 180, 90);
            arc.X = rectangle.X + rectangle.Width - d;
            path.AddArc(arc, 270, 90);
            arc.Y = rectangle.Y + rectangle.Height - d;
            path.AddArc(arc, 0, 90);
            arc.X = rectangle.X;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }

        public static void DrawRectangle(System.Drawing.Graphics g, Rectangle rectangle, Brush brush, CodeeloBorder border, int radius, Color shadowColor, int shadowRadius = 0, int offsetX = 0, int offsetY = 0)
        {
            if (shadowColor.A == 0 || (shadowRadius == 0 && offsetX == 0 && offsetY == 0))
            {
                DrawRectangle(g, rectangle, brush, border, radius);
                return;
            }

            GraphicsPath path = null;
            Bitmap shadow = null;
            try
            {
                path = GetRoundedRectangle(rectangle, radius);
                shadow = DropShadow.Create(path, shadowColor, shadowRadius);

                var shadowBounds = DropShadow.GetBounds(rectangle, shadowRadius);
                shadowBounds.Offset(offsetX, offsetY);

                g.DrawImageUnscaled(shadow, shadowBounds.Location);
                DrawPath(g, path, brush, border);
            }
            finally
            {
                path?.Dispose();
                shadow?.Dispose();
            }
        }

        public static void DrawRectangle(System.Drawing.Graphics g, Rectangle rectangle, Brush brush = null, CodeeloBorder border = null, int radius = 0)
        {
            using (var path = GetRoundedRectangle(rectangle, radius))
            {
                DrawPath(g, path, brush, border);
            }
        }
        public static void DrawPath(System.Drawing.Graphics g, GraphicsPath path, Brush brush = null, CodeeloBorder border = null)
        {
            if (CodeeloBorder.IsValid(border) && border.Behind)
            {
                g.DrawPath(border.Pen, path);
            }
            if (brush != null)
            {
                g.FillPath(brush, path);
            }
            if (CodeeloBorder.IsValid(border) && !border.Behind)
            {
                g.DrawPath(border.Pen, path);
            }
        }
    }
}
