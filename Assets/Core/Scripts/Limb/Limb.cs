using UnityEngine;

public class Limb : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other != null & other.GetComponent<BallControl>() != null)
        {
            St_ControlGamplay.player.canMove = false;
            St_ControlGamplay.Teleport();
        }
    }
}
