using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayFabAccountManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleLabel;
    [SerializeField] private TMP_Text _loadLabel;
    [SerializeField] private Button _logOutButton;



    private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string,CatalogItem>();

    private void Start()
    {
        _logOutButton.onClick.AddListener(ExitFromAccount);
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(),
        OnGetAccountSuccess, OnError);

        _loadLabel.text = "Loading";
        _loadLabel.color = Color.yellow;
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalogSuccess,OnFailure);
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

    private void OnFailure(PlayFabError error)
    {
        _loadLabel.text = "Error";
        _loadLabel.color = Color.red;
        var errorMessage = error.GenerateErrorReport();
    }

    private void OnGetCatalogSuccess(GetCatalogItemsResult result)
    {
        _loadLabel.text = "Catalog Ready";
        _loadLabel.color = Color.green;
        HandleCatalog(result.Catalog);
        Debug.Log($"Catalog was loaded successfully!");
    }
    private void HandleCatalog(List<CatalogItem> catalog)
    {
        foreach (var item in catalog)
        {
            _catalog.Add(item.ItemId, item);
            Debug.Log($"Catalog item {item.ItemId} was added successfully!");
        }
    }

}
