using System;
using UniParticleFluids.Data;
using UnityEngine;

namespace UniParticleFluids.Modules
{
    public class RenderModule : ModuleBase
    {
        public override int DefaultModuleOrder => ModuleOrder.Render;
        
        [SerializeField] Shader _renderModuleShader;
        
        private ParticleBuffer _particleBuffer;
        private Material _renderModuleMaterial;

        public override void Initialize(IObjectResolver resolver)
        {
            _renderModuleMaterial = new Material(_renderModuleShader);
            _particleBuffer = resolver.Resolve<ParticleBuffer>();
        }

        public override void Deinitialize()
        {
            DestroyImmediate(_renderModuleMaterial);
        }

        public override void Run()
        {
            _particleBuffer.SetToMaterial(_renderModuleMaterial, "_ParticleBuffer");
            Graphics.DrawProcedural(_renderModuleMaterial, new Bounds(Vector3.zero, Vector3.one * 10000), MeshTopology.Points, _particleBuffer.Size.x);
        }
    }
}