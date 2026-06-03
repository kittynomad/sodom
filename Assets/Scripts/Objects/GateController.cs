using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    [SerializeField] private float _gateMoveSpeed;
    [SerializeField] private GameObject _gate;
    [SerializeField] private GameObject[] _gatePositions;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private AudioSource movingGate;

    private int gatePositionIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer.gameObject.SetActive(true);
        _gate.transform.position = _gatePositions[gatePositionIndex].transform.position;
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
        movingGate.Play();
    }

    public IEnumerator MoveGate()
    {
        float elapsedTime = 0f;
        while (_gate.transform.position != _gatePositions[gatePositionIndex].transform.position)
        {
            _gate.transform.position = Vector3.Lerp(_gate.transform.position, _gatePositions[gatePositionIndex].transform.position, elapsedTime);
            elapsedTime += ((Time.deltaTime * _gateMoveSpeed) / 100f);
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
