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

    private void Start()
    {
        currentDangerScore = UIScoreCounter.Instance.gameScore;
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
        if (UIScoreCounter.Instance.gameScore > currentDangerScore + scoreUntilDanger && currentCaughtTime > 0)
        {
            SetDangerVisual(0.4f, camAmplitudeOnDanger, camFrequencyOnDanger);
            currentCaughtTime -= Time.deltaTime;
        }
        else if(currentCaughtTime <= 0)
        {
            SetDangerVisual(1, camAmplitudeOnCaught, camFrequencyOnCaught);
            
            StartCoroutine(CaughtPlaying(5));
        }
        else
        {
            currentCaughtTime = timeUntilCaught;
            SetDangerVisual(0, camAmplitudeNormal, camFrequencyNormal);
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
        currentDangerScore = UIScoreCounter.Instance.gameScore;
    }

    private IEnumerator CaughtPlaying(float time)
    {
        yield return new WaitForSeconds(time);
        var panelAlpha = UIScoreCounter.Instance.caughtPanel.GetComponent<Image>().color;
        panelAlpha.a = 1;
        UIScoreCounter.Instance.caughtPanel.GetComponent<Image>().color = panelAlpha;
        yield return new WaitForSeconds(secondsBeforeBlackScreenFadeOut);
        PickNewGame(panelAlpha);
    }

    private void PickNewGame(Color panelAlpha)
    {
        SetDangerVisual(0.3f, camAmplitudeNormal, camFrequencyNormal);
        panelAlpha.a = 0;
        foreach (var interactableObject in interactableObjects)
        {
            interactableObject.gameObject.SetActive(false);
        }

        foreach (var choosableObject in choosableObjects)
        {
            choosableObject.gameObject.SetActive(true);
        }
        
        UIScoreCounter.Instance.caughtPanel.GetComponent<Image>().color = panelAlpha;
    }
}
