using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : Creature
{

    protected Joystick joystick;

    [SerializeField]  private Animator animator = null;
    [SerializeField]  private Attack attack = null;
    [SerializeField] private Creature creature = null;

    /* Método que define a direção que o player irá se mover a partir da posição do joystick */
    public void GetInput()
    {
        if (!attack.IsAttacking)
        {
            DirectionToMove = Vector2.zero;
            if (joystick.Vertical > 0 && Mathf.Abs(joystick.Horizontal) < 0.9f)
            {
                DirectionToMove += Vector2.up;
            }
            if (joystick.Vertical < 0 && Mathf.Abs(joystick.Horizontal) < 0.9f)
            {
                DirectionToMove += Vector2.down;
            }
            if (joystick.Horizontal < 0 && Mathf.Abs(joystick.Vertical) < 0.9f)
            {
                DirectionToMove += Vector2.left;
               // transform.eulerAngles = new Vector2(0, 180); /* Inverte a direção da criatura no eixo X*/
                //StatusBarsTransform.eulerAngles = new Vector2(0, 360);/* Normaliza a direção das barras de vida e stamina criatura no eixo X*/
            }
            if (joystick.Horizontal > 0 && Mathf.Abs(joystick.Vertical) < 0.9f)
            {
                DirectionToMove += Vector2.right;
                //transform.eulerAngles = new Vector2(0, 0); /* Retorna a direção default da criatura no eixo X*/
                //StatusBarsTransform.eulerAngles = new Vector2(0, 360);/* Normaliza a direção das barras de vida e stamina criatura no eixo X*/
            }
        }
    }


    /* Método para o player se movimentar */
    private void Move()
    {
        var BodyPlayer = transform.GetComponent<Rigidbody2D>();
        GetInput();
        BodyPlayer.velocity = DirectionToMove.normalized * Speed; //Normalized para suavizar o movimento na diagonal
    }


    public void NoMove()
    {
        var BodyPlayer = transform.GetComponent<Rigidbody2D>();
        BodyPlayer.velocity = Vector2.zero;
    }

    /* Ignora colisão entre 'Player' e 'Monster' */
    public void OnColliderWithNoObject()
    {
        Physics2D.IgnoreLayerCollision(8, 10, true);
        Physics2D.IgnoreLayerCollision(10, 10, true);
        Physics2D.IgnoreLayerCollision(8, 8, true);
    }

    /* Método utilizado para ativar as Layers e desativar as que nao serão usadas */
    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < animator.layerCount; i++){
            animator.SetLayerWeight(i,0);
        }
        animator.SetLayerWeight(animator.GetLayerIndex(layerName),1);
    }

    [ClientRpc]
    private void RpcActivateLayer(string layerName)
    {
        ActivateLayer(layerName);
    }

    //Verifica as condições para enviar os comandos de ativação das layers de animação para o server.
    private void AnimationsConditions()
    {
        if (!IsAlive)
        {
            CmdDie();
        }

        if (IsMoving)
        {
            CmdWalk(DirectionToMove);
        }
        else if (attack.IsAttacking)
        {
            CmdAttack();
        }
        else
        {
            CmdIdle();
        }
    }

    [Command]
    private void CmdDie()
    {
        RpcActivateLayer("DieLayer");
    }

    [Command]
    private void CmdWalk(Vector2 DirectionToMove)
    {
        animator.SetFloat("VelX", DirectionToMove.x);
        animator.SetFloat("VelY", DirectionToMove.y);
        RpcActivateLayer("WalkLayer");
    }

    [Command]
    private void CmdAttack()
    {
        RpcActivateLayer("AtkLayer");
    }

    [Command]
    private void CmdIdle()
    {
        RpcActivateLayer("IdleLayer");
    }

    [ClientCallback]
    /* Função de inicialização após despertar script */
    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
    }

    public override void OnStartAuthority()
    {
        enabled = true;
    }

    [ClientCallback]
    /* Função de atualização */
    void Update()
    {
        if (!hasAuthority) return;

        if (gameObject.CompareTag("Player"))
        {            
            if (IsAlive)
            {
                Move();
            }
            else
            {
                NoMove();
            }
        }
        Life = creature.Life;
        OnColliderWithNoObject();
        AnimationsConditions();
    }
}
