using UniParticleFluids.Configs;
using UniParticleFluids.Data;
using UnityEngine;

namespace UniParticleFluids.Modules.Pic
{
    public class PressureProjectionModule : ModuleBase
    {
        public override int DefaultModuleOrder => ModuleOrder.PicUpdate.PressureProjection;

        [SerializeField] private ComputeShader _pressureProjectionCs;

        private ISimulationSpaceConfig _spaceConfig;
        private PicConfig _picConfig;
        private FieldVelocityDoubleBuffer _fieldVelocityDoubleBuffer;
        private FieldPressureDoubleBuffer _fieldPressureDoubleBuffer;
        private FieldDivergenceBuffer _fieldDivergenceBuffer;
        
        
        public override void Initialize(IObjectResolver resolver)
        {
            _spaceConfig = resolver.Resolve<ISimulationSpaceConfig>();
            _picConfig = resolver.Resolve<PicConfig>();
            _fieldVelocityDoubleBuffer = resolver.Resolve<FieldVelocityDoubleBuffer>();
            _fieldPressureDoubleBuffer = resolver.Resolve<FieldPressureDoubleBuffer>();
            _fieldDivergenceBuffer = resolver.Resolve<FieldDivergenceBuffer>();
        }

        public override void Run()
        {
            // Calc Divergence
            int kernelId = _pressureProjectionCs.FindKernel("CalcDivergence");
            _pressureProjectionCs.SetData(kernelId, "_Space", _spaceConfig);
            _pressureProjectionCs.SetData(kernelId, "_FieldVelocityBuffer", _fieldVelocityDoubleBuffer);
            _pressureProjectionCs.SetData(kernelId, "_FieldDivergenceBufferWrite", _fieldDivergenceBuffer);
            _pressureProjectionCs.DispatchDesired(kernelId, _fieldDivergenceBuffer.Size);
            
            // Projection
            kernelId = _pressureProjectionCs.FindKernel("Project");
            float spacing = _fieldVelocityDoubleBuffer.GridSpacing;
            Vector3 invSpacingSq = Vector3.one / spacing / spacing;
            float temp = 1f / (2f * (invSpacingSq.x + invSpacingSq.y + invSpacingSq.z));
            float projNeighborCoef = temp / spacing / spacing;
            float projCenterCoef = -temp;
            _pressureProjectionCs.SetFloat("_ProjNeighborCoef", projNeighborCoef);
            _pressureProjectionCs.SetFloat("_ProjCenterCoef", projCenterCoef);
            for (int i = 0; i < _picConfig.PressureIterationNum; i++)
            {
                _pressureProjectionCs.SetData(kernelId, "_FieldPressureBuffer", _fieldPressureDoubleBuffer);
                _pressureProjectionCs.SetData(kernelId, "_FieldDivergenceBuffer", _fieldDivergenceBuffer);
                _pressureProjectionCs.DispatchDesired(kernelId, _fieldPressureDoubleBuffer.Size);
                _fieldPressureDoubleBuffer.Swap();
            }
            
            // Update Velocity
            kernelId = _pressureProjectionCs.FindKernel("UpdateVelocity");
            _pressureProjectionCs.SetData(kernelId, "_FieldVelocityBufferWrite", _fieldVelocityDoubleBuffer);
            _pressureProjectionCs.SetData(kernelId, "_FieldPressureBuffer", _fieldPressureDoubleBuffer);
            _pressureProjectionCs.DispatchDesired(kernelId, _fieldVelocityDoubleBuffer.Size);
        }
    }
}