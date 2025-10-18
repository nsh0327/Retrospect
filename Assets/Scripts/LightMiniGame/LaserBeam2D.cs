using System.Collections.Generic;
using UnityEngine;

namespace LightMiniGame
{
    [RequireComponent(typeof(LineRenderer))]
    public class LaserBeam2D : MonoBehaviour
    {
        private const float k_MinimumOffset = 0.001f;
        private const int k_SafetyLimit = 100;

        [Header("Laser Settings")]
        [SerializeField] private Vector2 initialDirection = Vector2.right;
        [SerializeField] private float maxDistance = 100f;
        [SerializeField] private int maxBounces = 20;
        [SerializeField] private LayerMask hitMask;

        [Header("Visual")]
        [SerializeField] private Material coreMaterial;   
        [SerializeField] private Material glowMaterial;   
        [SerializeField] private float coreWidth = 0.05f;
        [SerializeField] private float glowWidth = 0.18f;

        private LineRenderer coreLR;
        private LineRenderer glowLR; 
        private readonly List<Vector3> points = new();

        // 현재 프레임에 맞은 타겟(엣지 트리거용)
        private Target2D currentTarget;   

        private void Awake()
        {
            
            coreLR = GetComponent<LineRenderer>();
            ConfigureLR(coreLR, coreMaterial, coreWidth, order: 11);

            if (glowMaterial != null)
            {
                var g = new GameObject("LaserGlow");
                g.transform.SetParent(transform, false);
                glowLR = g.AddComponent<LineRenderer>();
                ConfigureLR(glowLR, glowMaterial, glowWidth, order: 10);
            }
        }

        private void ConfigureLR(LineRenderer lr, Material mat, float width, int order)
        {
            lr.useWorldSpace = true;
            lr.alignment = LineAlignment.View;
            lr.numCapVertices = 8;
            lr.numCornerVertices = 8;
            lr.textureMode = LineTextureMode.Stretch;
            lr.sortingOrder = order;
            lr.widthMultiplier = width;

            if (mat != null)
            {
                lr.sharedMaterial = mat;
                var mats = lr.sharedMaterials;
                if (mats == null || mats.Length == 0 || mats[0] != mat)
                    lr.sharedMaterials = new[] { mat };
            }
        }

        private void Update() => DrawLaser();

        private void DrawLaser()
        {
            points.Clear();

            Vector2 origin = transform.position;
            Vector2 dir = initialDirection.normalized;

            points.Add(origin);
            int bounces = 0, safety = 0;

            // 이번 프레임에 새로 맞은 타겟
            Target2D hitTargetThisFrame = null;

            while (bounces <= maxBounces && safety++ < k_SafetyLimit)
            {
                var hit = Physics2D.Raycast(origin, dir, maxDistance, hitMask);

                if (hit.collider == null)
                {
                    points.Add(origin + dir * maxDistance);
                    break;
                }

                points.Add(hit.point);

                if (hit.collider.CompareTag("Mirror"))
                {
                    dir = Vector2.Reflect(dir, hit.normal).normalized;
                    origin = hit.point + dir * k_MinimumOffset;
                    bounces++;
                    continue;
                }

                if (hit.collider.CompareTag("Target"))
                    hitTargetThisFrame = hit.collider.GetComponent<Target2D>();

                break; // Target/Blocker에서 종료
            }

            // 엣지 트리거
            if (hitTargetThisFrame != null && hitTargetThisFrame != currentTarget)
            {
                hitTargetThisFrame.OnLaserArrived();
            }
            currentTarget = hitTargetThisFrame; // 계속 닿고 있으면 반복 호출 안 함

            // 라인 업데이트
            var count = points.Count;
            coreLR.positionCount = count;
            coreLR.SetPositions(points.ToArray());
            if (glowLR != null)
            {
                glowLR.positionCount = count;
                glowLR.SetPositions(points.ToArray());
            }
        }
    }
}
