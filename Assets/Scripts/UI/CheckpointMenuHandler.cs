using UnityEngine;
using UnityEngine.InputSystem;

public class CheckpointMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject _checkpointUIContent;

    public void ToggleCheckpoint(bool activated = true)
    {
        _checkpointUIContent.SetActive(activated);
        FindAnyObjectByType<PlayerInput>().enabled = !activated;
        FindAnyObjectByType<PlayerController>().ResetInput();

    }

    public void OnRest()
    {
        FindAnyObjectByType<PlayerBehaviors>().CurrentHealth = FindAnyObjectByType<PlayerBehaviors>().MaxHealth;
    }
}
