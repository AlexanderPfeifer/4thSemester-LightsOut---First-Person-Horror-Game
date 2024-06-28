using UnityEngine;
using UnityEngine.Rendering;

public class Book : Interaction
{
    private Vector3 putAwayPos;
    private float putAwayRotY;
    [SerializeField] public Volume holdVolume;

    //hold the position of where the book is put away
    private void Start()
    {
        putAwayPos = new Vector3(-5, -3, 3);
        putAwayRotY = -25;
    }

    //overrides the new put away position, because new object is taken
    //and resets the catch score of mother which is needed to trigger her
    public override void TakeInteractableObject(GameObject interactable, AnimationCurve animationCurve)
    {
        base.TakeInteractableObject(interactable, PlayerInputs.instance.takeOrPutAwayInteractable);
        holdVolume.weight = 1;
    }
    
    //Puts down book to put down position and deactivates blur effect
    public override void PutDownInteractableObject(GameObject interactable, AnimationCurve animationCurve)
    {
        base.PutDownInteractableObject(interactable, PlayerInputs.instance.takeOrPutAwayInteractable);
        holdVolume.weight = 0;
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
