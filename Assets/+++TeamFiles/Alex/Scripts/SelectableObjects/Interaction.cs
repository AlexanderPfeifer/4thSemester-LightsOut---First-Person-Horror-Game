using UnityEngine;

public abstract class Interaction : MonoBehaviour
{
    public float interactableObjectPutAwaySpeed = 4;
    [HideInInspector] public float interactablePutAwayRotationY;
    [HideInInspector] public Vector3 interactablePutAwayPosition;

    private void Start()
    {
        interactableObjectPutAwaySpeed = 4;
    }

    //Lerp the rotation and position of the object to the position of where the player holds the console
    public virtual void TakeInteractableObject(GameObject interactable)
    {
        interactableObjectPutAwaySpeed += Time.deltaTime * 2;
        interactable.transform.position = Vector3.Lerp(interactable.transform.position, Vector3.zero, Time.deltaTime * interactableObjectPutAwaySpeed);
        interactable.transform.localRotation = Quaternion.Lerp(interactable.transform.localRotation, Quaternion.Euler(0,0,0), Time.deltaTime * interactableObjectPutAwaySpeed);
    }

    //Lerp the rotation and position of the object to the position where it is put away
    public virtual void PutDownInteractableObject(GameObject interactable)
    {
        interactableObjectPutAwaySpeed += Time.deltaTime * 2;
        interactable.transform.position = Vector3.Lerp(interactable.transform.position, interactablePutAwayPosition, Time.deltaTime * interactableObjectPutAwaySpeed);
        interactable.transform.localRotation = Quaternion.Lerp(interactable.transform.localRotation, Quaternion.Euler(90, interactablePutAwayRotationY, 0), Time.deltaTime * interactableObjectPutAwaySpeed);
    }

    //Assigns a put away position according to where it was before it got taken
    public virtual void AssignPutDownPos()
    { }
    
    //Assigns a put away rotation according to where it was before it got taken
    public virtual void AssignPutDownRot()
    { }
}
