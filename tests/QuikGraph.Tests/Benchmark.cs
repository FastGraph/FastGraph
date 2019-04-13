using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Security;

namespace QuickGraph.Tests
{
    public static class PreciseTimer
    {
        [SuppressUnmanagedCodeSecurity]
        sealed class Win32
        {
            [DllImport("Kernel32.dll"), SuppressUnmanagedCodeSecurity]
            public static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

            [DllImport("Kernel32.dll"), SuppressUnmanagedCodeSecurity]
            public static extern bool QueryPerformanceFrequency(out long lpFrequency);
        }

        private readonly static long frequency;
        static PreciseTimer()
        {
            if (!Win32.QueryPerformanceFrequency(out frequency))
            {
                // high-performance counter not supported
                throw new Win32Exception();
            }
        }

        /// <summary>
        /// Gets the frequency.
        /// </summary>
        /// <value>The frequency.</value>
        public static long Frequency => frequency;

        /// <summary>
        /// Gets the current ticks value.
        /// </summary>
        /// <value>The now.</value>
        public static long Now
        {
            get
            {
                if (!Win32.QueryPerformanceCounter(out long startTime))
                    throw new Win32Exception("QueryPerformanceCounter failed");
                return startTime;
            }
        }

        /// <summary>
        /// Returns the duration of the timer (in seconds)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public static double ToSeconds(long start, long end)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(start >= 0 && end >= 0 && start <= end);
#endif

            return (end - start) / (double)frequency;
        }

        /// <summary>
        ///Returns the duration in seconds
        /// </summary>
        /// <param name="ticks">The ticks.</param>
        /// <returns></returns>
#if SUPPORTS_CONTRACTS
        [Pure]
#endif
        public static double ToSeconds(long ticks)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(ticks >= 0);
#endif

            return ticks / (double)frequency;
        }

        /// <summary>
        ///Returns the duration in seconds from <paramref name="start"/>
        /// </summary>
        /// <param name="start">The start.</param>
        /// <returns></returns>
        public static double ToSecondsFromNow(long start)
        {
            return ToSeconds(start, PreciseTimer.Now);
        }
    }

    public sealed class Benchmark
    {
        readonly string name;
        long duration = 0;
        long samples = 0;

        public Benchmark(string name)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(!String.IsNullOrEmpty(name));
#endif

            this.name = name;
        }

        public string Name
        {
            get { return this.name; }
        }

        public double Seconds
        {
            get { return PreciseTimer.ToSeconds(this.duration); }
        }

        public long Samples
        {
            get { return this.samples; }
        }

        public override string ToString()
        {
            return String.Format("{0}: {1}s, {2} samples", this.Name, this.Seconds, this.samples);
        }

        public void Run(Action action)
        {
            long start = PreciseTimer.Now;
            try
            {
                action();
            }
            finally
            {
                long finish = PreciseTimer.Now;
                this.duration += finish - start;
                this.samples++;
            }
        }
    }
}
