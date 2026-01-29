using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    [SerializeField] Rigidbody rb;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float direction = Vector3.Dot(rb.velocity, rb.transform.forward);
        Debug.Log(direction);
        if (direction >= 0.1f)
        {
            Vector3 forward = target.position + target.rotation * new Vector3(0, 2, -20);
            transform.position = Vector3.Lerp(transform.position, forward, 0.5f * Time.deltaTime);
            Quaternion lookRotation = Quaternion.LookRotation((target.position - transform.position).normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
        }
        else if( direction <= -0.1f)
        {
            Vector3 forward = target.position - target.rotation * new Vector3(0, -5, -20);
            transform.position = Vector3.Lerp(transform.position, forward, 0.5f * Time.deltaTime);
            Quaternion lookRotation = Quaternion.LookRotation((target.position - transform.position).normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
        }
    }
}
