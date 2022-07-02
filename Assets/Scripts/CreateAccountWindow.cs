using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class CreateAccountWindow : AccountDataBaseWindow
{
    [SerializeField] private InputField _emailField;
    [SerializeField] private Button _createAccountButton;

    private string _email;

    protected override void SubscriptionsElementsUI()
    {
        base.SubscriptionsElementsUI();
        _emailField.onValueChange.AddListener(UpdateEmail);
        _createAccountButton.onClick.AddListener(CreateAccount);
    }

    private void CreateAccount()
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = _username,
            Email = _email,
            Password = _password,
            RequireBothUsernameAndEmail = true
        }, result =>
        {
            Debug.Log($"Sign In Success: {_username}");
            EnterInGameScene();
        }, error => 
        { 
            Debug.LogError($"Something went wrong: {error.GenerateErrorReport()}"); 
        });
    }

    private void UpdateEmail(string email)
    {
        _email=email;
    }
}
