using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MotherBehaviour : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private List<GameObject> interactableObjects;
    [SerializeField] private List<GameObject> choosableObjects;
    [SerializeField] private float timeUntilBlackScreen;
    [SerializeField] private float timeInBlackScreen;
    [HideInInspector] public float camAmplitudeNormal;
    [HideInInspector] public float camFrequencyNormal;
    [SerializeField] public float camAmplitudeOnDanger;
    [SerializeField] public float camFrequencyOnDanger;
    [SerializeField] private float camAmplitudeOnCaught;
    [SerializeField] private float camFrequencyOnCaught;
    private float targetWeight;
    private float targetFrequency;
    private float targetAmplitude;
    private CinemachineBasicMultiChannelPerlin vCamShake;

    [Header("Volume")] 
    [SerializeField] private Volume motherCatchVolume;

    [Header("Player")] 
    private PlayerInputs playerInputs;
    public bool canPlayCaughtVisual = true;

    private void Start()
    {
        playerInputs = FindObjectOfType<PlayerInputs>();

        vCamShake = playerInputs.vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        CheckCatchTime();
        
        CamVisualUpdate();
    }

    //Checks if the time has expired, if yes, catches the player and lets the player choose new game
    private void CheckCatchTime()
    {
        if (UIScoreCounter.instance.currentCatchTime <= 0 && canPlayCaughtVisual)
        {
            CaughtPlayer();
            canPlayCaughtVisual = false;
        }
    }
    
    //Puts Down the interactable, is as void so the Put Down coroutine works without stopping
    private void CaughtPlayer()
    {
        StartCoroutine(CaughtPlayerCoroutine());
    }

    //starts visualization of player being caught
    private IEnumerator CaughtPlayerCoroutine()
    {
        playerInputs.isCaught = true;
        playerInputs.holdObjectState = PlayerInputs.HoldObjectState.LayingDown;
        StartCoroutine(playerInputs.PutDownInteractableCoroutine());
        SetCamVisual(1f, camAmplitudeOnCaught, camFrequencyOnCaught);
        yield return new WaitForSeconds(timeUntilBlackScreen);
        BlackScreen(1);
        yield return new WaitForSeconds(timeInBlackScreen);
        playerInputs.isCaught = false;
        SetCamVisual(0.3f, camAmplitudeNormal, camFrequencyNormal);
        BlackScreen(0);
        ChangeVisibleInteractblesOnTable(false, true);
    }

    //When player picked a new game, the objects reset from the game on the table to console and mathbook
    public IEnumerator PickedNewGame(float timeBeforeBlackScreen)
    {
        yield return new WaitForSeconds(timeBeforeBlackScreen);
        BlackScreen(1);
        yield return new WaitForSeconds(timeInBlackScreen);
        BlackScreen(0);
        SetCamVisual(0f, camAmplitudeNormal, camFrequencyNormal);
        playerInputs.holdObjectState = PlayerInputs.HoldObjectState.OutOfHand;
        ChangeVisibleInteractblesOnTable(true, false);
        UIScoreCounter.instance.ResetNeededCatchScore();
        UIScoreCounter.instance.scoreTextObject.gameObject.SetActive(false);
        canPlayCaughtVisual = true;
    }

    //Shortcut to fade blackscreen
    private void BlackScreen(int panelAlpha)
    {
        var wantedAlpha = new Color(0, 0, 0, panelAlpha);
        
        UIScoreCounter.instance.caughtPanel.GetComponent<Image>().color = wantedAlpha;
    }

    //Lerps the camera constantly to the target values
    private void CamVisualUpdate()
    {
        vCamShake.m_AmplitudeGain = Mathf.Lerp(vCamShake.m_AmplitudeGain, targetAmplitude, Time.deltaTime);
        vCamShake.m_FrequencyGain = Mathf.Lerp(vCamShake.m_FrequencyGain, targetFrequency, Time.deltaTime);
        motherCatchVolume.weight = Mathf.Lerp(motherCatchVolume.weight, targetWeight, Time.deltaTime);
    }
    
    //shortcut to apply visuals to the cam
    public void SetCamVisual(float weight, float camAmplitude, float camFrequency)
    {
        targetWeight = weight;
        targetAmplitude = camAmplitude;
        targetFrequency = camFrequency;
    }

    //A shortcut to loop through every object on the table and activate or deactivating it
    private void ChangeVisibleInteractblesOnTable(bool interactblesOnTable, bool gamesOnTable)
    {
        foreach (var interactableObject in interactableObjects)
        {
            interactableObject.gameObject.SetActive(interactblesOnTable);
        }

        foreach (var choosableObject in choosableObjects)
        {
            choosableObject.gameObject.SetActive(gamesOnTable);
        }
    }
}
