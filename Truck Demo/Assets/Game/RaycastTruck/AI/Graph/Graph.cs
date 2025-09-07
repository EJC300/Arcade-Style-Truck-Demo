using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Graph : MonoBehaviour
{

    public List<Waypoint> waypoints = new List<Waypoint>();

    public List<link> links = new List<link>();


    public void Init()
    {
        
        
            foreach (link l in links)
            {

                Waypoint w = l.l1;
                Waypoint w2 = l.l2;
                Edge e = new Edge(w, w2);
                if(!w.edges.Contains(e) && !w2.edges.Contains(e))
                {
                    w.edges.Add(e);
                     w2.edges.Add(e);
                }
           
        }
    }
    void Start()
    {
        Init();
    }

    void Update()
    {
        foreach(link l in links)
        {
            Debug.DrawLine(l.l1.transform.position, l.l2.transform.position, Color.blue);
        }
    }

}
