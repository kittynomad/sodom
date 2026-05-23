using UnityEngine;
using UnityEngine.InputSystem;

// TEMPORARY SCRIPT, BADLY CODED BY CAM TO TEST ANIMATIONS
// DELETE SCRIPT AFTER ANIMATIONS ARE CODED IN GRACEFULLY
public class TEMPORARYPlayerAnims : MonoBehaviour
{
    public Rigidbody2D _playerRB2D;
    public bool _isGrounded;
    public bool _isMoving;
    public float _ySpeed;
    public PlayerBehaviors _pB;
    public Animator _anim;

    private void FixedUpdate()
    {
        _isGrounded = _pB.IsGrounded();
        _anim.SetBool("IsGrounded", _isGrounded);
        if (_playerRB2D.linearVelocityX != 0f)
        {
            _isMoving = true;
        }
        else
        {
            _isMoving = false;
        }
        _anim.SetBool("IsMoving", _isMoving);
        _ySpeed = _playerRB2D.linearVelocityY;
        _anim.SetFloat("YSpeed", _ySpeed);
    }

}
