using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
public class Creature : NetworkBehaviour
{

    /* Gets and Sets */
    public string CreatureName { get; set; }

    public string CreatureType { get; set; }

    public string CreatureClassName { get; set; }

    public int CreatureLevel { get; set; }

    public int Life { get; set; }

    public int Stamina { get; set; }

    public int LifeMax { get; set; }

    public int StaminaMax { get; set; }

    public int Defense { get; set; }

    public int Strength { get; set; }

    public int Intellect { get; set; }

    public int Agility { get; set; }

    public int Speed { get; set; }

    public float LimitRange { get; set; }

    public GameObject[] Targets { get; set; }

    public Vector2 DirectionToMove { get; set; }

    public bool IsOnTarget { get; set; }

    public bool IsOnRange { get; set; }

    public Rigidbody2D Body { get; set; }

    public SpriteRenderer Sprite { get; set; }

    public Animator Animator { get; set; }

    public Creature Creatures { get; set; }
 
    public bool IsAlive { get { return Life > 0; } }

    public bool IsMoving => transform.GetComponent<AI>().DirectionToMove.x != 0 || transform.GetComponent<AI>().DirectionToMove.y != 0 || DirectionToMove.x != 0 || DirectionToMove.y != 0;

    /* Função de inicialização logo ao despertar o script */
    void Awake(){
        Creatures = transform.GetComponent<Creature>();
        CreatureName = Creatures.CreatureName;
        CreatureType = Creatures.CreatureType;
        CreatureClassName = Creatures.CreatureClassName;
        CreatureLevel = Creatures.CreatureLevel;
        LifeMax = Creatures.LifeMax;
        Life = Creatures.Life;
        StaminaMax = Creatures.StaminaMax;
        Stamina = Creatures.Stamina;
        Defense = Creatures.Defense;
        Strength = Creatures.Strength;
        Intellect = Creatures.Intellect;
        Agility = Creatures.Agility;
        Speed = Creatures.Speed;
        LimitRange = Creatures.LimitRange;
    }
}
