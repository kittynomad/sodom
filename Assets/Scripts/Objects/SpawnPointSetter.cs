using UnityEngine;

public class SpawnPointSetter : MonoBehaviour
{
    private bool openCheckpointMenu = true;
    public void SetSpawnPoint()
    {
        SpawnPointHolder.instance.UpdateSpawnPoint(transform.position);
        if(openCheckpointMenu)
        {
            FindAnyObjectByType<CheckpointMenuHandler>().ToggleCheckpoint(true);
        }
    }
}
