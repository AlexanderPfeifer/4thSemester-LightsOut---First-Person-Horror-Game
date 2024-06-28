using UnityEngine;

public abstract class Interaction : MonoBehaviour
{
    public float interactableObjectPutAwaySpeed = 4;
    [HideInInspector] public float interactablePutAwayRotationY;
    [HideInInspector] public Vector3 interactablePutAwayPosition;

    //Lerps the rotation and position of the object to the position of where the player holds the console
    public virtual void TakeInteractableObject(GameObject interactable, AnimationCurve animationCurve)
    {
        animationCurve.keys[1].time = interactablePutAwayPosition.x;
        interactable.transform.position = Vector3.Lerp(interactable.transform.position, Vector3.zero, animationCurve.Evaluate(Vector3.Distance(interactable.transform.position, interactablePutAwayPosition)));
        interactable.transform.localRotation = Quaternion.Lerp(interactable.transform.localRotation, Quaternion.Euler(0,0,0), animationCurve.Evaluate(interactable.transform.position.x / interactablePutAwayPosition.x * Time.deltaTime));
    }

    //Lerps the rotation and position of the object to the position where it is put away
    public virtual void PutDownInteractableObject(GameObject interactable, AnimationCurve animationCurve)
    {
        animationCurve.keys[1].time = interactablePutAwayPosition.x;
        interactable.transform.position = Vector3.Lerp(interactable.transform.position, interactablePutAwayPosition, animationCurve.Evaluate(interactable.transform.position.x / interactablePutAwayPosition.x * Time.deltaTime));
        interactable.transform.localRotation = Quaternion.Lerp(interactable.transform.localRotation, Quaternion.Euler(90, interactablePutAwayRotationY, 0), animationCurve.Evaluate(interactable.transform.position.x / interactablePutAwayPosition.x * Time.deltaTime));
    }

    //Assigns a put away position according to where it was before it got taken
    public virtual void AssignPutDownPos()
    { }
    
    //Assigns a put away rotation according to where it was before it got taken
    public virtual void AssignPutDownRot()
    { }
}
