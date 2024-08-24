using UniParticleFluids.Data;
using UnityEngine;

namespace UniParticleFluids.Modules
{
    public class ApplyNoiseModule : ModuleBase
    {
        public override int DefaultModuleOrder => ModuleOrder.ForceUpdate;
        
        [SerializeField] private ComputeShader _applyNoiseCs;
        [Space]
        [SerializeField] private float _noiseFrequency = 0.01f;
        [SerializeField] private float _noiseScale = 0.01f;
        [SerializeField] private float _noiseSpeed = 0.01f;

        private ParticleBuffer _particleBuffer;
        
        public override void Initialize(IObjectResolver resolver)
        {
            _particleBuffer = resolver.Resolve<ParticleBuffer>();
        }
        
        public override void Run()
        {
            int kernelId = _applyNoiseCs.FindKernel("ApplyNoise");
            _applyNoiseCs.SetFloat("_DeltaTime", Time.deltaTime);
            _applyNoiseCs.SetFloat("_SimulationTime", Time.time);
            _applyNoiseCs.SetFloat("_NoiseFrequency", _noiseFrequency);
            _applyNoiseCs.SetFloat("_NoiseScale", _noiseScale);
            _applyNoiseCs.SetFloat("_NoiseSpeed", _noiseSpeed);
            _particleBuffer.SetToComputeShader(_applyNoiseCs, kernelId, "_ParticleBuffer");
            _applyNoiseCs.DispatchDesired(kernelId, _particleBuffer.Size.x);
            
            _particleBuffer.Swap();
        }
    }
}