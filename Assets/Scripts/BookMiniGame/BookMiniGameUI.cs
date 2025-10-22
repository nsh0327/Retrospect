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
        public Text ResultText; // TMP ��� �⺻ Text ���

        [Header("Scene Names")]
        public string BackSceneName = "SampleScene";     // ���ư��� ��ư
        public string NextSceneName = "SampleScene"; // ��� ���� �� �̵��� ��

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
                ResultText.text = "��� å�� ���ڸ��� �ֽ��ϴ�!";
                isAllCorrect = true;
            }
            else
            {
                ResultText.text = $"{correct}���� å�� �ùٸ� ������ �ֽ��ϴ�.";
                isAllCorrect = false;
            }

            ResultOverlay.SetActive(true);
        }

        public void HideResult()
        {
            if (!ResultOverlay)
                return;

            ResultOverlay.SetActive(false);

            // ��� å�� ������ ���� ������ �̵�
            if (isAllCorrect && !string.IsNullOrEmpty(NextSceneName))
            {
                SceneManager.LoadScene(NextSceneName);
            }
        }
    }
}
