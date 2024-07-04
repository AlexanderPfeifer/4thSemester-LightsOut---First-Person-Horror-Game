using System.Collections.Generic;
using UnityEngine;

public class MotherTextManager : MonoBehaviour
{
    [SerializeField] public string startSentence;
    [SerializeField] public string endSentence;
    
    [SerializeField] private List<string> loseSentences;

    private int currentLoseSentence;

    public void StartMotherText()
    {
        MotherTimerManager.instance.currentTime = 50f;

        TextManager.Instance.DisplayText(startSentence,.03f);
        
        StartCoroutine(PlayerInputs.instance.PutDownInteractableCoroutine());

        FindObjectOfType<StartMemScape>().canInteractWithConsole = false;
    }

    public void PlayLoseText()
    {
        if (currentLoseSentence < loseSentences.Count)
        {
            TextManager.Instance.DisplayText(loseSentences[currentLoseSentence],.03f);
            currentLoseSentence++;
        }
        else
        {
            currentLoseSentence = 0;
            TextManager.Instance.DisplayText(loseSentences[currentLoseSentence],.03f);
            currentLoseSentence++;
        }
    }

    public void EndMotherText()
    {
        MotherTimerManager.instance.currentTime = 50f;
        
        TextManager.Instance.DisplayText(endSentence,.03f);
    }
}
