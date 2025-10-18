using System;
using System.Collections.Generic;
using UnityEngine;

namespace LightMiniGame
{
    public class LightMiniGameManager : MonoBehaviour
    {
        [Header("Scene References")]
        [SerializeField] private LaserBeam2D laserBeam;
        [SerializeField] private List<Target2D> targets = new();

        [Header("Puzzle Options")]
        [SerializeField] private bool autoStart = true;   // 자동 시작 (기본 On)
        [SerializeField] private bool requireAllTargets = false;

        private bool isRunning;
        private readonly HashSet<Target2D> clearedTargets = new();

        public event Action PuzzleCleared;

        private void Awake()
        {
            if (laserBeam == null)
                laserBeam = FindObjectOfType<LaserBeam2D>(true);

            if (targets.Count == 0)
                targets.AddRange(FindObjectsOfType<Target2D>(true));
        }

        private void OnEnable()
        {
            foreach (var t in targets)
                if (t != null)
                    t.LaserArrived += Target2D_OnLaserArrived;

            if (autoStart)
                StartPuzzle();
        }

        private void OnDisable()
        {
            foreach (var t in targets)
                if (t != null)
                    t.LaserArrived -= Target2D_OnLaserArrived;
        }

        private void StartPuzzle()
        {
            isRunning = true;
            clearedTargets.Clear();
            Debug.Log("[LightMiniGame] Puzzle Started");
        }

        private void Target2D_OnLaserArrived(Target2D target)
        {
            if (!isRunning || target == null) return;

            Debug.Log("[LightMiniGame] Target hit: " + target.name);

            if (!requireAllTargets)
            {
                OnPuzzleCleared();
                return;
            }

            if (clearedTargets.Add(target) && clearedTargets.Count >= targets.Count)
            {
                OnPuzzleCleared();
            }
        }

        private void OnPuzzleCleared()
        {
            if (!isRunning) return;
            isRunning = false;

            Debug.Log("[LightMiniGame] Puzzle Cleared!");
            PuzzleCleared?.Invoke(); 
        }
    }
}
