using UnityEngine;
using System.Collections;
using NaughtyAttributes;

public class GroundSaver : MonoBehaviour
{
    [SerializeField] private float _safeCheckDuration;
    [SerializeField] private LayerMask _safeLayers;

    private Vector2 _lastSafePosition;
    private Collider2D coll;

    public Vector2 LastSafePosition { get => _lastSafePosition; set => _lastSafePosition = value; }

    private void Start()
    {
        coll = gameObject.GetComponent<Collider2D>();
        StartCoroutine(SafeGroundUpdateCoroutine());
    }

    public IEnumerator SafeGroundUpdateCoroutine()
    {
        while(true)
        {
            if(IsSafelyGrounded())
            {
                _lastSafePosition = transform.position;
            }
            yield return new WaitForSeconds(_safeCheckDuration);
        }
    }

    //This function is currently identical to IsGrounded, but I've made a separate function in case special cases come up (i.e. moving platforms).
    public bool IsSafelyGrounded()
    {
        //check if grounded (duh)
        bool hg = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, _safeLayers);
        return hg;
    }

    [Button]
    public void SafeReturn()
    {
        transform.position = _lastSafePosition;
    }
}
