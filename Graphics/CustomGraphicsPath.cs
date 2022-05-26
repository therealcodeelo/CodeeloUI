using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using CodeeloUI.Controls;

namespace CodeeloUI.Graphics
{
    internal class CustomGraphicsPath
    {
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
    }
}
