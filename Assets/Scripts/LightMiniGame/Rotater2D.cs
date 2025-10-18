using UnityEngine;

namespace LightMiniGame
{
    public class Rotator2D : MonoBehaviour
    {
        [Header("Rotation Settings")]
        [SerializeField] private float stepAngle = 90f;

        [Header("Options")]
        [SerializeField] private bool isClockwise = true;
        [SerializeField] private bool isInteractive = true;
        [SerializeField] private LayerMask clickableMask = ~0; 

        private void OnMouseDown()
        {
            if (!isInteractive) return;

            Vector3 m3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 m2 = new(m3.x, m3.y);

            Collider2D hit = Physics2D.OverlapPoint(m2, clickableMask);
            if (hit != null && hit.gameObject == gameObject)
            {
                float angle = stepAngle * (isClockwise ? -1f : 1f);
                transform.Rotate(0f, 0f, angle);
            }
        }

        public bool IsInteractive() => isInteractive;
        public void SetInteractive(bool value) => isInteractive = value;
        public void ResetRotation() => transform.rotation = Quaternion.identity;
    }
}
