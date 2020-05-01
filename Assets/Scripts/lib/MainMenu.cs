using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerBC networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject hostPanel = null;

    public void HostLobby()
    {
        networkManager.StartHost();
        hostPanel.SetActive(false);
    }
}
