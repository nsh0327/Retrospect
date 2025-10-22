using UnityEngine;

namespace BookshelfMiniGame
{
    public class BookItem : MonoBehaviour
    {
        [Header("Book Data")]
        public int Id = 1; // 1~8

        [HideInInspector] public float Width;       // ���� �� ������(����)
        [HideInInspector] public float BaseY;       // ���� Y(����)
        [HideInInspector] public float BaseZ;       // ���� Z(����)
        [HideInInspector] public Vector3 BaseScale; // ���� ������(����)

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
            Width = Mathf.Abs(transform.localScale.x); // ���� localScale.x�� Ȯ��
        }
    }
}
