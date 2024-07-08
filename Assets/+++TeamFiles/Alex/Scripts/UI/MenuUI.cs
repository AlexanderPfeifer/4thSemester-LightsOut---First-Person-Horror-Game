using UnityEngine;

public class MenuUI : MonoBehaviour
{ 
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private GameObject graphicsMenu;
    [SerializeField] private GameObject loadingScreen;

    //Starts the first console game
    public void StartGame()
    {
        mainMenu.SetActive(false);
        AudioManager.Instance.Play("ButtonImportant");
    }

    //PLays button sound for main buttons
    public void PlayButtonSound()
    {
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
    }
    
    //Sets loading screen activation state
    public void LoadingScreen(bool state)
    {
        loadingScreen.SetActive(state);
    }

    //opens main menu
    public void MainMenu()
    {
        AudioManager.Instance.Play("ButtonImportant");
        audioMenu.SetActive(false);
        graphicsMenu.SetActive(false);
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
