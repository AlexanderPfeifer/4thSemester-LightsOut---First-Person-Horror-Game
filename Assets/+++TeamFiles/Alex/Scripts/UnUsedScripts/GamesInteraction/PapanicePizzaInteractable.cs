using UnityEngine;

public class PapanicePizzaInteractable : Interaction, IInteractableGame
{
    [SerializeField] private GameObject papanicePizzaMainMenu;
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
        papanicePizzaMainMenu.SetActive(true);
    }
}
