using UnityEngine;

public class HoleCollision : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {

         GameObject obj = other.gameObject;
        if (obj != null && obj.CompareTag("Ball"))
        {
            Invoke("EndGame", 0.3f);
        }  
    }

    void EndGame()
    {
        St_ControlGamplay.EndSeesinAction();
    }
}
