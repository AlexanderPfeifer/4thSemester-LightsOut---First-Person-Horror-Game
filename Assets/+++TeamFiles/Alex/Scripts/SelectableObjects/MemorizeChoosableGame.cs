
using UnityEngine;

public class MemorizeChoosableGame : Interaction, IChoosableGame
{
    [SerializeField] private GameObject memorizeGame;

    public override void TakeInteractableObject(GameObject interactable)
    {
        base.TakeInteractableObject(interactable);
        interactableObjectPutAwayPosition = transform.position;
    }

    public void OpenGame()
    {
        StartCoroutine(motherBehaviour.NewGameGotPicked(3));
        memorizeGame.SetActive(true);
    }
}
