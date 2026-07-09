using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class AnimationEventsConverter : MonoBehaviour
{
    private PlayerBehaviors pb;
    [SerializeField] private Animator _anim;
    [SerializeField] private CameraShake _camShake;
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
    public void CamSmallShake()
    {
        StartCoroutine(_camShake.Shake(0.02f, 0.02f, 0.02f));
    }

    public void CamMedShake()
    {
        StartCoroutine(_camShake.Shake(0.07f, 0.07f, 0.07f));
    }
    public void CamBigShake()
    {
        StartCoroutine(_camShake.Shake(0.1f, 0.1f, 0.1f));
    }
    public void CamLUNATICShake()
    {
        StartCoroutine(_camShake.Shake(0.12f, 0.2f, 0.2f));
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
