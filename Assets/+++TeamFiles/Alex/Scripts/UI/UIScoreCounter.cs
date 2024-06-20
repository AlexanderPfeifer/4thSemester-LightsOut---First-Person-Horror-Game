using TMPro;
using UnityEngine;

public class UIScoreCounter : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] public TextMeshProUGUI scoreTextObject;
    [HideInInspector] public int currentGameScore;
    [SerializeField] public int achievablePoints;
    [SerializeField] public int winGameUntilCombo;
    [SerializeField] public int scoreToWin;
    
    public int combo = 1;
    public int counterUntilMultiply;
    [SerializeField] private float continueHunting;
    
    [Header("CaughtTime")]
    [SerializeField] private float maxTerrorRadius = 10f;
    [HideInInspector] public float currentTerrorRadius;

    [Header("Mother")]
    public GameObject caughtPanel;
    private MotherBehaviour motherBehaviour;
    private bool danger;
    
    [Header("Singleton")]
    public static UIScoreCounter instance;

    private float t;
    private float moritzT = .7f;
    private bool tookBook;
    
    //Singleton
    private void Awake() => instance = this;
    
    private void Start()
    {
        currentTerrorRadius = 0;
        motherBehaviour = FindObjectOfType<MotherBehaviour>();
    }

    //displays the score as string for the in-game ui on the console. Also counts down time when player reaches a certain score
    private void Update()
    {
        scoreTextObject.text = currentGameScore.ToString();
        
        if (danger)
        {
            currentTerrorRadius += Time.deltaTime;
        }
        else
        {
            continueHunting -= Time.deltaTime;
        }

        if (tookBook)
        {
            continueHunting = Random.Range(40 - currentGameScore / 50, 70 - currentGameScore / 50);
            
            currentTerrorRadius = 0;
        
            FindObjectOfType<PlayerInputs>().isCaught = false;
            
            danger = false;
            
            motherBehaviour.SetCamVisual(0f, motherBehaviour.camAmplitudeNormal, motherBehaviour.camFrequencyNormal);

            tookBook = false;
        }
        
        if (continueHunting < 0 && !danger)
        {
            motherBehaviour.SetCamVisual(0.3f, motherBehaviour.camAmplitudeOnDanger, motherBehaviour.camFrequencyOnDanger);
            danger = true;
        }
        
        if (currentTerrorRadius > maxTerrorRadius)
        {
            motherBehaviour.CaughtPlayer();
            SubtractPointsFromScore();
            danger = false;
        }

        if (currentGameScore <= 0)
        {
            combo = 1;
            counterUntilMultiply = 0;
            continueHunting = 60;
            currentTerrorRadius = 0;
            danger = false;
        }
    }

    //Adds points to the score and those points multiply every few points that are gathered. Game ends at a certain score. 
    //Mother is triggered at a certain score.
    public void AddPointsToScore()
    {
        currentGameScore += combo * achievablePoints;

        counterUntilMultiply++;

        if (counterUntilMultiply >= winGameUntilCombo)
        {
            combo++;
            counterUntilMultiply = 0;
        }

        if (currentGameScore >= scoreToWin)
        {
            FindObjectOfType<MotherBehaviour>().PlayerWon();
        }
    }
    
    //Subtracts points from the score and those points multiply every few points that are gathered. Game ends at a certain score. 
    //Mother is triggered at a certain score.
    private void SubtractPointsFromScore()
    {
        ResetCombo();
        
        t += Time.deltaTime;
        
        if (moritzT >= .08f)
        {
            moritzT -= Time.deltaTime * .05f;
        }
        
        if (t >= moritzT)
        {
            currentGameScore -= 1;
            t = 0;
        }
    }

    //Resets the combo when a mistake is made in a game
    public void ResetCombo()
    {
        combo = 1;
        counterUntilMultiply = 0;
    }
    
    //Resets the needed for the mother to trigger and also resets all visualization to normal.
    public void PickedUpBook()
    {
        tookBook = true;
    }
}
