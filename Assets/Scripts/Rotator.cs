using System;
using UnityEngine;

namespace UniParticleFluids
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private Vector3 _rotationSpeed = new (0, 10, 0);
        
        private void Update()
        {
            transform.Rotate(_rotationSpeed * Time.deltaTime);
        }
    }
}