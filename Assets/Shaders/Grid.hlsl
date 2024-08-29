#ifndef HLSL_INCLUDE_GRID
#define HLSL_INCLUDE_GRID

inline uint CellIndexToCellID(int3 index, int3 grid_size)
{
    uint3 clamped_index = clamp(index, (int3)0, grid_size - 1);
    return clamped_index.x + clamped_index.y * grid_size.x + clamped_index.z * grid_size.x * grid_size.y;
}

inline int3 CellIDToCellIndex(uint id, int3 grid_size)
{
    int x = id % grid_size.x;
    int y = id / grid_size.x % grid_size.y;
    int z = id / (grid_size.x * grid_size.y);
    return int3(x, y, z);
}

inline float3 WorldPosToGridPos(float3 position, float3 grid_min, float grid_inv_spacing)
{
    return (position - grid_min) * grid_inv_spacing;
}

inline int3 WorldPosToCellIndex(float3 position, float3 grid_min, float grid_inv_spacing)
{
    return floor(WorldPosToGridPos(position, grid_min, grid_inv_spacing));
}

inline uint WorldPosToCellID(float3 position, float3 grid_min, int3 grid_size, float grid_inv_spacing)
{
    return CellIndexToCellID(WorldPosToCellIndex(position, grid_min, grid_inv_spacing), grid_size);
}

inline float3 WorldPosToUv(float3 position, float3 grid_min, float grid_inv_spacing, int3 grid_size)
{
    return WorldPosToGridPos(position, grid_min, grid_inv_spacing) / grid_size;
}

inline float3 CellIndexToWorldPos(int3 index, float3 grid_min, float grid_spacing)
{
    return grid_min + (index + 0.5f) * grid_spacing;
}

inline float3 CellIndexToUv(int3 index, int3 gridSize)
{
    return saturate(((float3)index + 0.5) / gridSize);
}


inline float3 GridPosToWorldPos(float3 position, float3 grid_min, float grid_spacing)
{
    return grid_min + position * grid_spacing;
}

#define FOR_EACH_NEIGHBOR_CELL_START(C_INDEX, NC_INDEX, NC_ID, RANGE, GRID_SIZE) {\
for (int i = (int)C_INDEX.x + RANGE[0]; i <= (int)C_INDEX.x + RANGE[1]; ++i)\
for (int j = (int)C_INDEX.y + RANGE[2]; j <= (int)C_INDEX.y + RANGE[3]; ++j)\
for (int k = (int)C_INDEX.z + RANGE[4]; k <= (int)C_INDEX.z + RANGE[5]; ++k) {\
    const int3 NC_INDEX = int3(i, j, k);\
    const uint NC_ID = CellIndexToCellID(NC_INDEX, GRID_SIZE);\

#define FOR_EACH_NEIGHBOR_CELL_END }}

#define FOR_EACH_NEIGHBOR_CELL_PARTICLE_START(C_INDEX, P_ID, P_ID_BUFFER, RANGE, GRID_SIZE) {\
for (int i = max((int)C_INDEX.x + RANGE[0], 0); i <= min((int)C_INDEX.x + RANGE[1], GRID_SIZE.x - 1); ++i)\
for (int j = max((int)C_INDEX.y + RANGE[2], 0); j <= min((int)C_INDEX.y + RANGE[3], GRID_SIZE.y - 1); ++j)\
for (int k = max((int)C_INDEX.z + RANGE[4], 0); k <= min((int)C_INDEX.z + RANGE[5], GRID_SIZE.z - 1); ++k) {\
    const uint2 index = P_ID_BUFFER[CellIndexToCellID(int3(i, j, k), GRID_SIZE)];\
    for (uint P_ID = index.x; P_ID < index.y; ++P_ID) {\

#define FOR_EACH_NEIGHBOR_CELL_PARTICLE_END }}}

#endif