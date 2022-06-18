using System;
using System.Diagnostics;
using System.Threading;

namespace CodeeloUI.Animation.Animator
{
    public class AnimationStatus : EventArgs
    {
        private readonly Stopwatch _stopwatch;

        public long ElapsedMilliseconds => _stopwatch.ElapsedMilliseconds;
        private CancellationTokenSource CancellationToken { get; set; }
        public bool IsCompleted { get; set; }

        public AnimationStatus(CancellationTokenSource token, Stopwatch stopwatch)
        {
            CancellationToken = token;
            _stopwatch = stopwatch;
        }
    }
}
