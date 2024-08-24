using UniParticleFluids.Configs;
using UniParticleFluids.Utilities.CustomAttributes;
using UnityEngine;

namespace UniParticleFluids.Data
{
    public class ParticleGridStartEndBuffer : GridDataBase
    {
        public Vector3Int GridSize => _gridSize;
        public override object Data => _startEndBuffer;
        
        [SerializeField] [Disable] private Vector3Int _gridSize;
        
        private GraphicsBuffer _startEndBuffer;
        
        public override void Initialize(IObjectResolver resolver)
        {
            var gridSpacingConfig = resolver.Resolve<IGridSpacingConfig>();
            var simSpaceConfig = resolver.Resolve<ISpaceConfig>();
            _gridSize = new Vector3Int
            (
                Mathf.CeilToInt(simSpaceConfig.Scale.x / gridSpacingConfig.GridSpacing),
                Mathf.CeilToInt(simSpaceConfig.Scale.y / gridSpacingConfig.GridSpacing),
                Mathf.CeilToInt(simSpaceConfig.Scale.z / gridSpacingConfig.GridSpacing)
            );
            
            _gridSpacing = gridSpacingConfig.GridSpacing;
            _size = new Vector3Int(_gridSize.x * _gridSize.y * _gridSize.z, 1, 1);
            _startEndBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, _size.x, 2 * sizeof(uint));
        }

        public override void Deinitialize()
        {
            _startEndBuffer?.Release();
            _startEndBuffer = null;
        }

        public override void SetToComputeShader(ComputeShader computeShader, int kernel, string shaderName)
        {
            computeShader.SetInts(shaderName + "GridSize", _gridSize.x, _gridSize.y, _gridSize.z);
            computeShader.SetFloat(shaderName + "GridSpacing", _gridSpacing);
            computeShader.SetFloat(shaderName + "GridInvSpacing", 1.0f / _gridSpacing);
            computeShader.SetBuffer(kernel, shaderName, _startEndBuffer);
        }

        public override void SetToMaterial(Material material, string shaderName)
        {
            material.SetBuffer(shaderName, _startEndBuffer);
        }
    }
}