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

        public void SetToComputeShader(ComputeShader computeShader, int kernel, string shaderName)
        {
            computeShader.SetVector($"{shaderName}Min", Position - Scale * 0.5f);
            computeShader.SetVector($"{shaderName}Max", Position + Scale * 0.5f);
        }

        public void SetToMaterial(Material material, string shaderName)
        {
            material.SetVector($"{shaderName}Min", Position - Scale * 0.5f);
            material.SetVector($"{shaderName}Max", Position + Scale * 0.5f);
        }
    }
}