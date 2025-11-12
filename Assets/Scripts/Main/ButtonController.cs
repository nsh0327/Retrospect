using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ButtonController : MonoBehaviour
{
    public enum ButtonAction { None, LoadScene, ShowQuitPanel, GoToMain, GoBack, QuitPanel_Confirm, QuitPanel_Cancel};

    [Header("1. Actton Setting")]
    [Tooltip("이 버튼이 수행할 기능을 선택하세요")]
    [SerializeField] private ButtonAction actionMode = ButtonAction.LoadScene;

    [Header("2. LoadScene Setting")]
    [Tooltip("LoadScene 모드일 때 이동할 다음 씬 이름을 입력하세요")]
    [SerializeField] private string nextSceneName = "";

    private void Awake()
    {
        Button button = GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError($"[{gameObject.name}]Button 컴포넌트가 필요합니다.");
            return;
        }
        if(UIManager.Instance == null)
        {
            Debug.LogError($"[{gameObject.name}]UIManger 인스턴스를 찾을 수 없습니다.");
            return;
        }

        button.onClick.RemoveAllListeners();

        try
        {
            switch(actionMode)
            {
                case ButtonAction.LoadScene://Start button
                    if (!string.IsNullOrEmpty(nextSceneName))
                    {
                        button.onClick.AddListener(() => UIManager.Instance.LoadScene(nextSceneName));
                    }
                    else
                    {
                        Debug.LogError($"[{gameObject.name}] LoadScene 모드이나, 씬 이름이 비어있습니다.");
                    }
                    break;

                case ButtonAction.ShowQuitPanel: //Quit Button
                    button.onClick.AddListener(UIManager.Instance.ShowQuitPanel);
                    break;

                case ButtonAction.GoToMain: //Main Button
                    button.onClick.AddListener(UIManager.Instance.GoToMain);
                    break;

                case ButtonAction.GoBack: //GoBack Button
                    button.onClick.AddListener(UIManager.Instance.GoBack);
                    break;

                case ButtonAction.QuitPanel_Confirm:
                    button.onClick.AddListener(UIManager.Instance.QuitGame);
                    break;

                case ButtonAction.QuitPanel_Cancel:
                    button.onClick.AddListener(UIManager.Instance.HideQuitPanel);
                    break;

                case ButtonAction.None:
                    Debug.LogWarning($"[{gameObject.name}] 버튼 액션이 설정되지 않았습니다");
                    break;


            }

        }
        catch (Exception e)
        {
            Debug.LogError($"버튼 연결 중 예외 발생 : {e.Message}");
        }


    }

}
