using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour {
	public GameObject[] displayObjs;
	public bool isSelected;
	public Movable mMovable;
    public CameraSelect mCS;
	void Start()
	{
		ToggleState(false);
		isSelected=false;
		mMovable=GetComponent<Movable>();
        mCS = GameObject.Find("BackgroundCamera").GetComponent<CameraSelect>();
        mCS.AddSelectable(gameObject);
	}
	public void Select()
	{
		isSelected=true;
		ToggleState(true);
	}
	public void DisSelect()
	{
		isSelected=false;
		ToggleState(false);
	}
	void ToggleState(bool isOn)
	{
		foreach(GameObject go in displayObjs)
		{
			go.SetActive(isOn);
		}
	}
	public Movable GetMovable()
	{
		return mMovable;
	}
}
