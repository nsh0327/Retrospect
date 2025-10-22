using UnityEngine;
using UnityEngine.EventSystems;

namespace BookshelfMiniGame
{
    public class CloseOnClick : MonoBehaviour, IPointerClickHandler
    {
        public BookMiniGameUI ui; // ������ UI �Ŵ���

        public void OnPointerClick(PointerEventData eventData)
        {
            if (ui != null)
                ui.HideResult(); // Ŭ�� �� UI �ݱ�
        }
    }
}
