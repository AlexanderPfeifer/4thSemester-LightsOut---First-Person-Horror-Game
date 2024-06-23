using System.Collections;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public static TextManager Instance { get; private set; } // Singleton instance.

    public TMP_Text tmpText; // Assign your TextMeshPro text component in the inspector.
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instance.
        }
        else
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
    }

    // Clears the text of the TMP component.
    private void ClearText()
    {
        if (FindObjectOfType<TutorialManager>() != null && !FindObjectOfType<TutorialManager>().canInteractWithConsole)
        {
            MotherTimerManager.instance.currentTime = 0f;
        }
        
        StartCoroutine(ClearTextLetterForLetter(0.007f));
    }

    // Starts the coroutine to display text letter by letter.
    public void DisplayText(string text, float delay)
    {
        if (tmpText != null)
        {
            StartCoroutine(TypeTextCoroutine(text, delay));
        }
    }

    // Coroutine for typing the text letter by letter.
    private IEnumerator TypeTextCoroutine(string text, float delay)
    {
        var words = text.Split(' ');
        var currentLine = "";
        var currentLineWidth = 0;
        var maxLineWidth = 100; // This is an arbitrary limit; you'd adjust this based on your UI.
    
        foreach (var word in words)
        {
            // Check if adding this word would overflow the line
            var wordLength = word.Length + 1; // +1 for the space
    
            if (currentLineWidth + wordLength > maxLineWidth || word.Contains("\n"))
            {
                // Time to wrap the line
                tmpText.text += currentLine.TrimEnd() + "\n";
                currentLine = word + " ";
                currentLineWidth = wordLength;
            }
            else
            {
                currentLine += word + " ";
                currentLineWidth += wordLength;
            }
    
            // Update the text for each word, not each character, for performance reasons
            //instead of adding it all at once we add it character by character
            foreach (var letter in currentLine)
            {
                tmpText.text += letter;
                yield return new WaitForSeconds(delay);
            }
            currentLine = ""; // Clear current line after updating the text
        }
    
        // Ensure the last line is added
        tmpText.text += currentLine;
        
        yield return new WaitForSeconds(2.5f);
        
        ClearText();
    }


    private IEnumerator ClearTextLetterForLetter(float delay)
    {
        for (int i = tmpText.text.Length - 1; i >= 0; i--)
        {
            tmpText.text = tmpText.text.Remove(i);
            yield return new WaitForSeconds(delay);
        }
    }
}
