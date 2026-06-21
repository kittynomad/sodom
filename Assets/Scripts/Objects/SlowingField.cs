using UnityEngine;

public class SlowingField : MonoBehaviour
{
    [SerializeField] [Range(0, 100f)] private float _movementPercent = 50f;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out PlayerBehaviors pb))
        {
            pb.MoveModifier = _movementPercent / 100f;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerBehaviors pb))
        {
            pb.MoveModifier = 1f;
        }
    }
}
