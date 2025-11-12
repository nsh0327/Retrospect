using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [Header("1. Dialogue Setting")]
    [Tooltip("클릭 시 TextManager로 보낼 텍스트 데이터의 ID를 입력하세요.")]
    [SerializeField] private string dialogueTextId;

    [Header("2. Scene Transition Setting")]
    [Tooltip("대화 없이 클릭 즉시 이동할 다음 씬의 이름을 입력하세요")]
    [SerializeField] private string targetSceneName;

    

    private void OnMouseDown()
    {
        Debug.Log($"아이템 클릭 감지 ID: {dialogueTextId}");

        if (UIManager.Instance == null || TextManager.Instance == null)
        {
            Debug.LogError($"ItemController 초기화되지 않았습니다.");
            return;
        }


        if (!string.IsNullOrEmpty(dialogueTextId))
        {

            TextManager.Instance.DisplayPrompt(dialogueTextId);
        }


        else if (!string.IsNullOrEmpty(targetSceneName))
        {
            
            UIManager.Instance.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogWarning($"[{gameObject.name}]에 설정된 액션이 없습니다.");
        }
    }
}
