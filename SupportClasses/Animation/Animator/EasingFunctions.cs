using System;

namespace CodeeloUI.SupportClasses.Animation.Animator
{
    public delegate double EasingDelegate(double currentTime,
        double minValue, double maxValue, double duration);
    public class EasingFunctions
    {
        #region Linear

        /// <summary>
        /// Функция простого линейное переключения без смягчения.
        /// </summary>
        public static double Linear(double currentTime, double minHeight, double maxHeight, double duration)
            => maxHeight * currentTime / duration + minHeight;

        #endregion

        #region Expo

        /// <summary>
        /// Функция экспоненциального (2^currentTime) замедления.
        /// https://easings.net/ru#easeOutExpo
        /// </summary>
        public static double ExpoEaseOut(double currentTime, double minHeight, double maxHeight, double duration)
            => (currentTime == duration) ?
            minHeight + maxHeight :
            maxHeight * (-Math.Pow(2, -10 * currentTime / duration) + 1) + minHeight;

        /// <summary>
        /// Функция экспоненциального (2^currentTime) ускорения.
        /// https://easings.net/ru#easeInExpo
        /// </summary>
        public static double ExpoEaseIn(double currentTime, double minHeight, double maxHeight, double duration)
            => (currentTime == 0) ?
            minHeight :
            maxHeight * Math.Pow(2, 10 * (currentTime / duration - 1)) + minHeight;

        /// <summary>
        /// Функция экспоненциального (2^currentTime) ускорения до половины пути, а после замедление.
        /// https://easings.net/ru#easeInOutExpo
        /// </summary>
        public static double ExpoEaseInOut(double currentTime, double minHeight, double maxHeight, double duration)
        {
            if (currentTime == 0)
                return minHeight;

            if (currentTime == duration)
                return minHeight + maxHeight;

            if ((currentTime /= duration / 2) < 1)
                return maxHeight / 2 * Math.Pow(2, 10 * (currentTime - 1)) + minHeight;

            return maxHeight / 2 * (-Math.Pow(2, -10 * --currentTime) + 2) + minHeight;
        }

        /// <summary>
        /// Функция экспоненциального (2^currentTime) замедления до половины пути, а после ускорение.
        /// </summary>
        public static double ExpoEaseOutIn(double currentTime, double minHeight, double maxHeight, double duration)
        {
            if (currentTime < duration / 2)
                return ExpoEaseOut(currentTime * 2, minHeight, maxHeight / 2, duration);

            return ExpoEaseIn((currentTime * 2) - duration, minHeight + maxHeight / 2, maxHeight / 2, duration);
        }

        #endregion

        #region Circular

        /// <summary>
        /// Функция для кругового (sqrt(1-currentTime^2)) замедления.
        /// https://easings.net/ru#easeOutCirc
        /// </summary>
        public static double CircEaseOut(double currentTime, double minHeight, double maxHeight, double duration)
            => maxHeight * Math.Sqrt(1 - (currentTime = currentTime / duration - 1) * currentTime) + minHeight;

        /// <summary>
        /// Функция для кругового (sqrt(1-currentTime^2)) ускорения.
        /// https://easings.net/ru#easeInCirc
        /// </summary>
        public static double CircEaseIn(double currentTime, double minHeight, double maxHeight, double duration)
        {
            var sqrt = Math.Sqrt(1 - (currentTime /= duration) * currentTime);

            if (double.IsNaN(sqrt))
                sqrt = 0;

            return -maxHeight * (sqrt - 1) + minHeight;
        }

        /// <summary>
        /// Функция для кругового (sqrt(1-currentTime^2)) ускорения до половины пути, а потом замедление.
        /// https://easings.net/ru#easeInOutCirc
        /// </summary>
        public static double CircEaseInOut(double currentTime, double minHeight, double maxHeight, double duration)
        {
            if ((currentTime /= duration / 2) < 1)
                return -maxHeight / 2 * (Math.Sqrt(1 - currentTime * currentTime) - 1) + minHeight;

            return maxHeight / 2 * (Math.Sqrt(1 - (currentTime -= 2) * currentTime) + 1) + minHeight;
        }

        /// <summary>
        /// Функция для кругового (sqrt(1-currentTime^2)) замедление до половины пути, а потом ускорение.
        /// </summary>
        public static double CircEaseOutIn(double currentTime, double minHeight, double maxHeight, double duration)
        {
            if (currentTime < duration / 2)
                return CircEaseOut(currentTime * 2, minHeight, maxHeight / 2, duration);

            return CircEaseIn((currentTime * 2) - duration, minHeight + maxHeight / 2, maxHeight / 2, duration);
        }

        #endregion

        #region Quad

        /// <summary>
        /// Функция квадратичного (currentTime^2) замедления.
        /// https://easings.net/ru#easeOutQuad
        /// </summary>
        public static double QuadEaseOut(double currentTime, double minHeight, double maxHeight, double duration)
            => -maxHeight * (currentTime /= duration) * (currentTime - 2) + minHeight;

        /// <summary>
        /// Функция квадратичного (currentTime^2) ускорения.
        /// https://easings.net/ru#easeInQuad
        /// </summary>
        public static double QuadEaseIn(double currentTime, double minHeight, double maxHeight, double duration)
            => maxHeight * (currentTime /= duration) * currentTime + minHeight;

        /// <summary>
        /// Функция квадратичного (currentTime^2) ускорения до половины пути, а потом замедление.
        /// https://easings.net/ru#easeInOutQuad
        /// </summary>
        public static double QuadEaseInOut(double currentTime, double minHeight, double maxHeight, double duration)
        {
            if ((currentTime /= duration / 2) < 1)
                return maxHeight / 2 * currentTime * currentTime + minHeight;

            return -maxHeight / 2 * ((--currentTime) * (currentTime - 2) - 1) + minHeight;
        }

        /// <summary>
        /// Функция квадратичного (currentTime^2) замедления до половины пути, а потом ускорение.
        /// </summary>
        public static double QuadEaseOutIn(double currentTime, double minHeight, double maxHeight, double duration)
        {
            if (currentTime < duration / 2)
                return QuadEaseOut(currentTime * 2, minHeight, maxHeight / 2, duration);

            return QuadEaseIn((currentTime * 2) - duration, minHeight + maxHeight / 2, maxHeight / 2, duration);
        }

        #endregion

        #region Sine

        /// <summary>
        /// Функция синусоидального (sin(currentTime)) замедления.
        /// https://easings.net/ru#easeOutSine
        /// </summary>
        public static double SineEaseOut(double currentTime, double minHeight, double maxHeight, double duration)
            => maxHeight * Math.Sin(currentTime / duration * (Math.PI / 2)) + minHeight;

        /// <summary>
        /// Функция синусоидального (sin(currentTime)) ускорения.
        /// https://easings.net/ru#easeInSine
        /// </summary>
        public static double SineEaseIn(double currentTime, double minHeight, double maxHeight, double duration)
            => -maxHeight * Math.Cos(currentTime / duration * (Math.PI / 2)) + maxHeight + minHeight;

        /// <summary>
        /// Функция синусоидального (sin(currentTime)) ускорения до половины пути, а потом замедление.
        /// https://easings.net/ru#easeInOutSine
        /// </summary>
        public static double SineEaseInOut(double currentTime, double minHeight, double maxHeight, double duration)
        {
            if ((currentTime /= duration / 2) < 1)
                return maxHeight / 2 * (Math.Sin(Math.PI * currentTime / 2)) + minHeight;

            return -maxHeight / 2 * (Math.Cos(Math.PI * --currentTime / 2) - 2) + minHeight;
        }

        /// <summary>
        /// Функция синусоидального (sin(currentTime)) замедление до половины пути, а потом ускорение.
        /// </summary>
        public static double SineEaseOutIn(double currentTime, double minHeight, double maxHeight, double duration)
        {
            if (currentTime < duration / 2)
                return SineEaseOut(currentTime * 2, minHeight, maxHeight / 2, duration);

            return SineEaseIn((currentTime * 2) - duration, minHeight + maxHeight / 2, maxHeight / 2, duration);
        }

        #endregion

        #region Cubic

        /// <summary>
        /// Функция кубического (currentTime^3) замедления.
        /// https://easings.net/ru#easeOutCubic
        /// </summary>
        public static double CubicEaseOut(double currentTime, double minHeight, double maxHeight, double duration)
            => maxHeight * ((currentTime = currentTime / duration - 1) * currentTime * currentTime + 1) + minHeight;

        /// <summary>
        /// Функция кубического (currentTime^3) ускорения.
        /// https://easings.net/ru#easeInCubic
        /// </summary>
        public static double CubicEaseIn(double currentTime, double minHeight, double maxHeight, double duration)
            => maxHeight * (currentTime /= duration) * currentTime * currentTime + minHeight;

        /// <summary>
        /// Функция кубического (currentTime^3) ускорения до половины пути, а потом замедление.
        /// https://easings.net/ru#easeInOutCubic
        /// </summary>
        public static double CubicEaseInOut(double currentTime, double minHeight, double maxHeight, double duration)
        {
            if ((currentTime /= duration / 2) < 1)
                return maxHeight / 2 * currentTime * currentTime * currentTime + minHeight;

            return maxHeight / 2 * ((currentTime -= 2) * currentTime * currentTime + 2) + minHeight;
        }

        /// <summary>
        /// Функция кубического (currentTime^3) замедления до половины пути, а потом ускорение.
        /// </summary>
        public static double CubicEaseOutIn(double currentTime, double minHeight, double maxHeight, double duration)
        {
            if (currentTime < duration / 2)
                return CubicEaseOut(currentTime * 2, minHeight, maxHeight / 2, duration);

            return CubicEaseIn((currentTime * 2) - duration, minHeight + maxHeight / 2, maxHeight / 2, duration);
        }

        #endregion

        #region Quartic

        /// <summary>
        /// Функция четырехкратного (currentTime^4) замедления.
        /// https://easings.net/ru#easeOutQuart
        /// </summary>
        public static double QuartEaseOut(double currentTime, double minHeight, double maxHeight, double duration)
            => -maxHeight * ((currentTime = currentTime / duration - 1) * currentTime * currentTime * currentTime - 1) + minHeight;


        /// <summary>
        /// Функция четырехкратного (currentTime^4) ускорения.
        /// https://easings.net/ru#easeInQuart
        /// </summary>
        public static double QuartEaseIn(double currentTime, double minHeight, double maxHeight, double duration)
            => maxHeight * (currentTime /= duration) * currentTime * currentTime * currentTime + minHeight;

        /// <summary>
        /// Функция четырехкратного (currentTime^4) ускорения до половины пути, а потом замедление.
        /// https://easings.net/ru#easeInOutQuart
        /// </summary>
        public static double QuartEaseInOut(double currentTime, double minHeight, double maxHeight, double duration)
        {
            if ((currentTime /= duration / 2) < 1)
                return maxHeight / 2 * currentTime * currentTime * currentTime * currentTime + minHeight;

            return -maxHeight / 2 * ((currentTime -= 2) * currentTime * currentTime * currentTime - 2) + minHeight;
        }

        /// <summary>
        /// Функция четырехкратного (currentTime^4) замедления до половины пути, а потом ускорение.
        /// </summary>
        public static double QuartEaseOutIn(double currentTime, double minHeight, double maxHeight, double duration)
        {
            if (currentTime < duration / 2)
                return QuartEaseOut(currentTime * 2, minHeight, maxHeight / 2, duration);

            return QuartEaseIn((currentTime * 2) - duration, minHeight + maxHeight / 2, maxHeight / 2, duration);
        }

        #endregion

        #region Quintic

        /// <summary>
        /// Функция пятикратного (currentTime^5) замедления.
        /// https://easings.net/ru#easeOutQuint
        /// </summary>
        public static double QuintEaseOut(double currentTime, double minHeight, double maxHeight, double duration)
            => maxHeight * ((currentTime = currentTime / duration - 1) * currentTime * currentTime * currentTime * currentTime + 1) + minHeight;

        /// <summary>
        /// Функция пятикратного (currentTime^5) ускорения.
        /// https://easings.net/ru#easeInQuint
        /// </summary>
        public static double QuintEaseIn(double currentTime, double minHeight, double maxHeight, double duration)
            => maxHeight * (currentTime /= duration) * currentTime * currentTime * currentTime * currentTime + minHeight;

        /// <summary>
        /// Функция пятикратного (currentTime^5) ускорения до половины пути, а потом замедление.
        /// https://easings.net/ru#easeInOutQuint
        /// </summary>
        public static double QuintEaseInOut(double currentTime, double minHeight, double maxHeight, double duration)
        {
            if ((currentTime /= duration / 2) < 1)
                return maxHeight / 2 * currentTime * currentTime * currentTime * currentTime * currentTime + minHeight;
            return maxHeight / 2 * ((currentTime -= 2) * currentTime * currentTime * currentTime * currentTime + 2) + minHeight;
        }

        /// <summary>
        /// Функция пятикратного (currentTime^5) замедления до половины пути, а потом ускорение.
        /// </summary>
        public static double QuintEaseOutIn(double currentTime, double minHeight, double maxHeight, double duration)
        {
            if (currentTime < duration / 2)
                return QuintEaseOut(currentTime * 2, minHeight, maxHeight / 2, duration);
            return QuintEaseIn((currentTime * 2) - duration, minHeight + maxHeight / 2, maxHeight / 2, duration);
        }

        #endregion

        #region Elastic

        /// <summary>
        /// Функция для эластичного (экспоненциально затухающего синусоидального) замедления.
        /// https://easings.net/ru#easeOutElastic
        /// </summary>
        public static double ElasticEaseOut(double currentTime, double minHeight, double maxHeight, double duration)
        {
            if ((currentTime /= duration) == 1)
                return minHeight + maxHeight;

            double p = duration * .3;
            double s = p / 4;

            return (maxHeight * Math.Pow(2, -10 * currentTime) * Math.Sin((currentTime * duration - s) * (2 * Math.PI) / p) + maxHeight + minHeight);
        }

        /// <summary>
        /// Функция для эластичного (экспоненциально затухающего синусоидального) ускорения.
        /// https://easings.net/ru#easeInElastic
        /// </summary>
        public static double ElasticEaseIn(double currentTime, double minHeight, double maxHeight, double duration)
        {
            if ((currentTime /= duration) == 1)
                return minHeight + maxHeight;

            double p = duration * .3;
            double s = p / 4;

            return -(maxHeight * Math.Pow(2, 10 * (currentTime -= 1)) * Math.Sin((currentTime * duration - s) * (2 * Math.PI) / p)) + minHeight;
        }

        /// <summary>
        /// Функция для эластичного (экспоненциально затухающего синусоидального) ускорения до половины пути, а потом замедление.
        /// https://easings.net/ru#easeInOutElastic
        /// </summary>
        public static double ElasticEaseInOut(double currentTime, double minHeight, double maxHeight, double duration)
        {
            if ((currentTime /= duration / 2) == 2)
                return minHeight + maxHeight;

            double p = duration * (.3 * 1.5);
            double s = p / 4;

            if (currentTime < 1)
                return -.5 * (maxHeight * Math.Pow(2, 10 * (currentTime -= 1)) * Math.Sin((currentTime * duration - s) * (2 * Math.PI) / p)) + minHeight;
            return maxHeight * Math.Pow(2, -10 * (currentTime -= 1)) * Math.Sin((currentTime * duration - s) * (2 * Math.PI) / p) * .5 + maxHeight + minHeight;
        }

        /// <summary>
        /// Функция для эластичного (экспоненциально затухающего синусоидального) замедления до половины пути, а потом ускорение.
        /// </summary>
        public static double ElasticEaseOutIn(double currentTime, double minHeight, double maxHeight, double duration)
        {
            if (currentTime < duration / 2)
                return ElasticEaseOut(currentTime * 2, minHeight, maxHeight / 2, duration);
            return ElasticEaseIn((currentTime * 2) - duration, minHeight + maxHeight / 2, maxHeight / 2, duration);
        }

        #endregion

        #region Bounce

        /// <summary>
        /// Функция отскакивающего (экспоненциально затухающий параболический отскок) замедления.
        /// https://easings.net/ru#easeOutBounce
        /// </summary>
        public static double BounceEaseOut(double currentTime, double minHeight, double maxHeight, double duration)
        {
            if ((currentTime /= duration) < (1 / 2.75))
                return maxHeight * (7.5625 * currentTime * currentTime) + minHeight;

            if (currentTime < (2 / 2.75))
                return maxHeight * (7.5625 * (currentTime -= (1.5 / 2.75)) * currentTime + .75) + minHeight;

            if (currentTime < (2.5 / 2.75))
                return maxHeight * (7.5625 * (currentTime -= (2.25 / 2.75)) * currentTime + .9375) + minHeight;

            return maxHeight * (7.5625 * (currentTime -= (2.625 / 2.75)) * currentTime + .984375) + minHeight;
        }

        /// <summary>
        /// Функция отскакивающего (экспоненциально затухающий параболический отскок) ускорения.
        /// https://easings.net/ru#easeInBounce
        /// </summary>
        public static double BounceEaseIn(double currentTime, double minHeight, double maxHeight, double duration)
            => maxHeight - BounceEaseOut(duration - currentTime, 0, maxHeight, duration) + minHeight;

        /// <summary>
        /// Функция отскакивающего (экспоненциально затухающий параболический отскок) ускорения до половины пути, а потом замедление.
        /// https://easings.net/ru#easeInOutBounce
        /// </summary>
        public static double BounceEaseInOut(double currentTime, double minHeight, double maxHeight, double duration)
        {
            if (currentTime < duration / 2)
                return BounceEaseIn(currentTime * 2, 0, maxHeight, duration) * .5 + minHeight;

            return BounceEaseOut(currentTime * 2 - duration, 0, maxHeight, duration) * .5 + maxHeight * .5 + minHeight;
        }

        /// <summary>
        /// Функция отскакивающего (экспоненциально затухающий параболический отскок) замедления до половины пути, а потом ускорение.
        /// </summary>
        public static double BounceEaseOutIn(double currentTime, double minHeight, double maxHeight, double duration)
        {
            if (currentTime < duration / 2)
                return BounceEaseOut(currentTime * 2, minHeight, maxHeight / 2, duration);

            return BounceEaseIn((currentTime * 2) - duration, minHeight + maxHeight / 2, maxHeight / 2, duration);
        }

        #endregion

        #region Back

        /// <summary>
        /// Функция обратного (проскакивающего кубического (s+1)*currentTime^3 - s*currentTime^2) замедления.
        /// https://easings.net/ru#easeOutBack
        /// </summary>
        public static double BackEaseOut(double currentTime, double minHeight, double maxHeight, double duration)
        {
            return maxHeight * ((currentTime = currentTime / duration - 1) * currentTime * ((1.70158 + 1) * currentTime + 1.70158) + 1) + minHeight;
        }

        /// <summary>
        /// Функция обратного (проскакивающего кубического (s+1)*currentTime^3 - s*currentTime^2) ускорения.
        /// https://easings.net/ru#easeInBack
        /// </summary>
        public static double BackEaseIn(double currentTime, double minHeight, double maxHeight, double duration)
        {
            return maxHeight * (currentTime /= duration) * currentTime * ((1.70158 + 1) * currentTime - 1.70158) + minHeight;
        }

        /// <summary>
        /// Функция обратного (проскакивающего кубического (s+1)*currentTime^3 - s*currentTime^2) ускорения до половины пути, а потом замедление.
        /// https://easings.net/ru#easeInOutBack
        /// </summary>
        public static double BackEaseInOut(double currentTime, double minHeight, double maxHeight, double duration)
        {
            double s = 1.70158;
            if ((currentTime /= duration / 2) < 1)
                return maxHeight / 2 * (currentTime * currentTime * (((s *= (1.525)) + 1) * currentTime - s)) + minHeight;
            return maxHeight / 2 * ((currentTime -= 2) * currentTime * (((s *= (1.525)) + 1) * currentTime + s) + 2) + minHeight;
        }

        /// <summary>
        /// Функция обратного (проскакивающего кубического (s+1)*currentTime^3 - s*currentTime^2) замедления до половины пути, а потом ускорение.
        /// </summary>
        public static double BackEaseOutIn(double currentTime, double minHeight, double maxHeight, double duration)
        {
            if (currentTime < duration / 2)
                return BackEaseOut(currentTime * 2, minHeight, maxHeight / 2, duration);
            return BackEaseIn((currentTime * 2) - duration, minHeight + maxHeight / 2, maxHeight / 2, duration);
        }

        #endregion
    }
}
