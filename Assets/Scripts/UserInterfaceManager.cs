using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI WordDisplay_Text;
    [SerializeField] private TextMeshProUGUI Score_Text;
    [SerializeField] private TextMeshProUGUI Timer_Text;
    [SerializeField] private Button Pause_Button;


    public void WordDisplay(string letter)
    {
        WordDisplay_Text.text += letter;
    }

    public void Reset_WordDisplay()
    {
        WordDisplay_Text.text = "";
    }

}