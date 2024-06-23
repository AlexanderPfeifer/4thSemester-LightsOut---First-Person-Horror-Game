using System.Collections.Generic;
using UnityEngine;

public class MotherTextManager : MonoBehaviour
{
    [SerializeField] public List<string> sentences;
    [HideInInspector] public bool finishedSentence;

    public void StartMotherText()
    {
        MotherTimerManager.instance.currentTime = 50f;

        TextManager.Instance.DisplayText(sentences[0],.05f);
        
        StartCoroutine(PlayerInputs.instance.PutDownInteractableCoroutine());

        FindObjectOfType<TutorialManager>().canInteractWithConsole = false;
    }
}
