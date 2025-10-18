using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//으흐흐

// 이 파일은 TextItem 및 GameTextData 클래스가 다른 파일(예: TextModels.cs)에 
// 올바르게 정의되어 있음을 전제로 합니다.

public class JsonData : MonoBehaviour
{
    // ⭐ 1. 싱글톤 인스턴스를 private static으로 변경
    private static JsonData _instance;
    private GameTextData gameTextData;
    private const string JSON_FILE_NAME = "Text"; // Assets/Resources/Text.json

    // ⭐ 2. public static Instance 접근자를 통해 초기화를 보장합니다.
    public static JsonData Instance
    {
        get
        {
            if (_instance == null)
            {
                // 씬에서 기존 인스턴스를 찾습니다.
                _instance = FindObjectOfType<JsonData>();

                if (_instance == null)
                {
                    // 없다면 새 GameObject를 생성하고 스크립트를 붙입니다.
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<JsonData>();
                    singletonObject.name = typeof(JsonData).Name + " (Singleton)";
                    DontDestroyOnLoad(singletonObject);
                }

                // 3. 인스턴스를 찾거나 생성한 후, 데이터 로드를 확인하고 실행합니다.
                if (_instance.gameTextData == null)
                {
                    _instance.LoadGameData();
                }
            }
            return _instance;
        }
    }

    // ⭐ 3. Awake()는 싱글톤 중복 체크 역할만 합니다.
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        // LoadGameData()는 이제 Instance 접근자 내부에서 호출됩니다.
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
        // 이제 gameTextData에 접근하기 전에 Instance 접근자를 사용해 로드가 보장됩니다.
        if (gameTextData == null || gameTextData.texts == null)
        {
            // 이 에러가 발생한다면 JSON 파일 구조/경로에 심각한 문제가 있는 것입니다.
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