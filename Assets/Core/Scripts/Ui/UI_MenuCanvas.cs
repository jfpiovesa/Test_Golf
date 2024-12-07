using UnityEditor;
using UnityEngine;

public class UI_MenuCanvas : MonoBehaviour
{

    public SceneAsset gamepalyeScene;
    public void LoadGamePlay()
    {
        AplicationInstance.instance.loadingManager.LoadSceneAction(gamepalyeScene.name);
    }
}
