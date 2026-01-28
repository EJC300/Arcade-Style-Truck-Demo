using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward =  target.position + target.rotation * new Vector3(0,0 ,-2);
        transform.position = Vector3.Lerp(transform.position, forward,0.5f * Time.deltaTime);
        Quaternion lookRotation = Quaternion.LookRotation(( target.position - transform.position).normalized,Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation,Time.deltaTime);
    }
}
