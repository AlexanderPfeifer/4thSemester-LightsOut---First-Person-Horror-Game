using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInteraction : MonoBehaviour
{
    public static UIInteraction instance;
    public bool canInteract = true;

    [HideInInspector] public Button currentSelectedButton;
    [HideInInspector] public Button lastSelectedButton;
    [HideInInspector] public Toggle currentSelectedToggle;

    //Singleton
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        CheckIfButtonInLookDirection();
        
        CheckIfToggleInLookDirection();

        if (PlayerInputs.instance.holdObjectState == PlayerInputs.HoldObjectState.LiftingUp)
        {
            canInteract = true;
        }
        else if (PlayerInputs.instance.holdObjectState == PlayerInputs.HoldObjectState.LayingDown)
        {
            canInteract = false;
        }
    }

    private void CheckIfToggleInLookDirection()
    {
        if (Physics.Raycast(PlayerInputs.instance.vCam.transform.position, PlayerInputs.instance.vCam.transform.forward, out var raycastHit, float.MaxValue) && canInteract)
        {
            if (raycastHit.transform.TryGetComponent(out Toggle toggle))
            {
                currentSelectedToggle = toggle;
                
                SetSelectedToggleColor(currentSelectedToggle, currentSelectedToggle.colors.highlightedColor.r,
                    currentSelectedToggle.colors.highlightedColor.b, currentSelectedToggle.colors.highlightedColor.g, currentSelectedToggle.colors.highlightedColor.a, .2f, .2f, .2f);

                if (Input.GetMouseButton(0))
                {
                    SetSelectedToggleColor(toggle, currentSelectedToggle.colors.pressedColor.r, 
                        currentSelectedToggle.colors.pressedColor.b,currentSelectedToggle.colors.pressedColor.g, 1, 1, 1, 1);
                }
                else if(Input.GetMouseButtonUp(0))
                {
                    currentSelectedToggle.isOn = !currentSelectedToggle.isOn;
                }
            }
        }
        else if (currentSelectedToggle != null)
        {
            SetSelectedToggleColor(currentSelectedToggle, 1, 1, 1, 1, 1f, 1f, 1f);
            currentSelectedToggle = null;
        }
    }

    //Checks if the objects that were displayed match with the objects that were pressed
    private void CheckIfButtonInLookDirection()
    {
        if (Physics.Raycast(PlayerInputs.instance.vCam.transform.position, PlayerInputs.instance.vCam.transform.forward, out var raycastHit, float.MaxValue) && canInteract)
        {
            if (raycastHit.transform.TryGetComponent(out Button button))
            {
                if (lastSelectedButton != null)
                {
                    SetSelectedButtonColor(lastSelectedButton, 1, 1, 1 ,1,  .2f, .2f, .2f);
                }
                
                currentSelectedButton = button;
                lastSelectedButton = currentSelectedButton;
                
                SetSelectedButtonColor(currentSelectedButton, currentSelectedButton.colors.highlightedColor.r,
                    currentSelectedButton.colors.highlightedColor.b,currentSelectedButton.colors.highlightedColor.g, currentSelectedButton.colors.highlightedColor.a, 1f, 1f, 1f);
                
                if (Input.GetMouseButton(0))
                {
                    SetSelectedButtonColor(currentSelectedButton, currentSelectedButton.colors.pressedColor.r, 
                        currentSelectedButton.colors.pressedColor.b,currentSelectedButton.colors.pressedColor.g, currentSelectedButton.colors.pressedColor.a, 1, 1, 1);
                }
                else if(Input.GetMouseButtonUp(0))
                {
                    if (currentSelectedButton != null)
                    {
                        currentSelectedButton.onClick.Invoke();
                    }
                }
            }
        }
        else if (currentSelectedButton != null)
        {
            SetSelectedButtonColor(currentSelectedButton, 1, 1, 1, 1, .2f, .2f, .2f);
            currentSelectedButton = null;
        }
    }

    //A shortcut to set the color of a button to the color I need
    public void SetSelectedButtonColor(Button selectedButton, float r, float b, float g, float a, float rText, float bText, float gText)
    {
        var selectedButtonColors = selectedButton.colors;
        selectedButtonColors.normalColor = new Color(r, b, g, a);
        selectedButton.colors = selectedButtonColors;

        if (selectedButton.GetComponentInChildren<TextMeshProUGUI>())
        {
            selectedButton.GetComponentInChildren<TextMeshProUGUI>().color = new Color(rText, bText, gText);
        }
    }
    
    private void SetSelectedToggleColor(Toggle selectedButton, float r, float b, float g, float a, float rText, float bText, float gText)
    {
        var selectedButtonColors = selectedButton.colors;
        selectedButtonColors.normalColor = new Color(r, b, g, a);
        selectedButton.colors = selectedButtonColors;

        if (selectedButton.GetComponentInChildren<TextMeshProUGUI>())
        {
            selectedButton.GetComponentInChildren<TextMeshProUGUI>().color = new Color(rText, bText, gText);
        }
    }
}
