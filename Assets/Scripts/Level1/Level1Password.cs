using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.Events;

public class Level1Password : MonoBehaviour
{
    public TMP_InputField passwordInput;

    public GameObject popupError;
    public GameObject popupSuccess;
    public GameObject popupAdvice;
    
    public UnityEvent OnAdvice;
    public UnityEvent OnSuccess;

    public void CheckPassword()
    {
        string password = passwordInput.text;

        Debug.Log("Checking password: " + password);

        // RULE: at least 8 chars, upper, lower, number, special
        bool isValid = Regex.IsMatch(password,
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$");

        if (isValid)
        {
            Debug.Log("SUCCESS");
            popupSuccess.SetActive(true);
        }
        else
        {
            Debug.Log("ERROR");
            popupError.SetActive(true);
        }
    }

    public void CloseError()
    {
        popupError.SetActive(false);
        popupAdvice.SetActive(true); // show advice after error
        OnAdvice.Invoke();
    }

    public void CloseSuccess()
    {
        popupSuccess.SetActive(false);
        popupAdvice.SetActive(true);
        OnAdvice.Invoke();
    }

    public void CloseAdvice()
    {
        popupAdvice.SetActive(false);
        OnSuccess.Invoke();
    }
}