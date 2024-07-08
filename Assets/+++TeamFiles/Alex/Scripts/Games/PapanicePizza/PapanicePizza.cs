using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PapanicePizza : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<GameObject> orderList;
    [SerializeField] private List<GameObject> ingredientsList;
    
    [Header("OrderCorrectness")]
    [SerializeField] private GameObject rightOrderReact;
    [SerializeField] private GameObject wrongOrderReact;
    
    [Header("Time")]
    [SerializeField] private int timeBonus;
    [SerializeField] private int timePenalty;
    [SerializeField] private Image timerBar;
    [SerializeField] private float maxTimeForOrder;
    [HideInInspector] public bool runTimer;
    private bool timerUp;
    private float currentTimeForOrder;

    [Header("Score")]
    [SerializeField] private int winScore;
    private int currentScore;
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI winScoreText;
    
    [Header("CurrentOrder")]
    bool gotDough;
    private bool generatedOrder;

    //Deactivates sprites and resets the timers and starts the game with a new order
    public void Start()
    {
        winScoreText.text = winScore.ToString();
        
        currentTimeForOrder = maxTimeForOrder;
        
        UIInteraction.instance.canInteract = false;

        wrongOrderReact.SetActive(false);
        rightOrderReact.SetActive(false);
        
        SetActivationListObject(ingredientsList, false);
        SetActivationListObject(orderList, false);

        StartCoroutine(GameStarted());

        if (PlayerInputs.Instance.holdObjectState != PlayerInputs.HoldObjectState.InHand)
            StopAllCoroutines();
    }
    
    private void Update() => Timer();

    //Starts the game a bit delayed to give the player time to react
    private IEnumerator GameStarted()
    {
        yield return new WaitForSeconds(2);
        
        AudioManager.Instance.Play("NewPizza");

        GenerateNewOrder();
        
        UIInteraction.instance.canInteract = true;

        MotherTimerManager.Instance.GameStarted();

        runTimer = true;
    }

    //Handles everything about the timer to complete orders
    private void Timer()
    {
        currentScoreText.text = currentScore.ToString();
        
        if (runTimer)
        {
            if (currentTimeForOrder <= 0 && !timerUp)
            {
                WrongPizza();
                timerUp = true;
            }
            else
            {
                timerBar.fillAmount = currentTimeForOrder / maxTimeForOrder;
        
                currentTimeForOrder -= Time.deltaTime;
            }   
        }
    }

    //Generates a new order with random outcome, but keeps the dough as the base
    private void GenerateNewOrder()
    {
        foreach (var orderIngredients in orderList)
        {
            orderIngredients.SetActive(Random.Range(0, 2) != 0);
        }
        
        orderList[0].SetActive(true);
    }

    //With every new ingredient added, checks if the ingredients of the pizza and the order match or not
    public void CheckIfMatch()
    {
        if (ingredientsList[0].activeSelf && !gotDough)
        {
            gotDough = true;
            CheckIfWin();
            return;
        }

        if(!ingredientsList[0].activeSelf)
        {
            WrongPizza();
            return;  
        }

        if (ingredientsList.Where((ingredient, i) => !orderList[i].activeSelf && ingredient.activeSelf).Any())
        {
            WrongPizza();
            return;
        }
        
        CheckIfWin();
    }
    
    //Checks win state on every added ingredient and adds a point to the score and gets a new order ready, plays a random sound effect on any ingredient 
    private void CheckIfWin()
    {
        var randomSound = Random.Range(0, 8);

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
                    MotherTimerManager.Instance.TimeBonus(timeBonus);
                    currentScore++;
                    if (maxTimeForOrder > 4)
                    {
                        maxTimeForOrder -= 1.5f;
                    }
                    
                    AudioManager.Instance.Play("MemScapeTimeToRemember");
                    
                    if (currentScore >= winScore)
                    {
                        MotherBehaviour.instance.PlayerWin();
                        return;
                    }
                }
            }
        }
        
        switch (randomSound)
        {
            case 0 :
                AudioManager.Instance.Play("MemScapeFirstImage");
                break;
            case 1 :
                AudioManager.Instance.Play("MemScapeSecondImage");
                break;
            case 2 :
                AudioManager.Instance.Play("MemScapeThirdImage");
                break;
            case 3 :
                AudioManager.Instance.Play("MemScapeFourthImage");
                break;
            case 4 :
                AudioManager.Instance.Play("MemScapeFifthImage");
                break;
            case 5 :
                AudioManager.Instance.Play("MemScapeSixthImage");
                break;
            case 6 :
                AudioManager.Instance.Play("MemScapeSeventhImage");
                break;
            case 7 :
                AudioManager.Instance.Play("MemScapeEighthImage");
                break;
        }
    }

    //Subtracts time from mother timer and gets a new order ready
    private void WrongPizza()
    {
        FindObjectOfType<MotherTextManager>().PlayLoseText();
        StartCoroutine(SetOrderStateCoroutine(wrongOrderReact));
        MotherTimerManager.Instance.TimePenalty(timePenalty);
        AudioManager.Instance.Play("PapanicePizzaWrong");
        PlayerInputs.Instance.PlayChildAggressiveAnimation();
    }

    //Shows if the order was right or wrong and gets a new order
    private IEnumerator SetOrderStateCoroutine(GameObject orderReactSprite)
    {
        runTimer = false;
        
        gotDough = false;
        
        UIInteraction.instance.canInteract = false;

        orderList[0].SetActive(false);
        orderList[1].SetActive(false);
        SetActivationListObject(orderList, false);

        orderReactSprite.SetActive(true);
        
        yield return new WaitForSeconds(2.0f);
        
        orderReactSprite.SetActive(false);
        
        UIInteraction.instance.canInteract = true;

        SetActivationListObject(ingredientsList, false);
        
        GenerateNewOrder();

        timerUp = false;

        runTimer = true;
        
        currentTimeForOrder = maxTimeForOrder;
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
