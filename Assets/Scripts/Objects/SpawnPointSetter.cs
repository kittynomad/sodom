using UnityEngine;

public class SpawnPointSetter : MonoBehaviour
{
    public void SetSpawnPoint()
    {
        SpawnPointHolder.instance.UpdateSpawnPoint(transform.position);
    }
}
