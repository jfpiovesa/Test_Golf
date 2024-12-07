using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_EndCanvas : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public SceneAsset sceneMenu;
    public SceneAsset sceneThis;
    public TMP_Text text_shots;
    private void Awake()
    {
        Desactive();
        St_ControlGamplay.endSession = EndSession;
    }

    public void EndSession()
    {
        Active();
        St_ControlGamplay.player.canMove = false;
    }

    void Active()
    {
        text_shots.text = St_ControlGamplay.scorre.ToString();
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
    }
    void Desactive()
    {
        text_shots.text = string.Empty;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
    }
    public void LoadingMenu()
    {
        
        AplicationInstance.instance.loadingManager.LoadSceneAction(sceneMenu.name);
    }
    public void Resent()
    {
        AplicationInstance.instance.loadingManager.LoadSceneAction(sceneThis.name);
    }

    void ClearShorts()
    {
        St_ControlGamplay.CleraShorts();
    }
}
