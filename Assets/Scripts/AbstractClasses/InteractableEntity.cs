using UnityEngine;

public abstract class InteractableEntity : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        TrySubscribeInteraction(collision);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        TryUnsubscribeInteraction(collision);
    }

    /*private void OnDestroy()
    {
        FindAnyObjectByType<PlayerBehaviors>().interactAction -= OnInteract;
    }*/

    public virtual void TrySubscribeInteraction(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerBehaviors pb))
        {
            pb.interactAction += OnInteract;
        }
    }

    public virtual void TryUnsubscribeInteraction(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerBehaviors pb))
        {
            pb.interactAction -= OnInteract;
        }
    }

    public virtual void OnInteract()
    {
        Debug.Log("object " + gameObject.name + " interacted smile");
    }
}
