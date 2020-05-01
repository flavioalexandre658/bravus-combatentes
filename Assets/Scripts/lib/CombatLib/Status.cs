using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour {
	private Image content;

	[SerializeField]
	private Text statValue = null;

	private float currentFill;

    private float currentValue;

    public float MyMaxValue{get;set;}

	public float MyCurrentValue{
		get{
			return currentValue;
		}
		set{
			if(value > MyMaxValue){
				currentValue = MyMaxValue;
			}else if (value < 0){
				currentValue = 0;
			}else{
				currentValue = value;
			}
			currentFill = currentValue/MyMaxValue;
		}
	}

	// Use this for initialization
	void Start () {
		content = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		if(currentFill != content.fillAmount){
			content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime *5);
			statValue.text = currentValue.ToString();
		}
	}

	public void Initialize(int currentValue, int maxValue){
		MyMaxValue = maxValue;
		MyCurrentValue = currentValue;
		statValue.text = MyCurrentValue.ToString();
	}
}
