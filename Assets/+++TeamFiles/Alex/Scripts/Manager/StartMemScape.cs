using UnityEngine;

public class StartMemScape : MonoBehaviour
{
    public bool canInteractWithConsole;
    [SerializeField] private GameObject memScapeMenu;
     
    
    public void OpenMemScape()
    {
        canInteractWithConsole = true;
        FindObjectOfType<MenuUI>().LoadingScreen(false);
        memScapeMenu.SetActive(true);
    }
}
