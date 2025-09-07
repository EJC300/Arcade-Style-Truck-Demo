using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class  WaypointDebug : MonoBehaviour
{

    public void debugWP(GameObject overlook)
    {
      
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Waypoint");
        int i = 1;
        foreach (GameObject go in gos)
        {

              
                if(go != overlook)
                {
    
                go.name =  " WP" + string.Format("{0:000}",i);
                i++;
                }
                  
        
        }
       
    }
   void OnDestroy()
    {

        debugWP(this.gameObject);
    }
   void Start()
    {
        if (this.transform.parent.gameObject.name != "Waypoint") return;
                debugWP(null);
    }
    void Update()
    {
        this.GetComponent<TextMesh>().text =this.transform.parent.gameObject.name;
    }
}
