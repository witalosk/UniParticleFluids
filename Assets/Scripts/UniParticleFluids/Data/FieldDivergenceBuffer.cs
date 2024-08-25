using UniParticleFluids.Configs;
using UnityEngine;
using UnityEngine.Rendering;

namespace UniParticleFluids.Data
{
    public class FieldDivergenceBuffer : GridDataBase
    {
        public override object Data => _divergenceBuffer;
        
        [SerializeField] private RenderTexture _divergenceBuffer;

        public override void Initialize(IObjectResolver resolver)
        {
            var gridSpacingConfig = resolver.Resolve<IGridSpacingConfig>();
            var simSpaceConfig = resolver.Resolve<ISimulationSpaceConfig>();
            
            _gridSpacing = gridSpacingConfig.GridSpacing;
            _size = new Vector3Int
            (
                Mathf.CeilToInt(simSpaceConfig.Scale.x / _gridSpacing),
                Mathf.CeilToInt(simSpaceConfig.Scale.y / _gridSpacing),
                Mathf.CeilToInt(simSpaceConfig.Scale.z / _gridSpacing)
            );

            _divergenceBuffer = new RenderTexture(_size.x, _size.y, 0, RenderTextureFormat.ARGBFloat)
            {
                enableRandomWrite = true,
                filterMode = FilterMode.Bilinear,
                volumeDepth = _size.z,
                dimension = TextureDimension.Tex3D,
                wrapMode = TextureWrapMode.Clamp
            };
        }

        public override void Deinitialize()
        {
            _divergenceBuffer.Release();
            _divergenceBuffer = null;
        }

        public override void SetToComputeShader(ComputeShader computeShader, int kernel, string shaderName)
        {
            base.SetToComputeShader(computeShader, kernel, shaderName);

            computeShader.SetTexture(kernel, shaderName, _divergenceBuffer);
        }

        public override void SetToMaterial(Material material, string shaderName)
        {
            base.SetToMaterial(material, shaderName);

            material.SetTexture(shaderName, _divergenceBuffer);
        }
    }
}