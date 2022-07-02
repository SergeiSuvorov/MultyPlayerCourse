using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class SignInWindow : AccountDataBaseWindow
{
    [SerializeField] private Button _signInButton;

    protected override void SubscriptionsElementsUI()
    {
        base.SubscriptionsElementsUI();

        _signInButton.onClick.AddListener(SignIn);
    }

    private void SignIn()
    {
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = _username,
            Password = _password
        }, result => 
        { 
            Debug.Log($"Sign In Success: {_username}");
            EnterInGameScene();
        }, error => 
        { 
            Debug.LogError($"Something went wrong: {error.GenerateErrorReport()}"); 
        });
    }
}
