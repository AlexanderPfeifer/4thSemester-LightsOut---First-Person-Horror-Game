using System.Collections;
using UnityEngine;

public class MiningInteractableGame : Interaction, IInteractableGame
{
    [SerializeField] private GameObject miningGame;
    [SerializeField] private int scoreToWin;
    [SerializeField] private int achievablePoints;
    [SerializeField] private int pointsUntilCombo;

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
        miningGame.SetActive(true);
        UIScoreCounter.instance.scoreToWin = scoreToWin;
        UIScoreCounter.instance.achievablePoints = achievablePoints;
        UIScoreCounter.instance.winGameUntilCombo = pointsUntilCombo;
    }
    
    public IEnumerator HideInteractable()
    {
        yield return new WaitForSeconds(3);
        
        gameObject.GetComponent<Transform>().GetChild(2).gameObject.SetActive(false);
    }
}
