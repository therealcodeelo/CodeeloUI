using CodeeloUI.Enums;
using System;
using System.Windows.Forms;

namespace CodeeloUI.SupportClasses.Animation.Effects.Opacity
{
    public class FormFadeEffect : IEffect
    {
        public int GetCurrentValue(Control control)
        {
            if (!(control is Form))
                throw new Exception("Fading effect can be applied only on forms");

            var form = (Form)control;
            return (int)(form.Opacity * 100);
        }

        public void SetValue(Control control, int originalValue, int valueToReach, int newValue)
        {
            if (!(control is Form))
                throw new Exception("Fading effect can be applied only on forms");

            var form = (Form)control;
            form.Opacity = ((float)newValue) / 100;
        }

        public int GetMinimumValue(Control control) => State.MIN_OPACITY;

        public int GetMaximumValue(Control control) => State.MAX_OPACITY;

        public EffectInteractions Interaction => EffectInteractions.TRANSPARENCY;
    }
}
