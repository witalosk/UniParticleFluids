using System;
using UniParticleFluids.Data;
using UnityEngine;
using UnityEngine.Rendering;

namespace UniParticleFluids
{
    public static class Utility
    {
        private static readonly int _desiredThreadNumId = Shader.PropertyToID("_DesiredThreadNum");

        /// <summary>
        /// Dispatch the compute shader with the desired thread num.
        /// </summary>
        public static void DispatchDesired(this ComputeShader cs, int kernel, int desiredX, int desiredY = 1, int desiredZ = 1)
        {
            cs.SetInts(_desiredThreadNumId, desiredX, desiredY, desiredZ);
            cs.GetKernelThreadGroupSizes(kernel, out uint x, out uint y, out uint z);
            cs.Dispatch(kernel, Mathf.CeilToInt(desiredX / (float)x), Mathf.CeilToInt(desiredY / (float)y), Mathf.CeilToInt(desiredZ / (float)z));
        }
        
        /// <summary>
        /// Dispatch the compute shader with the desired thread num.
        /// </summary>
        public static void DispatchDesired(this ComputeShader cs, int kernel, Vector3Int desired)
        {
            DispatchDesired(cs, kernel, desired.x, desired.y, desired.z);
        }
        
        public static int[] ToInts(this Vector3Int v)
        {
            return new[] {v.x, v.y, v.z};
        }
        
        public static int[] ToInts(this Vector2Int v)
        {
            return new[] {v.x, v.y};
        }
        
        public static void SetData(this ComputeShader cs, int kernel, string name, IShaderSettableData data)
        {
            data.SetToComputeShader(cs, kernel, name);
        }
        
        public static void SetData(this Material material, string name, IShaderSettableData data)
        {
            data.SetToMaterial(material, name);
        }
    }

    public class SwapBuffer : IDisposable
    {
        public GraphicsBuffer Read => _readBuffer;
        public GraphicsBuffer Write => _writeBuffer;
        
        private GraphicsBuffer _readBuffer;
        private GraphicsBuffer _writeBuffer;
        
        public SwapBuffer(int count, int stride)
        {
            _readBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, count, stride);
            _readBuffer.name = "buf1";
            _writeBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, count, stride);
            _writeBuffer.name = "buf2";
        }
        
        ~SwapBuffer()
        {
            Dispose();
        }
        
        public void Swap()
        {
            (_readBuffer, _writeBuffer) = (_writeBuffer, _readBuffer);
        }

        public void Dispose()
        {
            _readBuffer?.Dispose();
            _writeBuffer?.Dispose();
            _readBuffer = null;
            _writeBuffer = null;
        }
    }
    
    public class SwapTexture : IDisposable
    {
        public RenderTexture Read => _readBuffer;
        public RenderTexture Write => _writeBuffer;
        
        private RenderTexture _readBuffer;
        private RenderTexture _writeBuffer;
        
        public SwapTexture(Vector3Int resolution, RenderTextureFormat format, FilterMode filterMode, TextureWrapMode wrapMode)
        {
            _readBuffer = new RenderTexture(resolution.x, resolution.y, 0, format)
            {
                enableRandomWrite = true, 
                filterMode = filterMode,
                volumeDepth = resolution.z,
                dimension = TextureDimension.Tex3D,
                wrapMode = wrapMode
            };
            _readBuffer.Create();
            
            _writeBuffer = new RenderTexture(resolution.x, resolution.y, 0, format)
            {
                enableRandomWrite = true, 
                filterMode = filterMode,
                volumeDepth = resolution.z,
                dimension = TextureDimension.Tex3D,
                wrapMode = wrapMode
            };
            _writeBuffer.Create();
        }

        
        ~SwapTexture()
        {
            Dispose();
        }
        
        public void Swap()
        {
            (_readBuffer, _writeBuffer) = (_writeBuffer, _readBuffer);
        }

        public void Dispose()
        {
            _readBuffer?.Release();
            _writeBuffer?.Release();
            _readBuffer = null;
            _writeBuffer = null;
        }
    }
}