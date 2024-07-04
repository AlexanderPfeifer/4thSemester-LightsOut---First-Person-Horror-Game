using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MotherBehaviour : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private float timeUntilBlackOrWhiteScreen;
    [SerializeField] private float timeInBlackOrWhiteScreen;
    private float targetWeightMother;
    private float targetWeightPulsating;
    private float targetFrequency;
    private float targetAmplitude;
    private CinemachineBasicMultiChannelPerlin vCamShake;
    private const float TargetCamFov = 20;

    [Header("Volume")] 
    [SerializeField] public Volume motherCatchVolume;
    public GameObject blackScreen;

    [Header("DeathScreen")] 
    [SerializeField] private GameObject firstBulb;
    [SerializeField] private GameObject secondBulb;
    [SerializeField] private GameObject thirdBulb;
    [SerializeField] private GameObject firstBulbBroken;
    [SerializeField] private GameObject secondBulbBroken;
    [SerializeField] private GameObject thirdBulbBroken;
    [SerializeField] private GameObject mother;
    [SerializeField] private Transform motherHand;
    
    public static MotherBehaviour instance;

    private void Awake() => instance = this;

    private void Start()
    {
        vCamShake = PlayerInputs.instance.vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        CamVisualMotherUpdate();
        
        MotherHandPositionUpdate();
    }

    private void MotherHandPositionUpdate()
    {
        if (MotherTimerManager.instance.gameStarted)
        {
            motherHand.gameObject.SetActive(true);
        }

        motherHand.transform.localPosition = new Vector3(motherHand.transform.localPosition.x, motherHand.transform.localPosition.y,
            MotherTimerManager.instance.armPositionCurve.Evaluate(MotherTimerManager.instance.currentTime / MotherTimerManager.instance.timeWhenMotherCatchesPlayer));
        motherHand.GetComponent<Image>().fillAmount = MotherTimerManager.instance.armFilledCurve.Evaluate(MotherTimerManager.instance.currentTime / MotherTimerManager.instance.timeWhenMotherCatchesPlayer);
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
    
    //starts visualization of player being caught
    private IEnumerator PlayerWonCoroutine()
    {
        AudioManager.Instance.Play("LightsOutWin");
        PlayerInputs.instance.canInteract = true;
        MotherTimerManager.instance.pauseGameTime = false;
        MotherTimerManager.instance.gameStarted = false;
        FindObjectOfType<MenuUI>().LoadingScreen(true);
        MotherTimerManager.instance.currentTime -= 20;

        while (PlayerInputs.instance.vCam.m_Lens.FieldOfView > TargetCamFov + 1)
        {
            PlayerInputs.instance.vCam.transform.localRotation = Quaternion.Lerp(PlayerInputs.instance.vCam.transform.localRotation, Quaternion.Euler(0, 0, 0),PlayerInputs.instance.currentInteractableObject.GetComponent<Interaction>().interactableObjectPutAwaySpeed * Time.deltaTime);
            PlayerInputs.instance.vCam.m_Lens.FieldOfView = Mathf.Lerp(PlayerInputs.instance.vCam.m_Lens.FieldOfView, TargetCamFov, Time.deltaTime);
            yield return null;
        }
        
        PlayerInputs.instance.vCam.m_Lens.FieldOfView = TargetCamFov;
        
        MotherTimerManager.instance.diedInScene = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //When player picked a new game, the objects reset from the game on the table to console and mathbook
    private IEnumerator PlayerCaughtCoroutine()
    {
        AudioManager.Instance.Play("LightsOutLose");
        PlayerInputs.instance.canInteract = true;
        StartCoroutine(PlayerInputs.instance.PutDownInteractableCoroutine());
        yield return new WaitForSeconds(timeUntilBlackOrWhiteScreen);
        BlackScreen(1);
        AudioManager.Instance.Stop("Rain");
        MotherTimerManager.instance.currentTime = 0;
        yield return new WaitForSeconds(timeUntilBlackOrWhiteScreen);
        
        switch (MotherTimerManager.instance.currentPlayerTries)
        {
            case 2 :
                firstBulb.SetActive(false);
                firstBulbBroken.SetActive(true);
                mother.GetComponent<Image>().color = new Color(1, 1, 1, 0.039f);
                yield return new WaitForSeconds(timeInBlackOrWhiteScreen);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            case 1 :
                secondBulb.SetActive(false);
                secondBulbBroken.SetActive(true);
                mother.GetComponent<Image>().color = new Color(1, 1, 1, 0.39f);
                mother.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                mother.GetComponent<Transform>().position = new Vector3(350, -277, 0);
                yield return new WaitForSeconds(timeInBlackOrWhiteScreen);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            case 0 :
                thirdBulb.SetActive(false);
                thirdBulbBroken.SetActive(true);
                mother.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                mother.GetComponent<Transform>().position = new Vector3(350, -277, 0);
                yield return new WaitForSeconds(timeInBlackOrWhiteScreen);
                MotherTimerManager.instance.currentPlayerTries = MotherTimerManager.instance.maxPlayerTries;
                MotherTimerManager.instance.diedInScene = true;
                SceneManager.LoadScene(0);
                break;
        }
    }

    //Shortcut to fade black screen
    private void BlackScreen(int panelAlpha)
    {
        blackScreen.GetComponent<CanvasGroup>().alpha = panelAlpha;
    }

    //Lerp the camera constantly to the target values
    private void CamVisualMotherUpdate()
    {
        vCamShake.m_AmplitudeGain = Mathf.Lerp(vCamShake.m_AmplitudeGain, targetAmplitude, Time.deltaTime);
        vCamShake.m_FrequencyGain = Mathf.Lerp(vCamShake.m_FrequencyGain, targetFrequency, Time.deltaTime);
        motherCatchVolume.weight = Mathf.Lerp(motherCatchVolume.weight, targetWeightMother, Time.deltaTime);
    }

    //shortcut to apply visuals to the cam
    public void SetCamVisualCaught(float weight, float camAmplitude, float camFrequency)
    {
        targetWeightMother = weight;
        targetAmplitude = camAmplitude;
        targetFrequency = camFrequency;
    }
}
