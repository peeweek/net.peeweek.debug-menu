using System;
using System.Collections.Generic;
using UnityEngine;

namespace DebugMenuUtility
{
    [DebugMenuItem("Rendering")]
    class FPSDebugMenuItem : DebugMenuItem
    {
        public override string label => "Frame Rate";
        public override string value => UpdateTime();

        Queue<float> values;

        string UpdateTime()
        {
            if (values == null)
            {
                values = new Queue<float>();
                values.Enqueue(Time.unscaledTime);
            }

            float p = values.Peek();
            float t = Time.unscaledTime;

            if (values.Count > 512)
                values.Dequeue();

            values.Enqueue(Time.unscaledTime);

            return (1f / ((t - p) / values.Count)).ToString("F2");

        }
    }

    [DebugMenuItem("Rendering")]
    class FramerateLimitDebugMenuItem : DebugMenuItem
    {
        public override string label => "Limit Framerate";
        public override string value
        {
            get
            {
                if (Application.targetFrameRate == -1)
                    return "Unlimited";
                else
                    return $"{Application.targetFrameRate}";
            }
        }

        public override Action OnValidate => () => Application.targetFrameRate = -1;
        public override Action OnLeft => Decrement;
        public override Action OnRight => Increment;

        void Decrement()
        {
            if (Application.targetFrameRate == -1)
                Application.targetFrameRate = 30;
            else
            {
                Application.targetFrameRate -= 10;
                if (Application.targetFrameRate == 0)
                    Application.targetFrameRate = -1;
            }
        }

        void Increment()
        {
            if (Application.targetFrameRate == -1)
                Application.targetFrameRate = 60;
            else
            {
                Application.targetFrameRate += 10;
                if (Application.targetFrameRate > 240)
                    Application.targetFrameRate = -1;
            }
        }
    }

    [DebugMenuItem("Rendering")]
    class VSyncDebugMenuItem : DebugMenuItem
    {
        public override string label => "VSync";
        public override string value => QualitySettings.vSyncCount == 0 ? "Disabled" : "Enabled";
        public override Action OnValidate => Toggle;
        public override Action OnLeft => Toggle;
        public override Action OnRight => Toggle;

        void Toggle()
        {
            if (QualitySettings.vSyncCount == 0)
                QualitySettings.vSyncCount = 1;
            else
                QualitySettings.vSyncCount = 0;
        }
    }
}
