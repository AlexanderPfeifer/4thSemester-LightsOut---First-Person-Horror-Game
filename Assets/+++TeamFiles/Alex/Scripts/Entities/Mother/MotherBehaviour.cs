using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MotherBehaviour : MonoBehaviour
{
    [Header("Camera")] 
    private const float TimeUntilBlack = 4;
    private const float TimeInBlack = 4;
    private float targetWeightMotherCatchVolume;
    private float targetCamFrequency;
    private float targetCamAmplitude;
    private CinemachineBasicMultiChannelPerlin vCamShake;
    private const float TargetCamFov = 20;

    [Header("Volume")] 
    [SerializeField] public Volume motherCaughtPlayerVolume;
    public GameObject blackScreen;

    [Header("DeathScreen")]
    [SerializeField] private GameObject firstBulb;
    [SerializeField] private GameObject secondBulb;
    [SerializeField] private GameObject thirdBulb;
    [SerializeField] private Transform motherHand;
    [SerializeField] private Transform motherHandDeathScreen;
    [SerializeField] private Transform motherHandOpenDeathScreen;
    [SerializeField] private Transform motherHandGradient;
    [HideInInspector] public bool playerCaught;
    
    public static MotherBehaviour instance;

    private void Awake() => instance = this;

    private void Start()
    {
        switch (MotherTimerManager.Instance.currentScene)
        {
            case 0 :
                FindObjectOfType<Book>().transform.GetChild(1).GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", MotherTimerManager.Instance.firstTexture);
                break;
            case 1 :
                FindObjectOfType<Book>().transform.GetChild(1).GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", MotherTimerManager.Instance.secondTexture);
                break;
            case 2 :
                FindObjectOfType<Book>().transform.GetChild(1).GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", MotherTimerManager.Instance.thirdTexture);
                break;
            case 3 :
                FindObjectOfType<Book>().transform.GetChild(1).GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", MotherTimerManager.Instance.fourthTexture);
                break;
        }
        
        vCamShake = PlayerInputs.Instance.vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        CamVisualMotherUpdate();
        
        PlayerCaughtCheck();
    }

    #region PlayerCaught

    //Checks if player died and then changes camera rotation to the door
    private void PlayerCaughtCheck()
    {
        if (!playerCaught) 
            return;
        
        motherHand.gameObject.SetActive(true);

        StartCoroutine(PlayerInputs.Instance.PutDownInteractableCoroutine());

        Vector3 direction = motherHand.transform.position - PlayerInputs.Instance.vCam.transform.position;
        Quaternion toRotation = Quaternion.LookRotation(direction, transform.up);
        PlayerInputs.Instance.vCam.transform.localRotation = Quaternion.Lerp(PlayerInputs.Instance.vCam.transform.rotation, toRotation, PlayerInputs.Instance.currentInteractableObject.GetComponent<Interaction>().interactableObjectPutAwaySpeed * Time.deltaTime);
            
        PlayerInputs.Instance.vCam.m_Lens.FieldOfView = Mathf.Lerp(PlayerInputs.Instance.vCam.m_Lens.FieldOfView, 50, Time.deltaTime);
    }
    
    public void CaughtPlayer()
    {
        playerCaught = true;
        StartCoroutine(PlayerCaughtCoroutine());
    }

    //When player got caught the death screen activates and the hand and light bulbs are placed where they need to be depending on how many lives are left and the according to that, the SFX are played
    private IEnumerator PlayerCaughtCoroutine()
    {
        if (FindObjectOfType<PapanicePizza>())
        {
            FindObjectOfType<PapanicePizza>().runTimer = false;
        }
        
        MotherTimerManager.Instance.gameStarted = false;
        PlayerInputs.Instance.canInteract = false;

        switch (MotherTimerManager.Instance.currentPlayerTries)
        {
            case 1 :
                firstBulb.SetActive(false);
                var motherHandDeathScreenTransform = motherHandDeathScreen.transform;
                motherHandDeathScreenTransform.localPosition = new Vector3(-715, 80, 0);
                motherHandDeathScreenTransform.localScale = new Vector3(-.3f, .3f, .3f);
                break;
            case 0 :
                firstBulb.SetActive(false);
                secondBulb.SetActive(false);
                var motherHandDeathScreenTransformZeroLives = motherHandDeathScreen.transform;
                motherHandDeathScreenTransformZeroLives.localPosition = new Vector3(541,-412,0);
                motherHandDeathScreenTransformZeroLives.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
        }
        
        yield return new WaitForSeconds(TimeUntilBlack);
        
        BlackScreen(1);

        foreach (var sound in AudioManager.Instance.sounds)
        {
            AudioManager.Instance.Stop(sound.name);
        }
        
        MotherTimerManager.Instance.currentTime = 0;
        
        AudioManager.Instance.Play("TensionBuildUpOnDeath");
        yield return new WaitUntil(() => AudioManager.Instance.IsPlaying("TensionBuildUpOnDeath") == false);
        
        AudioManager.Instance.Play("BreakingBulb");

        switch (MotherTimerManager.Instance.currentPlayerTries)
        {
            case 2 :
                firstBulb.SetActive(false);
                motherHandDeathScreen.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                motherHandOpenDeathScreen.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                var motherHandGradientPosition = motherHandGradient.localPosition;
                motherHandGradientPosition = new Vector3(0, motherHandGradientPosition.y, motherHandGradientPosition.z);
                motherHandGradient.localPosition = motherHandGradientPosition;
                
                yield return new WaitForSeconds(TimeInBlack);
                
                MotherTimerManager.Instance.diedInScene = true;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            case 1 :
                secondBulb.SetActive(false);
                motherHandDeathScreen.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                motherHandOpenDeathScreen.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                var motherHandGradientPositionTwoLives = motherHandGradient.localPosition;
                motherHandGradientPositionTwoLives = new Vector3(0, motherHandGradientPositionTwoLives.y, motherHandGradientPositionTwoLives.z);
                motherHandGradient.localPosition = motherHandGradientPositionTwoLives;
                
                yield return new WaitForSeconds(TimeInBlack);
                
                MotherTimerManager.Instance.diedInScene = true;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            case 0 :
                thirdBulb.SetActive(false);
                motherHandDeathScreen.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                motherHandOpenDeathScreen.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                var motherHandGradientZeroLives = motherHandGradient.localPosition;
                motherHandGradientZeroLives = new Vector3(0, motherHandGradientZeroLives.y, motherHandGradientZeroLives.z);
                motherHandGradient.localPosition = motherHandGradientZeroLives;
                
                yield return new WaitForSeconds(TimeInBlack);
                
                MotherTimerManager.Instance.currentPlayerTries = MotherTimerManager.Instance.maxPlayerTries;
                MotherTimerManager.Instance.diedInScene = true;
                SceneManager.LoadScene(0);
                break;
        }
    }
    
    #endregion
    
    #region PlayerWin

    //Puts Down the interactable, is as void so the Put Down coroutine works without stopping
    public void PlayerWin()
    {
        StartCoroutine(PlayerWinCoroutine());
    }
    
    //zooming into the loading screen to have a seamless transition to the next scene, playing winning sfx 
    private IEnumerator PlayerWinCoroutine()
    {
        AudioManager.Instance.Play("LightsOutWin");
        PlayerInputs.Instance.canInteract = false;
        MotherTimerManager.Instance.pauseGameTime = false;
        MotherTimerManager.Instance.gameStarted = false;
        FindObjectOfType<MenuUI>().LoadingScreen(true);
        MotherTimerManager.Instance.currentTime -= 20;

        MotherTimerManager.Instance.currentScene++;

        while (PlayerInputs.Instance.vCam.m_Lens.FieldOfView > TargetCamFov + 1)
        {
            PlayerInputs.Instance.vCam.transform.localRotation = Quaternion.Lerp(PlayerInputs.Instance.vCam.transform.localRotation, Quaternion.Euler(0, 0, 0),PlayerInputs.Instance.currentInteractableObject.GetComponent<Interaction>().interactableObjectPutAwaySpeed * Time.deltaTime);
            PlayerInputs.Instance.vCam.m_Lens.FieldOfView = Mathf.Lerp(PlayerInputs.Instance.vCam.m_Lens.FieldOfView, TargetCamFov, Time.deltaTime);
            yield return null;
        }

        MotherTimerManager.Instance.diedInScene = false;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    #endregion

    #region Shortcuts

    //Shortcut to fade black screen
    private void BlackScreen(int panelAlpha)
    {
        blackScreen.GetComponent<CanvasGroup>().alpha = panelAlpha;
    }

    //Lerp the camera constantly to the target values
    private void CamVisualMotherUpdate()
    {
        vCamShake.m_AmplitudeGain = Mathf.Lerp(vCamShake.m_AmplitudeGain, targetCamAmplitude, Time.deltaTime);
        vCamShake.m_FrequencyGain = Mathf.Lerp(vCamShake.m_FrequencyGain, targetCamFrequency, Time.deltaTime);
        motherCaughtPlayerVolume.weight = Mathf.Lerp(motherCaughtPlayerVolume.weight, targetWeightMotherCatchVolume, Time.deltaTime);
    }

    //shortcut to apply visuals to the cam
    public void SetCamVisualCaught(float weight, float camAmplitude, float camFrequency)
    {
        targetWeightMotherCatchVolume = weight;
        targetCamAmplitude = camAmplitude;
        targetCamFrequency = camFrequency;
    }

    #endregion
}
