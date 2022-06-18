using System.Drawing;
using System.Windows.Forms;

namespace CodeeloUI.Animation.Effects.Transform
{
    public class RightAnchoredWidthEffect : IEffect
    {
        public int GetCurrentValue(Control control) => control.Width;

        public void SetValue(Control control, int originalValue, int valueToReach, int newValue)
        {

            var size = new Size(newValue, control.Height);
            var location = new Point(control.Left +
                (control.Width - newValue), control.Top);

            control.Bounds = new Rectangle(location, size);
        }

        public int GetMinimumValue(Control control)
            => control.MinimumSize.IsEmpty ? int.MinValue : control.MinimumSize.Width;

        public int GetMaximumValue(Control control)
            => control.MaximumSize.IsEmpty ? int.MaxValue : control.MaximumSize.Width;

        public EffectInteractions Interaction => EffectInteractions.BOUNDS;
    }
}
