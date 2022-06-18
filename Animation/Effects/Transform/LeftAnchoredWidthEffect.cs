using System.Windows.Forms;

namespace CodeeloUI.Animation.Effects.Transform
{
    public class LeftAnchoredWidthEffect : IEffect
    {
        public int GetCurrentValue(Control control) => control.Width;

        public void SetValue(Control control, int originalValue, int valueToReach, int newValue) 
            => control.Width = newValue;

        public int GetMinimumValue(Control control)
            => control.MinimumSize.IsEmpty ? int.MinValue : control.MinimumSize.Width;

        public int GetMaximumValue(Control control)
            => control.MaximumSize.IsEmpty ? int.MaxValue : control.MaximumSize.Width;

        public EffectInteractions Interaction => EffectInteractions.WIDTH;
    }
}
