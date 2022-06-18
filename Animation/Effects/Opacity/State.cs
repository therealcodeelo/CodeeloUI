using System.Drawing;

namespace CodeeloUI.Animation.Effects.Opacity
{
    internal class State
    {
        internal const int MAX_OPACITY = 100;
        internal const int MIN_OPACITY = 0;
        public int Opacity { get; set; }
        public System.Drawing.Graphics ParentGraphics { get; set; }
        public Rectangle PreviousBounds { get; set; }
        public Bitmap Snapshot { get; set; }
    }
}
