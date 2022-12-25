using CodeeloUI.Enums;
using System.Windows.Forms;

namespace CodeeloUI.SupportClasses.Animation.Effects.Transform
{
    public class YLocationEffect : IEffect
    {
        public int GetCurrentValue(Control control) => control.Top;

        public void SetValue(Control control, int originalValue, int valueToReach, int newValue)
            => control.Top = newValue;

        public int GetMinimumValue(Control control) => int.MinValue;

        public int GetMaximumValue(Control control) => int.MaxValue;

        public EffectInteractions Interaction => EffectInteractions.Y;
    }
}
