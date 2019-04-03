using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour {
    public float mXThreshold=20, mYThreshold=20;
    public float mXSpeed=20, mYSpeed=20,mScaleSpeed=20;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.mousePosition.x<mXThreshold&&Input.mousePosition.x>=0)
        {
            transform.Translate(new Vector3(-mXSpeed * Time.deltaTime, 0, 0), Space.World);
            
        }else if(Input.mousePosition.x >Screen.width- mXThreshold&& Input.mousePosition.x<=Screen.width)
        {
            transform.Translate(new Vector3(mXSpeed * Time.deltaTime, 0, 0), Space.World);
        }
        if (Input.mousePosition.y < mYThreshold&& Input.mousePosition.y>=0)
        {
            transform.Translate(new Vector3(0,0,-mYSpeed * Time.deltaTime), Space.World);
        }
        else if (Input.mousePosition.y > Screen.height - mYThreshold&& Input.mousePosition.y<=Screen.height)
        {
            transform.Translate(new Vector3(0,0,mYSpeed * Time.deltaTime), Space.World);
        }
        transform.Translate(new Vector3(0,Input.mouseScrollDelta.y*mScaleSpeed* Time.deltaTime,0), Space.World);
    }
}
