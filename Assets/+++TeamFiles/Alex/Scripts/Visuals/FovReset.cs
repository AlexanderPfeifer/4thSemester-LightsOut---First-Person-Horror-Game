using System.Collections;
using UnityEngine;

public class FovReset : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(CamReset());
    }
    
    //Resets camera fox after player got caught and depending on if player died, makes the game play instantly instead of playing after taking the book
    private IEnumerator CamReset()
    {
        PlayerInputs.Instance.canInteract = false;
            
        while (PlayerInputs.Instance.vCam.m_Lens.FieldOfView < 59)
        {
            PlayerInputs.Instance.vCam.m_Lens.FieldOfView = Mathf.Lerp(PlayerInputs.Instance.vCam.m_Lens.FieldOfView, 60, Time.deltaTime * 1.5f);
            yield return null;
        }

        PlayerInputs.Instance.vCam.m_Lens.FieldOfView = 60;
        
        if (!MotherTimerManager.Instance.diedInScene)
        {
            PlayerInputs.Instance.canInteract = true;

            StartCoroutine(PlayerInputs.Instance.PutDownInteractableCoroutine());
        }
        else
        {
            FindObjectOfType<MenuUI>().LoadingScreen(false);
            
            PlayerInputs.Instance.currentInteractableObject.GetComponent<Collider>().enabled = false;

            FindObjectOfType<UIInteraction>().canInteract = true;
            
            PlayerInputs.Instance.canInteract = true;
            
            FindObjectOfType<Lighting>().blackMat.color = new Color(0, 0, 0);
        }
    }
}
