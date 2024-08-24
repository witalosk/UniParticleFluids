using UnityEngine;

namespace UniParticleFluids.Modules
{
    public abstract class ModuleBase : MonoBehaviour, IControllerObject
    {
        public bool IsActive => _isActive;
        public abstract int DefaultModuleOrder { get; }
        
        [SerializeField] protected bool _isActive = true;

        public virtual void Initialize(IObjectResolver resolver) { }
        public virtual void Deinitialize() { }
        public virtual void Run() { }
    }
}