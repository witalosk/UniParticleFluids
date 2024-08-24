using System.Collections.Generic;
using UniParticleFluids.Data;
using UniParticleFluids.Modules;
using UnityEngine;

namespace UniParticleFluids
{
    public class GpuParticleController : MonoBehaviour, IObjectResolver
    {
        [SerializeField] private List<DataBase> _dataList = new();
        [SerializeField] private List<ModuleBase> _moduleList = new();
        
        private void Start()
        {
            _dataList.AddRange(GetComponentsInChildren<DataBase>());
            _moduleList.AddRange(GetComponentsInChildren<ModuleBase>());
            
            // Initialize
            foreach (var data in _dataList)
            {
                data.Initialize(this);
            }
            
            _moduleList.Sort((a, b) => a.DefaultModuleOrder.CompareTo(b.DefaultModuleOrder));
            foreach (var module in _moduleList)
            {
                module.Initialize(this);
            }
        }

        private void Update()
        {
            foreach (var module in _moduleList)
            {
                if (!module.IsActive || module.DefaultModuleOrder < ModuleOrder.PreUpdate) continue;
                module.Run();
            }
        }

        private void OnDestroy()
        {
            foreach (var data in _dataList)
            {
                data.Deinitialize();
            }
            foreach (var module in _moduleList)
            {
                module.Deinitialize();
            }
        }

        public T Resolve<T>()
        {
            foreach (var data in _dataList)
            {
                if (data is T t) return t;
            }
            foreach (var module in _moduleList)
            {
                if (module is T t) return t;
            }
            
            return default;
        }
    }
}