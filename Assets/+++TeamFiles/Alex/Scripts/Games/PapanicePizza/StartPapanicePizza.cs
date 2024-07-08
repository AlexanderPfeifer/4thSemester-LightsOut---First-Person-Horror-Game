using UnityEngine;

public class StartPapanicePizza : MonoBehaviour
{
    
    //Opens the game after the book is opened for the first time in the current scene
    public void OpenPapanicePizza()
    {
        FindObjectOfType<MenuUI>().LoadingScreen(false);
    }
}
