using UnityEngine;

namespace UniParticleFluids.Configs
{
    public class ConfigBase : MonoBehaviour, IControllerObject
    {
        public virtual void Initialize(IObjectResolver resolver) { }
        public virtual void Deinitialize() { }
        public void Run() { }
    }
}