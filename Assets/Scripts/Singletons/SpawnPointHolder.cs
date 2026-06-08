using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPointHolder : MonoBehaviour
{
    public static SpawnPointHolder instance;

    private Dictionary<string, Vector2> sceneSpawnPoints;

    public Dictionary<string, Vector2> SceneSpawnPoints { get => sceneSpawnPoints; set => sceneSpawnPoints = value; }

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

    public void UpdateSpawnPoint(Vector2 pos)
    {
        if(sceneSpawnPoints.ContainsKey(SceneManager.GetActiveScene().name))
        {
            sceneSpawnPoints.Remove(SceneManager.GetActiveScene().name);
        }

        sceneSpawnPoints.Add(SceneManager.GetActiveScene().name, pos);
    }

    public bool GetRelevantSpawnPoint(out Vector2 pos)
    {
        if(sceneSpawnPoints.ContainsKey(SceneManager.GetActiveScene().name))
        {
            pos = sceneSpawnPoints[SceneManager.GetActiveScene().name];
            return true;
        }
        else
        {
            pos = Vector2.zero;
            return false;
        }
    }
}
