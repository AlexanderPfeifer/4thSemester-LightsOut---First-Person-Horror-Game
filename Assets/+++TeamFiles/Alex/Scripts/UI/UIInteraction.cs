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

        CheckInteractionState();
    }

    //checks if ui can be interacted with or not depending on if interactable is put away or not
    private void CheckInteractionState()
    {
        if (PlayerInputs.Instance.holdObjectState == PlayerInputs.HoldObjectState.LiftingUp)
        {
            canInteract = true;
        }
        else if (PlayerInputs.Instance.holdObjectState == PlayerInputs.HoldObjectState.LayingDown)
        {
            canInteract = false;
        }
    }

    #region CheckInLookDirection

    //Checks if toggle can be interacted with in look direction and changes the color of sprite and text and invokes on click
    private void CheckIfToggleInLookDirection()
    {
        if (Physics.Raycast(PlayerInputs.Instance.vCam.transform.position, PlayerInputs.Instance.vCam.transform.forward, out var raycastHit, float.MaxValue) && canInteract)
        {
            if (raycastHit.transform.TryGetComponent(out Toggle toggle))
            {
                currentSelectedToggle = toggle;
                
                SetSelectedToggleColor(currentSelectedToggle, currentSelectedToggle.colors.highlightedColor.r,
                    currentSelectedToggle.colors.highlightedColor.b, currentSelectedToggle.colors.highlightedColor.g, currentSelectedToggle.colors.highlightedColor.a, .2f, .2f, .2f);

                if (Input.GetMouseButton(0))
                {
                    SetSelectedToggleColor(toggle, currentSelectedToggle.colors.pressedColor.r, 
                        currentSelectedToggle.colors.pressedColor.b,currentSelectedToggle.colors.pressedColor.g, 1, .2f, .2f, .2f);
                }
                else if(Input.GetMouseButtonUp(0))
                {
                    currentSelectedToggle.isOn = !currentSelectedToggle.isOn;
                }
            }
        }
        else if (currentSelectedToggle != null)
        {
            SetSelectedToggleColor(currentSelectedToggle, 1, 1, 1, 1, 1, 1, 1);
            currentSelectedToggle = null;
        }
    }

    //Checks if button can be interacted with in look direction and changes the color of sprite and text and invokes on click
    private void CheckIfButtonInLookDirection()
    {
        if (Physics.Raycast(PlayerInputs.Instance.vCam.transform.position, PlayerInputs.Instance.vCam.transform.forward, out var raycastHit, float.MaxValue) && canInteract)
        {
            if (raycastHit.transform.TryGetComponent(out Button button))
            {
                if (lastSelectedButton != null)
                {
                    SetSelectedButtonColor(lastSelectedButton, 1, 1, 1 ,1,  1,1,1);
                }
                
                currentSelectedButton = button;
                lastSelectedButton = currentSelectedButton;
                
                SetSelectedButtonColor(currentSelectedButton, currentSelectedButton.colors.highlightedColor.r,
                    currentSelectedButton.colors.highlightedColor.b,currentSelectedButton.colors.highlightedColor.g, currentSelectedButton.colors.highlightedColor.a, .2f, .2f, .2f);
                
                if (Input.GetMouseButton(0))
                {
                    SetSelectedButtonColor(currentSelectedButton, currentSelectedButton.colors.pressedColor.r, 
                        currentSelectedButton.colors.pressedColor.b,currentSelectedButton.colors.pressedColor.g, currentSelectedButton.colors.pressedColor.a, .2f, .2f, .2f);
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
            SetSelectedButtonColor(currentSelectedButton, 1, 1, 1, 1, 1, 1, 1);
            currentSelectedButton = null;
        }
    }

    #endregion

    #region Shortcut

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
    
    //A shortcut to set the color of a toggle to the color I need
    private void SetSelectedToggleColor(Toggle selectedToggle, float r, float b, float g, float a, float rText, float bText, float gText)
    {
        var selectedButtonColors = selectedToggle.colors;
        selectedButtonColors.normalColor = new Color(r, b, g, a);
        selectedToggle.colors = selectedButtonColors;

        if (selectedToggle.GetComponentInChildren<TextMeshProUGUI>())
        {
            selectedToggle.GetComponentInChildren<TextMeshProUGUI>().color = new Color(rText, bText, gText);
        }
    }

    #endregion
}
