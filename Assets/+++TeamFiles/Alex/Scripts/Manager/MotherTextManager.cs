using System.Collections.Generic;
using UnityEngine;

public class MotherTextManager : MonoBehaviour
{
    [SerializeField] public string startSentence;
    [SerializeField] public string endSentence;

    [SerializeField] private List<string> passiveAggressiveSentences;
    [SerializeField] private List<string> madSentences;
    [SerializeField] private List<string> loseSentences;

    private int currentLoseSentence;
    private int currentPassiveAggressiveSentence;
    private int currentMadSentence;

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

    public void PlayRandomTextPassiveAggressive()
    {
        if (currentPassiveAggressiveSentence <= passiveAggressiveSentences.Count)
        {
            TextManager.Instance.DisplayText(passiveAggressiveSentences[currentPassiveAggressiveSentence],.03f);
            currentPassiveAggressiveSentence++;
            if (currentPassiveAggressiveSentence == passiveAggressiveSentences.Count)
            {
                currentPassiveAggressiveSentence = 0;
            }
        }
    }
    
    public void PlayRandomTextMad()
    {
        if (currentMadSentence <= madSentences.Count)
        {
            TextManager.Instance.DisplayText(madSentences[currentMadSentence],.03f);
            currentMadSentence++;     
            if (currentMadSentence == madSentences.Count)
            {
                currentMadSentence = 0;
            }
        }
    }

    public void EndMotherText()
    {
        MotherTimerManager.instance.currentTime = 50f;
        
        TextManager.Instance.DisplayText(endSentence,.03f);
    }
}
