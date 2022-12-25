using CodeeloUI.Enums;
using System.Windows.Forms;

namespace CodeeloUI.SupportClasses.Animation.Effects
{
    public interface IEffect
    {
        EffectInteractions Interaction { get; }
        int GetCurrentValue(Control control);
        void SetValue(Control control, int originalValue, int valueToReach, int newValue);
        int GetMinimumValue(Control control);
        int GetMaximumValue(Control control);
    }
}
