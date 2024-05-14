using UnityEngine;

public abstract class Interaction : MonoBehaviour, IInteractable
{
    public float interactableObjectPutAwaySpeed = 4;
    [HideInInspector] public Vector3 interactableObjectPutAwayPosition;
    [HideInInspector] public Vector3 interactableObjectInHandPosition;

    public virtual void TakeInteractableObject(GameObject interactable)
    {
        interactable.transform.position = Vector3.Lerp(interactable.transform.position, interactableObjectInHandPosition, Time.deltaTime * interactableObjectPutAwaySpeed);
        interactable.transform.localRotation = Quaternion.Lerp(interactable.transform.localRotation, Quaternion.Euler(0, 90, 0), Time.deltaTime * interactableObjectPutAwaySpeed);
    }

    public virtual void PutDownInteractableObject(GameObject interactable)
    {
        interactable.transform.position = Vector3.Lerp(interactable.transform.position, interactableObjectPutAwayPosition, Time.deltaTime * interactableObjectPutAwaySpeed);
        interactable.transform.localRotation = Quaternion.Lerp(interactable.transform.localRotation, Quaternion.Euler(0, 90, 90), Time.deltaTime * interactableObjectPutAwaySpeed);
    }

    //public abstract bool CanInteract(GameObject interactable);
    
    public abstract GameObject GetGameObject();
}
