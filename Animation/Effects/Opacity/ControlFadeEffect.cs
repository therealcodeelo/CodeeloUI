using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CodeeloUI.Animation.Effects.Opacity
{
    public class ControlFadeEffect : IEffect
    {
        private static Dictionary<Control, State> _controlCache = new Dictionary<Control, State>();

        public ControlFadeEffect(Control control)
        {
            if (!_controlCache.ContainsKey(control))
            {
                var parentGraphics = control.Parent.CreateGraphics();
                parentGraphics.CompositingQuality = CompositingQuality.HighSpeed;

                var state = new State()
                {
                    ParentGraphics = parentGraphics,
                    Opacity = control.Visible ? State.MAX_OPACITY : State.MIN_OPACITY,
                };

                _controlCache.Add(control, state);
            }
        }

        public int GetCurrentValue(Control control) => _controlCache[control].Opacity;

        public void SetValue(Control control, int originalValue, int valueToReach, int newValue)
        {
            var state = _controlCache[control];

            var region = new Region(state.PreviousBounds);
            
            region.Exclude(control.Bounds);
            control.Parent.Invalidate(region);

            var snapshot = control.GetSnapshot();
            
            if (snapshot != null)
            {
                snapshot = (Bitmap)snapshot.ChangeOpacity(newValue);
                var bgBlendedSnapshot = BlendWithBackgroundColor(snapshot, control.Parent.BackColor);
                state.Snapshot = bgBlendedSnapshot;
            }
            state.PreviousBounds = control.Bounds;

            if (newValue == State.MAX_OPACITY)
            {
                control.Visible = true;
                return;
            }

            control.Visible = false;
            state.Opacity = newValue;

            if (newValue > 0)
            {
                var rect = control.Parent.RectangleToClient(
                    control.RectangleToScreen(control.ClientRectangle));

                if (state.Snapshot != null)
                    state.ParentGraphics.DrawImage(state.Snapshot, rect);
            }
            else
            {
                control.Parent.Invalidate();
            }
        }

        public int GetMinimumValue(Control control) => State.MIN_OPACITY;

        public int GetMaximumValue(Control control) => State.MAX_OPACITY;

        public EffectInteractions Interaction => EffectInteractions.TRANSPARENCY;

        private Bitmap BlendWithBackgroundColor(Image image1,System.Drawing.Color bgColor)
        {
            var finalImage = new Bitmap(image1.Width, image1.Height);
            using (var g = System.Drawing.Graphics.FromImage(finalImage))
            {
                g.Clear(System.Drawing.Color.Black);

                g.FillRectangle(new SolidBrush(bgColor), new Rectangle(0, 0, image1.Width, image1.Height));
                g.DrawImage(image1, new Rectangle(0, 0, image1.Width, image1.Height));
            }

            return finalImage;
        }
    }
}
