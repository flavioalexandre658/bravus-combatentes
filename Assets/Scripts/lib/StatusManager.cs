using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class StatusManager: NetworkBehaviour
{
    [SerializeField]
    private Text textPlayerName = null;

    [SerializeField]
	private Status health = null;

    
	[SerializeField]
	private Status stamina = null;

    [SerializeField]
    private GameObject cam = null;

    [SerializeField]
    private Creature creature = null;

    public override void OnStartAuthority()
    {
        cam.SetActive(true);
    }
    void Start(){

        textPlayerName.text = creature.CreatureName;

        health.Initialize(creature.Life, creature.LifeMax);

        if(stamina != null)
		    stamina.Initialize(creature.Stamina, creature.StaminaMax);
	}

	void Update () {
		health.MyCurrentValue = creature.Life;

        if (stamina != null)
            stamina.MyCurrentValue = creature.Stamina;
	}
}
