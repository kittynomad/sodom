using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPointHolder : MonoBehaviour
{
    public static SpawnPointHolder instance;
    public void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
