using UnityEngine;

public class MathBook : Interaction
{
    private Vector3 putAwayPos;

    //hold the position of where the book is put away
    private void Start()
    {
        putAwayPos = transform.position;
    }

    //overrides the new put away position, because new object is taken
    //and resets the catch score of mother which is needed to trigger her
    public override void TakeInteractableObject(GameObject interactable)
    {
        base.TakeInteractableObject(interactable);
        interactableObjectPutAwayPosition = transform.position;
        UIScoreCounter.instance.PickedUpBook();
    }
    
    //Assigns a new put away position
    public override void AssignPutDownPos()
    {
        interactableObjectPutAwayPosition = putAwayPos;
    }
}
