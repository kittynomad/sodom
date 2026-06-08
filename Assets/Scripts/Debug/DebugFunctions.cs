using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using UnityEngine.SceneManagement;

public class DebugFunctions : MonoBehaviour
{
    private PlayerLoadout pl;
    private void Start()
    {
        pl = FindAnyObjectByType<PlayerLoadout>();
    }
    
    public void ToggleDoubleJump()
    {
        pl.DoubleJumpUnlocked = !pl.DoubleJumpUnlocked;
    }

    public void TogglePound()
    {
        pl.PoundUnlocked = !pl.PoundUnlocked;
    }

    public void ToggleDash()
    {
        pl.DashUnlocked = !pl.DashUnlocked;
    }

    [Button]
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
