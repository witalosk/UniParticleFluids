using UniParticleFluids.Configs;
using UniParticleFluids.Data;
using UnityEngine;

namespace UniParticleFluids.Modules.Pic
{
    public class ParticleToGridModule : ModuleBase
    {
        public override int DefaultModuleOrder => ModuleOrder.PicUpdate.ParticleToGrid;
        
        [SerializeField] private ComputeShader _particleToGridCs;
        
        private ParticleBuffer _particleBuffer;
        private ParticleGridStartEndBuffer _gridStartEndBuffer;
        private FieldVelocityBuffer _fieldVelocityBuffer;
        private ISimulationSpaceConfig _simulationSpaceConfig;
        private IGridSpacingConfig _gridSpacingConfig;

        public override void Initialize(IObjectResolver resolver)
        {
            _particleBuffer = resolver.Resolve<ParticleBuffer>();
            _gridStartEndBuffer = resolver.Resolve<ParticleGridStartEndBuffer>();
            _fieldVelocityBuffer = resolver.Resolve<FieldVelocityBuffer>();
            _simulationSpaceConfig = resolver.Resolve<ISimulationSpaceConfig>();
            _gridSpacingConfig = resolver.Resolve<IGridSpacingConfig>();
        }

        public override void Run()
        {
            _particleToGridCs.SetVector("_GridMin", _simulationSpaceConfig.Min);
            _particleToGridCs.SetVector("_GridMax", _simulationSpaceConfig.Max);
            _particleToGridCs.SetInts("_GridSize", _gridStartEndBuffer.GridSize.ToInts());
            _particleToGridCs.SetFloat("_GridSpacing", _gridSpacingConfig.GridSpacing);
            _particleToGridCs.SetFloat("_GridInvSpacing", 1f / _gridSpacingConfig.GridSpacing);

            int kernel = _particleToGridCs.FindKernel("ParticleToGrid");
            _particleToGridCs.SetData(kernel, "_ParticleBuffer", _particleBuffer);
            _particleToGridCs.SetData(kernel, "_GridStartEndBuffer", _gridStartEndBuffer);
            _particleToGridCs.SetData(kernel, "_FieldVelocityBuffer", _fieldVelocityBuffer);

            _particleToGridCs.DispatchDesired(kernel, _fieldVelocityBuffer.Size);
        }
    }
}