using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI WordDisplay_Text;
    [SerializeField] private TextMeshProUGUI Score_Text;
    [SerializeField] private TextMeshProUGUI Timer_Text;
    [SerializeField] private Button Pause_Button;


    public void WordDisplay(ref string word, bool exists)
    {
        WordDisplay_Text.text = word;
        WordDisplay_Text.color = exists ? Color.green : Color.red;
    }

    public void IncrementLetterDisplay(ref string letter)
    {
        WordDisplay_Text.text += letter;
    }

    public void Reset_WordDisplay()
    {
        WordDisplay_Text.text = "";
        WordDisplay_Text.color = Color.white;
    }

    public void UpdateScore(int scoreToAdd)
    {
        Score_Text.text = $"SCORE: {scoreToAdd}";
    }




}