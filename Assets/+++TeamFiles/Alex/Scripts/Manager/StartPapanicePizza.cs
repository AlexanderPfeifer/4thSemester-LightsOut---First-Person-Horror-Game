using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPapanicePizza : MonoBehaviour
{
    public void OpenPapanicePizza()
    {
        FindObjectOfType<MenuUI>().LoadingScreen(false);
    }
}
