using TMPro;
using UnityEngine;

public class UIScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreTextObject;
    public GameObject caughtPanel;
    public int gameScore;
    public static UIScoreCounter Instance;
    private float currentSafeScore;
    //make checkpoints on every 25

    private void Awake() => Instance = this;

    private void Update()
    {
        scoreTextObject.text = gameScore.ToString();
    }
}
