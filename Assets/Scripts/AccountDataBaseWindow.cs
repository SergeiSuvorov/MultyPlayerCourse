using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AccountDataBaseWindow : MonoBehaviour
{
    [SerializeField] private InputField _usernameField;
    [SerializeField] private InputField _passwordField;
    [SerializeField] private Button _backButton;
    [SerializeField] private Canvas _previusCanvas;

    protected string _username;
    protected string _password;

    private void Start()
    {
        SubscriptionsElementsUI();
    }

    protected virtual void SubscriptionsElementsUI()
    {
        _usernameField.onValueChange.AddListener(UpdateUsername);
        _passwordField.onValueChange.AddListener(UpdatePassword);
        _backButton.onClick.AddListener(OnBackButtonClick);
    }

    private void UpdatePassword(string password)
    {
        _password = password;
    }

    private void UpdateUsername(string username)
    {
        _username = username;
    }

    private void OnBackButtonClick()
    {
        _previusCanvas.enabled = true;
        GetComponent<Canvas>().enabled = false;
    }

    protected void EnterInGameScene()
    {
        SceneManager.LoadScene(1);
    }
}
