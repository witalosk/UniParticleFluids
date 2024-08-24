namespace UniParticleFluids.Modules
{
    /// <summary>
    /// Module Order
    /// 0 - 999 : Initialize
    /// 1000 - 1999 : Pre Update
    /// 2000 - 3999 : Update
    /// 4000 - 5999 : Force Update
    /// 6000 - 6999 : Integrate
    /// 7000 - 7999 : Finalize
    /// 8000 - 8999 : Post Update
    /// 9000 - 9999 : Render
    /// 10000 - : Custom
    /// </summary>
    public static class ModuleOrder
    {
        public const int Initialize = 0;
        public const int PreUpdate = 1000;
        public const int Update = 2000;
        public const int ForceUpdate = 4000;
        public const int Integrate = 6000;
        public const int Finalize = 7000;
        public const int PostUpdate = 8000;
        public const int Render = 9000;
        public const int Custom = 10000;
        
        
        public static class PicUpdate
        {
            public const int ParticleToGrid = 2000;
            public const int ApplyVelocityToField = 2050;
            public const int GridToParticle = 2080;
            public const int Advect = 2090;
        }

        public static class FieldUpdate
        {
            public const int ApplyVelocityToField = 2050;
        }
    }
}