using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour{

	private Rigidbody2D myRBody;
	private Animator myAnimator;
	private bool isCollision = false;
	public Transform OriginTransform {get; set;}
	private float timeToDestroy = 1.5f;

	void Start () {
		myRBody = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();
	}

	void FixedUpdate(){
		Vector2 Direction = OriginTransform.eulerAngles;
		timeToDestroy -= Time.deltaTime;

		if(Direction.y > 0 && isCollision == false){
			Vector2 direction = Vector2.left;
			myRBody.velocity = direction.normalized * 5;
			// Converte a direção da particula para cada posição que o player estiver em realção ao target
			float angle = Mathf.Atan2(direction.y, direction.x)* Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}else if(Direction.y == 0 && isCollision == false){
			Vector2 direction = Vector2.right;
			myRBody.velocity = direction.normalized * 7;
			// Converte a direção da particula para cada posição que o player estiver em realção ao target
			float angle = Mathf.Atan2(direction.y, direction.x)* Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}

		if(timeToDestroy <= 0){
			myAnimator.SetTrigger("Impactar");
			Attack atk = OriginTransform.GetComponent<Attack>();
			Destroy(gameObject);
			atk.StopAttack();
		}
	}
	void OnTriggerEnter2D(Collider2D other){
		if(other.transform.tag != "Monster"){
			myAnimator.SetTrigger("Impactar");
			int dmg = ((OriginTransform.GetComponent<Creature>().Strength/2 + 1)*1);
			Attack atk = OriginTransform.GetComponent<Attack>();
			atk.applyDmg(OriginTransform.gameObject, other.gameObject, dmg);
			myRBody.velocity = Vector2.zero;
			isCollision = true;
			timeToDestroy = 0f;
			Destroy(gameObject);
			atk.StopAttack();
		}
	}
}
