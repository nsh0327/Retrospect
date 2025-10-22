using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace BookshelfMiniGame
{
    public class BookMiniGameUI : MonoBehaviour
    {
        [Header("Refs")]
        public ShelfGameController Controller;

        [Header("Buttons")]
        public Button BackButton;
        public Button RetryButton;
        public Button CheckButton;

        [Header("Result Popup")]
        public GameObject ResultOverlay;
        public Text ResultText; // TMP 대신 기본 Text 사용

        [Header("Scene Names")]
        public string BackSceneName = "SampleScene";     // 돌아가기 버튼
        public string NextSceneName = "SampleScene"; // 모든 정답 시 이동할 씬

        private bool isAllCorrect = false;

        void Awake()
        {
            if (BackButton) BackButton.onClick.AddListener(GoBack);
            if (RetryButton) RetryButton.onClick.AddListener(ApplyInitialOrder);
            if (CheckButton) CheckButton.onClick.AddListener(ShowResult);

            if (ResultOverlay)
                ResultOverlay.SetActive(false);
        }

        void GoBack()
        {
            if (!string.IsNullOrEmpty(BackSceneName))
                SceneManager.LoadScene(BackSceneName);
        }

        void ApplyInitialOrder()
        {
            if (Controller)
                Controller.ApplyInitialOrder();
        }

        void ShowResult()
        {
            if (!Controller || !ResultOverlay || !ResultText)
                return;

            int correct = Controller.GetCorrectCount();
            int total = Controller.Books.Count;

            if (correct == total)
            {
                ResultText.text = "모든 책이 제자리에 있습니다!";
                isAllCorrect = true;
            }
            else
            {
                ResultText.text = $"{correct}개의 책이 올바른 순서에 있습니다.";
                isAllCorrect = false;
            }

            ResultOverlay.SetActive(true);
        }

        public void HideResult()
        {
            if (!ResultOverlay)
                return;

            ResultOverlay.SetActive(false);

            // 모든 책이 맞으면 다음 씬으로 이동
            if (isAllCorrect && !string.IsNullOrEmpty(NextSceneName))
            {
                SceneManager.LoadScene(NextSceneName);
            }
        }
    }
}
