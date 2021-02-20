using UnityEngine;
using System.Diagnostics;
using System.Collections.Generic;

public static class MathCheck_AndDiagnostics 
{
    private class TickCounter 
    {
        public long ticks;
        public long longestTick;
        public long framesCounted;
    }

    private static Stopwatch stopwatch = new Stopwatch();

    private static Dictionary<int, TickCounter> tickCounters = new Dictionary<int, TickCounter>();

    public static void AddForDiagnostics(int instanceID) => tickCounters.Add(instanceID, new TickCounter());
    public static void AddTicks(int id, long ticks) 
    {
        if (!tickCounters.ContainsKey(id))
            return;

        tickCounters[id].ticks += ticks;
        
        if (ticks > tickCounters[id].longestTick)
            tickCounters[id].longestTick = ticks;

        tickCounters[id].framesCounted++; 
    }

    public static long GetAverageTicksForID(int id)
    {
        if (!tickCounters.ContainsKey(id) || tickCounters[id].framesCounted == 0)
            return -1;

        return tickCounters[id].ticks / tickCounters[id].framesCounted;
    }

    public static long GetLongestTickForID(int id) 
    {
        if (!tickCounters.ContainsKey(id) || tickCounters[id].framesCounted == 0)
            return -1;

        return tickCounters[id].longestTick;
    }

    public static void StartClock() 
    {
        stopwatch.Restart();

        stopwatch.Start();
    }

    public static long StopClockGetTicks() 
    {
        stopwatch.Stop();

        return stopwatch.ElapsedTicks;
    }

    public static long StopClockGetElapsedMilliseconds() 
    {
        stopwatch.Stop();

        return stopwatch.ElapsedMilliseconds;
    }

    public static string GetElapsedMSasString() => StopClockGetElapsedMilliseconds().ToString();

    public static float ManualPower(float n, float power)
    {
        float result = 1;
        for (int i = 0; i < (int)power; ++i) 
        {
            result *= n;
        }
        return result;
    }

    public static float ClampNegPos1 (float value)
    {
        if (value < -1f) return -1f;
        if (value > 1f) return 1f;
        return value;
    }

    public static int ClampNegPos1(int value)
    {
        if (value < -1) return -1;
        if (value > 1) return 1;
        return value;
    }

    /// <summary>
    /// Returns true if a random value is greater than or equal to the probabilty.
    /// </summary>
    /// <param name="probabiltyThreshold">Threshold at which this returns true.</param>
    /// <returns>True or false.</returns>
    public static bool Probability(float probabiltyThreshold) 
    {
        return probabiltyThreshold < 1f ? probabiltyThreshold >= Random.Range(0f, 1f) : probabiltyThreshold >= Random.Range(0,100);
    }

    public static float DebugBuiltinPower(float n, float power)
    {
        stopwatch.Restart();

        stopwatch.Start();
        float result = Mathf.Pow(n, power);

        stopwatch.Stop();

        UnityEngine.Debug.LogWarning("Power of " + n.ToString() + " to the " + power.ToString() + "th power is " + result.ToString());

        UnityEngine.Debug.LogWarning("Time passed to do Math.Pow: " + stopwatch.ElapsedMilliseconds.ToString() + " Ticks: " + stopwatch.ElapsedTicks.ToString());

        return result;
    }

    public static float DebugManualPower(float n, float power)
    {
        stopwatch.Restart();

        stopwatch.Start();

        float result = 1;
        for (int i = 0; i < (int)power; ++i)
        {
            result *= n;
        }

        stopwatch.Stop();

        UnityEngine.Debug.LogWarning("Power of " + n.ToString() + " to the " + power.ToString() + "th power is " + result.ToString());

        UnityEngine.Debug.LogWarning("Time passed to do manual Pow: " + stopwatch.ElapsedMilliseconds.ToString() + " Ticks: " + stopwatch.ElapsedTicks.ToString());

        return result;
    }

    public static void DebugForwardArc(Vector3 forward, float distForward, float arcAngle) 
    {
        Quaternion rotLeft = Quaternion.AngleAxis(-arcAngle, Vector3.up);
        Quaternion rotRight = Quaternion.AngleAxis(arcAngle, Vector3.up);

        Vector3 forwardToLeft = rotLeft * (forward * distForward);
        Vector3 forwardToRight = rotRight * (forward * distForward);

        Gizmos.color = Color.red;

        Gizmos.DrawLine(forward, forward * distForward);
        Gizmos.DrawLine(forward, forwardToLeft);
        Gizmos.DrawLine(forward, forwardToRight);
    }
}
