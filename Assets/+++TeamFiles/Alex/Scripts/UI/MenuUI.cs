using UnityEngine;

public class MenuUI : MonoBehaviour
{ 
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private GameObject graphicsMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject loadingScreen;

    //Starts the first console game
    public void StartGame()
    {
        mainMenu.SetActive(false);
        AudioManager.Instance.Play("ButtonImportant");
    }

    //Sets option screen to true
    public void Options()
    {
        AudioManager.Instance.Play("ButtonImportant");
        optionsMenu.SetActive(true);
        mainMenu.SetActive(false);
        audioMenu.SetActive(false);
        graphicsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }
    
    public void LoadingScreen(bool state)
    {
        loadingScreen.SetActive(state);
    }

    public void MainMenu()
    {
        AudioManager.Instance.Play("ButtonImportant");
        audioMenu.SetActive(false);
        graphicsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
    
    //Quits game
    public void QuitGame()
    {
        AudioManager.Instance.Play("ButtonImportant");
        Application.Quit();
    }
}
