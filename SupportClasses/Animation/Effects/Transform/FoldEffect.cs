using CodeeloUI.Enums;
using System;
using System.Windows.Forms;

namespace CodeeloUI.SupportClasses.Animation.Effects.Transform
{
    public class FoldEffect : IEffect
    {
        public EffectInteractions Interaction => EffectInteractions.BOUNDS;

        public int GetCurrentValue(Control control) => control.Width * control.Height;

        public void SetValue(Control control, int originalValue, int valueToReach, int newValue)
        {
            int actualValueChange = Math.Abs(originalValue - valueToReach);
            int currentValue = GetCurrentValue(control);

            double absoluteChangePerc = ((double)((originalValue - newValue) * 100)) / actualValueChange;
            absoluteChangePerc = Math.Abs(absoluteChangePerc);

            if (absoluteChangePerc > 100.0f)
                return;
        }

        public int GetMinimumValue(Control control)
            => control.MinimumSize.IsEmpty ? 0 : control.MinimumSize.Width * control.MinimumSize.Height;


        public int GetMaximumValue(Control control)
            => control.MaximumSize.IsEmpty ? int.MaxValue : control.MaximumSize.Width * control.MaximumSize.Height;
    }
}
