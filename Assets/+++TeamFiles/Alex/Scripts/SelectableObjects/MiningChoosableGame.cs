using UnityEngine;

public class MiningChoosableGame : Interaction, IChoosableGame
{
    [SerializeField] private GameObject miningGame;
    
    public override void TakeInteractableObject(GameObject interactable)
    {
        base.TakeInteractableObject(interactable);
        interactableObjectPutAwayPosition = transform.position;
    }

    public void OpenGame()
    {
        StartCoroutine(motherBehaviour.NewGameGotPicked(3));
        miningGame.SetActive(true);
    }
}
