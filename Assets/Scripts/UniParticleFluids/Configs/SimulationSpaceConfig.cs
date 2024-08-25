using UnityEngine;
using UnityEngine.Rendering;

namespace UniParticleFluids.Configs
{
    public class SimulationSpaceConfig : ConfigBase, ISimulationSpaceConfig
    {
        public Vector3 Position => transform.position;
        public Vector3 Scale => transform.lossyScale;

        public override void Initialize(IObjectResolver resolver)
        {
            // RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
        }
        
        public override void Deinitialize()
        {
            // RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
        }
        
        private void Update()
        {
            // スペースの矩形を描画する
            Debug.DrawLine(Position - Scale * 0.5f, Position + new Vector3(Scale.x, -Scale.y, -Scale.z) * 0.5f, Color.red);
            Debug.DrawLine(Position - Scale * 0.5f, Position + new Vector3(-Scale.x, Scale.y, -Scale.z) * 0.5f, Color.green);
            Debug.DrawLine(Position - Scale * 0.5f, Position + new Vector3(-Scale.x, -Scale.y, Scale.z) * 0.5f, Color.green);
            Debug.DrawLine(Position + Scale * 0.5f, Position + new Vector3(-Scale.x, Scale.y, Scale.z) * 0.5f, Color.green);
            Debug.DrawLine(Position + Scale * 0.5f, Position + new Vector3(Scale.x, -Scale.y, Scale.z) * 0.5f, Color.green);
            Debug.DrawLine(Position + Scale * 0.5f, Position + new Vector3(Scale.x, Scale.y, -Scale.z) * 0.5f, Color.green);
            
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            // Gizmos.DrawWireCube(Position, Scale);
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