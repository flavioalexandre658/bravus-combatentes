using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class CameraCtrl : NetworkBehaviour {

	public GameObject player;
	private Vector3 offset;

    // Use this for initialization
    void Start () {
        offset = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = offset + player.transform.position;
	}
}
