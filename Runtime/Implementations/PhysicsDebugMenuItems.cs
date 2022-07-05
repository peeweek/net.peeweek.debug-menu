using System;
using System.Collections.Generic;
using UnityEngine;

namespace DebugMenuUtility
{
    [DebugMenuItem("Physics")]
    class PhysicsAutoSimMenuItem : DebugMenuItem
    {
        public override string label => "Auto Simulation";
        public override string value => Physics.autoSimulation.ToString();

        public override Action OnValidate => Update;
        public override Action OnLeft => Update;
        public override Action OnRight => Update;

        void Update()
        {
            Physics.autoSimulation = !Physics.autoSimulation;
        }
    }

}
