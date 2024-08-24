using UnityEngine;

namespace UniParticleFluids.Configs
{
    public interface ISpaceConfig : IShaderSettableData
    {
        Vector3 Position { get; }
        Vector3 Scale { get; }
        
        Vector3 Min => Position - Scale * 0.5f;
        Vector3 Max => Position + Scale * 0.5f;
    }
}