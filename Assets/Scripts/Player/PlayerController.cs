/*****************************************************************************
// File Name : PlayerController.cs
// Author : Pierce
// Creation Date : -
// Last Modified : 6/21/2026
//
// Brief Description : Reads and reacts to player input.
*****************************************************************************/
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
    }

    public void OnFire()
    {
        pb.FireBehavior(aimDirection);
    }

    public void OnJump(InputValue iVal)
    {
        //Debug.Log(iVal.Get<float>());
        if (iVal.Get<float>() == 1)
            pb.JumpBehavior();
        else if (iVal.Get<float>() == 0)
            pb.EndJumpBehavior();
    }

    public void OnAttack()
    {
        pb.AttackBehavior();
    }

    public void OnPound()
    {
        pb.PoundBehavior();
    }

    public void OnInteract()
    {
        Debug.Log("interact key");
        pb.InteractBehavior();
    }
}
