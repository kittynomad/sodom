using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class AnimationEventsConverter : MonoBehaviour
{
    private PlayerBehaviors pb;
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private CameraShake _camShake;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pb = GetComponentInParent<PlayerBehaviors>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
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
        anim.SetBool("AttackBuffered", false);
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
            pb.MoveLocked = false;
        else if (i == 1)
            pb.MoveLocked = true;
    }
    public void SetMeleeChaining(float f)
    {
        if (f == 0)
            pb.MeleeChaining = false;
        else if (f == 1)
            pb.MeleeChaining = true;
    }

    public void SetMeleeTurnWindow(float f)
    {
        if (f == 0)
            pb.MeleeTurnWindow = false;
        else if (f == 1)
            pb.MeleeTurnWindow = true;
    }

    public void AnimApplyForce(float f)
    {
        Vector2 v = new Vector2(0, 0);
        if (pb.transform.localScale.x == -1)
            v = Vector2.left;
        else
            v = Vector2.right;
        pb.AnimApplyForce(f, v);
    }
}
