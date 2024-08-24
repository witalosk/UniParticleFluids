using UniParticleFluids.Modules;

namespace UniParticleFluids
{
    public interface IControllerObject
    {
        void Initialize(IObjectResolver resolver);
		void Deinitialize();
	    void Run();
    }
}