using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Imp : Creature, ICreatureAttacks
{

	public Imp(){
        //Data
        CreatureType = "Monster";
		CreatureClassName = "Imp";
		
		//Status
		LifeMax = 320;
		Life = 320;
		StaminaMax = 400;
		Stamina = 400;
		Defense = 20;
		Strength = 25;
		Intellect = 25;
		Agility = 25;
		Speed = 3;
		LimitRange = 6f;
	}

    [SerializeField] private GameObject[] spellPrefab = null;

	//[SerializeField] // Define os pontos de saídas da spell
	public Transform exitPoints;

	/* A criatura realiza um attack básico */
	public IEnumerator FirstAttack(){
			Attack atk = transform.GetComponent<Attack>();
			Animator myAnimator =  transform.GetComponent<Animator>();
			if(atk.GetAttackCoolDown() <= 0){
				atk.IsAttacking = true;
				myAnimator.SetBool("attack", atk.IsAttacking);
				atk.SetAttackCoolDown(1.5f);
				yield return new WaitForSeconds(0.30f);
					Spells missile = Instantiate(spellPrefab[0], exitPoints.position, Quaternion.identity).GetComponent<Spells>();
					missile.OriginTransform = transform;
			}
	}
}
