
using UnityEngine;

public class MemorizeGame : Interaction, IGame
{
    private Vector3 putAwayPos;

    private void Start()
    {
        putAwayPos = transform.position;
    }

    public override void TakeInteractableObject(GameObject interactable)
    {
        interactable.transform.position = Vector3.Lerp(interactable.transform.position, interactableObjectInHandPosition, Time.deltaTime * interactableObjectPutAwaySpeed);
        interactable.transform.localRotation = Quaternion.Lerp(interactable.transform.localRotation, Quaternion.Euler(0, 90, 0), Time.deltaTime * interactableObjectPutAwaySpeed);
        interactableObjectPutAwayPosition = transform.position;
        //consoleHoldVolume.weight = 1;
    }
    
    public override void PutDownInteractableObject(GameObject interactable)
    {
        interactable.transform.position = Vector3.Lerp(interactable.transform.position, putAwayPos, Time.deltaTime * interactableObjectPutAwaySpeed);
        interactable.transform.localRotation = Quaternion.Lerp(interactable.transform.localRotation, Quaternion.Euler(0, 90, 90), Time.deltaTime * interactableObjectPutAwaySpeed);
    }

    public override GameObject GetGameObject()
    {
        return gameObject;
    }

    public void OpenGame()
    {
        //Open whatever
    }
}
