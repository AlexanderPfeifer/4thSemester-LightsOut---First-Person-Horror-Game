using System.Collections.Generic;
using UnityEngine;

public class MotherTextManager : MonoBehaviour
{
    [SerializeField] public List<string> sentences;

    public void StartMotherText()
    {
        MotherTimerManager.instance.currentTime = 50f;

        TextManager.Instance.DisplayText(sentences[0],.05f);
        
        StartCoroutine(PlayerInputs.instance.PutDownInteractableCoroutine());

        FindObjectOfType<StartMemScape>().canInteractWithConsole = false;
    }

    public void EndMotherText()
    {
        MotherTimerManager.instance.currentTime = 50f;

        //Play another text 
        
        TextManager.Instance.DisplayText(sentences[0],.05f);
    }
}
