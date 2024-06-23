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

    private void Start()
    {
        AddMemorizeObject();
        StartCoroutine(SetButtonColors());
    }

    private void Update()
    {
        CheckConsoleState();
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
        goThroughList.Clear();

        UIInteraction.instance.canInteract = false;
        UIInteraction.instance.SetSelectedButtonColor(UIInteraction.instance.lastSelectedButton, 1, 1, 1, 1, .2f, .2f, .2f);
        
        for (int i = 0; i < memorizeOrder.Count; i++) 
        {
            goThroughList.Add(clickableButtons[memorizeOrder[i]]);

            while (clickableButtons[memorizeOrder[i]].colors.normalColor.g > 0.1f)
            {
                UIInteraction.instance.SetSelectedButtonColor(clickableButtons[memorizeOrder[i]], 1, Mathf.Lerp(clickableButtons[memorizeOrder[i]].colors.normalColor.g, 0, 
                    Time.deltaTime * 2), Mathf.Lerp(clickableButtons[memorizeOrder[i]].colors.normalColor.g, 0, Time.deltaTime * 2),1, 0.2f, 0.2f, 0.2f);

                yield return null;
            }
        
            while (clickableButtons[memorizeOrder[i]].colors.normalColor.g < 0.9f)
            {
                UIInteraction.instance.SetSelectedButtonColor(clickableButtons[memorizeOrder[i]], 1,Mathf.Lerp(clickableButtons[memorizeOrder[i]].colors.normalColor.g, 
                    1, Time.deltaTime * 2), Mathf.Lerp(clickableButtons[memorizeOrder[i]].colors.normalColor.g, 1, Time.deltaTime * 2),1, 0.2f, 0.2f, 0.2f);

                yield return null;
            }
        
            UIInteraction.instance.SetSelectedButtonColor(clickableButtons[memorizeOrder[i]], 1, 1, 1, 1, 0.2f, 0.2f, 0.2f);
        }

        UIInteraction.instance.canInteract = true;
    }

    public void CheckWinState()
    {
        if (goThroughList[0].gameObject.GetComponent<Button>() == UIInteraction.instance.currentSelectedButton)
        {
            goThroughList.RemoveAt(0);
            
            if (goThroughList.Count == 0) 
            {
                AddMemorizeObject();
                if (!(memorizeOrder.Count >= winningCount))
                {
                    StartCoroutine(SetButtonColors());
                    MotherTimerManager.instance.TimeBonus();
                }
                else
                {
                    MotherBehaviour.instance.PlayerWon();
                }
            }
        }
        else
        {
            goThroughList.Clear();
            MotherTimerManager.instance.TimePenalty();
            StartCoroutine(SetButtonColors());
        }
    }
}
