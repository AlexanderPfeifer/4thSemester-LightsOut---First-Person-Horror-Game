using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Cooking_Minigame : MonoBehaviour
{
    
    [SerializeField] private GameObject orderDough;
    [SerializeField] private GameObject orderSauce;

    [SerializeField] private GameObject chosenDough;
    [SerializeField] private GameObject chosenSauce;

    [SerializeField] private List<GameObject> orderList;
    [SerializeField] private List<GameObject> ingList;
    
    [SerializeField] private bool workingOnOrder = false;

    [SerializeField] private GameObject rightOrderReact;
    [SerializeField] private GameObject wrongOrderReact;

    [SerializeField] private Image timerBar;
    public float timerLength;
    private float totalTime;
    private float timeLeft;
    [SerializeField] private bool isTimerRunning;

    public void Start()
    {
        chosenDough.SetActive(false);
        chosenSauce.SetActive(false);
        wrongOrderReact.SetActive(false);
        rightOrderReact.SetActive(false);

        totalTime = timerLength;
        timeLeft = totalTime;
        
        foreach (var ingredient in ingList)
        {
            ingredient.SetActive(false);
        }
        
        GenerateOrder();

        StartCoroutine(StartTimerCoRo());
    }

    public IEnumerator StartTimerCoRo()
    {
        float elapsedTime = 0f;

        yield return new WaitForSeconds(0.25f);

        isTimerRunning = true;
        
        while (timeLeft > 0 && isTimerRunning)
        {
            elapsedTime += Time.deltaTime;
            
            float fillAmountRatio = Mathf.Clamp01(elapsedTime / totalTime);
            timerBar.fillAmount = 1 - fillAmountRatio;
            

            if (elapsedTime >= totalTime)
            {
                StartCoroutine(WrongOrder());
            }

            yield return null;
        }

        if (!isTimerRunning)
        {
            Debug.Log("Timer Paused");
        }
    }

    public void GenerateOrder()
    {
        rightOrderReact.SetActive(false);
        wrongOrderReact.SetActive(false);
        
        StartCoroutine(StartTimerCoRo());
        orderDough.SetActive(true);
        orderSauce.SetActive(true);

        foreach (var orderIng in orderList)
        {
            if (Random.Range(0, 2) == 0)
            {
                orderIng.SetActive(false);
                Debug.Log("false");
            }
            else
            {
                orderIng.SetActive(true);
                Debug.Log("true");
            } 
        }

        workingOnOrder = true;
    }

    public void Update()
    {
        if (chosenDough.activeSelf == true && chosenSauce.activeSelf == true && workingOnOrder == true)
        {
            CheckIfMatch();
        }
    }

    public void CheckIfMatch()
    {
        
        bool allMatch = true;
        
        for (int i = 0; i < orderList.Count; i++)
        {
            if (orderList[i].activeSelf != ingList[i].activeSelf)
            {
                allMatch = false;
                break;
            }
        }

        if (allMatch)
        {
            StartCoroutine(CorrectOrder());
        }

    }

    public IEnumerator CorrectOrder()
    {
        StopCoroutine(StartTimerCoRo());
        isTimerRunning = false;
        workingOnOrder = false;
        orderDough.SetActive(false);
        orderSauce.SetActive(false);

        foreach (var order in orderList)
        {
            order.SetActive(false);
        } 
        
        rightOrderReact.SetActive(true);
        
        yield return new WaitForSeconds(1.0f);

        foreach (var ing in ingList)
        {
            ing.SetActive(false);
        }
        
        chosenDough.SetActive(false);
        chosenSauce.SetActive(false);
        
        Debug.Log("generating new order");
        
        GenerateOrder();
        StopCoroutine(CorrectOrder());
    }

    public IEnumerator WrongOrder()
    {
        StopCoroutine(StartTimerCoRo());
        isTimerRunning = false;
        workingOnOrder = false;
        orderDough.SetActive(false);
        orderSauce.SetActive(false);

        foreach (var order in orderList)
        {
            order.SetActive(false);
        } 
        
        wrongOrderReact.SetActive(true);
        
        yield return new WaitForSeconds(1.0f);

        foreach (var ing in ingList)
        {
            ing.SetActive(false);
        }
        
        chosenDough.SetActive(false);
        chosenSauce.SetActive(false);
        
        Debug.Log("generating new order");
        
        GenerateOrder();
        StopCoroutine(CorrectOrder());
    }
    
    #region Pick and Remove Ingredients
    
    public void PickIngredient1()
    {
        ingList[0].SetActive(true);
        
    }

    public void RemoveIngredient1()
    {
        ingList[0].SetActive(false);
    }
    
    public void PickIngredient2()
    {
        ingList[1].SetActive(true);
        
    }

    public void RemoveIngredient2()
    {
        ingList[1].SetActive(false);
    }
    
    public void PickIngredient3()
    {
        ingList[2].SetActive(true);
        
    }

    public void RemoveIngredient3()
    {
        ingList[2].SetActive(false);
    }
    
    public void PickIngredient4()
    {
        ingList[3].SetActive(true);
        
    }

    public void RemoveIngredient4()
    {
        ingList[3].SetActive(false);
    }
    
    public void PickIngredient5()
    {
        ingList[4].SetActive(true);
        
    }

    public void RemoveIngredient5()
    {
        ingList[4].SetActive(false);
    }
    
    public void PickIngredient6()
    {
        ingList[5].SetActive(true);
        
    }

    public void RemoveIngredient6()
    {
        ingList[5].SetActive(false);
    }
    
    public void PickDough()
    {
        chosenDough.SetActive(true);
        
    }
    
    
    public void PickSauce()
    {
        chosenSauce.SetActive(true);
        
    }
    
    
    #endregion
    
}
