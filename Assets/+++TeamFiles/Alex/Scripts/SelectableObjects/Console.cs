using UnityEngine;
using UnityEngine.Rendering;

public class Console : Interaction
{
    private Vector3 putAwayPos;
    private float putAwayRotY;
    [SerializeField] public Volume holdVolume;

    //assigns the position of where the objects are held in front of the player and the put away position and activates a blur effect
    private void Start()
    {
        putAwayPos = new Vector3(5, -3, 3);
        putAwayRotY = 25;
    }
    
    //Takes the object and activates blur effect
    public override void TakeInteractableObject(GameObject interactable)
    {
        base.TakeInteractableObject(interactable);
        holdVolume.weight = 1;
    }
    
    //Puts down console to put down position and deactivates blur effect
    public override void PutDownInteractableObject(GameObject interactable)
    {
        base.PutDownInteractableObject(interactable);
        holdVolume.weight = 0;
    }
    
    //Assigns the put down position of the console
    public override void AssignPutDownRot()
    {
        interactablePutAwayRotationY = putAwayRotY;
    }

    //Assigns the put down position of the console
    public override void AssignPutDownPos()
    {
        interactablePutAwayPosition = putAwayPos;
    }
}
