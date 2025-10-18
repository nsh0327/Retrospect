using System;
using UnityEngine;

namespace LightMiniGame
{
    public class Target2D : MonoBehaviour
    {
        public event Action<Target2D> LaserArrived; 
        private bool isCleared;

        public void OnLaserArrived()
        {
            if (isCleared) return;

            isCleared = true;
            Debug.Log($"[LightMiniGame] Target reached: {name}");
            LaserArrived?.Invoke(this); 
        }

        public bool IsCleared() => isCleared;

        public void ResetState()
        {
            isCleared = false;
        }
    }
}
