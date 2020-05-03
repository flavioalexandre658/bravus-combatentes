using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;


public class StatusManager: NetworkBehaviour
{
    [SerializeField] private Text textPlayerName = null;

    [SerializeField] private Status health = null;
    
	[SerializeField] private Status stamina = null;

    [SerializeField] private GameObject cam = null;

    [SerializeField] private Creature creature = null;

    public override void OnStartAuthority()
    {
        CmdPlayerName(PlayerNameInput.DisplayName);
        cam.SetActive(true);
    }

    [Command]
    private void CmdPlayerName(string playerName) => RpcPlayerName(playerName);

    [ClientRpc]
    private void RpcPlayerName(string playerName) => textPlayerName.text = playerName;

    private void Start()
    {
        health.Initialize(creature.Life, creature.LifeMax);

        if (stamina != null)
            stamina.Initialize(creature.Stamina, creature.StaminaMax);
    }

    [ClientCallback]
    void Update () {
        if (hasAuthority) { CmdPlayerName(PlayerNameInput.DisplayName); }
        health.MyCurrentValue = creature.Life;
        if (stamina != null)
                stamina.MyCurrentValue = creature.Stamina;
    }

    [Command]
    void CmdTeste() => Debug.Log(creature.Stamina + ";" + creature.Life);
}
