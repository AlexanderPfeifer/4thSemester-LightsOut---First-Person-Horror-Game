using UnityEngine;

public class MotherTimerManager : MonoBehaviour
{
    [Header("PlayerTries")] 
    [SerializeField] public int maxPlayerTries;
    [SerializeField] public int currentPlayerTries;
    [HideInInspector] public bool diedInScene;
    
    [Header("Time")] 
    [HideInInspector] public bool gameStarted;
    [SerializeField] public float currentTime;
    [SerializeField] public float timeWhenMotherCatchesPlayer;
    [SerializeField] private AnimationCurve motherVisualCurve;
    [SerializeField] public AnimationCurve armFilledCurve;
    [SerializeField] private AnimationCurve camFrequencyVisualCurve;
    [SerializeField] public AnimationCurve armPositionCurve;
    [HideInInspector] public bool pauseGameTime;
    [SerializeField] private int minimalTime = -5;
    [SerializeField] private float timeUntilDialogue;
    private float maxTimeUntilDialogue = 15;

    [Header("Singleton")]
    public static MotherTimerManager instance;

    //Singleton
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        currentTime = 0;
        currentPlayerTries = maxPlayerTries;
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
        currentTime -= timeBonus;
    }
    
    //Removes Time that is needed for mother to catch you. Game ends at a certain time
    public void TimePenalty(float timePenalty)
    {
        if (currentTime <= minimalTime)
            return;
        
        currentTime += timePenalty;
    }
    
    //The mother visuals are based on the time you have left before mother catches you
    private void MotherVisualOverTime()
    {
        MotherBehaviour.instance.SetCamVisualCaught(motherVisualCurve.Evaluate(currentTime / timeWhenMotherCatchesPlayer),1, camFrequencyVisualCurve.Evaluate(currentTime / timeWhenMotherCatchesPlayer));
    }
    
    private void CheckPlayerLooseState()
    {
        if (currentTime > timeWhenMotherCatchesPlayer && gameStarted)
        {
            currentPlayerTries--;

            MotherBehaviour.instance.CaughtPlayer();
            
            gameStarted = false;
        }
    }

    private void CurrentTimeUpdate()
    {
        if (gameStarted && !pauseGameTime)
        {
            currentTime += Time.deltaTime;
            timeUntilDialogue += Time.deltaTime;
        }
        
        if (timeUntilDialogue >= maxTimeUntilDialogue)
        {
            if (currentTime is > 30 and <= 45)
            {
                FindObjectOfType<MotherTextManager>().PlayRandomTextPassiveAggressive();
                timeUntilDialogue = 0;
                maxTimeUntilDialogue = 7;
            }
            else
            {
                FindObjectOfType<MotherTextManager>().PlayRandomTextMad();
                timeUntilDialogue = 0;
                maxTimeUntilDialogue = 7;
            }
        }

        FindObjectOfType<Lighting>().roomLight.SetActive(currentTime > 50);

        if (currentTime >= 30)
        {
            FindObjectOfType<Lighting>().otherRoomLight.SetActive(true);
            FindObjectOfType<Lighting>().blackMat.color = new Color(1, .76f, 0);
        }
        else
        {
            FindObjectOfType<Lighting>().otherRoomLight.SetActive(false);
            FindObjectOfType<Lighting>().blackMat.color = new Color(0, 0, 0);
        }
    }
    
    //Game starts when the start is pressed in the menu of the mini games
    public void GameStarted()
    {
        gameStarted = true;
    }
}
