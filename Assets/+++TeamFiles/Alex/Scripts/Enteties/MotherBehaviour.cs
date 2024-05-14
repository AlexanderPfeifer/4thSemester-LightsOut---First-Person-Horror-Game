using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MotherBehaviour : MonoBehaviour
{
    [Header("Time")]
    private int currentDangerScore;
    [SerializeField] private int scoreUntilDanger = 3;
    [SerializeField] private float timeUntilCaught = 7.5f;
    [HideInInspector] public float currentCaughtTime;

    [Header("Camera")]
    private float camAmplitudeNormal;
    private float camFrequencyNormal;
    [SerializeField] private List<GameObject> interactableObjects;
    [SerializeField] private List<GameObject> choosableObjects;
    [SerializeField] private float secondsBeforeBlackScreenFadeOut;
    [SerializeField] private float camAmplitudeOnDanger;
    [SerializeField] private float camFrequencyOnDanger;
    [SerializeField] private float camAmplitudeOnCaught;
    [SerializeField] private float camFrequencyOnCaught;
    [SerializeField] private Volume dangerVolume;
    [SerializeField] private CinemachineVirtualCamera vCam;
    private CinemachineBasicMultiChannelPerlin vCamShake;

    [Header("Player")] 
    private PlayerInputs playerInputs;
    private bool isChoosingGame;
    
    private void Start()
    {
        playerInputs = FindObjectOfType<PlayerInputs>();
        currentDangerScore = UIScoreCounter.instance.gameScore;
        vCamShake = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        camAmplitudeNormal = vCamShake.m_AmplitudeGain;
        camFrequencyNormal = vCamShake.m_FrequencyGain;
        currentCaughtTime = timeUntilCaught;
    }

    private void Update()
    {
        TryCatchingPlayer();
    }

    private void TryCatchingPlayer()
    {
        if (isChoosingGame) 
            return;
        
        if (UIScoreCounter.instance.gameScore > currentDangerScore + scoreUntilDanger && currentCaughtTime > 0)
        {
            SetDangerVisual(0.4f, camAmplitudeOnDanger, camFrequencyOnDanger);
            currentCaughtTime -= Time.deltaTime;
        }
        else if(currentCaughtTime <= 0)
        {
            SetDangerVisual(1, camAmplitudeOnCaught, camFrequencyOnCaught);
            
            StartCoroutine(CaughtPlayingVisual(5));

            playerInputs.gotCaught = true;
        }
    }

    private void SetDangerVisual(float weight, float camAmplitude, float camFrequency)
    {
        dangerVolume.weight = Mathf.Lerp(dangerVolume.weight, weight, Time.deltaTime);
        vCamShake.m_AmplitudeGain = Mathf.Lerp(vCamShake.m_AmplitudeGain, camAmplitude, Time.deltaTime);
        vCamShake.m_FrequencyGain = Mathf.Lerp(vCamShake.m_FrequencyGain, camFrequency, Time.deltaTime);
    }

    public void ResetCaughtScore()
    {
        currentDangerScore = UIScoreCounter.instance.gameScore;
        
        SetDangerVisual(0, camAmplitudeNormal, camFrequencyNormal);
        
        currentCaughtTime = timeUntilCaught;
    }

    private IEnumerator CaughtPlayingVisual(float time)
    {
        yield return new WaitForSeconds(time);
        BlackScreenFade(1);
        yield return new WaitForSeconds(secondsBeforeBlackScreenFadeOut);
        PickNewGame();
    }

    public IEnumerator NewGameGotPicked(float time)
    {
        yield return new WaitForSeconds(time);
        isChoosingGame = true;
        BlackScreenFade(1);
        ChangeVisibleInteractbles(true, false);
        yield return new WaitForSeconds(secondsBeforeBlackScreenFadeOut);
        isChoosingGame = false;
        BlackScreenFade(0);
    }

    private void BlackScreenFade(int colorAlpha)
    {
        var panelAlpha = UIScoreCounter.instance.caughtPanel.GetComponent<Image>().color;
        panelAlpha.a = colorAlpha;
        UIScoreCounter.instance.caughtPanel.GetComponent<Image>().color = panelAlpha;
    }
    
    private void PickNewGame()
    {
        ChangeVisibleInteractbles(false, true);
        
        SetDangerVisual(0.3f, camAmplitudeNormal, camFrequencyNormal);
        
        BlackScreenFade(0);

        playerInputs.gotCaught = false;
    }

    private void ChangeVisibleInteractbles(bool interactableActiveState, bool gamesActiveState)
    {
        foreach (var interactableObject in interactableObjects)
        {
            interactableObject.gameObject.SetActive(interactableActiveState);
        }

        foreach (var choosableObject in choosableObjects)
        {
            choosableObject.gameObject.SetActive(gamesActiveState);
        }
    }
}
