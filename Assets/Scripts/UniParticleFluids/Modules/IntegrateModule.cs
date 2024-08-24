using UniParticleFluids.Data;
using UnityEngine;

namespace UniParticleFluids.Modules
{
    public class IntegrateModule : ModuleBase
    {
        public override int DefaultModuleOrder => ModuleOrder.Integrate;
        
        [SerializeField] private ComputeShader _integrateCs;
        
        private ParticleBuffer _particleBuffer;

        public override void Initialize(IObjectResolver resolver)
        {
            base.Initialize(resolver);
            _particleBuffer = resolver.Resolve<ParticleBuffer>();
        }
        
        public override void Run()
        {
            int kernelId = _integrateCs.FindKernel("Integrate");
            _integrateCs.SetFloat("_DeltaTime", Time.deltaTime);
            _particleBuffer.SetToComputeShader(_integrateCs, kernelId, "_ParticleBuffer");
            _integrateCs.DispatchDesired(kernelId, _particleBuffer.Size.x);
                        
            _particleBuffer.Swap();
        }
    }
}