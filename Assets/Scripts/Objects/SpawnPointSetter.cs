using UnityEngine;

public class SpawnPointSetter : MonoBehaviour
{
    private bool openCheckpointMenu = true;
    public void SetSpawnPoint()
    {
        SpawnPointHolder.instance.UpdateSpawnPoint(transform.position);
        if(openCheckpointMenu)
        {
            try
            {
                FindAnyObjectByType<CheckpointMenuHandler>().ToggleCheckpoint(true);
            }
            catch
            {
                Debug.LogWarning("Unable to open checkpoint menu, may not exist in scene??");
            }
        }
    }
}
