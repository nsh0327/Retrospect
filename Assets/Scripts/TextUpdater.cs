using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextUpdater : MonoBehaviour
{
 

        public Text clueTextUI;

 
        private const string CLUE_TEXT_ID = "1"; 

        public void DisplayPrompt()
        {
            if (clueTextUI == null || JsonData.Instance == null)
            {
                Debug.LogError("텍스트 UI 또는 JsonData가 연결/준비되지 않았습니다.");
                return;
            }

     
            string prompt = JsonData.Instance.GetTextById(CLUE_TEXT_ID);

            clueTextUI.text = prompt;
            clueTextUI.gameObject.SetActive(true);
            Debug.Log($"자막 표시 성공: {prompt}");
        }
   

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
