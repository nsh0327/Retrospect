using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject First;
    public GameObject Second;

    public Text initialPromptText;

    public TextUpdater textUpdater;

    // JSON에서 첫 텍스트를 가져올 ID
    private const string INITIAL_TEXT_ID = "1";

    private bool textVisible = true;

    void Awake()
    {
   
        // 2. Second 화면은 시작 시 비활성화 상태인지 확인
        if (Second != null)
        {
            Second.SetActive(false);
        }
    }
    void Start()
    {

        LoadInitialText();

        textVisible = true;
    }

    void Update()
    {
        if(textVisible && Input.GetMouseButtonDown(0))
        {
            HideInitialText();
        }
    }
    private void HideInitialText()
    {
        if(initialPromptText != null)
        {
            initialPromptText.gameObject.SetActive(false);
            textVisible = false;
            Debug.Log("초기 텍스트 제거 완료");
        }
    }

    private void LoadInitialText()
    {
        // JsonData 인스턴스가 로드되었는지 확인
        if (JsonData.Instance == null)
        {
            Debug.LogError("JsonData가 초기화되지 않았습니다.");
            return;
        }

        if (initialPromptText != null)
        {
            // JsonData를 통해 텍스트 로드
            string textToShow = JsonData.Instance.GetTextById(INITIAL_TEXT_ID);

            initialPromptText.text = textToShow;
            initialPromptText.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Initial Prompt Text가 연결되지 않았습니다. Inspector를 확인하세요.");
        }
    }


    public void LoadScene()
    {
        if (First != null)
        {
            First.SetActive(false);
        }
        else
        {
            Debug.LogError("First 오브젝트가 연결되지 않았습니다.");

        }

        if (Second != null)
        {
            Second.SetActive(true);
            Debug.Log("화면 전환");
        }
        else
        {
            Debug.LogError("Second 오브젝트가 연결되지 않았습니다.");
        }



    }
    public void LoadSceneBack()
    {
        if (Second != null)
        {
            Second.SetActive(false);
        }
        else
        {
            Debug.LogError("Second 오브젝트가 연결되지 않았습니다.");

        }

        if (First != null)
        {
            First.SetActive(true);
            Debug.Log("화면 전환");
        }
        else
        {
            Debug.LogError("Second 오브젝트가 연결되지 않았습니다.");
        }
    }
}
