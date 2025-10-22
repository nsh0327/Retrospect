using UnityEngine;
using UnityEngine.EventSystems;

namespace BookshelfMiniGame
{
    public class CloseOnClick : MonoBehaviour, IPointerClickHandler
    {
        public BookMiniGameUI ui; // 연결할 UI 매니저

        public void OnPointerClick(PointerEventData eventData)
        {
            if (ui != null)
                ui.HideResult(); // 클릭 시 UI 닫기
        }
    }
}
