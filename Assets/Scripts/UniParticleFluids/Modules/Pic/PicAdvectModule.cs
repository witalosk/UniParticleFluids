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
        private FieldVelocityDoubleBuffer _fieldVelocityDoubleBuffer;
        private ITimeStepConfig _timeStepConfig;
        private ISimulationSpaceConfig _simulationSpaceConfig;
        private IGridSpacingConfig _gridSpacingConfig;
        
        public override void Initialize(IObjectResolver resolver)
        {
            _particleBuffer = resolver.Resolve<ParticleBuffer>();
            _fieldVelocityDoubleBuffer = resolver.Resolve<FieldVelocityDoubleBuffer>();
            _timeStepConfig = resolver.Resolve<ITimeStepConfig>();
            _simulationSpaceConfig = resolver.Resolve<ISimulationSpaceConfig>();
            _gridSpacingConfig = resolver.Resolve<IGridSpacingConfig>();
        }

        public override void Run()
        {
            int kernel = _picAdvectCs.FindKernel("PicAdvect");
            _picAdvectCs.SetInts("_GridSize", _fieldVelocityDoubleBuffer.Size.ToInts());
            _picAdvectCs.SetFloat("_GridInvSpacing", 1f / _gridSpacingConfig.GridSpacing);
            _picAdvectCs.SetFloat("_SimulationStep", _timeStepConfig.TimeStep);
            
            _picAdvectCs.SetData(kernel, "_Space", _simulationSpaceConfig);
            _picAdvectCs.SetData(kernel, "_ParticleBuffer", _particleBuffer);
            _picAdvectCs.SetData(kernel, "_FieldVelocityBuffer", _fieldVelocityDoubleBuffer);
            _picAdvectCs.DispatchDesired(kernel, _particleBuffer.Size.x);            
        }
    }
}