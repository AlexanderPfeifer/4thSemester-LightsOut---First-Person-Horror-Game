using UnityEngine;

public class Book : Interaction
{
    private Vector3 putAwayPos;
    private float putAwayRotY;

    //hold the position of where the book is put away
    private void Start()
    {
        putAwayPos = transform.position;
        putAwayRotY = -25;
    }

    //overrides the new put away position, because new object is taken
    //and resets the catch score of mother which is needed to trigger her
    public override void TakeInteractableObject(GameObject interactable)
    {
        base.TakeInteractableObject(interactable);
        interactablePutAwayPosition = transform.position;
    }
    
    //Assigns the put down position of the console
    public override void AssignPutDownRot()
    {
        interactablePutAwayRotationY = putAwayRotY;
    }
    
    //Assigns a new put away position
    public override void AssignPutDownPos()
    {
        interactablePutAwayPosition = putAwayPos;
    }
}
