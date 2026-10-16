using NaughtyAttributes;
using UnityEngine;

public class CorpseHold : MonoBehaviour
{
    [SerializeField] private Transform _skewerPoint;
    [SerializeField] private Rigidbody2D _rB2D;

    private void FixedUpdate()
    {
        //gameObject.transform.position = _skewerPoint.position;
        _rB2D.linearVelocity = new Vector2(0, 0);
        _rB2D.MovePosition(_skewerPoint.position);
    }
}
