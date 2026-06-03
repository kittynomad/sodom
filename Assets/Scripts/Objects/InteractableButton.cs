using UnityEngine;
using UnityEngine.Events;

public class InteractableButton : InteractableEntity
{
    [SerializeField] private UnityEvent _pressActions;
    [SerializeField] private bool _multipleInteractsAllowed = true;
    public override void OnInteract(PlayerBehaviors pb)
    {
        base.OnInteract(pb);
        _pressActions.Invoke();
    }
}
