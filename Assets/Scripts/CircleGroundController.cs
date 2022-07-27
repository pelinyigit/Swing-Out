using UnityEngine;

public class CircleGroundController : MonoBehaviour
{
    public enum CircleGround
    {
        Ground0 = 0,
        Ground1 = 1,
        Ground2 = 2,
        Ground3 = 3,
        Ground4 = 4,
        Ground5 = 5,
        Ground6 = 6,
    }

    public CircleGround circleGround;
    public GameObject currentGround;

    private void Awake()
    {
        //InvokeRepeating("SetCircleGround", 0f, 1f);
        //SetCircleGround();
    }

    private void SetCircleGround()
    {
        for (int i = 0; i < 7; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        currentGround = transform.GetChild((int)circleGround).gameObject;
        currentGround.SetActive(true);
        currentGround.AddComponent<RingSelection>();
        currentGround.GetComponent<RingSelection>().SelectRing(1);
    }
}
