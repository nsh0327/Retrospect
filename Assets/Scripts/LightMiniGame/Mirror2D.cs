using UnityEngine;

namespace LightMiniGame
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Mirror2D : MonoBehaviour
    {
        private void Awake()
        {
            if (string.IsNullOrEmpty(tag) || tag == "Untagged")
                tag = "Mirror";
        }
    }
}
