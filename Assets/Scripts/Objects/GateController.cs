/*****************************************************************************
// File Name : GateController.cs
// Author : Pierce
// Creation Date : 6/3/2026
// Last Modified : 6/21/2026
//
// Brief Description : Logic for a moving object, manually triggered to move
// between points.
*****************************************************************************/
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

    private Rigidbody2D rb;
    private int gatePositionIndex = 0;

    void Start()
    {
        rb = _gate.GetComponent<Rigidbody2D>();
        _lineRenderer.gameObject.SetActive(true);
        rb.MovePosition(_gatePositions[gatePositionIndex].transform.position);
        _gate.transform.position = _gatePositions[gatePositionIndex].transform.position;
        if (_lineRenderer != null)
        {
            for (int i = 0; i < _gatePositions.Length; i++)
            {
                //Debug.Log("draw");
                //_lineRenderer.SetPosition(i, _gatePositions[i].transform.position);
            }
        }
    }

    public void MoveToNextPosition()
    {
        StopAllCoroutines();
        //_gate.transform.position = _gatePositions[gatePositionIndex].transform.position;
        gatePositionIndex = (gatePositionIndex + 1) >= _gatePositions.Length ? 0 : gatePositionIndex + 1;
        StartCoroutine(MoveGate());
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

    
}
