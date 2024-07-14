using UnityEngine;

public class MotherTimerManager : MonoBehaviour
{
    [Header("Book")] 
    public Texture firstTexture;
    public Texture secondTexture;
    public Texture thirdTexture;
    public Texture fourthTexture;
     
    [Header("PlayerTries")] 
    [SerializeField] public int maxPlayerTries;
    [SerializeField] public int currentPlayerTries;
    [HideInInspector] public bool diedInScene;
    [HideInInspector] public int currentScene;
    
    [Header("Time")] 
    [HideInInspector] public bool gameStarted;
    [SerializeField] public float currentTime;
    [SerializeField] public float timeWhenMotherCatchesPlayer;
    [SerializeField] private AnimationCurve motherVisualCurve;
    [SerializeField] private AnimationCurve camFrequencyVisualCurve;
    [HideInInspector] public bool pauseGameTime;
    [SerializeField] private int minimalTime = -5;
    private bool changedFirstTimeBehaviour;
    private bool changedSecondTimeBehaviour;
    private bool changedThirdTimeBehaviour;
    [HideInInspector] public int multiplierCurrentTimeSubtraction = 1;

    [Header("Audio")]
    private bool footstepsPlayed;
    private bool firstKnockingPlayed;
    private bool secondKnockingPlayed;
    private bool otherRoomLightOnPlayed;

    [Header("Singleton")]
    public static MotherTimerManager Instance;

    //Singleton
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        currentTime = 0;
        currentPlayerTries = maxPlayerTries;
        AudioManager.Instance.Play("MotherTalking");
        FindObjectOfType<Lighting>().blackMat.color = new Color(0, 0, 0);
    }

    private void Update()
    {
        CurrentTimeUpdate();

        MotherVisualOverTime();

        CheckPlayerLooseState();
    }

    //Adds Time before mother catches you.
    public void TimeBonus(float timeBonus)
    {
        if (currentTime <= minimalTime)
            return;
        
        currentTime -= timeBonus;
    }
    
    //Removes Time that is needed for mother to catch you. Game ends at a certain time
    public void TimePenalty(float timePenalty)
    {
        currentTime += timePenalty;
    }
    
    //The mother visuals are based on the time you have left before mother catches you
    private void MotherVisualOverTime()
    {
        MotherBehaviour.instance.SetCamVisualCaught(motherVisualCurve.Evaluate(currentTime / timeWhenMotherCatchesPlayer),1f, camFrequencyVisualCurve.Evaluate(currentTime / timeWhenMotherCatchesPlayer));
    }
    
    private void CheckPlayerLooseState()
    {
        if (currentTime > timeWhenMotherCatchesPlayer && gameStarted)
        {
            currentPlayerTries--;

            AudioManager.Instance.Stop("HeartBeatStadiumThree");
            
            changedThirdTimeBehaviour = false;
            
            MotherBehaviour.instance.CaughtPlayer();
            
            gameStarted = false;
        }
    }

    private void CurrentTimeUpdate()
    {
        if (gameStarted && !pauseGameTime)
        {
            currentTime += Time.deltaTime * multiplierCurrentTimeSubtraction;
        }
        
        if (currentTime < 30 && changedFirstTimeBehaviour)
        {
            changedSecondTimeBehaviour = false;
            changedThirdTimeBehaviour = false;
            changedFirstTimeBehaviour = false;
            FindObjectOfType<Lighting>().otherRoomLight.SetActive(false);
            FindObjectOfType<Lighting>().blackMat.color = new Color(0, 0, 0);
            AudioManager.Instance.FadeOut("HeartBeatStadiumOne");
        }
        else if (currentTime is > 30 and < 45 && !changedFirstTimeBehaviour)
        {
            changedSecondTimeBehaviour = false;
            changedThirdTimeBehaviour = false;

            AudioManager.Instance.FadeOut("HeartBeatStadiumTwo");
            
            FindObjectOfType<Lighting>().otherRoomLight.SetActive(true);
            FindObjectOfType<Lighting>().blackMat.color = new Color(1, .76f, 0);
            
            if (!otherRoomLightOnPlayed)
            {
                AudioManager.Instance.Play("MainRoomLightOn");
                otherRoomLightOnPlayed = true;
            }
            
            AudioManager.Instance.Play("HeartBeatStadiumOne");
            AudioManager.Instance.FadeIn("HeartBeatStadiumOne");
            changedFirstTimeBehaviour = true;
        }
        else if(currentTime is > 45 and < 60 && !changedSecondTimeBehaviour)
        {
            changedFirstTimeBehaviour = false;
            changedThirdTimeBehaviour = false;
            AudioManager.Instance.FadeOut("HeartBeatStadiumOne");
            AudioManager.Instance.Play("HeartBeatStadiumTwo");
            AudioManager.Instance.FadeIn("HeartBeatStadiumTwo");
            
            if (!footstepsPlayed)
            {
                AudioManager.Instance.Play("Footsteps");
                footstepsPlayed = true;
            }

            if (currentPlayerTries == 2 && !firstKnockingPlayed)
            {
                AudioManager.Instance.Play("FirstKnocking");
                firstKnockingPlayed = true;
            }
            
            if (currentPlayerTries == 1 && !secondKnockingPlayed)
            {
                AudioManager.Instance.Play("SecondKnocking");
                secondKnockingPlayed = true;
            }

            changedSecondTimeBehaviour = true;
        }
        else if(currentTime > 60 && !changedThirdTimeBehaviour)
        {
            changedSecondTimeBehaviour = false;
            changedFirstTimeBehaviour = false;
            FindObjectOfType<Lighting>().roomLight.SetActive(true);
            AudioManager.Instance.FadeOut("HeartBeatStadiumTwo");
            AudioManager.Instance.Play("HeartBeatStadiumThree");
            AudioManager.Instance.Play("DoorOpening");
            changedThirdTimeBehaviour = true;
        }
    }
    
    //Game starts when the start is pressed in the menu of the mini games
    public void GameStarted()
    {
        gameStarted = true;
    }
}
