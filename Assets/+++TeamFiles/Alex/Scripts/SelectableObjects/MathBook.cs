using UnityEngine;

public class MathBook : Interaction
{
    private Vector3 putAwayPos;

    private void Start()
    {
        putAwayPos = transform.position;
    }

    public override void TakeInteractableObject(GameObject interactable)
    {
        base.TakeInteractableObject(interactable);
        interactableObjectPutAwayPosition = transform.position;
        UIScoreCounter.instance.ResetCaughtScore();
    }
    
    public override void AssignPutDownPos()
    {
        interactableObjectPutAwayPosition = putAwayPos;
    }
}
