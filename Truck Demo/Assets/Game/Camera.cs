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
        Vector3 forward =  target.position + target.rotation * new Vector3(0, 5,-15);
        transform.position = Vector3.Lerp(transform.position, forward,1f * Time.deltaTime);
        transform.LookAt(target,target.up);
    }
}
