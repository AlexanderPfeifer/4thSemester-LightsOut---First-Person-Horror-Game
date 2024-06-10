using UnityEngine;
using UnityEngine.UI;

public class ButtonInteraction : MonoBehaviour
{
    public static ButtonInteraction instance;

    public bool canInteract = true;

    public Button currentSelectedButton;
    private Button lastSelectedButton;

    //Singleton
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        CheckIfClickedObjectsMatch();
    }
    
    //Checks if the objects that were displayed match with the objects that were pressed
    private void CheckIfClickedObjectsMatch()
    {
        if (Physics.Raycast(PlayerInputs.instance.vCam.transform.position, PlayerInputs.instance.vCam.transform.forward, out var raycastHit, float.MaxValue) && canInteract)
        {
            if (raycastHit.transform.TryGetComponent(out Button button))
            {
                if (lastSelectedButton != null)
                {
                    SetSelectedButtonColor(lastSelectedButton, 1,1,1,1);
                }
                currentSelectedButton = button;
                lastSelectedButton = currentSelectedButton;
                SetSelectedButtonColor(currentSelectedButton, 1,1,1,0.5f);

                if (Input.GetMouseButtonDown(0))
                {
                    SetSelectedButtonColor(currentSelectedButton, 1,1,1,0.2f);
                    currentSelectedButton.onClick.Invoke();
                }
            }
        }
        else
        {
            if (currentSelectedButton != null)
            {
                SetSelectedButtonColor(currentSelectedButton, 1,1,1,1);
                currentSelectedButton = null;
            }
        }
    }

    //A shortcut to set the color of a button to the color I need
    public void SetSelectedButtonColor(Button selectedButton, float r, float b, float g, float a)
    {
        var selectedButtonColors = selectedButton.colors;
        selectedButtonColors.normalColor = new Color(r, b, g, a);

        selectedButton.colors = selectedButtonColors;
    }
}
