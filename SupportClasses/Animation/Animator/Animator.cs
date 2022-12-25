using CodeeloUI.Enums;
using CodeeloUI.SupportClasses.Animation.Effects;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace CodeeloUI.SupportClasses.Animation.Animator
{
    public static class Animator
    {
        public static event EventHandler<AnimationStatus> Animated;
        public static AnimationStatus Animate(Control control, IEffect iEffect,
            EasingDelegate easing, int valueToReach, int duration, int delay,
            bool reverse = false, int loops = 1)
        {
            var stopwatch = new Stopwatch();
            var cancelTokenSource = new CancellationTokenSource();
            var animationStatus = new AnimationStatus(cancelTokenSource, stopwatch);

            new System.Threading.Timer((state) =>
            {
                int originalValue = iEffect.GetCurrentValue(control);
                if (originalValue == valueToReach)
                {
                    animationStatus.IsCompleted = true;
                    return;
                }

                int maxVal = iEffect.GetMaximumValue(control);
                if (valueToReach > maxVal)
                {
                    var msg = "Value must be lesser than the maximum allowed. " +
                              $"Max: {maxVal}, provided value: {valueToReach}";

                    throw new ArgumentException(msg, nameof(valueToReach));
                }

                int minVal = iEffect.GetMinimumValue(control);
                if (valueToReach < iEffect.GetMinimumValue(control))
                {
                    var msg = "Value must be greater than the minimum allowed. " +
                              $"Min: {minVal}, provided value: {valueToReach}";

                    throw new ArgumentException(msg, nameof(valueToReach));
                }

                bool reversed = false;
                int performedLoops = 0;

                int actualValueChange = Math.Abs(originalValue - valueToReach);

                var animationTimer = new System.Timers.Timer();

                animationTimer.Interval = (duration > actualValueChange) ?
                    (duration / actualValueChange) : actualValueChange;

                if (iEffect.Interaction == EffectInteractions.COLOR)
                    animationTimer.Interval = 10;

                animationTimer.Elapsed += (o, e2) =>
                {
                    if (cancelTokenSource.Token.IsCancellationRequested)
                    {
                        animationStatus.IsCompleted = true;
                        animationTimer.Stop();
                        stopwatch.Stop();
                        return;
                    }

                    bool increasing = originalValue < valueToReach;

                    int minValue = Math.Min(originalValue, valueToReach);
                    int maxValue = Math.Abs(valueToReach - originalValue);
                    int newValue = (int)easing(stopwatch.ElapsedMilliseconds, minValue, maxValue, duration);

                    if (!increasing)
                        newValue = (originalValue + valueToReach) - newValue - 1;

                    control.BeginInvoke(new MethodInvoker(() =>
                    {
                        iEffect.SetValue(control, originalValue, valueToReach, newValue);

                        bool timeout = stopwatch.ElapsedMilliseconds >= duration;
                        if (timeout)
                        {
                            if (reverse && (!reversed || loops <= 0 || performedLoops < loops))
                            {
                                reversed = !reversed;
                                if (reversed)
                                    performedLoops++;

                                int initialValue = originalValue;
                                int finalValue = valueToReach;

                                valueToReach = valueToReach == finalValue ? initialValue : finalValue;
                                originalValue = valueToReach == finalValue ? initialValue : finalValue;

                                stopwatch.Restart();
                                animationTimer.Start();
                            }
                            else
                            {
                                animationStatus.IsCompleted = true;
                                animationTimer.Stop();
                                stopwatch.Stop();

                                if (Animated != null)
                                    Animated(control, animationStatus);
                            }
                        }
                    }));
                };

                stopwatch.Start();
                animationTimer.Start();

            }, null, delay, Timeout.Infinite);

            return animationStatus;
        }
        public static AnimationStatus Animate(Control control, IEffect iEffect,
            EasingDelegate easing, int valueToReach, int duration, int delay, Action doSomething,
            bool reverse = false, int loops = 1)
        {
            var stopwatch = new Stopwatch();
            var cancelTokenSource = new CancellationTokenSource();
            var animationStatus = new AnimationStatus(cancelTokenSource, stopwatch);

            new System.Threading.Timer((state) =>
            {
                int originalValue = iEffect.GetCurrentValue(control);
                if (originalValue == valueToReach)
                {
                    animationStatus.IsCompleted = true;
                    return;
                }

                int maxVal = iEffect.GetMaximumValue(control);
                if (valueToReach > maxVal)
                {
                    var msg = "Value must be lesser than the maximum allowed. " +
                              $"Max: {maxVal}, provided value: {valueToReach}";

                    throw new ArgumentException(msg, nameof(valueToReach));
                }

                int minVal = iEffect.GetMinimumValue(control);
                if (valueToReach < iEffect.GetMinimumValue(control))
                {
                    var msg = "Value must be greater than the minimum allowed. " +
                              $"Min: {minVal}, provided value: {valueToReach}";

                    throw new ArgumentException(msg, nameof(valueToReach));
                }

                bool reversed = false;
                int performedLoops = 0;

                int actualValueChange = Math.Abs(originalValue - valueToReach);

                var animationTimer = new System.Timers.Timer();

                animationTimer.Interval = (duration > actualValueChange) ?
                    (duration / actualValueChange) : actualValueChange;

                if (iEffect.Interaction == EffectInteractions.COLOR)
                    animationTimer.Interval = 10;

                animationTimer.Elapsed += (o, e2) =>
                {
                    if (cancelTokenSource.Token.IsCancellationRequested)
                    {
                        animationStatus.IsCompleted = true;
                        animationTimer.Stop();
                        stopwatch.Stop();
                        return;
                    }

                    bool increasing = originalValue < valueToReach;

                    int minValue = Math.Min(originalValue, valueToReach);
                    int maxValue = Math.Abs(valueToReach - originalValue);
                    int newValue = (int)easing(stopwatch.ElapsedMilliseconds, minValue, maxValue, duration);

                    if (!increasing)
                        newValue = (originalValue + valueToReach) - newValue - 1;

                    control.BeginInvoke(new MethodInvoker(() =>
                    {
                        iEffect.SetValue(control, originalValue, valueToReach, newValue);

                        bool timeout = stopwatch.ElapsedMilliseconds >= duration;

                        if (stopwatch.ElapsedMilliseconds > duration / 3.5f
                        && stopwatch.ElapsedMilliseconds < duration / 3)
                            doSomething();

                        if (timeout)
                        {
                            if (reverse && (!reversed || loops <= 0 || performedLoops < loops))
                            {
                                reversed = !reversed;
                                if (reversed)
                                    performedLoops++;

                                int initialValue = originalValue;
                                int finalValue = valueToReach;

                                valueToReach = valueToReach == finalValue ? initialValue : finalValue;
                                originalValue = valueToReach == finalValue ? initialValue : finalValue;

                                stopwatch.Restart();
                                animationTimer.Start();
                            }
                            else
                            {
                                animationStatus.IsCompleted = true;
                                animationTimer.Stop();
                                stopwatch.Stop();
                                if (Animated != null)
                                    Animated(control, animationStatus);
                            }
                        }
                    }));
                };
                stopwatch.Start();
                animationTimer.Start();

            }, null, delay, Timeout.Infinite);
            return animationStatus;
        }
    }
}
