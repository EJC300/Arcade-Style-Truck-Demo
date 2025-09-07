using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DioramaCamera : MonoBehaviour
{

    Vector3 direction;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        direction = transform.position - target.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        transform.position =   target.transform.position + direction;
        transform.Rotate(Vector3.up * 15 * Time.deltaTime);
    }
}
