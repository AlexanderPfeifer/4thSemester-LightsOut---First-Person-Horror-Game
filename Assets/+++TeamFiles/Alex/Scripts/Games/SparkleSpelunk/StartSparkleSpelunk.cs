using UnityEngine;

public class StartSparkleSpelunk : MonoBehaviour
{
    
    //Opens the game after the book is opened for the first time in the current scene
    public void OpenSparkleSpelunk()
    {
        FindObjectOfType<MenuUI>().LoadingScreen(false);
    }
}
