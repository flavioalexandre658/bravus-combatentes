using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerBC networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject hostPanel = null;
    [SerializeField] private TMP_InputField ipAddressInputField = null;
    [SerializeField] private Button joinButton = null;

    private void OnEnable()
    {
        NetworkManagerBC.OnClientConnected += HandleClientConnected;
        NetworkManagerBC.OnClientDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        NetworkManagerBC.OnClientConnected -= HandleClientConnected;
        NetworkManagerBC.OnClientDisconnected -= HandleClientDisconnected;
    }

    public void JoinLobby()//Quando conectar normalmente
    {
        string ipAddress = ipAddressInputField.text;//Pega o texto do ip adress

        networkManager.networkAddress = ipAddress;//Passa o ip adress
        Debug.Log(networkManager.networkAddress);
        networkManager.StartClient();//Inicia o client

        joinButton.interactable = false;//Desativa o botão de conectar
    }

    private void HandleClientConnected()//Quando Tenta conectar mas da falha
    {
        joinButton.interactable = true;

        gameObject.SetActive(false);
        hostPanel.SetActive(false);
    }

    private void HandleClientDisconnected()//Reativa o botão apos uma falha na tentativa de conectar
    {
        joinButton.interactable = true;
    }
}