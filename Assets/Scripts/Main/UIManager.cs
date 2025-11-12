using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static UIManager Instance;


    public static string PreviousSceneName = "Main";

    public void Awake()
    {


        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadScene(string nextSceneName)
    {
        if(string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError("로드할 씬이 없습니다.");
            return;
        }
        PreviousSceneName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(nextSceneName);
    }

    public void ShowQuitPanel()
    {
        Transform[] allTransforms = Resources.FindObjectsOfTypeAll<Transform>();

        Transform quitPanelTransform = allTransforms.FirstOrDefault(t => t.name == "QuitPanel" && t.hideFlags == HideFlags.None);

        if(quitPanelTransform != null)
        {
            quitPanelTransform.gameObject.SetActive(true);
            Debug.Log("종료 패널 활성화");
        }
        else
        {
            Debug.LogError("패널을 찾을 수 없습니다.");
        }
            
    }

    public void HideQuitPanel()
    {
        GameObject quitPanel = GameObject.Find("QuitPanel");
        if(quitPanel != null)
        {
            quitPanel.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }

    public void GoToMain()
    {
        LoadScene(PreviousSceneName);
    }

    public void GoBack()
    {

        if(SceneManager.GetActiveScene().name == PreviousSceneName)
        {
            ShowQuitPanel();
        }
        else
        {
            LoadScene(PreviousSceneName);
        }
    }
   
}
