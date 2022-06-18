using System;
using System.Windows.Forms;

namespace CodeeloUI.Animation.Effects.Color
{
    public class ColorShiftEffect : IEffect
    {
        public EffectInteractions Interaction => EffectInteractions.COLOR;

        public int GetCurrentValue(Control control) => control.BackColor.ToArgb();

        public void SetValue(Control control, int originalValue, int valueToReach, int newValue)
        {
            int actualValueChange = Math.Abs(originalValue - valueToReach);
            int currentValue = GetCurrentValue(control);

            double absoluteChangePerc = ((double)((originalValue - newValue) * 100)) / actualValueChange;
            absoluteChangePerc = Math.Abs(absoluteChangePerc);

            if (absoluteChangePerc > 100.0f)
                return;

            var originalColor = System.Drawing.Color.FromArgb(originalValue);
            var newColor = System.Drawing.Color.FromArgb(valueToReach);

            int newA = (int)Interpolate(originalColor.A, newColor.A, absoluteChangePerc);
            int newR = (int)Interpolate(originalColor.R, newColor.R, absoluteChangePerc);
            int newG = (int)Interpolate(originalColor.G, newColor.G, absoluteChangePerc);
            int newB = (int)Interpolate(originalColor.B, newColor.B, absoluteChangePerc);

            control.BackColor = System.Drawing.Color.FromArgb(newA, newR, newG, newB);
        }

        public int GetMinimumValue(Control control) => System.Drawing.Color.Black.ToArgb();

        public int GetMaximumValue(Control control) => System.Drawing.Color.White.ToArgb();

        private int Interpolate(int val1, int val2, double changePerc)
        {
            int difference = val2 - val1;
            int distance = (int)(difference * (changePerc / 100));
            return (int)(val1 + distance);
        }
    }
}
