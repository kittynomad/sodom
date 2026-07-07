using NaughtyAttributes;
using UnityEngine;

public class AnimationEventsConverter : MonoBehaviour
{
    private PlayerBehaviors pb;
    [SerializeField] private Animator _anim;
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

    public void UnbufferAttack()
    {
        _anim.SetBool("AttackBuffered", false);
    }

    public void MoveLockOn()
    {
        pb.MoveLockOn();
    }
    public void MoveLockOff()
    {
        pb.MoveLockOff();
    }
}
