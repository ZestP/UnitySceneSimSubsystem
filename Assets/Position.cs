using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position
{

    public Vector3 Pos;
        public int T { get; set; }
    public Position(Vector3 p,int t)
    {
        Pos = p;
        T = t;
    }
}
