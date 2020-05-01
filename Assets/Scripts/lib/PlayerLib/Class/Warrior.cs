using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Warrior : Creature, ICreatureAttacks
{

	public Warrior(){
        //Data
        CreatureName = PlayerNameInput.DisplayName;
        CreatureType = "Player";
        CreatureClassName = "Warrior";
        CreatureLevel = 1;
		
		//Status
		LifeMax = 500;
		Life = 500;
		StaminaMax = 350;
		Stamina = 350;
		Defense = 25;
		Strength = 25;
		Intellect = 25;
		Agility = 25;
		Speed = 5;
	}


    [SerializeField] private Transform FirstAttackTransform = null;
    [SerializeField] private float AreaDmgFirstAttack = 1.5f;
    [SerializeField] private Attack attack = null;
    [SerializeField] private Creature creature = null;
    [SerializeField] private Animator animator = null;

    /* O player realiza um attack básico */
    public IEnumerator FirstAttack(){
        if (hasAuthority)
        {
            attack.IsAttacking = true;
            CmdAttackAnimation(attack.IsAttacking);
            CmdCoust();
            yield return new WaitForSeconds(0.35f);//A função retornará após o WaitForSeconds
            CmdExec();
        }
    }

    [Command]
    private void CmdAttackAnimation(bool isAttacking) => RpcAttackAnimation(isAttacking);

    [ClientRpc]
    private void RpcAttackAnimation(bool isAttacking)
    {
        Debug.Log(isAttacking);
        animator.SetBool("attack", isAttacking);
    }

    [Command]
    private void CmdCoust() => RpcCoust();

    [ClientRpc]
    private void RpcCoust()
    {
        
        int Stamina = creature.Stamina;
        int coustStamina = 20;
        if (Stamina > coustStamina)
        {
            attack.applyStaminaCoust(coustStamina);
        }
    }

    [Command]
    private void CmdExec() => RpcExec();

    [ClientRpc]
    private void RpcExec()
    {
        transform.Translate(DashFirstAttack(transform) * Speed * Time.deltaTime);
        //CastSpell();
        int dmg = ((creature.Strength + 6) * 2);
        Collider2D[] enemies = Physics2D.OverlapCircleAll(FirstAttackTransform.position, AreaDmgFirstAttack, attack.whatIsEnemies);
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].gameObject != null)
            {
                attack.applyDmg(transform.gameObject, enemies[i].gameObject, dmg);
            }
        }
        attack.StopAttack();
    }

    /* O player da um avanço ao realizar uma ação */
    private Vector2 DashFirstAttack(Transform transform){
		Vector2 Dash = new Vector2();
		if(transform.GetComponent<SpriteRenderer>().flipX){
			Dash = Vector2.left*10;
			return Dash;
		}else{
			Dash = Vector2.right*10;
			return Dash;
		}
	}
}
