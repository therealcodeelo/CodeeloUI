using CodeeloUI.Enums;
using System.Windows.Forms;

namespace CodeeloUI.SupportClasses.Animation.Effects.Transform
{
    public class TopAnchoredHeightEffect : IEffect
    {
        public int GetCurrentValue(Control control) => control.Height;

        public void SetValue(Control control, int originalValue, int valueToReach, int newValue)
            => control.Height = newValue;

        public int GetMinimumValue(Control control)
            => control.MinimumSize.IsEmpty ? int.MinValue : control.MinimumSize.Height;

        public int GetMaximumValue(Control control)
            => control.MaximumSize.IsEmpty ? int.MaxValue : control.MaximumSize.Height;

        public EffectInteractions Interaction => EffectInteractions.HEIGHT;
    }
}
