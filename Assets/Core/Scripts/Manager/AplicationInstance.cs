using UnityEngine;

public class AplicationInstance : MonoBehaviour
{

    public static AplicationInstance instance;
    public LoadingManager loadingManager;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
