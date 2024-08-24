using UnityEngine;

namespace UniParticleFluids
{
    public interface IShaderSettableData
    {
        void SetToComputeShader(ComputeShader computeShader, int kernel, string shaderName);
        void SetToMaterial(Material material, string shaderName);
    }
}