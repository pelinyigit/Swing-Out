using UnityEngine;

public class Testmono : MonoBehaviour
{
    public float number;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            number += -.1f;
            number = Mathf.Clamp(number, -5, -1);
            Debug.Log(number);
        }
    }
}
