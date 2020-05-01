using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Creature {

    private float RangeMinForTarget = 12.0f;

    /* Estabelece um target 'Player' dentro do raio de 6 pixels para a criatura. */
    public void SetTargetToPlayer(Transform My, GameObject Target)
    {
        var rangeMin = Vector2.Distance(My.position, Target.GetComponent<Transform>().position);
        if (rangeMin <= RangeMinForTarget && !IsOnTarget && Target.GetComponent<Creature>().IsAlive)
        { // Se o Player estiver no raio de 6 pixel entra
             IsOnTarget = true;
        }
        if (rangeMin >= RangeMinForTarget)
        { // Se o Player estiver fora do alcance minimo (rangeMin)
             IsOnTarget = false;
            TargetOnRangeMin(My,  Targets);
        }
    }

    /* Estabelece um target 'Player' dentro do raio minimo para a criatura */
    public void SetTargetToMonster(Transform My, GameObject Target)
    {
        var rangeMin = Vector2.Distance(My.position, Target.GetComponent<Transform>().position);
        var atk =  transform.GetComponent<Attack>();
        if (Target.CompareTag("Player") && My.CompareTag("Monster"))
        {
            if (!atk.IsAttacking)
            {
                if (rangeMin >  LimitRange)
                { // Se a criatura se aproximar ate o Limite irá parar de se mover
                  /* Atribui uma nova posição para Criatura */
                    My.position = Vector2.MoveTowards(My.position, Target.GetComponent<Transform>().position,  Speed * Time.deltaTime);
                }
                /* Calcula a velocidade da criatura */
                 DirectionToMove = Target.GetComponent<Transform>().position - transform.position;
                if ( DirectionToMove.x < 0)
                {
                    transform.eulerAngles = new Vector2(0, 180); // Retorna a direção default da criatura no eixo X
                    // StatusBarsTransform.eulerAngles = new Vector2(0, 360);/* Normaliza a direção das barras de vida e stamina criatura no eixo X*/
                }
                else
                {
                    transform.eulerAngles = new Vector2(0, 0); // Inverte a direção da criatura no eixo X
                     //StatusBarsTransform.eulerAngles = new Vector2(0, 360);/* Normaliza a direção das barras de vida e stamina criatura no eixo X*/
                }
                IsOnTarget = true;
            }
            if (rangeMin <=  LimitRange)
            { // Se a criatura se aproximar ate o Limite irá realizar um Atk e ficar parado
                 DirectionToMove = Vector2.zero;
                if (!atk.IsAttacking && IsOnTarget && !IsMoving)
                {

                    if ( CreatureType == "Monster")
                    {
                        atk.attackRoutine= StartCoroutine(transform.GetComponent<ICreatureAttacks>().FirstAttack());//Inicia uma rotina que pode ser cancela a qualquer momento	
                    }
                }
            }
            if (rangeMin >= (RangeMinForTarget - 1))
            { // Se a Criatura estiver fora do alcance minimo (rangeMin) perde o Target
                 IsOnTarget = false;
                 TargetOnRangeMin(My,  Targets);
                 DirectionToMove = Vector2.zero;
            }
        }
    }

    /* Calcula qual Player esta mais próximo da Criatura e retorna a distancia */
    public GameObject TargetOnRangeMin(Transform My, GameObject[] Target)
    {
        float range = 0.0f;
        float min = RangeMinForTarget;
        GameObject aux, aux2;
        for (int i = 0; i < Target.Length; i++)
        {
            if (Target[i].GetComponent<Creature>().IsAlive)
            {
                range = Vector2.Distance(My.position, Target[i].GetComponent<Transform>().position);
                if (range <= min)
                {
                    min = range;
                    aux = Target[i];
                    aux2 = Target[0];
                    Target[0] = aux;
                    Target[i] = aux2;
                }
            }
        }
        return Target[0];
    }

    /* Seleciona o Target dentro do Range Minimo */
    public void SelectTargetsOnRange()
    {
        bool isOnRange = false;
        GameObject Target;
        //Targets =  Targets;


        for (int i = 0; i < Targets.Length; i++)
        {
            float range = Vector2.Distance(transform.position, Targets[i].GetComponent<Transform>().position);
            if (range < RangeMinForTarget)
            {
                isOnRange = true;
                break;
            }
        }

        Target = TargetOnRangeMin(transform, Targets);
        if (Target.GetComponent<Creature>().IsAlive)
        {
            if (Targets.Length != 0 && isOnRange == true)
            {
                if (CompareTag("Player"))
                {
                    SetTargetToPlayer(transform, Target);
                }
                else
                {
                    SetTargetToMonster(transform, Target);
                }
            }
        }
        else
        {
            IsOnTarget = false;
            DirectionToMove = Vector2.zero;
        }
    }


    /* Função de inicialização depois de despertar o script */
    void Update()
    {
        if (CreatureType != "Player")
        {
            Targets = GameObject.FindGameObjectsWithTag("Player");
        }
        else
        {
            Targets = GameObject.FindGameObjectsWithTag("Monster");
        }
    }

    /* Função de atualização frame-frame */
    void FixedUpdate()
    {
        if (Targets == null || Targets.Length == 0)
            return;

        Life = transform.GetComponent<Creature>().Life;
        if (IsAlive)
        {
            SelectTargetsOnRange();
        }
    }
 }