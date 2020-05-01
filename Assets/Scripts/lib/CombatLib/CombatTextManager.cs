using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CombatTextManager : MonoBehaviour {

	private static CombatTextManager instance;

	public GameObject textPrefab;

	public RectTransform canvasTransform;

	public float speed = 0.5f;

	public float fadeTime;

	public Vector3 direction;

	public static CombatTextManager Instance{
		get{
			if(instance == null){
				instance = GameObject.FindObjectOfType<CombatTextManager>();
			}
			return instance;
		}
	}

	public void CreateText(Vector3 position, string text, Color color){
		GameObject sct = (GameObject)Instantiate(textPrefab, position, Quaternion.identity);
		sct.transform.SetParent(canvasTransform);
		sct.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
		sct.GetComponent<CombatText>().Initialize(speed, direction, fadeTime);
		sct.GetComponent<Text>().text = text;
		sct.GetComponent<Text>().color = color;
	}
}
