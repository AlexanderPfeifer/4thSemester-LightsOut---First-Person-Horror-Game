using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject rabbitGame;
    [SerializeField] private GameObject scoreCanvas;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    
    //Starts the first console game
    public void StartGame()
    {
        scoreCanvas.SetActive(true);
        rabbitGame.SetActive(true);
        mainMenu.SetActive(false);
    }

    //Sets option screen to true
    public void Options()
    {
        optionsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    
    //Quits game
    public void QuitGame()
    {
        Application.Quit();
    }
}
