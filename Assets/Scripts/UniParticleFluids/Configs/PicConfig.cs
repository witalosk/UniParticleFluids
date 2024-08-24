using UnityEngine;

namespace UniParticleFluids.Configs
{
    public class PicConfig : ConfigBase, IGridSpacingConfig
    {
        public float GridSpacing => _gridSpacing;
        public float Flipness => _flipness;
        
        [SerializeField] private float _gridSpacing = 0.25f;
        [SerializeField] private float _flipness = 0.99f;
    }
}