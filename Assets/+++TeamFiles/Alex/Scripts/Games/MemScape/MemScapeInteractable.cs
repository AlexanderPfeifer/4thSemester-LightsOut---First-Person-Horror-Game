using UnityEngine;

public class MemScapeInteractable : Interaction, IInteractableGame
{
    [SerializeField] private GameObject memorizeGame;
    [SerializeField] private int scoreToWin;

    //Takes the game to hold position
    public override void TakeInteractableObject(GameObject interactable)
    {
        base.TakeInteractableObject(interactable);
        interactableObjectPutAwayPosition = transform.position;
    }

    //Opens the new game assigned to the object
    public void OpenGame()
    {
        StartCoroutine(motherBehaviour.PickedNewGame(3));
        memorizeGame.SetActive(true);
    }
}
