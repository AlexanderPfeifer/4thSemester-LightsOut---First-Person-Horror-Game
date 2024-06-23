using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class MemScapeInteractable : Interaction, IInteractableGame
{
    [SerializeField] private GameObject memScapeMainMenu;
    [SerializeField] private int scoreToWin;

    //Takes the game to hold position
    public override void TakeInteractableObject(GameObject interactable)
    {
        base.TakeInteractableObject(interactable);
        interactablePutAwayPosition = transform.position;
    }

    //Opens the new game assigned to the object
    public void OpenGame()
    {
        memScapeMainMenu.SetActive(true);
        UIScoreCounter.instance.scoreToWin = scoreToWin;
    }
}
