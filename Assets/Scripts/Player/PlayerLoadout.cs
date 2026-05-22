using UnityEngine;

public class PlayerLoadout : MonoBehaviour
{
    [SerializeField] private bool doubleJumpUnlocked = true;

    public bool DoubleJumpUnlocked { get => doubleJumpUnlocked; set => doubleJumpUnlocked = value; }
}
