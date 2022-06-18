using System.Windows.Forms;

namespace CodeeloUI.Animation.Effects.Transform
{
    public class XLocationEffect : IEffect
    {
        public int GetCurrentValue(Control control) => control.Left;

        public void SetValue(Control control, int originalValue, int valueToReach, int newValue) => control.Left = newValue;

        public int GetMinimumValue(Control control) => int.MinValue;

        public int GetMaximumValue(Control control) => int.MaxValue;

        public EffectInteractions Interaction => EffectInteractions.X;
    }
}
