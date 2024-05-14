using TMPro;
using UnityEngine;

public class UIScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreTextObject;
    public GameObject caughtPanel;
    public int gameScore;
    public static UIScoreCounter instance;
    private float currentSafeScore;
    //make checkpoints on every 25

    private void Awake() => instance = this;

    private void Update()
    {
        scoreTextObject.text = gameScore.ToString();
    }
}
