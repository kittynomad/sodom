using UnityEngine;
using UnityEngine.Events;

public class InteractableButton : InteractableEntity
{
    [SerializeField] private UnityEvent _pressActions;
    [SerializeField] private bool _multipleInteractsAllowed = true;

    private bool interactedWith = false;
    public override void OnInteract(PlayerBehaviors pb)
    {
        if(_multipleInteractsAllowed || !interactedWith)
        {
            base.OnInteract(pb);
            _pressActions.Invoke();
            interactedWith = true;
        }
        
    }
}
