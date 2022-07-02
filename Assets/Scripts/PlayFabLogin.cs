using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] Button _connectButton;
    [SerializeField] TMP_Text _textResult;

    private const string AuthGuidKey = "authorization-guid";

    private void Start()
    {
        _connectButton.onClick.AddListener(Connect);
    }

    private void OnDestroy()
    {
        _connectButton.onClick.RemoveAllListeners();
    }

    private void Connect()
    {
        StartConnection();

        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = " DFCB9";
        }

        var needCreation = PlayerPrefs.HasKey(AuthGuidKey);
        var id = PlayerPrefs.GetString(AuthGuidKey, Guid.NewGuid().ToString());


        var request = new LoginWithCustomIDRequest
        {
            CustomId = id,
            CreateAccount = !needCreation
        };
        PlayFabClientAPI.LoginWithCustomID(request, success => { PlayerPrefs.SetString(AuthGuidKey, id); OnLoginSuccess(success); }, OnLoginFailure);
    }

    private void StartConnection()
    {
        _textResult.text = "Connection";
        _textResult.color = Color.yellow;
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made successful API call!");
        _textResult.text = "Connect";
        _textResult.color = Color.green;
    }
    private void OnLoginFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
        _textResult.text = "Error";
        _textResult.color = Color.red;
    }
}
