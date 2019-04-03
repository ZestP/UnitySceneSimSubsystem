﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour {
	public Movable mMovable;
	public LineRenderer mTrail;
	// Use this for initialization
	void Start () {
		mTrail=GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if(mMovable!=null&&mTrail!=null&&isActiveAndEnabled)
		{
			UpdateTrail();
		}
	}
	void UpdateTrail()
	{
		Vector3[] tmp=mMovable.getTargets().ToArray();
		mTrail.positionCount=tmp.Length;
		mTrail.SetPositions(tmp);
	}
}