using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestIcon : MonoBehaviour {
	public float initHeight=10f;
	public float speed=10f;
	public GameObject projector;
	// Use this for initialization
	void Start () {
		projector.transform.localPosition=new Vector3(0,initHeight,0);
	}
	
	// Update is called once per frame
	void Update () {
		if(projector.transform.position.y>5)
		{
			projector.transform.Translate(new Vector3(0,0,speed*Time.deltaTime));
		}else{
			Destroy(this.gameObject);
		}
	}
}
