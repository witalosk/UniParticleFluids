using UniParticleFluids.Data;
using UnityEngine;

namespace UniParticleFluids.Modules
{
    public class InitializeParticleModule : ModuleBase
    {
        public override int DefaultModuleOrder => ModuleOrder.Initialize;
        
        [SerializeField] private float _initialSize = 0.03f;
        [SerializeField] private Color _initialColor = Color.cyan;
        [Space]
        [SerializeField] private float _initialPositionRadius = 5f;
        [SerializeField] private Vector3 _initialVelocity = Vector3.up;

        public override void Initialize(IObjectResolver resolver)
        {
            var particleBuffer = resolver.Resolve<ParticleBuffer>();
            
            // Set initial particle data
            int count = particleBuffer.Size.x;
            var cpuBuffer = new Particle[particleBuffer.Size.x];
            for (int i = 0; i < count; i++)
            {
                var particle = new Particle
                {
                    Uuid = i,
                    Size = _initialSize,
                    Position = Random.insideUnitSphere * _initialPositionRadius,
                    Velocity = _initialVelocity,
                    Color = _initialColor
                };
                cpuBuffer[i] = particle;
            }
            ((GraphicsBuffer)(particleBuffer.Data)).SetData(cpuBuffer);
        }
    }
}