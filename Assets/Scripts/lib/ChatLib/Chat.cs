using Mirror;
using System;
using TMPro;
using UnityEngine;

    public class Chat : NetworkBehaviour
    {
        [SerializeField] private GameObject chatUI = null;
        [SerializeField] private TMP_Text chatText = null;
        [SerializeField] private TMP_InputField inputField = null;

        private static event Action<string> OnMessage;
        private string playerName = null;

        public override void OnStartAuthority()
        {
            CmdPlayerName(PlayerNameInput.DisplayName);
            chatUI.SetActive(true);
            OnMessage += HandleNewMessage;
        }

        [ClientCallback]
        private void OnDestroy()
        {
            if (!hasAuthority) { return; }

            OnMessage -= HandleNewMessage;
        }

        private void HandleNewMessage(string message)
        {
            chatText.text += message;
        }

        [Client]
        public void Send(string message)
        {
            if (!hasAuthority) { return; }
            if (!Input.GetKeyDown(KeyCode.Return)) { return; }

            if (string.IsNullOrWhiteSpace(message)) { return; }

            CmdSendMessage(message);

            inputField.text = string.Empty;
        }

        [Command]
        private void CmdPlayerName(string playerName) => RpcPlayerName(playerName);

        [ClientRpc]
        private void RpcPlayerName(string playerName) => this.playerName = playerName;

        [Command]
        private void CmdSendMessage(string message)
        {
            RpcHandleMessage($"[{playerName}]: {message}");
        }
        
        [ClientRpc]
        private void RpcHandleMessage(string message)
        {
            OnMessage?.Invoke($"\n{message}");
        }
    }

