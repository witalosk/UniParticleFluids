using UniParticleFluids.Configs;
using UniParticleFluids.Data;
using UniParticleFluids.GPUBufferOperators;
using UnityEngine;

namespace UniParticleFluids.Modules
{
    public class ParticleSortModule : ModuleBase
    {
        public override int DefaultModuleOrder => ModuleOrder.PreUpdate;
        
        [SerializeField] private ComputeShader _gridSortCs;
        
        private ParticleBuffer _particleBuffer;
        private ParticleGridStartEndBuffer _gridStartEndBuffer;
        private ISimulationSpaceConfig _simulationSpaceConfig;
        private IGridSpacingConfig _gridSpacingConfig;
        private GPURadixSort _radixSort;

        private GraphicsBuffer _objectCellIDPairBuffer;

        public override void Initialize(IObjectResolver resolver)
        {
            _particleBuffer = resolver.Resolve<ParticleBuffer>();
            _gridStartEndBuffer = resolver.Resolve<ParticleGridStartEndBuffer>();
            _simulationSpaceConfig = resolver.Resolve<ISimulationSpaceConfig>();
            _gridSpacingConfig = resolver.Resolve<IGridSpacingConfig>();
            _radixSort = new GPURadixSort();
            _objectCellIDPairBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, _particleBuffer.Size.x, sizeof(uint) * 2);
        }

        public override void Deinitialize()
        {
            _radixSort?.Dispose();
            _objectCellIDPairBuffer?.Release();
            _objectCellIDPairBuffer = null;
        }

        public override void Run()
        {
            int numObjects = _particleBuffer.Size.x;
            int numGrids = _gridStartEndBuffer.Size.x;
            var gridMin = _simulationSpaceConfig.Min;
            var gridMax = _simulationSpaceConfig.Max;
            var gridSize = _gridStartEndBuffer.GridSize;
            float gridSpacing = _gridSpacingConfig.GridSpacing;

            var cs = _gridSortCs;

            cs.SetInt("_NumObjects", numObjects);

            cs.SetVector("_GridMin", gridMin);
            cs.SetVector("_GridMax", gridMax);
            cs.SetInts("_GridSize", gridSize.ToInts());
            cs.SetFloat("_GridSpacing", gridSpacing);
            cs.SetFloat("_GridInvSpacing", 1f / gridSpacing);

            // make <cellID, objectID> pair
            int k = cs.FindKernel("MakeObjectCellIDPair");
            cs.SetData(k, "_ObjectBuffer", _particleBuffer);
            cs.SetBuffer(k, "_ObjectCellIDPairBufferWrite", _objectCellIDPairBuffer);
            cs.DispatchDesired(k, numObjects);

            // sort
            _radixSort.Sort(_objectCellIDPairBuffer, GPURadixSort.KeyType.UInt, (uint)numGrids - 1);

            // clear grid objectID
            k = cs.FindKernel("ClearGridObjectID");
            
            cs.SetData(k, "_GridObjectIDBufferWrite", _gridStartEndBuffer);
            cs.DispatchDesired(k, numGrids);

            // set grid objectID
            k = cs.FindKernel("SetGridObjectID");
            cs.SetBuffer(k, "_ObjectCellIDPairBufferRead", _objectCellIDPairBuffer);
            cs.SetData(k, "_GridObjectIDBufferWrite", _gridStartEndBuffer);
            cs.DispatchDesired(k, numObjects);

            // rearrange object
            k = cs.FindKernel("RearrangeObject");
            cs.SetBuffer(k, "_ObjectCellIDPairBufferRead", _objectCellIDPairBuffer);
            cs.SetData(k, "_ObjectBuffer", _particleBuffer);
            cs.DispatchDesired(k,numObjects);

            // swap object buffer
            _particleBuffer.Swap();
        }
    }
}