using UnityEngine;

public class PlayerParentingObject : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerBehaviors>(out PlayerBehaviors pb))
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
