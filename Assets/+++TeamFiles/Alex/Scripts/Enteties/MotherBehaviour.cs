using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MotherBehaviour : MonoBehaviour
{
    [Header("Camera")]
    private float camAmplitudeNormal;
    private float camFrequencyNormal;
    [SerializeField] private List<GameObject> interactableObjects;
    [SerializeField] private List<GameObject> choosableObjects;
    [SerializeField] private float timeUntilBlackScreen;
    [SerializeField] private float timeInBlackScreen;
    [SerializeField] private float camAmplitudeOnDanger;
    [SerializeField] private float camFrequencyOnDanger;
    [SerializeField] private float camAmplitudeOnCaught;
    [SerializeField] private float camFrequencyOnCaught;
    [SerializeField] private CinemachineVirtualCamera vCam;
    private CinemachineBasicMultiChannelPerlin vCamShake;

    [Header("Volume")] 
    [SerializeField] private Volume motherCatchVolume;
    
    [Header("Score")]
    private int currentDangerScore;
    [SerializeField] private int scoreUntilDanger = 3;
    
    [Header("CaughtTime")]
    [SerializeField] private float timeUntilCaught = 7.5f;
    [HideInInspector] public float currentCaughtTime;

    [Header("Player")] 
    private PlayerInputs playerInputs;
    private bool isChoosingGame;

    private void Start()
    {
        playerInputs = FindObjectOfType<PlayerInputs>();
        vCamShake = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        currentCaughtTime = timeUntilCaught;
    }

    private void Update()
    {
        CatchingPlayer();
    }

    private void CatchingPlayer()
    {
        if (isChoosingGame) 
            return;
        
        if (UIScoreCounter.instance.gameScore > currentDangerScore + scoreUntilDanger && currentCaughtTime > 0)
        {
            StartCoroutine(SetCamVisual(0.3f, camAmplitudeOnDanger, camFrequencyOnDanger, 3));
            currentCaughtTime -= Time.deltaTime;
        }
        else if(currentCaughtTime <= 0)
        {
            StartCoroutine(CaughtPlayingVisual());
        }
    }

    private IEnumerator CaughtPlayingVisual()
    {
        playerInputs.isCaught = true;
        isChoosingGame = true;
        StartCoroutine(SetCamVisual(1f, camAmplitudeOnCaught, camFrequencyOnCaught, 3));
        yield return new WaitForSeconds(timeUntilBlackScreen);
        BlackScreen(1);
        yield return new WaitForSeconds(timeInBlackScreen);
        playerInputs.isCaught = false;
        PickNewGame();
    }
    
    private void PickNewGame()
    {
        ChangeVisibleInteractblesOnTable(false, true);
        StartCoroutine(SetCamVisual(0.3f, camAmplitudeNormal, camFrequencyNormal, 3));
        BlackScreen(0);
    }
    
    public IEnumerator NewGameGotPicked(float timeBeforeBlackScreen)
    {
        yield return new WaitForSeconds(timeBeforeBlackScreen);
        BlackScreen(1);
        ChangeVisibleInteractblesOnTable(true, false);
        yield return new WaitForSeconds(timeInBlackScreen);
        isChoosingGame = false;
        playerInputs.isCaught = false;
        ResetCaughtScore();
        BlackScreen(0);
    }

    public void ResetCaughtScore()
    {
        currentDangerScore = UIScoreCounter.instance.gameScore;
        
        StartCoroutine(SetCamVisual(0, camAmplitudeNormal, camFrequencyNormal, 1));
        
        currentCaughtTime = timeUntilCaught;
    }

    private void BlackScreen(int panelAlpha)
    {
        var wantedAlpha = new Color(0, 0, 0, panelAlpha);
        
        UIScoreCounter.instance.caughtPanel.GetComponent<Image>().color = wantedAlpha;
    }
    
    private IEnumerator SetCamVisual(float weight, float camAmplitude, float camFrequency, float fadeTime)
    {
        var elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            vCamShake.m_AmplitudeGain = Mathf.Lerp(vCamShake.m_AmplitudeGain, camAmplitude, (elapsedTime / fadeTime));
            vCamShake.m_FrequencyGain = Mathf.Lerp(vCamShake.m_FrequencyGain, camFrequency, (elapsedTime / fadeTime));
            motherCatchVolume.weight = Mathf.Lerp(motherCatchVolume.weight, weight, (elapsedTime / fadeTime));
            elapsedTime += Time.deltaTime;
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
