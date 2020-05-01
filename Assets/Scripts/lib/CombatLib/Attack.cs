using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Attack : Creature {

    protected Joybutton joybutton;
    public Coroutine attackRoutine;
	private bool isAttacking = false;

	[SerializeField] private Animator myAnimator = null;
    [SerializeField] private Creature creature = null;

    private float attackCoolDown;
		
	public LayerMask whatIsEnemies;

    public bool IsAttacking{
		get{return isAttacking;}
		set{isAttacking = value;}
	}

	public void SetAttackCoolDown(float attackCD){
		this.attackCoolDown = attackCD;
	}

	public float GetAttackCoolDown(){
		return this.attackCoolDown -= Time.deltaTime;
	}

    [Command]
    private void CmdCoroutine() => RpcCoroutine();

    [ClientRpc]
    private void RpcCoroutine() => attackRoutine = StartCoroutine(transform.GetComponent<ICreatureAttacks>().FirstAttack());//Inicia uma rotina que pode ser cancela a qualquer momento		

    public void GetInput()
    {
        if (hasAuthority)
        {
            if (joybutton.Pressed && IsAlive)
            {
                if (!IsAttacking && !IsMoving /*&& (transform.GetComponent<AI>().IsOnTarget || IsOnTarget)*/)
                {
                    CmdCoroutine();
                }
            }
        }
    }

	/* Encerra a realização de um Attack */
	public void StopAttack()
    {//Coroutine nao acessivel no Servidor é impossivel parar por isso o trecho comentado
		//if(attackRoutine != null){
			//StopCoroutine(attackRoutine);// Para a rotina em execução, para evitar que attack continue executando apos o stopAtk
			IsAttacking = false;
			myAnimator.SetBool("attack", IsAttacking);
		//}
	}

	/* Aplica dano em um Player ao realizar um Attack */
	public void applyDmg(GameObject fromCreature, GameObject toCreature, int dmg){
		Color colorDmg;
		if(toCreature.CompareTag("Player")){
			Creature toPlayer = toCreature.GetComponent<Creature>();
			toPlayer.Life -= dmg;
			if(fromCreature.CompareTag("Player")){//Se a origem do dano for de um Player para um Player alterar cor do dano
				colorDmg = new Color(1f, 1f, 1f);
			}else{
				colorDmg = new Color(1f, 0.04448121f, 0.0235f);
			}
			//Linha abaixo chama função pra mostrar o dano causado
			CombatTextManager.Instance.CreateText(toCreature.GetComponent<Transform>().position, dmg.ToString(), colorDmg);
			if(toPlayer.Life <= 0){
				toPlayer.Life = 0;
                toPlayer.GetComponent<Transform>().GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                toPlayer.GetComponent<Animator>().SetTrigger("die");
				toPlayer.tag ="CreatureDead";//Importante para remover dos targets
                StartCoroutine(WaitAndDeath(toPlayer.gameObject));
            }
		}else if(toCreature.CompareTag("Monster")){
            Creature toMonster = toCreature.GetComponent<Creature>();
			toMonster.Life -= dmg;
			colorDmg = new Color(1f, 1f, 1f);
			//Linha abaixo chama função pra mostrar o dano causado
			CombatTextManager.Instance.CreateText(toCreature.GetComponent<Transform>().position, dmg.ToString(), colorDmg);
			if(toMonster.Life <= 0){
				toMonster.Life = 0;
                toMonster.GetComponent<Transform>().GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                toMonster.GetComponent<Animator>().SetTrigger("die");
				toMonster.tag ="CreatureDead";//Importante para remover dos targets
                StartCoroutine(WaitAndDeath(toMonster.gameObject));
            }
		}
	}

    /* Função para destruir o corpo do player após um tempo */
    private IEnumerator WaitAndDeath(GameObject CreatureObject)
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(CreatureObject);
    }

    /* Aplica o custo de Stamina ao realizar uma ação */
    public void applyStaminaCoust(int coust)
    {
        if (creature != null)
        {
            creature.Stamina -= coust;
        }
    }

  //  /* Função iniciada após despertar */
    void Start(){
		//myAnimator = GetComponent<Animator>();
        joybutton = FindObjectOfType<Joybutton>();
    }

    /* Função de atualização */
    void Update()
    {
        myAnimator = GetComponent<Animator>();
        Life = creature.Life;
        GetInput();
    }

    /* Não funcionando 
	public void DrawFirstAttackTransform() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(FirstAttackTransform.position, AreaDmgFirstAttack);
	}*/
}
