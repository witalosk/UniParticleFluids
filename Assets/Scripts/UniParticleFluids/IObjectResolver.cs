namespace UniParticleFluids
{
    public interface IObjectResolver
    {
        T Resolve<T>();
    }
}