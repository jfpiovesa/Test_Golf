using UnityEngine;
using System;
using TMPro;
public static class St_ControlGamplay
{
    public static BallControl player { get; private set; }
    public static Vector3 startPosition { get; private set; }
    public static int scorre { get; private set; }
    public static Action endSession { get; set; }
    public static TMP_Text text_shots { get; set; }

    public static void AddShorts(int value)
    {
        scorre += value;
        text_shots.text  = scorre.ToString();
    }
    public static void CleraShorts()
    {
        scorre = 0;
        text_shots.text = scorre.ToString();
    }
    public static void EndSeesinAction()
    {
        endSession?.Invoke();
    }
    public static void SetPlayer(BallControl value)
    {
        player = value;
    }
    public static void SetStartPosition(Vector3 value)
    {
        startPosition = value;
    }
    public static void Teleport()
    {
        player.transform.position = startPosition;
        player.canMove = true;
    }
}
