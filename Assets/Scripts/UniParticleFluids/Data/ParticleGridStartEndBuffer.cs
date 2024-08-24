using UniParticleFluids.Configs;
using UnityEngine;

namespace UniParticleFluids.Data
{
    public class ParticleGridStartEndBuffer : DataBase
    {
        public override Vector3Int Size { get; }
        public override object Data { get; }
        
        public override void Initialize(IObjectResolver resolver)
        {
            var gridSpacingConfig = resolver.Resolve<IGridSpacingConfig>();
            
        }
        
        public override void SetToComputeShader(ComputeShader computeShader, int kernel, string name)
        {
            throw new System.NotImplementedException();
        }

        public override void SetToMaterial(Material material, string name)
        {
            throw new System.NotImplementedException();
        }
    }
}