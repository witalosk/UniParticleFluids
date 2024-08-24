#ifndef HLSL_INCLUDE_MACROS
#define HLSL_INCLUDE_MACROS

#define DEFINE_SPACE(name) float3 name##Min; float3 name##Max;
#define DEFINE_GRID(name) int3 name##GridSize; float name##GridSpacing; float name##GridInvSpacing;

#endif  