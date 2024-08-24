using UnityEngine;

namespace UniParticleFluids.Configs
{
    public class PicConfig : ConfigBase, IGridSpacingConfig
    {
        public float GridSpacing => _gridSpacing;
        
        [SerializeField] private float _gridSpacing = 0.25f;
    }
}