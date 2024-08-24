using UnityEngine;

namespace UniParticleFluids.Configs
{
    public class TimeStepConfig : ConfigBase, ITimeStepConfig
    {
        public float TimeStep => _timeStep;
        
        [SerializeField] private float _timeStep = 0.03f;
    }
}