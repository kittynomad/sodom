using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 movementDirection = Vector2.zero;
    private Vector2 aimDirection = Vector2.zero;
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

    public void OnAim(InputValue iVal)
    {
        Vector2 aimScreenPoint = iVal.Get<Vector2>();
        Vector2 aimWorldPoint = Camera.main.ScreenToWorldPoint(aimScreenPoint);
        aimDirection = (aimWorldPoint - (Vector2)transform.position).normalized;
        Debug.Log(Camera.main.ScreenToWorldPoint(aimScreenPoint));
    }

    public void OnFire()
    {
        pb.FireBehavior(aimDirection);
    }

    public void OnJump()
    {
        pb.JumpBehavior();
    }

    public void OnAttack()
    {
        pb.AttackBehavior();
    }

    public void OnInteract()
    {
        Debug.Log("interact key");
        pb.InteractBehavior();
    }
}
