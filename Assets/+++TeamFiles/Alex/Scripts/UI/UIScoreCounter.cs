using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UIScoreCounter : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] public TextMeshProUGUI scoreTextObject;
    [HideInInspector] public int gameScore;
    [SerializeField] private int scoreUntilDanger = 3;
    [SerializeField] private int scoreToWin;
    public int combo = 1;
    public int counterUntilMultiply;
    private int lastGameScore;

    [Header("CaughtTime")]
    [SerializeField] private float timeUntilCaught = 7.5f;
    [FormerlySerializedAs("currentCaughtTime")] [HideInInspector] public float currentCatchTime;

    [Header("Mother")]
    public GameObject caughtPanel;
    private MotherBehaviour motherBehaviour;
    private bool danger;
    
    [Header("Singleton")]
    public static UIScoreCounter instance;

    //Singleton
    private void Awake() => instance = this;
    
    private void Start()
    {
        currentCatchTime = timeUntilCaught;
        motherBehaviour = FindObjectOfType<MotherBehaviour>();
    }

    //displays the score as string for the in-game ui on the console. Also counts down time when player reaches a certain score
    private void Update()
    {
        scoreTextObject.text = gameScore.ToString();
        
        if (danger)
        {
            currentCatchTime -= Time.deltaTime;
        }
    }

    //Adds points to the score and those points multiply every few points that are gathered. Game ends at a certain score. 
    //Mother is triggered at a certain score.
    public void AddPointsToScore()
    {
        gameScore += combo;

        counterUntilMultiply++;

        if (counterUntilMultiply >= 5)
        {
            combo++;
            counterUntilMultiply = 0;
        }

        if (gameScore >= scoreToWin)
        {
            Debug.Log("Won");
        }
        
        if (gameScore > lastGameScore + scoreUntilDanger && currentCatchTime > 0)
        {
            motherBehaviour.SetCamVisual(0.3f, motherBehaviour.camAmplitudeOnDanger, motherBehaviour.camFrequencyOnDanger);
            danger = true;
        }
    }

    //Resets the combo when a mistake is made in a game
    public void ResetCombo()
    {
        combo = 1;
        counterUntilMultiply = 0;
    }
    
    //Resets the needed for the mother to trigger and also resets all visualization to normal.
    public void ResetNeededCatchScore()
    {
        if (danger)
        {
            lastGameScore = gameScore;
            
            currentCatchTime = timeUntilCaught;
        
            FindObjectOfType<PlayerInputs>().isCaught = false;
            
            danger = false;
            
            motherBehaviour.SetCamVisual(0f, motherBehaviour.camAmplitudeNormal, motherBehaviour.camFrequencyNormal);
        }
    }
}
