using PathCreation;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public PathCreator pathCreator;
    public float speed;
    float distanceTravelled;
    public ParticleSystem sawParticle;

    private void Start()
    {
        SawParticle();
    }

    void Update()
    {
        transform.GetChild(0).Rotate(new Vector3(0f, 180f * Time.deltaTime, 0f));
        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Reverse);
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, EndOfPathInstruction.Reverse);
    }

    private void SawParticle()
    {
        sawParticle.Play();
    }  
}
