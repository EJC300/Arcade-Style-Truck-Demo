using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    bool exists;

    public void Awake()
    {

        if(!exists)
        {
            exists = true;
            DontDestroyOnLoad(this.gameObject);
          
            
        }
        else
        {
            exists = false;
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
     
    }


}
