using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class SawPath : MonoBehaviour
{
    public PathCreator pathCreator;
    public float speed;
    float distanceTravelled;

    private void OnTriggerEnter(Collider other)
    {
        
    }

    void Update()
    {
        
        transform.GetChild(0).Rotate(new Vector3(0f, 180f * Time.deltaTime, 0f));
        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Reverse);
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, EndOfPathInstruction.Reverse);
    }
}
