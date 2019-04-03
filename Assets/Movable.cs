using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour {
	private List<Vector3> mTargets;
	public float epsilon=1f;
	public float speed=2f;
	// Use this for initialization
	void Start () {
		mTargets=new List<Vector3>();
		mTargets.Add(transform.position);
		//Test();
	}
	
	void Test()
	{
		for(int i=0;i<(int)(Random.Range(0,20));i++)
			mTargets.Add(new Vector3(Random.Range(-100,100),0,Random.Range(-100,100)));
	}

	// Update is called once per frame
	void Update () {
		Move();
		mTargets[0]=transform.position;
	}
	void Move()
	{
		if(mTargets.Count>1)
		{
			if((transform.position-mTargets[1]).sqrMagnitude>epsilon)
			{
				transform.position+=(mTargets[1]-transform.position).normalized*speed*Time.deltaTime;
				transform.LookAt(mTargets[1]);
			}else{
				mTargets.RemoveAt(1);
			}
		}
	}
	public List<Vector3> getTargets()
	{
		return mTargets;
	}
	
	public void AddTarget(Vector3 target)
	{
		mTargets.Add(target);
	}
	public void ClearTargets()
	{
		mTargets.Clear();
		mTargets.Add(transform.position);
	}
	public void SetTarget(Vector3 target)
	{
		ClearTargets();
		AddTarget(target);
	}
}
