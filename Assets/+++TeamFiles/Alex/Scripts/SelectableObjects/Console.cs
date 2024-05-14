using UnityEngine;

public class Console : Interaction
{
    private Vector3 putAwayPos;

    private void Start()
    {
        var position = transform.position;

        interactableObjectInHandPosition = position;
        
        putAwayPos = new Vector3(position.x + 3, position.y - 3, position.z + 3);
    }

    public override void TakeInteractableObject(GameObject interactable)
    {
        interactable.transform.position = Vector3.Lerp(interactable.transform.position, interactableObjectInHandPosition, Time.deltaTime * interactableObjectPutAwaySpeed);
        interactable.transform.localRotation = Quaternion.Lerp(interactable.transform.localRotation, Quaternion.Euler(0, 90, 0), Time.deltaTime * interactableObjectPutAwaySpeed);
        //consoleHoldVolume.weight = 1;
    }

    public override GameObject GetGameObject()
    {
        return gameObject;
    }

    public override void PutDownInteractableObject(GameObject interactable)
    {
        interactable.transform.position = Vector3.Lerp(interactable.transform.position, putAwayPos, Time.deltaTime * interactableObjectPutAwaySpeed);
        interactable.transform.localRotation = Quaternion.Lerp(interactable.transform.localRotation, Quaternion.Euler(0, 90, 90), Time.deltaTime * interactableObjectPutAwaySpeed);
    }
}
