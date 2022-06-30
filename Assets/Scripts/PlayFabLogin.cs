using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] Button _connectButton;
    [SerializeField] TMP_Text _textResult;

   
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
        // Here we need to check whether TitleId property is configured in settings or not

        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            /*
            * If not we need to assign it to the appropriate variable manually
            * Otherwise we can just remove this if statement at all
            */
            PlayFabSettings.staticSettings.TitleId = " DFCB9";
        }
        var request = new LoginWithCustomIDRequest
        {
            CustomId = "Player1",
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
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
