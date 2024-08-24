using UniParticleFluids.Configs;
using UnityEngine;

namespace UniParticleFluids.Data
{
    public class FieldVelocityBuffer : DoubleGridDataBase
    {
        public override Vector3Int Size => _size;
        public override object Data => _buffer.Read;
        public override object Read => _buffer.Read;
        public override object Write => _buffer.Write;
        
        [SerializeField] private RenderTexture _fieldVelocity;
        
        private SwapTexture _buffer;

        public override void Initialize(IObjectResolver resolver)
        {
            var gridSpacingConfig = resolver.Resolve<IGridSpacingConfig>();
            var simSpaceConfig = resolver.Resolve<ISpaceConfig>();
            
            _size = new Vector3Int
            (
                Mathf.CeilToInt(simSpaceConfig.Scale.x / gridSpacingConfig.GridSpacing),
                Mathf.CeilToInt(simSpaceConfig.Scale.y / gridSpacingConfig.GridSpacing),
                Mathf.CeilToInt(simSpaceConfig.Scale.z / gridSpacingConfig.GridSpacing)
            );
            
            _gridSpacing = gridSpacingConfig.GridSpacing;
            _buffer = new SwapTexture(_size, RenderTextureFormat.ARGBFloat, FilterMode.Bilinear, TextureWrapMode.Clamp);
            _fieldVelocity = _buffer.Read;
        }

        public override void Deinitialize()
        {
            _buffer.Dispose();
            _buffer = null;
        }
        
        public override void SetToComputeShader(ComputeShader computeShader, int kernel, string shaderName)
        {
            base.SetToComputeShader(computeShader, kernel, shaderName);
            
            computeShader.SetTexture(kernel, shaderName, _buffer.Read);
            computeShader.SetTexture(kernel, $"{shaderName}Write", _buffer.Write);
        }

        public override void SetToMaterial(Material material, string shaderName)
        {
            base.SetToMaterial(material, shaderName);
            
            material.SetTexture(shaderName, _buffer.Read);
        }
    }
}