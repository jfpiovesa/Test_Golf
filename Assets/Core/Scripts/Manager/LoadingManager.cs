using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    [SerializeField] private Slider progressBar;
    Coroutine loading;

    private void Awake()
    {
        DesactiveCanavas();
    }
    public void LoadSceneAction(string value) {

        loading = StartCoroutine(LoadSceneAsync(value));

    }
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        ActiveCansvas();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f); 
            if (progressBar != null)
                progressBar.value = progress; 

       
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
               
            }

            yield return null;
        }
        DesactiveCanavas();
    }

    void ActiveCansvas()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
    }
    void DesactiveCanavas()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
    }
}
