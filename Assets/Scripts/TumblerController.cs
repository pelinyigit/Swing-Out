using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TumblerController : MonoBehaviour
{
    [SerializeField] FixedJoystick joystick;
    [SerializeField] TumblerParameter tumblerParameter;
    [SerializeField] GameObject SphereFixedJoint;
    [SerializeField] GameObject ground;

    private GameContanier gameContanier;
    private Rigidbody body;
    private float horizontalInput;
    private float verticalInput;

    public Vector3 torque;
    public Vector3 inputDirection;
    public float angle;

    public GameObject[] buttons;
    public Transform targetButton;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        buttons = GameObject.FindGameObjectsWithTag("Button");
        gameContanier = GameContanier.Instance;
    }

    private void Update()
    {
        HandleInput();
        Swing();
        HandleAngularDrag();
    }

    private void HandleInput()
    {
        if (gameContanier.isGameOver)
        {
            verticalInput = 0f;
            horizontalInput = 0f;
            joystick.gameObject.SetActive(false);
            return;
        }
        verticalInput = -joystick.Horizontal;
        horizontalInput = joystick.Vertical;
    }

    private void Swing()
    {
        var localPipeEndUpPos = new Vector3(GameContanier.Instance.pipeEnd.transform.position.x,
            GameContanier.Instance.pipeEnd.transform.position.y,
            GameContanier.Instance.pipeEnd.transform.localPosition.z);
        //var angleBetween = Vector3.Angle(Vector3.up, transform.localPosition);
        angle = Vector3.Angle(GameContanier.Instance.pipeEnd.transform.position, Vector3.up);
        // Debug.Log(angleBetween);

        inputDirection = new Vector3(horizontalInput, 0f, verticalInput);

        Vector3 newInput = new Vector3(inputDirection.z * -1f, 0f, inputDirection.x);

        // targetButton = LockButton(newInput);

        if (angle < 50f)
        {
            torque = inputDirection * tumblerParameter.TorqueSpeed;
            body.AddTorque(torque, ForceMode.Impulse);
        }
        else
        {
            body.angularVelocity = Vector3.zero;
        }
    }

    public Transform LockButton(Vector3 inputDir)
    {
        List<Transform> availableButtons = new List<Transform>();

        for (int i = 0; i < buttons.Length; i++)
        {
            Transform testedButton = buttons[i].transform;

            Vector3 dir = testedButton.position - Vector3.zero;

            dir.y = 0f;

            float angle = Vector3.Angle(dir, inputDir);

            if (angle <= 30f)
            {
                availableButtons.Add(testedButton);
            }
        }

        Transform closestButton = null;

        for (int j = 0; j < availableButtons.Count; j++)
        {
            Transform tested = availableButtons[j];

            Vector3 dir1 = tested.transform.position - (transform.position + inputDir);
            Vector3 dir2 = Vector3.zero;

            if (closestButton != null)
            {
                dir2 = closestButton.transform.position - (transform.position + inputDir);
            }

            if (tested.gameObject.activeSelf)
            {
                if (!closestButton)
                {
                    closestButton = tested;
                }
                else
                {
                    if (dir2.sqrMagnitude > dir1.sqrMagnitude)
                    {
                        closestButton = tested;
                    }
                }
            }
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<SkinnedMeshRenderer>().materials[0].color = Color.black;
        }

        if (closestButton != null)
        {
            closestButton.GetComponent<SkinnedMeshRenderer>().materials[0].color = Color.white;
        }

        return closestButton;
    }

    private void HandleAngularDrag()
    {
        if (horizontalInput != 0f || verticalInput != 0f)
        {
            float angularDrag = tumblerParameter.AngularDrag;
            SphereFixedJoint.GetComponent<Rigidbody>().angularDrag = angularDrag / 1000f;
            body.angularDrag = angularDrag / 10f;

        }
        else if (horizontalInput == 0f && verticalInput == 0f)
        {
            float angularDrag = tumblerParameter.AngularDrag;
            body.angularDrag = angularDrag;
            SphereFixedJoint.GetComponent<Rigidbody>().angularDrag = angularDrag / tumblerParameter.AngularDragDamper;
        }
    }
}
