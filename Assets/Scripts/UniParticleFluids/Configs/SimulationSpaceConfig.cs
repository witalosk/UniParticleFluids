using System;
using UnityEngine;

namespace UniParticleFluids.Configs
{
    public class SimulationSpaceConfig : ConfigBase, ISimulationSpaceConfig
    {
        public Vector3 Position => transform.position;
        public Vector3 Scale => transform.lossyScale;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Position, Scale);
        }
    }
}