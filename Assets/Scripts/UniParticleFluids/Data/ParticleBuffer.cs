using System.Runtime.InteropServices;
using UnityEngine;

namespace UniParticleFluids.Data
{
    public class ParticleBuffer : DataBase
    {
        public override Vector3Int Size => new(_particleCount, 1, 1);
        public override object Data => _buffer.Read;
        
        [SerializeField] private int _particleCount = 10000;
        
        private SwapBuffer _buffer;
        
        public override void Initialize(IObjectResolver resolver)
        {
            _buffer = new SwapBuffer(_particleCount, Marshal.SizeOf<Particle>());
        }

        public override void Deinitialize()
        {
            _buffer.Dispose();
        }

        public override void SetToComputeShader(ComputeShader computeShader, int kernel, string shaderName)
        {
            computeShader.SetBuffer(kernel, shaderName, _buffer.Read);
            computeShader.SetBuffer(kernel, $"{shaderName}Write", _buffer.Write);
        }

        public override void SetToMaterial(Material material, string shaderName)
        {
            material.SetBuffer(shaderName, _buffer.Read);
        }

        public void Swap()
        {
            _buffer.Swap();
        }
    }
}