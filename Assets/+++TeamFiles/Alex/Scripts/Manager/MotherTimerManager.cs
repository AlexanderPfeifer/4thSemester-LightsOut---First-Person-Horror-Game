using UnityEngine;

public class MotherTimerManager : MonoBehaviour
{
    [Header("PlayerTries")] 
    [SerializeField] private int maxPlayerTries;
    [SerializeField] private int currentPlayerTries;
    
    [Header("Time")] 
    [HideInInspector] public bool gameStarted;
    [SerializeField] public float currentTime;
    [SerializeField] private float timeWhenMotherCatchesPlayer;
    [SerializeField] private float timeBonus;
    [SerializeField] private float timePenalty;
    [SerializeField] private AnimationCurve motherVisualCurve;
    [SerializeField] private AnimationCurve camAmplitudeVisualCurve;
    [SerializeField] private AnimationCurve camFrequencyVisualCurve;

    [Header("Mother")]
    public GameObject caughtPanel;
    
    [Header("Singleton")]
    public static MotherTimerManager instance;

    /*
    private float t;
    private float moritzT = .7f;
    */
    
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
        CheckIfGameHasStarted();

        MotherVisualOverTime();

        CheckPlayerLooseState();
    }

    //Adds Time before mother catches you.
    public void TimeBonus()
    {
        currentTime -= timeBonus;
    }
    
    //Removes Time that is needed for mother to catch you. Game ends at a certain time
    public void TimePenalty()
    {
        currentTime += timePenalty;
    }
    
    //The mother visuals are based on the time you have left before mother catches you
    private void MotherVisualOverTime()
    {
        MotherBehaviour.instance.SetCamVisualCaught(motherVisualCurve.Evaluate(currentTime / timeWhenMotherCatchesPlayer), 
            camAmplitudeVisualCurve.Evaluate(currentTime / timeWhenMotherCatchesPlayer), 
            camFrequencyVisualCurve.Evaluate(currentTime / timeWhenMotherCatchesPlayer));
    }
    
    private void CheckPlayerLooseState()
    {
        if (currentTime > timeWhenMotherCatchesPlayer)
        {
            MotherBehaviour.instance.CaughtPlayer();
            
            currentPlayerTries--;
            
            gameStarted = false;
        }

        if (currentPlayerTries <= 0)
        {
            MotherBehaviour.instance.PlayerLost();

            currentPlayerTries = maxPlayerTries;
        }
    }

    private void CheckIfGameHasStarted()
    {
        if (gameStarted)
        {
            currentTime += Time.deltaTime;
        }
    }
    
    //Game starts when the start is pressed in the menu of the mini games
    public void GameStarted()
    {
        gameStarted = true;
    }

    //For counting something down faster when at the end
    /*
    t += Time.deltaTime;
        
    if (moritzT >= .08f)
    {
        moritzT -= Time.deltaTime * .05f;
    }
        
    if (t >= moritzT)
    {
        currentTime -= 1;
        t = 0;
    }
    */
}
