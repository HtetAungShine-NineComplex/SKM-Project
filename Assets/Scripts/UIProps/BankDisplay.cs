using UnityEngine;
using TMPro;

public class BankDisplay : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI[] digitTexts;  

   
    public void DisplayNumber(string number)
    {
        
        string paddedNumber = number.PadLeft(digitTexts.Length, '0');

        
        for (int i = 0; i < digitTexts.Length; i++)
        {
            digitTexts[i].text = paddedNumber[i].ToString();
        }
    }
}
