using UnityEngine;

public class MenuUI : MonoBehaviour
{ 
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private GameObject graphicsMenu;
    [SerializeField] private GameObject creditsMenu;
     
    //Starts the first console game
    public void StartGame()
    {
        mainMenu.SetActive(false);
    }

    //Sets option screen to true
    public void Options()
    {
        optionsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void MainMenu()
    {
        audioMenu.SetActive(true);
        graphicsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
    
    //Quits game
    public void QuitGame()
    {
        Application.Quit();
    }
}
