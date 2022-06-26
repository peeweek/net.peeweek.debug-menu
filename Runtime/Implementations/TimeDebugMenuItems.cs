using System;
using UnityEngine;

namespace DebugMenuUtility
{
    [DebugMenuItem("Time")]
    class TimeScaleDebugMenuItem : DebugMenuItem
    {
        public override string label => "Time Scale";
        public override string value => $"{Time.timeScale.ToString("F1")}";
        public override Action OnValidate => () => Time.timeScale = 1.0f;
        public override Action OnLeft => () => Time.timeScale = Mathf.Max(0f, Time.timeScale - 0.1f);
        public override Action OnRight => () => Time.timeScale = Mathf.Min(10f, Time.timeScale + 0.1f);
    }

    [DebugMenuItem("Time")]
    class TimeDebugMenuItem : DebugMenuItem
    {
        public override string label => "Total Time";
        public override string value => $"{Time.time.ToString("F1")}";
    }

    [DebugMenuItem("Time")]
    class UnscaledTimeDebugMenuItem : DebugMenuItem
    {
        public override string label => "Total Time (Unscaled)";
        public override string value => $"{Time.unscaledTime.ToString("F1")}";
    }

    [DebugMenuItem("Time")]
    class FrameDebugMenuItem : DebugMenuItem
    {
        public override string label => "Frame";
        public override string value => $"{Time.frameCount}";
    }
}
