using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cooking_Minigame : MonoBehaviour
{
    [SerializeField] private GameObject ingredient;
    [SerializeField] private GameObject ingredientChosen;
    [SerializeField] private GameObject order;

    [SerializeField] private List<GameObject> ingredientList;

    [SerializeField] private int ingNum;

    [SerializeField] private GameObject customer;

    [SerializeField] private bool orderDone = false;

    public void Start()
    {
        /*ingredient.SetActive(false);
        ingredientChosen.SetActive(false);
        order.SetActive(false);
        customer.SetActive(false);
        StartCoroutine("BeginGameCoRo"); */
        
        GenerateOrder();
    }

    public IEnumerator BeginGameCoRo()
    {
        /*
        wait 3 seconds. 
        customer appears.
        speechbubble with customers order appears.     
         */

        yield break;
    }

    public void GenerateOrder()
    {
        foreach (var ingredient in ingredientList)
        {
            
        }

        for (int i = 0; i < 7; i++)
        {
            if (Random.Range(0, 2) == 0)
            {
                //ingredient.SetActive(false);
                Debug.Log("false");
            }
            else
            {
                //ingredient.SetActive(true);
                Debug.Log("true");
            }
        }

}

    public void Update()
    {
        
    }

    public void AddIngredient()
    {
        
    }
    
}
