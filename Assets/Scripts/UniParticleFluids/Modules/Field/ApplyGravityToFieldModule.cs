using UniParticleFluids.Data;
using UnityEngine;

namespace UniParticleFluids.Modules.Field
{
    public class ApplyGravityToFieldModule : ModuleBase
    {
        public override int DefaultModuleOrder => ModuleOrder.FieldUpdate.ApplyVelocityToField;
        
        [SerializeField] private ComputeShader _applyGravityToFieldCs;
        
        private FieldVelocityBuffer _fieldVelocityBuffer;

        public override void Initialize(IObjectResolver resolver)
        {
            _fieldVelocityBuffer = resolver.Resolve<FieldVelocityBuffer>();
        }

        public override void Run()
        {
            int kernel = _applyGravityToFieldCs.FindKernel("ApplyGravityToField");
            _applyGravityToFieldCs.SetData(kernel, "_FieldVelocityBuffer", _fieldVelocityBuffer);
            _applyGravityToFieldCs.DispatchDesired(kernel, _fieldVelocityBuffer.Size);
        }
    }
}