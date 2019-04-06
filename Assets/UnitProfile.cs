using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitProfile : MonoBehaviour
{
    public Selectable mSelectable;
    public Movable mMovable;
    public Spawnable mSpawnable;
    // Start is called before the first frame update
    void Start()
    {
        mSelectable = GetComponent<Selectable>();
        mMovable = GetComponent<Movable>();
        mSpawnable = GetComponent<Spawnable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddTarget(Position p)
    {
        if (mMovable != null)
        {
            mMovable.AddTarget(p);
        }
    }
    public void ModifyTarget(int prevTime,Position p)
    {
        if (mMovable != null)
        {
            mMovable.ModifyTarget(prevTime,p);
        }
    }
    public void ClearTargets()
    {
        if (mMovable != null)
        {
            mMovable.ClearTargets();
        }
    }
    
}
