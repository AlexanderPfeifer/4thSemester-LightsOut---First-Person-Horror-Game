using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class MotherBehaviour : MonoBehaviour
{
    private int attackScore;
    [SerializeField] private int scoreUntilAttack = 3;
    private float timeBeforeAttack;

    [Header("Visuals")]
    [SerializeField] private float camAmplitudeOnAttack = 4f;
    [SerializeField] private float camFrequencyOnAttack = 2.5f;
    [SerializeField] private Volume attackVolume;
    [SerializeField] private CinemachineVirtualCamera vCam;
    private CinemachineBasicMultiChannelPerlin vCamShake;
    private float camAmplitudeNormal;
    private float camFrequencyNormal;
    
    

    private void Start()
    {
        attackScore = UIScoreCounter.Instance.gameScore;
        vCamShake = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        camAmplitudeNormal = vCamShake.m_AmplitudeGain;
        camFrequencyNormal = vCamShake.m_FrequencyGain;
    }

    private void Update()
    {
        TryCatchingPlayer();
    }

    private void TryCatchingPlayer()
    {
        if (UIScoreCounter.Instance.gameScore > attackScore + scoreUntilAttack)
        {
            attackVolume.weight = Mathf.Lerp(attackVolume.weight, 1, Time.deltaTime);
            vCamShake.m_AmplitudeGain = Mathf.Lerp(vCamShake.m_AmplitudeGain, camAmplitudeOnAttack, Time.deltaTime);
            vCamShake.m_FrequencyGain = Mathf.Lerp(vCamShake.m_FrequencyGain, camFrequencyOnAttack, Time.deltaTime);
        }
        else
        {
            attackVolume.weight = Mathf.Lerp(attackVolume.weight, 0, Time.deltaTime);
            vCamShake.m_AmplitudeGain = Mathf.Lerp(vCamShake.m_AmplitudeGain, camAmplitudeNormal, Time.deltaTime);
            vCamShake.m_FrequencyGain = Mathf.Lerp(vCamShake.m_FrequencyGain, camFrequencyNormal, Time.deltaTime);
        }
    }

    public void ResetAttackScore()
    {
        attackScore = UIScoreCounter.Instance.gameScore;
    }
}
