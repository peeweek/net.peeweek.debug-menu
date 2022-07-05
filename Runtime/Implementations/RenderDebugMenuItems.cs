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

    [DebugMenuItem("Rendering")]
    class FullScreenModeMenuItem : DebugMenuItem
    {
        public override string label => "Full Screen Mode";
        public override string value => Screen.fullScreenMode.ToString();
        public override Action OnValidate => Increase;
        public override Action OnLeft => Decrease;
        public override Action OnRight => Increase;

        void Increase() => Toggle(1);
        void Decrease() => Toggle(-1);
        void Toggle(int pad = 1)
        {
            Screen.fullScreenMode = (FullScreenMode)(((int)Screen.fullScreenMode + pad) % 4);
        }
    }

    [DebugMenuItem("Rendering")]
    class ResolutionMenuItem : DebugMenuItem
    {
        bool inited = false;

        public override string label => "Screen Resolution";
        public override string value => $"{Screen.currentResolution.width}×{Screen.currentResolution.height}";

        public override Action OnValidate => Increase;
        public override Action OnLeft => Decrease;
        public override Action OnRight => Increase;

        void Increase() => Toggle(1);
        void Decrease() => Toggle(-1);

        void Toggle(int pad = 1)
        {
            var current = Screen.currentResolution;
            int rate = current.refreshRate;
            int i = 0;
            int found = -1;
            List<Resolution> resolutions = new List<Resolution>();
            foreach (var r in Screen.resolutions)
            {
                if (r.refreshRate == rate)
                {
                    resolutions.Add(r);
                    if(r.width == current.width && r.height == current.height)
                    {
                        found = i;
                    }
                }
                i++;
            }
            var next = resolutions[(i + pad) % resolutions.Count];
            Screen.SetResolution(next.width, next.height, Screen.fullScreen);
        }
    }
}
