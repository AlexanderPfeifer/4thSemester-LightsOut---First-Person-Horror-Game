using System;
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
    [SerializeField] private CinemachineVirtualCamera vCam;
    private CinemachineBasicMultiChannelPerlin vCamShake;
    private float t;

    [Header("Volume")] 
    [SerializeField] private Volume motherCatchVolume;

    [Header("Player")] 
    private PlayerInputs playerInputs;
    [SerializeField] private float lerpSpeed = 0.5f;
    private bool canPlayCaughtVisual = true;

    private void Start()
    {
        playerInputs = FindObjectOfType<PlayerInputs>();
        vCamShake = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if (UIScoreCounter.instance.currentCaughtTime <= 0 && canPlayCaughtVisual)
        {
            PlayCaughtVisual();
            canPlayCaughtVisual = false;
        }
    }

    private void PlayCaughtVisual()
    {
        StartCoroutine(CaughtPlayingVisual());
    }

    private IEnumerator CaughtPlayingVisual()
    {
        playerInputs.isCaught = true;
        SetCamVisual(1f, camAmplitudeOnCaught, camFrequencyOnCaught);
        yield return new WaitForSeconds(timeUntilBlackScreen);
        BlackScreen(1);
        yield return new WaitForSeconds(timeInBlackScreen);
        playerInputs.isCaught = false;
        PickNewGame();
    }
    
    private void PickNewGame()
    {
        SetCamVisual(0.3f, camAmplitudeNormal, camFrequencyNormal);
        BlackScreen(0);
        ChangeVisibleInteractblesOnTable(false, true);
    }
    
    public IEnumerator NewGameGotPicked(float timeBeforeBlackScreen)
    {
        yield return new WaitForSeconds(timeBeforeBlackScreen);
        BlackScreen(1);
        yield return new WaitForSeconds(timeInBlackScreen);
        BlackScreen(0);
        SetCamVisual(0f, camAmplitudeNormal, camFrequencyNormal);
        playerInputs.isCaught = false;
        playerInputs.holdObjectState = PlayerInputs.HoldObjectState.OutOfHand;
        ChangeVisibleInteractblesOnTable(true, false);
        UIScoreCounter.instance.ResetCaughtScore();
        canPlayCaughtVisual = true;
    }

    private void BlackScreen(int panelAlpha)
    {
        var wantedAlpha = new Color(0, 0, 0, panelAlpha);
        
        UIScoreCounter.instance.caughtPanel.GetComponent<Image>().color = wantedAlpha;
    }

    public void SetCamVisual(float weight, float camAmplitude, float camFrequency)
    {
        t = 1.1f;
        StartCoroutine(SetCamVisualCoroutine(weight, camAmplitude, camFrequency));
    }
    
    private IEnumerator SetCamVisualCoroutine(float weight, float camAmplitude, float camFrequency)
    {
        t = 0f;
        
        while (t < 1f) 
        {
            t += Time.deltaTime * (1f / lerpSpeed);
            vCamShake.m_AmplitudeGain = Mathf.Lerp(vCamShake.m_AmplitudeGain, camAmplitude, t);
            vCamShake.m_FrequencyGain = Mathf.Lerp(vCamShake.m_FrequencyGain, camFrequency, t);
            motherCatchVolume.weight = Mathf.Lerp(motherCatchVolume.weight, weight, t);
            yield return null;
        }
    }
    
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
