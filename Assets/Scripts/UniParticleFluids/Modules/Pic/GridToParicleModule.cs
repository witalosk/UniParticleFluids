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
        private IGridSpacingConfig _gridSpacingConfig;
        private PicConfig _picConfig;

        public override void Initialize(IObjectResolver resolver)
        {
            _particleBuffer = resolver.Resolve<ParticleBuffer>();
            _fieldVelocityBuffer = resolver.Resolve<FieldVelocityBuffer>();
            _simulationSpaceConfig = resolver.Resolve<ISimulationSpaceConfig>();
            _gridSpacingConfig = resolver.Resolve<IGridSpacingConfig>();
            _picConfig = resolver.Resolve<PicConfig>();
        }

        public override void Run()
        {
            _particleToGridCs.SetVector("_GridMin", _simulationSpaceConfig.Min);
            _particleToGridCs.SetVector("_GridMax", _simulationSpaceConfig.Max);
            _particleToGridCs.SetInts("_GridSize", _fieldVelocityBuffer.Size.ToInts());
            _particleToGridCs.SetFloat("_GridSpacing", _gridSpacingConfig.GridSpacing);
            _particleToGridCs.SetFloat("_GridInvSpacing", 1f / _gridSpacingConfig.GridSpacing);
            _particleToGridCs.SetFloat("_Flipness", _picConfig.Flipness);

            int kernel = _particleToGridCs.FindKernel("GridToParticle");
            _particleToGridCs.SetData(kernel, "_ParticleBuffer", _particleBuffer);
            _particleToGridCs.SetData(kernel, "_FieldVelocityBuffer", _fieldVelocityBuffer);

            _particleToGridCs.DispatchDesired(kernel, _particleBuffer.Size.x);
        }
    }
}