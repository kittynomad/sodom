/*****************************************************************************
// File Name : SwordController.cs
// Author : Pierce
// Creation Date : -
// Last Modified : 6/21/2026
//
// Brief Description : Handles functions related to the player's sword.
*****************************************************************************/
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private PlayerBehaviors pb;
    private GameObject attachedObject;
    //speed which corpses are jettisoned when detached
    [SerializeField] private float detachSpeed = 5f;

    public bool HasCorpseAttached { get => attachedObject != null; }
    public GameObject AttachedObject { get => attachedObject; set => attachedObject = value; }
    private void Start()
    {
        pb = FindAnyObjectByType<PlayerBehaviors>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //deal damage while attacking
        if(pb.IsAttacking && collision.gameObject.TryGetComponent(out IKillable ik))
        {
            ik.OnDamage(1f, gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //deal damage while attacking
        if (pb.IsAttacking && collision.gameObject.TryGetComponent(out IKillable ik))
        {
            ik.OnDamage(1f, gameObject);
        }
    }

    public bool TryAttachCorpse(GameObject corpse)
    {
        //attach corpse if nothing is already attached
        if (corpse.TryGetComponent(out CorpseController cc) && attachedObject == null)
        {
            corpse.transform.parent = gameObject.transform;
            corpse.transform.localPosition = Vector2.zero;
            corpse.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            corpse.GetComponent<Rigidbody2D>().excludeLayers = LayerMask.GetMask("Player");
            attachedObject = corpse;
            return true;
        }
        return false;
    }
    public void DetachObject(Vector2 direction, bool destroyImmediate = false)
    {
        
        if(attachedObject != null)
        {
            
            attachedObject.transform.parent = null;
            attachedObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            //attachedObject.GetComponent<Rigidbody2D>().excludeLayers = LayerMask.GetMask();
            attachedObject.GetComponent<Rigidbody2D>().linearVelocity = transform.parent.GetComponent<Rigidbody2D>().linearVelocity;
            attachedObject.GetComponent<Rigidbody2D>().AddForce(direction * detachSpeed, ForceMode2D.Impulse);
            attachedObject.GetComponent<CorpseController>().Discarded = true;
        }
        if(destroyImmediate)
        {
            attachedObject.GetComponent<CorpseController>().DestroyCorpse();
        }
        attachedObject = null;
    }
}
