using UniParticleFluids.Configs;
using UniParticleFluids.Data;
using UnityEngine;

namespace UniParticleFluids.Modules.Pic
{
    public class PicAdvectModule : ModuleBase
    {
        public override int DefaultModuleOrder => ModuleOrder.PicUpdate.Advect;
        
        [SerializeField] private ComputeShader _picAdvectCs;
        
        private ParticleBuffer _particleBuffer;
        private FieldVelocityBuffer _fieldVelocityBuffer;
        private ISimulationStepConfig _simulationStepConfig;
        private ISimulationSpaceConfig _simulationSpaceConfig;
        private IGridSpacingConfig _gridSpacingConfig;
        
        public override void Initialize(IObjectResolver resolver)
        {
            _particleBuffer = resolver.Resolve<ParticleBuffer>();
            _fieldVelocityBuffer = resolver.Resolve<FieldVelocityBuffer>();
            _simulationStepConfig = resolver.Resolve<ISimulationStepConfig>();
            _simulationSpaceConfig = resolver.Resolve<ISimulationSpaceConfig>();
            _gridSpacingConfig = resolver.Resolve<IGridSpacingConfig>();
        }

        public override void Run()
        {
            int kernel = _picAdvectCs.FindKernel("PicAdvect");
            _picAdvectCs.SetVector("_GridMin", _simulationSpaceConfig.Min);
            _picAdvectCs.SetVector("_GridMax", _simulationSpaceConfig.Max);
            _picAdvectCs.SetInts("_GridSize", _fieldVelocityBuffer.Size.ToInts());
            _picAdvectCs.SetFloat("_GridInvSpacing", 1f / _gridSpacingConfig.GridSpacing);
            _picAdvectCs.SetFloat("_SimulationStep", _simulationStepConfig.SimulationStep);
            
            _picAdvectCs.SetData(kernel, "_ParticleBuffer", _particleBuffer);
            _picAdvectCs.SetData(kernel, "_FieldVelocityBuffer", _fieldVelocityBuffer);
            _picAdvectCs.DispatchDesired(kernel, _particleBuffer.Size.x);            
        }
    }
}