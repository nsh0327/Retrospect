using UnityEngine;

namespace BookshelfMiniGame
{
    public class BookItem : MonoBehaviour
    {
        [Header("Book Data")]
        public int Id = 1; // 1~8

        [HideInInspector] public float Width;       // 시작 시 스냅샷(고정)
        [HideInInspector] public float BaseY;       // 시작 Y(고정)
        [HideInInspector] public float BaseZ;       // 시작 Z(고정)
        [HideInInspector] public Vector3 BaseScale; // 시작 스케일(고정)

        void Awake()
        {
            RecacheFromTransform();
        }

        [ContextMenu("Recache From Transform")]
        public void RecacheFromTransform()
        {
            BaseScale = transform.localScale;
            BaseY = transform.position.y;
            BaseZ = transform.position.z;
            Width = Mathf.Abs(transform.localScale.x); // 폭은 localScale.x로 확정
        }
    }
}
