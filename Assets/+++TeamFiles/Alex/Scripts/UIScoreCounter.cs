using TMPro;
using UnityEngine;

public class UIScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreTextObject;
    public int gameScore;
    public static UIScoreCounter Instance;
    //make checkpoints on every 25

    private void Awake()
    {
        Instance = this;
    }
    
    private void Update()
    {
        scoreTextObject.text = gameScore.ToString();
    }
}
