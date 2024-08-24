using UniParticleFluids.Configs;
using UniParticleFluids.Data;
using UnityEngine;

namespace UniParticleFluids.Modules.Field
{
    public class ApplyGravityToFieldModule : ModuleBase
    {
        public override int DefaultModuleOrder => ModuleOrder.PicUpdate.ApplyVelocityToField;
        
        [SerializeField] private ComputeShader _applyGravityToFieldCs;
        [SerializeField] private Vector3 _gravity = new(0, -9.81f, 0);
        
        private FieldVelocityBuffer _fieldVelocityBuffer;
        private ITimeStepConfig _timeStepConfig;

        public override void Initialize(IObjectResolver resolver)
        {
            _fieldVelocityBuffer = resolver.Resolve<FieldVelocityBuffer>();
            _timeStepConfig = resolver.Resolve<ITimeStepConfig>();
        }

        public override void Run()
        {
            int kernel = _applyGravityToFieldCs.FindKernel("ApplyGravityToField");
            _applyGravityToFieldCs.SetVector("_Gravity", _gravity);
            _applyGravityToFieldCs.SetFloat("_TimeStep", _timeStepConfig.TimeStep);
            _applyGravityToFieldCs.SetData(kernel, "_FieldVelocityBufferRead", _fieldVelocityBuffer);
            _applyGravityToFieldCs.SetData(kernel, "_FieldVelocityBuffer", _fieldVelocityBuffer);
            _applyGravityToFieldCs.DispatchDesired(kernel, _fieldVelocityBuffer.Size);
        }
    }
}