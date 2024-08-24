using UniParticleFluids.Configs;
using UniParticleFluids.Data;
using UnityEngine;

namespace UniParticleFluids.Modules.Pic
{
    public class GridToParticleModule : ModuleBase
    {
        public override int DefaultModuleOrder => ModuleOrder.PicUpdate.GridToParticle;
        
        [SerializeField] private ComputeShader _particleToGridCs;
        
        private ParticleBuffer _particleBuffer;
        private FieldVelocityBuffer _fieldVelocityBuffer;
        private ISimulationSpaceConfig _simulationSpaceConfig;
        private PicConfig _picConfig;

        public override void Initialize(IObjectResolver resolver)
        {
            _particleBuffer = resolver.Resolve<ParticleBuffer>();
            _fieldVelocityBuffer = resolver.Resolve<FieldVelocityBuffer>();
            _simulationSpaceConfig = resolver.Resolve<ISimulationSpaceConfig>();
            _picConfig = resolver.Resolve<PicConfig>();
        }

        public override void Run()
        {
            int kernel = _particleToGridCs.FindKernel("GridToParticle");
            
            _particleToGridCs.SetFloat("_Flipness", _picConfig.Flipness);

            _particleToGridCs.SetData(kernel, "_Space", _simulationSpaceConfig);
            _particleToGridCs.SetData(kernel, "_ParticleBuffer", _particleBuffer);
            _particleToGridCs.SetData(kernel, "_FieldVelocityBuffer", _fieldVelocityBuffer);

            _particleToGridCs.DispatchDesired(kernel, _particleBuffer.Size.x);
        }
    }
}