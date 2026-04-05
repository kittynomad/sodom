using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 movementDirection = Vector2.zero;

    public Vector2 MovementDirection { get => movementDirection; set => movementDirection = value; }

    public void OnMove(InputValue iVal)
    {
        movementDirection = iVal.Get<Vector2>();
    }
}
