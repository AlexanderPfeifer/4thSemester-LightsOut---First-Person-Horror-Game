using TMPro;
using UnityEngine;

public class UIScoreCounter : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TextMeshProUGUI scoreTextObject;
    public int gameScore;
    
    [Header("Mother")]
    public GameObject caughtPanel;
    
    [Header("Singleton")]
    public static UIScoreCounter instance;

    private void Awake() => instance = this;

    private void Update()
    {
        scoreTextObject.text = gameScore.ToString();
    }
}
