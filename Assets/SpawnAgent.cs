using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAgent : MonoBehaviour
{
    public bool spawning = false;
    public AsynchronousClient netAgent;
    public CameraSelect mCS;
    public GameObject[] agentObjs;
    public Spawnable[] spawnableObjs;

    public GameObject agentObj;
    int objID = -1;
    RaycastHit hit;
    Vector3 dest;
    // Start is called before the first frame update
    void Start()
    {

        units = new List<UnitProfile>();
        unitAvability = new List<bool>();
        dest = new Vector3(0, 0, 0);



        mCS = GameObject.Find("BackgroundCamera").GetComponent<CameraSelect>();


    }

    // Update is called once per frame
    void Update()
    {
        if(!spawning)
        {
            netAgent.GetMsg("Cancel");
            List<string> msg=netAgent.GetMsg("Spawn");
            ParseMsg(msg);
            msg = netAgent.GetMsg("Select");
            ParseMsg(msg);
        }
        else
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000))
            {
                 dest= new Vector3(hit.point.x, 0, hit.point.z);
            }
            if (netAgent.GetMsg("Cancel")!=null)
            {
                mCS.enabled = true;
                spawning = false;
                Destroy(agentObj);
            }

            agentObj.transform.position = dest;
            if(Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Left click in Spawn mode");
                if (objID >= 0) {
                    //Spawn obj
                    int uid = AddUnit(dest);
                    netAgent.SendMessage($"Spawn DD {uid} {dest.x} {dest.y} {dest.z}");
                }
            }
            
        }
    }
    public void ToggleSpawningState(bool to)
    {
        spawning = to;
    }
    public void ParseMsg(List<string> msg)
    {
        if(msg!=null)
        {
            switch(msg[0])
            {
                case "Spawn":
                    switch (msg[1])
                    {
                        case "DD":
                            objID = 0;
                            break;
                        default:
                            break;
                    }
                    agentObj = Instantiate(agentObjs[objID]).gameObject;
                    spawning = true;
                    mCS.ClearSelection();
                    mCS.enabled = false;
                    break;
                case "Select":
                    int selID = int.Parse(msg[1]);
                    mCS.ClearSelection();
                    units[selID].mSelectable.Select();
                    break;
                
            }
            
            
        }
    }

    
    private List<UnitProfile> units;
    private List<bool> unitAvability;
    
    public int AddUnit(Vector3 initPos)
    {
        UnitProfile tup=SpawnUnit();
        for (int i = 0; i < unitAvability.Count; i++)
        {
            if (!unitAvability[i])
            {
                units[i] = tup;
                unitAvability[i] = true;
                tup.AddTarget(new Position(initPos, 0));
                return i;
            }
        }
        Debug.Log($"Add unit at{initPos}");
        tup.AddTarget(new Position(initPos, 0));
        units.Add(tup);
        unitAvability.Add(true);
        return units.Count - 1;
    }
    private UnitProfile SpawnUnit()
    {
        GameObject tg = Instantiate(spawnableObjs[objID]).gameObject;
        UnitProfile tup=tg.GetComponent<UnitProfile>();

        return tup;
    }

    public UnitProfile GetUnit(int uid)
    {
        if(unitAvability[uid])
        {
            return units[uid];
        }
        return null;
    }
    public void RemoveUnit(int uid)
    {
        if(unitAvability[uid])
        {
            Destroy(units[uid].gameObject);
            unitAvability[uid] = false;
        }
    }
}
