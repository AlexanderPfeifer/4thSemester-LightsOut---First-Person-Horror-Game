using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MotherBehaviour : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private float timeUntilBlackScreen;
    [SerializeField] private float timeInBlackOrWhiteScreen;
    [SerializeField] public float maxCamAmplitude;
    [SerializeField] public float maxCamFrequency;
    private float targetWeight;
    private float targetFrequency;
    private float targetAmplitude;
    private CinemachineBasicMultiChannelPerlin vCamShake;

    [Header("Volume")] 
    [SerializeField] public Volume motherCatchVolume;

    public static MotherBehaviour instance;

    private void Awake() => instance = this;

    private void Start()
    {
        vCamShake = PlayerInputs.instance.vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        CamVisualUpdate();
    }

    //Puts Down the interactable, is as void so the Put Down coroutine works without stopping
    public void PlayerWon()
    {
        StartCoroutine(PlayerWonCoroutine());
    }

    public void CaughtPlayer()
    {
        StartCoroutine(PlayerCaughtCoroutine());
    }

    public void PlayerLost()
    {
        StartCoroutine(PlayerCaughtThreeTimesCoroutine());
    }
    
    //starts visualization of player being caught
    private IEnumerator PlayerWonCoroutine()
    {
        PlayerInputs.instance.isCaught = true;
        StartCoroutine(PlayerInputs.instance.PutDownInteractableCoroutine());
        //Make screen beautiful
        yield return new WaitForSeconds(timeUntilBlackScreen);
        WhiteOrBlackScreen(1, 1);
        yield return new WaitForSeconds(timeInBlackOrWhiteScreen);
        WhiteOrBlackScreen(1,0);
        PlayerInputs.instance.isCaught = false;
        //Make screen normal again
    }

    //When player picked a new game, the objects reset from the game on the table to console and mathbook
    private IEnumerator PlayerCaughtCoroutine()
    {
        PlayerInputs.instance.isCaught = true;
        yield return new WaitForSeconds(timeUntilBlackScreen);
        WhiteOrBlackScreen(0, 1);
        MotherTimerManager.instance.currentTime = 0;
        yield return new WaitForSeconds(timeInBlackOrWhiteScreen);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private IEnumerator PlayerCaughtThreeTimesCoroutine()
    {
        PlayerInputs.instance.isCaught = true;
        yield return new WaitForSeconds(timeUntilBlackScreen);
        WhiteOrBlackScreen(0, 1);
        MotherTimerManager.instance.currentTime = 0;
        yield return new WaitForSeconds(timeInBlackOrWhiteScreen);
        SceneManager.LoadScene(0);
    }

    //Shortcut to fade blackscreen
    private void WhiteOrBlackScreen(float color, int panelAlpha)
    {
        var wantedAlpha = new Color(color, color, color, panelAlpha);
        
        MotherTimerManager.instance.caughtPanel.GetComponent<Image>().color = wantedAlpha;
    }

    //Lerps the camera constantly to the target values
    private void CamVisualUpdate()
    {
        vCamShake.m_AmplitudeGain = Mathf.Lerp(vCamShake.m_AmplitudeGain, targetAmplitude, Time.deltaTime);
        vCamShake.m_FrequencyGain = Mathf.Lerp(vCamShake.m_FrequencyGain, targetFrequency, Time.deltaTime);
        motherCatchVolume.weight = Mathf.Lerp(motherCatchVolume.weight, targetWeight, Time.deltaTime);
    }
    
    //shortcut to apply visuals to the cam
    public void SetCamVisualCaught(float weight, float camAmplitude, float camFrequency)
    {
        targetWeight = weight;
        targetAmplitude = camAmplitude;
        targetFrequency = camFrequency;
    }
}
