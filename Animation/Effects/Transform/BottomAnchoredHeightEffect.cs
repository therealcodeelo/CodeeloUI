using System.Drawing;
using System.Windows.Forms;

namespace CodeeloUI.Animation.Effects.Transform
{
    public class BottomAnchoredHeightEffect : IEffect
    {
        public int GetCurrentValue(Control control) => control.Height;

        public void SetValue(Control control, int originalValue, int valueToReach, int newValue)
        {
            var size = new Size(control.Width, newValue);
            var location = new Point(control.Left, control.Top +
                (control.Height - newValue));

            control.Bounds = new Rectangle(location, size);
        }

        public int GetMinimumValue(Control control) 
            => control.MinimumSize.IsEmpty ? int.MinValue : control.MinimumSize.Height;


        public int GetMaximumValue(Control control)
            => control.MaximumSize.IsEmpty ? int.MaxValue : control.MaximumSize.Height;

        public EffectInteractions Interaction => EffectInteractions.BOUNDS;
    }
}
