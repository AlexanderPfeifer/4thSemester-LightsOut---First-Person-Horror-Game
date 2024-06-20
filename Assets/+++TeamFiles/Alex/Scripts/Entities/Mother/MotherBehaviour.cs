using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MotherBehaviour : MonoBehaviour
{
    [Header("Camera")]
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

    [Header("Interactbles")]
    [SerializeField] private List<GameObject> interactableObjects;
    [SerializeField] private List<GameObject> choosableObjects;
    [SerializeField] private List<GameObject> mathbookScribbles;
    private int currentMathbookScribble;

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
        SetCamVisual(1f, camAmplitudeOnCaught, camFrequencyOnCaught);
    }
    
    //starts visualization of player being caught
    private IEnumerator PlayerWonCoroutine()
    {
        PlayerInputs.instance.isCaught = true;
        PlayerInputs.instance.holdObjectState = PlayerInputs.HoldObjectState.LayingDown;
        StartCoroutine(PlayerInputs.instance.PutDownInteractableCoroutine());
        SetCamVisual(1f, camAmplitudeOnCaught, camFrequencyOnCaught);
        yield return new WaitForSeconds(timeUntilBlackScreen);
        BlackScreen(1);
        yield return new WaitForSeconds(timeInBlackScreen);
        PlayerInputs.instance.isCaught = false;
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
        PlayerInputs.instance.holdObjectState = PlayerInputs.HoldObjectState.OutOfHand;
        ChangeVisibleInteractblesOnTable(true, false);
        UIScoreCounter.instance.PickedUpBook();
        mathbookScribbles[currentMathbookScribble].SetActive(false);
        currentMathbookScribble++;
        mathbookScribbles[currentMathbookScribble].SetActive(true);
        UIScoreCounter.instance.scoreTextObject.gameObject.SetActive(false);
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
            if(choosableObject.transform.localPosition.y > 0)
            {
                choosableObject.gameObject.GetComponent<Transform>().GetChild(1).gameObject.SetActive(false);
            }

            choosableObject.gameObject.SetActive(gamesOnTable);
        }
    }
}
