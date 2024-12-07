using TMPro;
using UnityEngine;

public class UI_GamePlayCanvas : MonoBehaviour
{
    public  TMP_Text text_shots;
    private void OnEnable()
    {
        St_ControlGamplay.text_shots = text_shots;
    }
    private void OnDisable()
    {
        St_ControlGamplay.text_shots = null;

    }
}
