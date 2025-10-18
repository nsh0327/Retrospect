using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LightMiniGame
{
    public class ClearUIController : MonoBehaviour
    {
        [SerializeField] private LightMiniGameManager gameManager;
        [SerializeField] private CanvasGroup panel;
        [SerializeField] private float fadeDuration = 0.25f;

        private Coroutine fadeRoutine;

        private void Awake()
        {
            if (panel != null)
            {
                panel.alpha = 0f;
                panel.interactable = false;
                panel.blocksRaycasts = false;
            }
        }

        private void OnEnable()
        {
            if (gameManager == null)
                gameManager = FindObjectOfType<LightMiniGameManager>(true);

            if (gameManager != null)
                gameManager.PuzzleCleared += ShowPanel;
        }

        private void OnDisable()
        {
            if (gameManager != null)
                gameManager.PuzzleCleared -= ShowPanel;
        }

        private void ShowPanel() => Show(true);

        public void OnClickContinue()
        {
            Show(false);

            SceneManager.LoadScene("SampleScene");
        }

        private void Show(bool visible)
        {
            if (panel == null) return;
            if (fadeRoutine != null) StopCoroutine(fadeRoutine);
            fadeRoutine = StartCoroutine(FadeTo(visible ? 1f : 0f));
        }

        private IEnumerator FadeTo(float targetAlpha)
        {
            float start = panel.alpha;
            float t = 0f;

            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / Mathf.Max(0.0001f, fadeDuration);
                panel.alpha = Mathf.Lerp(start, targetAlpha, t);
                yield return null;
            }

            panel.alpha = targetAlpha;
            bool isVisible = targetAlpha > 0.99f;
            panel.interactable = isVisible;
            panel.blocksRaycasts = isVisible;
        }
    }
}
