using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MemScape : MonoBehaviour
{
    [Header("Memorize Objects")]
    [SerializeField] private List<Button> clickableButtons;
    [SerializeField] private List<int> memorizeOrder;
    [SerializeField] private List<Button> goThroughList;
    
    [Header("TimeToRememberBackground")]
    [SerializeField] private CanvasGroup timeToRemember;

    [Header("GameTime")]
    [SerializeField] private int winningCount;
    [SerializeField] private int timeBonus;

    private void Start()
    {
        AddRandomMemorizeObject();

        StartCoroutine(TimeToRememberScreen());
    }

    //Shows a screen that says "time to remember" and then adds an object to remember
    private IEnumerator TimeToRememberScreen()
    {
        UIInteraction.instance.canInteract = false;

        while (timeToRemember.alpha < .9f)
        {
            timeToRemember.alpha = Mathf.Lerp(timeToRemember.alpha, 1, Time.deltaTime);
            yield return null;
        }
        
        AudioManager.Instance.Play("MemScapeTimeToRemember");

        while (timeToRemember.alpha > 0.01f)
        {
            timeToRemember.alpha = Mathf.Lerp(timeToRemember.alpha, 0, Time.deltaTime * 2);
            yield return null;
        }

        timeToRemember.alpha = 0;
        
        StartCoroutine(SetButtonColors());

        yield return null;
    }

    //Adds Random Object to remember
    private void AddRandomMemorizeObject()
    {
        memorizeOrder.Add(Random.Range(0, clickableButtons.Count));
    }
    
    //Sets the button colors according to the objects and their sequence
    private IEnumerator SetButtonColors()
    {
        UIInteraction.instance.canInteract = false;
        
        goThroughList.Clear();

        if (UIInteraction.instance.lastSelectedButton != null)
        {
            UIInteraction.instance.SetSelectedButtonColor(UIInteraction.instance.lastSelectedButton, 1, 1, 1, 1, .2f, .2f, .2f);
        }
        
        for (int i = 0; i < memorizeOrder.Count; i++) 
        {
            switch (i)
            {
                case 0:
                    AudioManager.Instance.Play("MemScapeFirstImage");
                    break;
                case 1:
                    AudioManager.Instance.Play("MemScapeSecondImage");
                    break;
                case 2:
                    AudioManager.Instance.Play("MemScapeThirdImage");
                    break;
                case 3:
                    AudioManager.Instance.Play("MemScapeFourthImage");
                    break;
                case 4:
                    AudioManager.Instance.Play("MemScapeFifthImage");
                    break;
                case 5:
                    AudioManager.Instance.Play("MemScapeSixthImage");
                    break;
                case 6:
                    AudioManager.Instance.Play("MemScapeSeventhImage");
                    break;
                case 7:
                    AudioManager.Instance.Play("MemScapeEighthImage");
                    break;
            }
            
            goThroughList.Add(clickableButtons[memorizeOrder[i]]);
            
            while (clickableButtons[memorizeOrder[i]].transform.GetChild(0).GetComponent<Image>().color.a < 0.7f)
            {
                clickableButtons[memorizeOrder[i]].transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1, Mathf.Lerp(clickableButtons[memorizeOrder[i]].transform.GetChild(0).GetComponent<Image>().color.a, 1, Time.deltaTime * 3));

                yield return null;
            }
        
            while (clickableButtons[memorizeOrder[i]].transform.GetChild(0).GetComponent<Image>().color.a  > 0.1f)
            {
                clickableButtons[memorizeOrder[i]].transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1, Mathf.Lerp(clickableButtons[memorizeOrder[i]].transform.GetChild(0).GetComponent<Image>().color.a, 0, Time.deltaTime * 3));
                
                yield return null;
            }
            
            clickableButtons[memorizeOrder[i]].transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1, 0);
        }
        
        MotherTimerManager.Instance.pauseGameTime = false;
        
        UIInteraction.instance.canInteract = true;
    }

    //Checks if the game is won after and object was pressed 
    public void CheckWinState()
    {
        if (goThroughList[0].gameObject.GetComponent<Button>() == UIInteraction.instance.currentSelectedButton)
        {
            goThroughList.RemoveAt(0);
            
            if (goThroughList.Count == 0)
            {
                MotherTimerManager.Instance.TimeBonus(timeBonus);

                MotherTimerManager.Instance.pauseGameTime = true;

                AddRandomMemorizeObject();

                if (!(memorizeOrder.Count >= winningCount))
                {
                    StartCoroutine(CorrectSequence());

                }
                else
                {
                    MotherBehaviour.instance.PlayerWin();
                }
                
                return;
            }
            
            MotherTimerManager.Instance.TimeBonus(timeBonus);

            AudioManager.Instance.Play("MemScapeCorrectClick");
        }
        else
        {
            StartCoroutine(WrongSequence());
        }
    }

    //When every object that needed to be remember is pressed, adds another object to remember
    private IEnumerator CorrectSequence()
    {
        UIInteraction.instance.canInteract = false;

        AudioManager.Instance.Play("MemScapeSequenceCorrect");
        
        yield return new WaitUntil(() => AudioManager.Instance.IsPlaying("MemScapeSequenceCorrect") == false);
        
        StartCoroutine(SetButtonColors());
    }

    //When a wrong object is pressed, the sequence resets and is played again
    private IEnumerator WrongSequence()
    {
        UIInteraction.instance.canInteract = false;
        
        goThroughList.Clear();
        FindObjectOfType<MotherTextManager>().PlayLoseText();
        PlayerInputs.Instance.PlayChildAggressiveAnimation();

        AudioManager.Instance.Play("MemScapeWrongClick");

        yield return new WaitUntil(() => AudioManager.Instance.IsPlaying("MemScapeWrongClick") == false);

        StartCoroutine(SetButtonColors());
    }
}
