#ifndef HLSL_INCLUDE_PARTICLE
#define HLSL_INCLUDE_PARTICLE

struct Particle
{
    int uuid;
    float size;
    float3 position;
    float3 velocity;
    float4 color;
};

#endif // HLSL_INCLUDE_PARTICLE