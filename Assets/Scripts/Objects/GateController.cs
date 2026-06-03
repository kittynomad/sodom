using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    [SerializeField] private float _gateMoveSpeed;
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
        float elapsedTime = 0f;
        while (_gate.transform.position != _gatePositions[gatePositionIndex].transform.position)
        {
            rb.MovePosition(Vector2.Lerp(rb.position, _gatePositions[gatePositionIndex].transform.position, elapsedTime));
            //_gate.transform.position = Vector3.Lerp(_gate.transform.position, _gatePositions[gatePositionIndex].transform.position, elapsedTime);
            elapsedTime += ((Time.deltaTime * _gateMoveSpeed) / 100f);
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<PlayerBehaviors>(out PlayerBehaviors pb))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerBehaviors>(out PlayerBehaviors pb))
        {
            collision.transform.SetParent(null);
        }
    }
}
