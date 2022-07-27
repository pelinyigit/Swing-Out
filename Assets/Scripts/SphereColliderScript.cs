using UnityEngine;

public class SphereColliderScript : MonoBehaviour
{
    public TumblerController tumbler;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            tumbler.GetComponent<Rigidbody>().drag = 10;
        }
    }


}
