using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayFabAccountManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleLabel;
    [SerializeField] private Button _logOutButton;

    private void Start()
    {
        _logOutButton.onClick.AddListener(ExitFromAccount);
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(),
        OnGetAccountSuccess, OnError);
    }
    private void OnGetAccountSuccess(GetAccountInfoResult result)
    {
        var accountInfo = result.AccountInfo;
        _titleLabel.text = $"Welcome,  {accountInfo.Username} " +
            $"\nPlayer ID {accountInfo.PlayFabId} " +
            $"\nEmail {accountInfo.PrivateInfo.Email}";
    }
    private void OnError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
    }


    private void ExitFromAccount()
    {
        _titleLabel.text = "";
        PlayFabClientAPI.ForgetAllCredentials();
        ExitToLogInScene();
    }

    private void ExitToLogInScene()
    {
        SceneManager.LoadScene(0);
    }
}
