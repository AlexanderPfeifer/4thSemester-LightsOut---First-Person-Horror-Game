using TMPro;
using UnityEngine;

public class UIScoreCounter : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TextMeshProUGUI scoreTextObject;
    [HideInInspector] public int gameScore;
    private int lastGameScore;
    [SerializeField] private int scoreUntilDanger = 3;
    public int combo = 1;
    public int counterUntilMultiply;
    [SerializeField] private int scoreToWin;
    
    [Header("CaughtTime")]
    [SerializeField] private float timeUntilCaught = 7.5f;
    [HideInInspector] public float currentCaughtTime;

    [Header("Mother")]
    public GameObject caughtPanel;
    private MotherBehaviour motherBehaviour;
    private bool danger;
    
    [Header("Singleton")]
    public static UIScoreCounter instance;

    private void Awake() => instance = this;

    private void Start()
    {
        currentCaughtTime = timeUntilCaught;
        motherBehaviour = FindObjectOfType<MotherBehaviour>();
    }

    private void Update()
    {
        scoreTextObject.text = gameScore.ToString();
        
        if (danger)
        {
            currentCaughtTime -= Time.deltaTime;
        }
    }

    public void AddScore()
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
        
        if (gameScore > lastGameScore + scoreUntilDanger && currentCaughtTime > 0)
        {
            motherBehaviour.SetCamVisual(0.3f, motherBehaviour.camAmplitudeOnDanger, motherBehaviour.camFrequencyOnDanger);
            danger = true;
        }
    }

    public void ResetCombo()
    {
        combo = 1;
        counterUntilMultiply = 0;
    }
    
    public void ResetCaughtScore()
    {
        if (danger)
        {
            lastGameScore = gameScore;
            
            currentCaughtTime = timeUntilCaught;
        
            FindObjectOfType<PlayerInputs>().isCaught = false;
            
            danger = false;
            
            motherBehaviour.SetCamVisual(0f, motherBehaviour.camAmplitudeNormal, motherBehaviour.camFrequencyNormal);
        }
    }
}
