using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct link
{
    public Waypoint l1;

    public Waypoint l2;

    public enum Direction
    {
        undirected,
        directed
    }
    public Direction direction;
    public link(Waypoint l1,Waypoint l2,Direction direction)
    {
        this.l1 = l1;
        this.l2 = l2;
        this.direction = direction;
    }
}


public class Waypoint : MonoBehaviour
{
    public List<Edge> edges = new List<Edge>();

    public List<Waypoint> children = new List<Waypoint>();

    public Waypoint parent;

    void Update()
    {
        foreach(Edge e in edges)
        {
            e.from.parent = this.parent;
            if (!children.Contains(e.from))
            {

                if (e.from != this)
                {
                    children.Add(e.from);

                }
            }
        }
    }
}
