using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MemScape : MonoBehaviour
{
    private int randomNewImage;
    [SerializeField] private List<Image> clickableButtons;
    [SerializeField] private List<int> memorizeOrder;
    private PlayerInputs playerInputs;
    [SerializeField] private Camera mainCam;

    private void Start()
    {
        StartMemorizeGame();
        playerInputs = FindObjectOfType<PlayerInputs>();
    }

    private void Update()
    {
        CheckClickedObject();
    }

    private void StartMemorizeGame()
    {
        randomNewImage = Random.Range(0, clickableButtons.Count);
        memorizeOrder.Add(randomNewImage);
        
        var originalButtonColor = new Color(0, 0, 0);
        var redButtonColor = new Color(1, 0, 0);

        for (int i = 0; i < memorizeOrder.Count; i++)
        {
            while (clickableButtons[i].color.r <= 0.9f)
            {
                clickableButtons[i].color = Color.Lerp(clickableButtons[i].color,redButtonColor, Time.deltaTime);
            }
            
            clickableButtons[i].color = Color.Lerp(clickableButtons[i].color,originalButtonColor, Time.deltaTime);
        }
    }

    private void CheckClickedObject()
    {
        Vector3 mousePos = Input.mousePosition;
        var worldPosition = mainCam.ScreenToWorldPoint(mousePos);
        
        if (Physics.Raycast(playerInputs.vCam.transform.position, worldPosition, out var raycastHit, float.MaxValue))
        {
            Debug.Log("ah3");

            if (raycastHit.transform.TryGetComponent(out Button button))
            {
                Debug.Log("ah2");
            }
        }
    }
}
