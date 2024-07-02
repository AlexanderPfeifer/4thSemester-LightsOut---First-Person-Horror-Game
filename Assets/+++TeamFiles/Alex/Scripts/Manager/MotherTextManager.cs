using System.Collections.Generic;
using UnityEngine;

public class MotherTextManager : MonoBehaviour
{
    [SerializeField] public List<string> sentences;

    [SerializeField] private List<string> friendlySentences;
    [SerializeField] private List<string> passiveAggressiveSentences;
    [SerializeField] private List<string> madSentences;

    public void StartMotherText()
    {
        MotherTimerManager.instance.currentTime = 50f;

        TextManager.Instance.DisplayText(sentences[0],.05f);
        
        StartCoroutine(PlayerInputs.instance.PutDownInteractableCoroutine());

        FindObjectOfType<StartMemScape>().canInteractWithConsole = false;
    }

    public void PlayRandomTextFriendly()
    {
        //TextManager.Instance.DisplayText(friendlySentences[Random.Range(0, friendlySentences.Count)],.05f);
    }
    
    public void PlayRandomTextPassiveAggressive()
    {
        //TextManager.Instance.DisplayText(friendlySentences[Random.Range(0, passiveAggressiveSentences.Count)],.05f);
    }
    
    public void PlayRandomTextMad()
    {
        //TextManager.Instance.DisplayText(friendlySentences[Random.Range(0, madSentences.Count)],.05f);
    }

    public void EndMotherText()
    {
        MotherTimerManager.instance.currentTime = 50f;

        //Play another text 
        
        TextManager.Instance.DisplayText(sentences[0],.05f);
    }
}
