using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour {
    private int currentTargetPtr;
	private List<Position> mTargets;
    private Dictionary<int, int> mTimeDict;
    public Clock mClock;
	public float epsilon=1f;
	public float speed=2f;
	// Use this for initialization
	void Start () {
        currentTargetPtr = 0;
        if (mTargets == null)
            mTargets = new List<Position>();
        //Test();
        if(mTimeDict==null)
            mTimeDict = new Dictionary<int, int>();
        mClock = GameObject.Find("Clock").GetComponent<Clock>();
        Move();
	}
	
	void Test()
	{
	
	}

	// Update is called once per frame
	void Update () {
        if(mClock.IsPlaying)
		    Move();
	}
	void Move()
	{

            if(mTargets.Count==1||(mTargets.Count>1&&currentTargetPtr==mTargets.Count-1))
            {
                transform.position = mTargets[mTargets.Count-1].Pos;
                
            }
            else if(mTargets.Count>1)
            {
                if (mClock.CurrentTime - mTargets[currentTargetPtr + 1].T < 0)
                {
                    transform.position = Vector3.Lerp(mTargets[currentTargetPtr].Pos, mTargets[currentTargetPtr + 1].Pos, (mClock.CurrentTime - mTargets[currentTargetPtr].T) / (float)(mTargets[currentTargetPtr + 1].T - mTargets[currentTargetPtr].T));
                    transform.LookAt(mTargets[currentTargetPtr+1].Pos-mTargets[currentTargetPtr].Pos);
                }
                else
                {
                    currentTargetPtr++;
                }
            }

	}
	public List<Position> GetTargets()
	{
		return mTargets;
	}
	
	public void AddTarget(Position target)
	{
        Debug.Log("Add Target at" + target.Pos.ToString() + target.T);
        if(mTargets==null)
            mTargets = new List<Position>();

        mTargets.Add(target);
            mTargets.Sort((left, right) =>
            {
                if (left.T < right.T)
                    return 1;
                else if (left.T == right.T)
                    return 0;
                else
                    return -1;
            });
        RebuildTimeDict();
        Move();
	}
	public void ClearTargets()
	{
		mTargets.Clear();
        mTimeDict.Clear();
	}
	public void ModifyTarget(int prevTime,Position target)
	{
        mTargets[mTimeDict[prevTime]] = target;
        mTargets.Sort((left, right) =>
        {
            if (left.T < right.T)
                return 1;
            else if (left.T == right.T)
                return 0;
            else
                return -1;
        });
        RebuildTimeDict();
        RelocateCurrentTargetPtr();
        Move();
    }
    private void RebuildTimeDict()
    {
        if (mTimeDict == null)
            mTimeDict = new Dictionary<int, int>();
        mTimeDict.Clear();
        for(int i=0;i<mTargets.Count;i++)
        {
            mTimeDict.Add(mTargets[i].T, i);
        }
    }
    private void RelocateCurrentTargetPtr()
    {
        int i = 0;
        for(i=0; i < mTargets.Count-1; i++)
        {
            if(mTargets[i].T<=mClock.CurrentTime&&mTargets[i+1].T> mClock.CurrentTime)
            {
                break;
            }
        }
        currentTargetPtr = i;
    }
}
