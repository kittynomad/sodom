using UnityEngine;

public class PlayerLoadout : MonoBehaviour
{
    [SerializeField] private bool _doubleJumpUnlocked = false;
    [SerializeField] private bool _poundUnlocked = true;
    [SerializeField] private bool _dashUnlocked = true;

    public bool DoubleJumpUnlocked { get => _doubleJumpUnlocked; set => _doubleJumpUnlocked = value; }
    public bool PoundUnlocked { get => _poundUnlocked; set => _poundUnlocked = value; }
    public bool DashUnlocked { get => _dashUnlocked; set => _dashUnlocked = value; }
}
