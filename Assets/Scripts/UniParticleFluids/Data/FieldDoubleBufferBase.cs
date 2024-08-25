using UniParticleFluids.Configs;
using UnityEngine;

namespace UniParticleFluids.Data
{
    public class FieldDoubleBufferBase<TElement> : DoubleGridDataBase
    {
        public override Vector3Int Size => _size;
        public override object Data => _buffer.Read;
        public override object Read => _buffer.Read;
        public override object Write => _buffer.Write;
        
        [SerializeField] private RenderTexture _fieldTexture;
        
        private SwapTexture _buffer;

        public override void Initialize(IObjectResolver resolver)
        {
            ElementType = typeof(TElement);
            
            var gridSpacingConfig = resolver.Resolve<IGridSpacingConfig>();
            var simSpaceConfig = resolver.Resolve<ISimulationSpaceConfig>();
            
            _gridSpacing = gridSpacingConfig.GridSpacing;
            _size = new Vector3Int
            (
                Mathf.CeilToInt(simSpaceConfig.Scale.x / _gridSpacing),
                Mathf.CeilToInt(simSpaceConfig.Scale.y / _gridSpacing),
                Mathf.CeilToInt(simSpaceConfig.Scale.z / _gridSpacing)
            );

            RenderTextureFormat rtFormat = 
                ElementType == typeof(float) ? RenderTextureFormat.RFloat
                : ElementType == typeof(Vector2) ? RenderTextureFormat.RGFloat
                : RenderTextureFormat.ARGBFloat;
            
            _buffer = new SwapTexture(_size, rtFormat, FilterMode.Bilinear, TextureWrapMode.Clamp);
            _fieldTexture = _buffer.Read;
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

        public void Swap()
        {
            _buffer.Swap();
        }
    }
}