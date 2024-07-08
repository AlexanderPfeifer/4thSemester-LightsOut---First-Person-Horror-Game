using System.Collections.Generic;
using UnityEngine;

public class MotherTextManager : MonoBehaviour
{
    [SerializeField] public string startSentence;
    [SerializeField] public string endSentence;
    
    [SerializeField] private List<string> loseSentences;

    private int currentLoseSentence;

    //The first sentence of the mother, when the game starts
    public void StartMotherText()
    {
        MotherTimerManager.Instance.currentTime = 35f;

        TextManager.Instance.TypeText(startSentence,.03f);
        
        StartCoroutine(PlayerInputs.Instance.PutDownInteractableCoroutine());

        FindObjectOfType<StartMemScape>().canInteractWithConsole = false;
    }

    //When the player makes a mistake inside a game, a random text is displayed that notifies the player, that the mother heard something
    public void PlayLoseText()
    {
        if (currentLoseSentence < loseSentences.Count)
        {
            TextManager.Instance.TypeText(loseSentences[currentLoseSentence],.03f);
            currentLoseSentence++;
        }
        else
        {
            currentLoseSentence = 0;
            TextManager.Instance.TypeText(loseSentences[currentLoseSentence],.03f);
            currentLoseSentence++;
        }
    }

    //The last sentence of the mother when the game ends
    public void EndMotherText()
    {
        MotherTimerManager.Instance.currentTime = 35f;
        
        TextManager.Instance.TypeText(endSentence,.03f);
    }
}
