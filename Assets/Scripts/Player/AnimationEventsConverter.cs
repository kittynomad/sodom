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

    public void AttackCycle(int i)
    {
        if (i == 0)
            pb.AttackCycle(false);
        else if (i == 1)
            pb.AttackCycle(true);
    }

    public void UnbufferAttack()
    {
        _anim.SetBool("AttackBuffered", false);
    }

    public void CamShake(string s)
    {
        if (s.Equals("Small"))
            StartCoroutine(_camShake.Shake(0.02f, 0.02f, 0.02f));
        else if (s.Equals("Medium"))
            StartCoroutine(_camShake.Shake(0.07f, 0.07f, 0.07f));
        else if (s.Equals("Big"))
            StartCoroutine(_camShake.Shake(0.1f, 0.1f, 0.1f));
        else if (s.Equals("LUNATIC"))
            StartCoroutine(_camShake.Shake(0.12f, 0.2f, 0.2f));
    }

    public void SetMoveLock(int i)
    {
        if (i == 0)
            pb.SetMoveLock(false);
        else if (i == 1)
            pb.SetMoveLock(true);
    }
}
