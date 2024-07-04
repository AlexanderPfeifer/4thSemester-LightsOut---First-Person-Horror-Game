using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MemScape : MonoBehaviour
{
    [SerializeField] private List<Button> clickableButtons;
    [SerializeField] private List<int> memorizeOrder;
    [SerializeField] private List<Button> goThroughList;
    private bool selectedConsole;
    [SerializeField] private int winningCount;
    [SerializeField] private int timeBonus;
    [SerializeField] private CanvasGroup timeToRemember;

    private void Start()
    {
        memorizeOrder.Add(1);

        StartCoroutine(TimeToRememberScreen());
    }

    private void Update()
    {
        CheckConsoleState();
    }

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
    
    private void CheckConsoleState()
    {
        if (PlayerInputs.instance.holdObjectState == PlayerInputs.HoldObjectState.InHand && PlayerInputs.instance.currentInteractableObject.TryGetComponent(out Console console))
        {
            if (selectedConsole)
            {
                StartCoroutine(SetButtonColors());
                selectedConsole = false;
            }
        }
        else
        {
            selectedConsole = true;
        }
    }
    
    private void AddMemorizeObject()
    {
        memorizeOrder.Add(Random.Range(0, clickableButtons.Count));
    }
    
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
                clickableButtons[memorizeOrder[i]].transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1, Mathf.Lerp(clickableButtons[memorizeOrder[i]].transform.GetChild(0).GetComponent<Image>().color.a, 1, Time.deltaTime * 2));

                yield return null;
            }
        
            while (clickableButtons[memorizeOrder[i]].transform.GetChild(0).GetComponent<Image>().color.a  > 0.1f)
            {
                clickableButtons[memorizeOrder[i]].transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1, Mathf.Lerp(clickableButtons[memorizeOrder[i]].transform.GetChild(0).GetComponent<Image>().color.a, 0, Time.deltaTime * 2));
                
                yield return null;
            }
            
            clickableButtons[memorizeOrder[i]].transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1, 0);
        }
        
        MotherTimerManager.instance.pauseGameTime = false;
        
        UIInteraction.instance.canInteract = true;
    }

    public void CheckWinState()
    {
        if (goThroughList[0].gameObject.GetComponent<Button>() == UIInteraction.instance.currentSelectedButton)
        {
            goThroughList.RemoveAt(0);
            
            if (goThroughList.Count == 0)
            {
                MotherTimerManager.instance.TimeBonus(timeBonus);

                MotherTimerManager.instance.pauseGameTime = true;

                AddMemorizeObject();

                if (!(memorizeOrder.Count >= winningCount))
                {
                    StartCoroutine(SetButtonColors());
                    AudioManager.Instance.Play("MemScapeSequenceCorrect");
                }
                else
                {
                    MotherBehaviour.instance.PlayerWon();
                }
                
                return;
            }
            
            MotherTimerManager.instance.TimeBonus(timeBonus);

            AudioManager.Instance.Play("MemScapeCorrectClick");
        }
        else
        {
            goThroughList.Clear();
            AudioManager.Instance.Play("MemScapeWrongClick");
            FindObjectOfType<MotherTextManager>().PlayLoseText();
            PlayerInputs.instance.PlayChildAggressiveAnimation();
            StartCoroutine(DelayedRestart());
        }
    }

    private IEnumerator DelayedRestart()
    {
        yield return new WaitForSeconds(1f);
        
        StartCoroutine(SetButtonColors());
    }
}
