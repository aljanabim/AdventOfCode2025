using System.Diagnostics;

namespace Utils;


public static class Timing
{
    public static (T result, TimeSpan elapsed) Measure<T>(Func<T> func)
    {
        var sw = Stopwatch.StartNew();
        T result = func();
        sw.Stop();
        return (result, sw.Elapsed);
    }
}
