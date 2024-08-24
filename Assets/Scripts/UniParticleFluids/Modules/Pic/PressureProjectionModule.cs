using UniParticleFluids.Data;

namespace UniParticleFluids.Modules.Pic
{
    public class PressureProjectionModule : ModuleBase
    {
        public override int DefaultModuleOrder => ModuleOrder.PicUpdate.PressureProjection;
        
        private FieldVelocityBuffer _fieldVelocityBuffer;
        
        public override void Initialize(IObjectResolver resolver)
        {
            _fieldVelocityBuffer = resolver.Resolve<FieldVelocityBuffer>();
            
        }

        public override void Run()
        {
            
        }
    }
}