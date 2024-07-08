using UnityEngine;

public class StartMemScape : MonoBehaviour
{
    public bool canInteractWithConsole;
    [SerializeField] private GameObject memScapeMenu;
     
    //Opens the game after the book is opened for the first time in the current scene
    public void OpenMemScape()
    {
        canInteractWithConsole = true;
        FindObjectOfType<MenuUI>().LoadingScreen(false);
        memScapeMenu.SetActive(true);
    }
}
