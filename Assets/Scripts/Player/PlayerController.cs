using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 movementDirection = Vector2.zero;
    private PlayerBehaviors pb;

    public Vector2 MovementDirection { get => movementDirection; set => movementDirection = value; }


    private void Start()
    {
        pb = gameObject.GetComponent<PlayerBehaviors>();
    }

    public void OnMove(InputValue iVal)
    {
        movementDirection = iVal.Get<Vector2>();
    }

    public void OnJump()
    {
        pb.JumpBehavior();
    }
}
