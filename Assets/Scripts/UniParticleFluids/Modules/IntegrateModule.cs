using UniParticleFluids.Configs;
using UniParticleFluids.Data;
using UnityEngine;

namespace UniParticleFluids.Modules
{
    public class IntegrateModule : ModuleBase
    {
        public override int DefaultModuleOrder => ModuleOrder.Integrate;
        
        [SerializeField] private ComputeShader _integrateCs;
        
        private ParticleBuffer _particleBuffer;
        private ISimulationSpaceConfig _simulationSpaceConfig;
        private ITimeStepConfig _timeStepConfig;

        public override void Initialize(IObjectResolver resolver)
        {
            base.Initialize(resolver);
            _particleBuffer = resolver.Resolve<ParticleBuffer>();
            _simulationSpaceConfig = resolver.Resolve<ISimulationSpaceConfig>();
            _timeStepConfig = resolver.Resolve<ITimeStepConfig>();
        }
        
        public override void Run()
        {
            int kernelId = _integrateCs.FindKernel("Integrate");
            _integrateCs.SetFloat("_DeltaTime", _timeStepConfig.TimeStep);
            _integrateCs.SetVector("_SpaceMin", _simulationSpaceConfig.Min);
            _integrateCs.SetVector("_SpaceMax", _simulationSpaceConfig.Max);
            _particleBuffer.SetToComputeShader(_integrateCs, kernelId, "_ParticleBuffer");
            _integrateCs.DispatchDesired(kernelId, _particleBuffer.Size.x);
                        
            _particleBuffer.Swap();
        }
    }
}