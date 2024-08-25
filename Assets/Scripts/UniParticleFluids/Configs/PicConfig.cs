using UnityEngine;

namespace UniParticleFluids.Configs
{
    public class PicConfig : ConfigBase, IGridSpacingConfig, ITimeStepConfig
    {
        public float GridSpacing => _gridSpacing;
        public float Flipness => _flipness;
        public int PressureIterationNum => _pressureIterationNum;
        public float TimeStep => _simulationStep;
        
        [SerializeField] private float _simulationStep = 0.03f;
        [SerializeField] private float _gridSpacing = 0.25f;
        [SerializeField] private float _flipness = 0.99f;
        [SerializeField] private int _pressureIterationNum = 5;
    }
}