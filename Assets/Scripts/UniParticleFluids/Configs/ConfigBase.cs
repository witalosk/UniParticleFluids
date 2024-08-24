using UnityEngine;

namespace UniParticleFluids.Configs
{
    public class ConfigBase : MonoBehaviour, IControllerObject
    {
        public void Initialize(IObjectResolver resolver) { }
        public void Deinitialize() { }
        public void Run() { }
    }
}