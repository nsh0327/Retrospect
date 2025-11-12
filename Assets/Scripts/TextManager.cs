using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TextManager : MonoBehaviour
{

    public static TextManager Instance;

    public GameObject dialoguePanel;

    public TMP_Text clueTextUI;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void DisplayPrompt(string textId)
        {
            if (clueTextUI == null || dialoguePanel == null || JsonData.Instance == null)
            {
                Debug.LogError("텍스트 UI, Panel, JsonData가 연결/준비되지 않았습니다.");
                return;
            }

     
            string prompt = JsonData.Instance.GetTextById(textId);

        if (prompt.StartsWith("ERROR"))
        {
            clueTextUI.text = "Error";
            Debug.LogError($"JSON  데이터 오류: ID '{textId}'를 찾을 수 없습니다.");

        }
        else
        {
            clueTextUI.text = prompt;
            Debug.Log($"자막 표시 성공: {prompt}");
        }

            
            clueTextUI.gameObject.SetActive(true);
            
        }


   

  
}
