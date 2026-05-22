using UnityEngine;

public class PlayerLoadout : MonoBehaviour
{
    [SerializeField] private bool doubleJumpUnlocked = false;

    public bool DoubleJumpUnlocked { get => doubleJumpUnlocked; set => doubleJumpUnlocked = value; }
}
