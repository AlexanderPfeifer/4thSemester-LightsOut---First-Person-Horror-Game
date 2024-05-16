using UnityEngine;
using UnityEngine.Rendering;

public class Console : Interaction
{
    private Vector3 putAwayPos;
    [SerializeField] private Volume consoleHoldVolume;

    private void Start()
    {
        var position = transform.position;
        interactableObjectInHandPosition = position;

        consoleHoldVolume.weight = 1;
        
        putAwayPos = new Vector3(position.x + 3, position.y - 3, position.z + 3);
    }
    
    public override void TakeInteractableObject(GameObject interactable)
    {
        base.TakeInteractableObject(interactable);
        consoleHoldVolume.weight = 1;
    }

    public override void AssignPutDownPos()
    {
        interactableObjectPutAwayPosition = putAwayPos;
    }

    public override void PutDownInteractableObject(GameObject interactable)
    {
        base.PutDownInteractableObject(interactable);
        consoleHoldVolume.weight = 0;
    }
}
