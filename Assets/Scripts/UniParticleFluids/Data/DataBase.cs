using System;
using UniParticleFluids.Utilities.CustomAttributes;
using UnityEngine;

namespace UniParticleFluids.Data
{
    public abstract class DataBase : MonoBehaviour, IControllerObject, IShaderSettableData
    {
        public virtual Vector3Int Size => _size;
        public abstract object Data { get; }
        public Type ElementType { get; protected set; }
        
        [SerializeField] [Disable] protected Vector3Int _size = new(1, 1, 1);
        
        public virtual void Initialize(IObjectResolver resolver) { }
        public virtual void Deinitialize() { }
        public virtual void Run() { }
        public abstract void SetToComputeShader(ComputeShader computeShader, int kernel, string shaderName);
        public abstract void SetToMaterial(Material material, string shaderName);
    }
    
    public abstract class GridDataBase : DataBase
    {
        public virtual float GridSpacing => _gridSpacing;
        
        [SerializeField] [Disable] protected float _gridSpacing = 1.0f;
        
        public override void SetToComputeShader(ComputeShader computeShader, int kernel, string shaderName)
        {
            computeShader.SetInts(shaderName + "GridSize", _size.x, _size.y, _size.z);
            computeShader.SetFloat(shaderName + "GridSpacing", _gridSpacing);
            computeShader.SetFloat(shaderName + "GridInvSpacing", 1.0f / _gridSpacing);
        }

        public override void SetToMaterial(Material material, string shaderName)
        {
            material.SetVector(shaderName + "GridSize", new Vector4(_size.x, _size.y, _size.z, 0));
            material.SetFloat(shaderName + "GridSpacing", _gridSpacing);
            material.SetFloat(shaderName + "GridInvSpacing", 1.0f / _gridSpacing);
        }
    }
    
    public abstract class DoubleDataBase : DataBase
    {
        public abstract object Read { get; }
        public abstract object Write { get; }
    }
    
    public abstract class DoubleGridDataBase : GridDataBase
    {
        public abstract object Read { get; }
        public abstract object Write { get; }
    }
}
