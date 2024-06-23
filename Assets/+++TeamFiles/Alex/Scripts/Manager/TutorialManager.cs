using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public bool canInteractWithConsole;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject memScapeMenu;
     
    
    public void OpenFirstGame()
    {
        canInteractWithConsole = true;
        loadingScreen.SetActive(false);
        memScapeMenu.SetActive(true);
    }
}
