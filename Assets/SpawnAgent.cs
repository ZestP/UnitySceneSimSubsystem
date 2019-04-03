using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAgent : MonoBehaviour
{
    bool spawning = false;
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
                if (objID >= 0) { GameObject tg=Instantiate(spawnableObjs[objID]).gameObject;tg.transform.position = dest; }
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
            switch(msg[1])
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
            
        }
    }
}
