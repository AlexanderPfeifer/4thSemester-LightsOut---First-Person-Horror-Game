using UnityEngine;

public class QuitAtEndOfGame : MonoBehaviour
{
    //Closes game on its own at the end of the game
    public void CloseGame()
    {
        Application.Quit();
    }
}
