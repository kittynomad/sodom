using UnityEngine;

public class CorpseHold : MonoBehaviour
{
    [SerializeField] private Transform _skewerPoint;

    private void FixedUpdate()
    {
        gameObject.transform.position = _skewerPoint.position;
    }
}
