using UnityEngine;
using UnityEngine.UI;

public class ButtonInteraction : MonoBehaviour
{
    public static ButtonInteraction instance;

    public bool canInteract = true;

    public Button currentSelectedButton;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        CheckClickedObject();
    }
    
    private void CheckClickedObject()
    {
        if (Physics.Raycast(PlayerInputs.instance.vCam.transform.position, PlayerInputs.instance.vCam.transform.forward, out var raycastHit, float.MaxValue) && canInteract)
        {
            if (raycastHit.transform.TryGetComponent(out Button button))
            {
                currentSelectedButton = button;
                SetSelectedButtonColor(currentSelectedButton, 1,1,1,0.5f);

                if (Input.GetMouseButtonDown(0))
                {
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

    public void SetSelectedButtonColor(Button selectedButton, float r, float b, float g, float a)
    {
        var selectedButtonColors = selectedButton.colors;
        selectedButtonColors.normalColor = new Color(r, b, g, a);

        selectedButton.colors = selectedButtonColors;
    }
}
