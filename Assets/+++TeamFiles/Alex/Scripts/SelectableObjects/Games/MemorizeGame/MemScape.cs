using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MemScape : MonoBehaviour
{
    private int randomNewImage;
    [SerializeField] private List<Button> clickableButtons;
    [SerializeField] private List<int> memorizeOrder;
    [SerializeField] private List<Button> goThroughList;

    private void Start()
    {
        StartCoroutine(AddMemorizeObjects());
    }

    private IEnumerator AddMemorizeObjects()
    {
        randomNewImage = Random.Range(0, clickableButtons.Count);
        memorizeOrder.Add(randomNewImage);
        
        for (int i = 0; i < memorizeOrder.Count; i++) 
        {
            goThroughList.Add(clickableButtons[memorizeOrder[i]]);

            while (clickableButtons[memorizeOrder[i]].colors.normalColor.g > 0.1f)
            {
                ButtonInteraction.instance.SetSelectedButtonColor(clickableButtons[memorizeOrder[i]], 1,Mathf.Lerp(clickableButtons[memorizeOrder[i]].colors.normalColor.g, 0, Time.deltaTime * 2), 
                    Mathf.Lerp(clickableButtons[memorizeOrder[i]].colors.normalColor.g, 0, Time.deltaTime * 2),1);

                yield return null;
            }
            
            while (clickableButtons[memorizeOrder[i]].colors.normalColor.g < 0.9f)
            {
                ButtonInteraction.instance.SetSelectedButtonColor(clickableButtons[memorizeOrder[i]], 1,Mathf.Lerp(clickableButtons[memorizeOrder[i]].colors.normalColor.g, 1, Time.deltaTime * 2), 
                    Mathf.Lerp(clickableButtons[memorizeOrder[i]].colors.normalColor.g, 1, Time.deltaTime * 2),1);

                yield return null;
            }
            
            ButtonInteraction.instance.SetSelectedButtonColor(clickableButtons[memorizeOrder[i]], 1,1, 1,1);
        }

        ButtonInteraction.instance.canInteract = true;
    }

    public void CheckWinState()
    {
        if (goThroughList[0].gameObject.GetComponent<Button>() == ButtonInteraction.instance.currentSelectedButton)
        {
            goThroughList.RemoveAt(0);
            
            if (goThroughList.Count == 0)
            {
                ButtonInteraction.instance.canInteract = false;
                StartCoroutine(AddMemorizeObjects());
                UIScoreCounter.instance.AddScore();
            }
        }
        else
        {
            memorizeOrder.Clear();
            goThroughList.Clear();
            UIScoreCounter.instance.ResetCombo();
            ButtonInteraction.instance.canInteract = false;
            StartCoroutine(AddMemorizeObjects());
        }
    }
}
