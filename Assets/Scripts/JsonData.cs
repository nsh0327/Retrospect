using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JsonData : MonoBehaviour
{

    private static JsonData _instance;
    private GameTextData gameTextData;
    private const string JSON_FILE_NAME = "Text"; 

    public static JsonData Instance
    {
        get
        {
            if (_instance == null)
            {
              
                _instance = FindObjectOfType<JsonData>();

                if (_instance == null)
                {
                    
                    GameObject singletonObject = new GameObject(typeof(JsonData).Name + "(Singleton)");
                    _instance = singletonObject.AddComponent<JsonData>();
                    DontDestroyOnLoad(singletonObject);
                }

          
                if (_instance.gameTextData == null)
                {
                    _instance.LoadGameData();
                }
            }
            return _instance;
        }
    }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
     
    }

    private void LoadGameData()
    {
        TextAsset jsontextAsset = Resources.Load<TextAsset>(JSON_FILE_NAME);

        if (jsontextAsset != null)
        {
            gameTextData = JsonUtility.FromJson<GameTextData>(jsontextAsset.text);
            Debug.Log("JSON 데이터 로드 완료");
        }
        else
        {
            Debug.LogError("JSON 파일을 찾을 수 없음: Resources/Text.json 경로를 확인하세요.");
        }
    }

    public string GetTextById(string id)
    {
       
        if (gameTextData == null || gameTextData.texts == null)
        {
          
            return "ERROR : Data Not Initialized";
        }

        TextItem textItem = gameTextData.texts.FirstOrDefault(t => t.id == id);

        if (textItem != null)
        {
            return textItem.content;
        }
        else
        {
            return "ERROR : id not found";
        }
    }

  
}