using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public Transform target;
    
    void Start()
    {
        
    }

    private void Update()
    {
        Vector3 pos = new Vector3(target.position.x, 0f, target.position.z);
        transform.position = pos;
    }
}
