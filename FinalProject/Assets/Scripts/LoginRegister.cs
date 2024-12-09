using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

// ADD A LINE TO SWITCH SCENES TO THE SINGLEPLAYER SCREEN AND THEN ADD IT TO THE On Logged In () SECTION OF THE _PlayFabSystem
//The above should be good in the login script now

public class LoginRegister : MonoBehaviour
{
    [HideInInspector]
    public string playFabId;

    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TextMeshProUGUI displayText;

    public UnityEvent onLoggedIn;

    public static LoginRegister instance;
    void Awake() { instance = this; }
    
    public void OnRegister()
    {
        RegisterPlayFabUserRequest registerRequest = new RegisterPlayFabUserRequest
        {
            Username = usernameInput.text,
            DisplayName = usernameInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(registerRequest,
 result =>
 {
     Debug.Log(result.PlayFabId);
 },
 error =>
 {
     Debug.Log(error.ErrorMessage);
 }
);
    }

    public async Task OnLoginButtonAsync()
    {
        LoginWithPlayFabRequest loginRequest = new LoginWithPlayFabRequest
        {
            Username = usernameInput.text,
            Password = passwordInput.text
        };

        PlayFabClientAPI.LoginWithPlayFab(loginRequest,
            result => {
                SetDisplayText("Logged in as: " + result.PlayFabId, Color.green);

                if (onLoggedIn != null)
                    onLoggedIn.Invoke();
                playFabId = result.PlayFabId;

            },
            error => SetDisplayText(error.ErrorMessage, Color.red));


        await Task.Delay(2000);
        SceneManager.LoadScene("SinglePlayer");

    }

    void SetDisplayText(string text, Color color)
    {
        displayText.text = text;
        displayText.color = color;
    }
    
}
