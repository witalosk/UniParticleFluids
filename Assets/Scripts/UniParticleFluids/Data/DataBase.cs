using UnityEngine;

namespace UniParticleFluids.Data
{
    public abstract class DataBase : MonoBehaviour, IControllerObject
    {
        public abstract Vector3Int Size { get; }
        public abstract object Data { get; }
        public virtual void Initialize(IObjectResolver resolver) { }
        public virtual void Deinitialize() { }
        public virtual void Run() { }
        public abstract void SetToComputeShader(ComputeShader computeShader, int kernel, string name);
        public abstract void SetToMaterial(Material material, string name);
    }
}