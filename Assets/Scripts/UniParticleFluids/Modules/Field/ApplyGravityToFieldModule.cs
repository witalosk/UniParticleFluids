using UniParticleFluids.Configs;
using UniParticleFluids.Data;
using UnityEngine;

namespace UniParticleFluids.Modules.Field
{
    public class ApplyGravityToFieldModule : ModuleBase
    {
        public override int DefaultModuleOrder => ModuleOrder.FieldUpdate.ApplyVelocityToField;
        
        [SerializeField] private ComputeShader _applyGravityToFieldCs;
        [SerializeField] private Vector3 _gravity = new(0, -9.81f, 0);
        
        private FieldVelocityDoubleBuffer _fieldVelocityDoubleBuffer;
        private ITimeStepConfig _timeStepConfig;

        public override void Initialize(IObjectResolver resolver)
        {
            _fieldVelocityDoubleBuffer = resolver.Resolve<FieldVelocityDoubleBuffer>();
            _timeStepConfig = resolver.Resolve<ITimeStepConfig>();
        }

        public override void Run()
        {
            int kernel = _applyGravityToFieldCs.FindKernel("ApplyGravityToField");
            _applyGravityToFieldCs.SetVector("_Gravity", _gravity);
            _applyGravityToFieldCs.SetFloat("_TimeStep", _timeStepConfig.TimeStep);
            _applyGravityToFieldCs.SetData(kernel, "_FieldVelocityBufferRead", _fieldVelocityDoubleBuffer);
            _applyGravityToFieldCs.SetData(kernel, "_FieldVelocityBuffer", _fieldVelocityDoubleBuffer);
            _applyGravityToFieldCs.DispatchDesired(kernel, _fieldVelocityDoubleBuffer.Size);
        }
    }
}