using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject rabbitGame;
    [SerializeField] private GameObject scoreCanvas;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    
    public void StartGame()
    {
        scoreCanvas.SetActive(true);
        rabbitGame.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void Options()
    {
        optionsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
