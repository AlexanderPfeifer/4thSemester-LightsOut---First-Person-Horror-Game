using System.Collections;
using UnityEngine;

public class FovReset : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(CamReset());
    }
    
    private IEnumerator CamReset()
    {
        while (PlayerInputs.instance.vCam.m_Lens.FieldOfView < 59)
        {
            PlayerInputs.instance.vCam.m_Lens.FieldOfView = Mathf.Lerp(PlayerInputs.instance.vCam.m_Lens.FieldOfView, 60, Time.deltaTime);
            yield return null;
        }

        PlayerInputs.instance.vCam.m_Lens.FieldOfView = 60;

        StartCoroutine(PlayerInputs.instance.PutDownInteractableCoroutine());

        yield return null;
    }
}
