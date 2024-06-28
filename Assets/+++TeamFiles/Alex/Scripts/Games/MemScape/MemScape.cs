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
    private int firstFourObjects;
    private bool selectedConsole;
    [SerializeField] private int winningCount;
    [SerializeField] private int timeBonus;

    private void Start()
    {
        memorizeOrder.Add(1);
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
            
            while (clickableButtons[memorizeOrder[i]].transform.GetChild(0).GetComponent<Image>().color.a < 0.9f)
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
            
            MotherTimerManager.instance.TimeBonus(timeBonus);

            if (goThroughList.Count == 0)
            {
                MotherTimerManager.instance.pauseGameTime = true;

                if (firstFourObjects >= 3)
                {
                    AddMemorizeObject();
                }
                else if(firstFourObjects == 0)
                {
                    memorizeOrder.Add(3);
                    firstFourObjects = 1;
                }
                else if(firstFourObjects == 1)
                {
                    memorizeOrder.Add(2);
                    firstFourObjects = 2;
                }
                else if(firstFourObjects == 2)
                {
                    memorizeOrder.Add(0);
                    firstFourObjects = 3;
                }

                if (!(memorizeOrder.Count >= winningCount))
                {
                    StartCoroutine(SetButtonColors());
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
            StartCoroutine(SetButtonColors());
        }
    }
}
