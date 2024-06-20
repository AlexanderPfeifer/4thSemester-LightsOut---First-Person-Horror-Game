using UnityEngine;

public class PizzaInteractableGame : Interaction, IInteractableGame
{
    [SerializeField] private GameObject pizzaGame;
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
        pizzaGame.SetActive(true);
        UIScoreCounter.instance.scoreToWin = scoreToWin;
        UIScoreCounter.instance.achievablePoints = achievablePoints;
        UIScoreCounter.instance.winGameUntilCombo = pointsUntilCombo;
    }
}
