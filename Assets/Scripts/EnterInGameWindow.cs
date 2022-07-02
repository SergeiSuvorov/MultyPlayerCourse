using UnityEngine;
using UnityEngine.UI;

public class EnterInGameWindow : MonoBehaviour
{
    [SerializeField] private Button _signInButton;

    [SerializeField] private Button _createAccauntButton;

    [SerializeField] private Canvas _enterInGameCanvas;

    [SerializeField] private Canvas _signInCanvas;

    [SerializeField] private Canvas _createAccauntCanvas;
    
    void Start()
    {
        _signInButton.onClick.AddListener(OpenSignInWindow);
        _createAccauntButton.onClick.AddListener(OpenCreateAccauntWindow);
    }

    private void OpenCreateAccauntWindow()
    {
        _createAccauntCanvas.enabled = true;
        _enterInGameCanvas.enabled = false;
    }

    private void OpenSignInWindow()
    {
        _signInCanvas.enabled = true;
        _enterInGameCanvas.enabled = false;
    }

    private void OnDestroy()
    {
        _signInButton?.onClick.RemoveAllListeners();
        _createAccauntButton?.onClick.RemoveAllListeners();
    }
}
