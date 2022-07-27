using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    public float speed;
    public FixedJoystick fixedJoystick;
    public Rigidbody rb;

    public void FixedUpdate()
    {
      //  Vector3 direction = Vector3.forward * fixedJoystick.Vertical + Vector3.right * fixedJoystick.Horizontal;
       // rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        rb.AddForceAtPosition(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * 1f, new Vector3(0f, 0.75f, 0f));
    }
}