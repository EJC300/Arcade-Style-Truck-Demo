using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[System.Serializable]
public class Edge
{

    public Waypoint from;
    public Waypoint to;

   public Edge(Waypoint from,Waypoint to)
    {
        this.from = from;

        this.to = to;
    }

}
