using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    [SerializeField] private bool _moveLinear;
    [SerializeField] private float _gateMoveSpeed;
    [SerializeField] private float _gateMinSpeed;
    [SerializeField] private GameObject _gate;
    [SerializeField] private GameObject[] _gatePositions;
    [SerializeField] private LineRenderer _lineRenderer;
    //[SerializeField] private AudioSource movingGate;

    private Rigidbody2D rb;
    private int gatePositionIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = _gate.GetComponent<Rigidbody2D>();
        _lineRenderer.gameObject.SetActive(true);
        rb.MovePosition(_gatePositions[gatePositionIndex].transform.position);
        //_gate.transform.position = _gatePositions[gatePositionIndex].transform.position;
        if (_lineRenderer != null)
        {
            for (int i = 0; i < _gatePositions.Length; i++)
            {
                Debug.Log("draw");
                _lineRenderer.SetPosition(i, _gatePositions[i].transform.position);
            }
        }
    }

    public void MoveToNextPosition()
    {
        StopAllCoroutines();
        //_gate.transform.position = _gatePositions[gatePositionIndex].transform.position;
        gatePositionIndex = (gatePositionIndex + 1) >= _gatePositions.Length ? 0 : gatePositionIndex + 1;
        StartCoroutine(MoveGate());
        //movingGate.Play();
    }

    public IEnumerator MoveGate()
    {
        if(_moveLinear)
        {
            while(Vector2.Distance(_gate.transform.position, _gatePositions[gatePositionIndex].transform.position) > 0.1f)
            {
                Vector2 direction = Vector2.Normalize(_gatePositions[gatePositionIndex].transform.position - _gate.transform.position);
                float distance = Vector2.Distance(_gate.transform.position, _gatePositions[gatePositionIndex].transform.position);
                rb.linearVelocity = direction * _gateMoveSpeed;
                yield return new WaitForFixedUpdate();
            }
            rb.linearVelocity = Vector2.zero;
            rb.MovePosition(_gatePositions[gatePositionIndex].transform.position);
        }
        else
        {
            float startDistance = Vector2.Distance(_gate.transform.position, _gatePositions[gatePositionIndex].transform.position);
            while (Vector2.Distance(_gate.transform.position, _gatePositions[gatePositionIndex].transform.position) > 0.1f)
            {
                Vector2 direction = Vector2.Normalize(_gatePositions[gatePositionIndex].transform.position - _gate.transform.position);
                float distance = Vector2.Distance(_gate.transform.position, _gatePositions[gatePositionIndex].transform.position);
                //thank you desmos
                float speed = -Mathf.Pow((2 * (1 - (distance / startDistance)) - 1), 4) + 1;
                /*float speed;
                if (distance / startDistance < 0.1f) speed = 10 * (distance / startDistance);
                else if (distance / startDistance < 0.9f) speed = 1f;
                else speed = (-10 * (distance / startDistance)) + 10;*/
                rb.linearVelocity = Vector2.Lerp(direction * _gateMinSpeed, direction * _gateMoveSpeed, speed);
                yield return new WaitForFixedUpdate();
            }
            while (rb.linearVelocity.magnitude > 0f)
            {
                rb.linearVelocity = rb.linearVelocity / 2;
                yield return new WaitForFixedUpdate();
            }
        }
        
        
    }

    
}
