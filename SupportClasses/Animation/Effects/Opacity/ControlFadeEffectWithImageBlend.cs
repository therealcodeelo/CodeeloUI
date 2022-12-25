using CodeeloUI.Enums;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace CodeeloUI.SupportClasses.Animation.Effects.Opacity
{
    public class ControlFadeEffectWithImageBlend : IEffect
    {
        private static Dictionary<Control, State> _controlCache
            = new Dictionary<Control, State>();

        public ControlFadeEffectWithImageBlend(Control control)
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

            var form = control.FindForm();
            var formRelativeCoords = form.RectangleToClient(control.RectangleToScreen(control.ClientRectangle));

            var controlSnapshot = control.GetSnapshot();
            if (controlSnapshot != null)
            {
                controlSnapshot = (Bitmap)controlSnapshot.ChangeOpacity(newValue);

                var formSnapshot = form.GetFormBorderlessSnapshot().Clone(formRelativeCoords, PixelFormat.DontCare);
                var blendedSnapshot = BlendImages(formSnapshot, controlSnapshot);

                state.Snapshot = blendedSnapshot;
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
                form.CreateGraphics().DrawImage(state.Snapshot, formRelativeCoords);
            else
                control.Parent.Invalidate();
        }

        public int GetMinimumValue(Control control) => State.MIN_OPACITY;

        public int GetMaximumValue(Control control) => State.MAX_OPACITY;

        public EffectInteractions Interaction => EffectInteractions.TRANSPARENCY;

        private Bitmap BlendImages(Image image1, Image image2)
        {
            var finalImage = new Bitmap(image1.Width, image1.Height);
            using (var g = Graphics.FromImage(finalImage))
            {
                g.Clear(System.Drawing.Color.Black);

                g.DrawImage(image1, new Rectangle(0, 0, image1.Width, image1.Height));
                g.DrawImage(image2, new Rectangle(0, 0, image1.Width, image1.Height));
            }

            return finalImage;
        }
    }
}
