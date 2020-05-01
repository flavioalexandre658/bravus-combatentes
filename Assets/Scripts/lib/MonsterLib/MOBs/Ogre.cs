using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ogre : Creature, ICreatureAttacks
{

	public Ogre(){
        //Data
        CreatureType = "Monster";
        CreatureClassName = "Ogre";
		
		//Status
		LifeMax = 250;
		Life = 250;
		StaminaMax = 300;
		Stamina = 300;
		Defense = 20;
		Strength = 18;
		Intellect = 3;
		Agility = 2;
		Speed = 2;
		LimitRange = 1.5f;
	}

    [SerializeField] private Transform FirstAttackTransform = null;
    [SerializeField] private float AreaDmgFirstAttack = 1.5f;

    /* A criatura realiza um attack básico */
    public IEnumerator FirstAttack(){
		Attack atk = transform.GetComponent<Attack>();
		Animator myAnimator =  transform.GetComponent<Animator>();
		if(atk.GetAttackCoolDown() <= 0){
			atk.SetAttackCoolDown(1f);
			atk.IsAttacking = true;
			myAnimator.SetBool("attack", atk.IsAttacking);
			int dmg = ((transform.GetComponent<Creature>().Strength/2 + 1)*1);
			atk.applyStaminaCoust(100);
			yield return new WaitForSeconds(0.35f);//A função retornará após o WaitForSeconds
				Collider2D[] enemies = Physics2D.OverlapCircleAll(FirstAttackTransform.position, AreaDmgFirstAttack, atk.whatIsEnemies);
				for(int i = 0; i < enemies.Length; i++){
					if (enemies[i].gameObject != null){
						atk.applyDmg(transform.gameObject, enemies[i].gameObject, dmg);
					}
				}
				atk.StopAttack();
		}
	}
}
