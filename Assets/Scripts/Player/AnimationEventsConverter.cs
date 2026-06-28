using UnityEngine;

public class AnimationEventsConverter : MonoBehaviour
{
    private PlayerBehaviors pb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pb = GetComponentInParent<PlayerBehaviors>();
    }

    public void StartAttack()
    {
        pb.StartAttack();
    }

    public void EndAttack()
    {
        pb.EndAttack();
    }
}
