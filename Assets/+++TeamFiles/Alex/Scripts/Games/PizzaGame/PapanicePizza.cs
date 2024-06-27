using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PapanicePizza : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<GameObject> orderList;
    [SerializeField] private List<GameObject> ingredientsList;
    
    [Header("OrderCorrectness")]
    [SerializeField] private GameObject rightOrderReact;
    [SerializeField] private GameObject wrongOrderReact;
    
    [SerializeField] private int timeBonus;
    [SerializeField] private int timePenalty;

    [SerializeField] private int winScore;
    private int currentScore;
    
    bool gotDough;
    bool gotSauce;
    bool gotCheese;

    //Deactivates sprites and resets the timers and starts the game with a new order
    public void Start()
    {
        wrongOrderReact.SetActive(false);
        rightOrderReact.SetActive(false);

        SetActivationListObject(ingredientsList, false);
        SetActivationListObject(orderList, false);
        
        GenerateNewOrder();

        if (PlayerInputs.instance.holdObjectState != PlayerInputs.HoldObjectState.InHand)
            StopAllCoroutines();
    }

    //Generates a new order with random outcome, but keeps sauce and dough as the base
    private void GenerateNewOrder()
    {
        foreach (var orderIngredients in orderList)
        {
            orderIngredients.SetActive(Random.Range(0, 2) != 0);
        }
        
        orderList[0].SetActive(true);
    }

    //With every new ingredient added, checks if the ingredients of the pizza and the order match
    public void CheckIfMatch()
    {
        if (ingredientsList[0].activeSelf && !gotDough)
        {
            gotDough = true;
            CheckWin();
            return;
        }
        
        if (orderList[1].activeSelf && ingredientsList[1].activeSelf && gotDough && !gotSauce)
        {
            gotSauce = true;
            CheckWin();
            return;
        }
        
        if(!ingredientsList[0].activeSelf)
        {
            StartCoroutine(SetOrderStateCoroutine(wrongOrderReact));
            MotherTimerManager.instance.TimePenalty(timePenalty);
            return;  
        }
        
        if (orderList[1].activeSelf && !gotSauce && gotDough)
        {
            StartCoroutine(SetOrderStateCoroutine(wrongOrderReact));
            MotherTimerManager.instance.TimePenalty(timePenalty);
            return;
        }

        for (int i = 1; i < ingredientsList.Count; i++)
        {
            if (!orderList[i].activeSelf && ingredientsList[i].activeSelf)
            {
                StartCoroutine(SetOrderStateCoroutine(wrongOrderReact));
                MotherTimerManager.instance.TimePenalty(timePenalty);
                return;
            }
        }
        
        CheckWin();
    }

    private void CheckWin()
    {
        var activeOrder = orderList.Count(orderElement => orderElement.activeSelf);

        var correctIngredients = 0;

        for (var i = 0; i < ingredientsList.Count; i++)
        {
            if (orderList[i].activeSelf && ingredientsList[i].activeSelf)
            {
                correctIngredients++;

                if (correctIngredients == activeOrder)
                {
                    StartCoroutine(SetOrderStateCoroutine(rightOrderReact));
                    MotherTimerManager.instance.TimeBonus(timeBonus);
                    currentScore++;
                    
                    if (currentScore >= winScore)
                    {
                        MotherBehaviour.instance.PlayerWon();
                    }
                }
            }
        }
    }

    //Shows if the order was right or wrong and gets a new order
    private IEnumerator SetOrderStateCoroutine(GameObject orderReactSprite)
    {
        gotDough = false;

        orderList[0].SetActive(false);
        orderList[1].SetActive(false);
        SetActivationListObject(orderList, false);

        orderReactSprite.SetActive(true);
        
        yield return new WaitForSeconds(1.0f);
        
        orderReactSprite.SetActive(false);

        SetActivationListObject(ingredientsList, false);

        GenerateNewOrder();
    }
    
    //Just a shortcut to set active every list object to a bool state
    private void SetActivationListObject(List<GameObject> list, bool activationState)
    {
        foreach (var gObject in list)
        {
            gObject.SetActive(activationState);
        } 
    }

}
