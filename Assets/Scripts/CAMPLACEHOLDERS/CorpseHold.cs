using NaughtyAttributes;
using UnityEngine;

public class CorpseHold : MonoBehaviour
{
    [SerializeField] private GameObject[] _corpseParts;
    [SerializeField] private PlayerBehaviors _pB;
    [SerializeField] private Transform _skewerPoint;
    [SerializeField] private Rigidbody2D _rB2D;

    private void FixedUpdate()
    {
        _rB2D.linearVelocity = new Vector2(0, 0);
        if (_pB.IsFacingRight() && transform.localScale.x == -1)
        {
            _rB2D.simulated = false;
            for (int i = 0; i < _corpseParts.Length; i++)
            {
                _corpseParts[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; 
                _corpseParts[i].GetComponent<HingeJoint2D>().enabled = false;
            }
            transform.localScale = new Vector2(1, 1);
            _rB2D.simulated = true;
            for (int i = 0; i < _corpseParts.Length; i++)
            {
                _corpseParts[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                _corpseParts[i].GetComponent<HingeJoint2D>().enabled = true;
            }
        }
        else if (!_pB.IsFacingRight() && transform.localScale.x == 1)
        {
            _rB2D.simulated = false;
            for (int i = 0; i < _corpseParts.Length; i++)
            {
                _corpseParts[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                _corpseParts[i].GetComponent<HingeJoint2D>().enabled = false;
            }
            transform.localScale = new Vector2(-1, 1);
            _rB2D.simulated = true;
            for (int i = 0; i < _corpseParts.Length; i++)
            {
                _corpseParts[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                _corpseParts[i].GetComponent<HingeJoint2D>().enabled = true;
            }
        }
        //gameObject.transform.position = _skewerPoint.position;
        //_rB2D.rotation = _skewerPoint.rotation.z;
        _rB2D.MovePosition(_skewerPoint.position);
    }
}
