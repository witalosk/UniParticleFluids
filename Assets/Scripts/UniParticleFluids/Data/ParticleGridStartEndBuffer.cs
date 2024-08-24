using UniParticleFluids.Configs;
using UnityEngine;

namespace UniParticleFluids.Data
{
    public class ParticleGridStartEndBuffer : DataBase
    {
        public override Vector3Int Size => new(_size, 1, 1);
        public Vector3Int GridSize => _gridSize;
        public override object Data => _startEndBuffer;
        
        [SerializeField] private Vector3Int _gridSize;
        [SerializeField] private int _size;
        
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
            
            _size = _gridSize.x * _gridSize.y * _gridSize.z;
            _startEndBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, _size, 2 * sizeof(uint));
        }

        public override void Deinitialize()
        {
            _startEndBuffer?.Release();
            _startEndBuffer = null;
        }

        public override void SetToComputeShader(ComputeShader computeShader, int kernel, string shaderName)
        {
            computeShader.SetBuffer(kernel, shaderName, _startEndBuffer);
        }

        public override void SetToMaterial(Material material, string shaderName)
        {
            material.SetBuffer(shaderName, _startEndBuffer);
        }
    }
}