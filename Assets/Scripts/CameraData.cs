using UnityEngine;

[CreateAssetMenu(fileName = "Camera Data", menuName = "Swing Out/Camera Data")]
public class CameraData : ScriptableObject
{
    [Header("Position")]
    [SerializeField] Vector3 initialPosition = new Vector3(0f, 1.8f, -1.8f);

    [Header("Rotation")]
    [SerializeField] Vector3 initialRotation = new Vector3(45f, 0f, 0f);

    [Header("Zoom In-Out")]

    [Header("X Axis")]
    [Range(0f, 1f)] [SerializeField] float zoomInOutYDamper = 0.3f;
    [Range(0f, 10f)] [SerializeField] float zoomInOutRangeMinY = 1.8f;
    [Range(0f, 10f)] [SerializeField] float zoomInOutRangeMaxY = 3.5f;

    [Header("Z Axis")]
    [Range(0f, 1f)] [SerializeField] float zoomInOutZDamper = 0.3f;
    [Range(-10f, 0f)] [SerializeField] float zoomInOutRangeMinZ = -3.5f;
    [Range(-10f, 0f)] [SerializeField] float zoomInOutRangeMaxZ = -1.8f;

    [Tooltip("In Seconds")] [SerializeField] float zoomInOutCompletionTime = 1f;

    [Header("Sideways Movement")]
    [Tooltip("In Br.")] [SerializeField] float lerpSpeed = 10f;
    [Tooltip("In Br.")] [SerializeField] float moveRange = 2f;

    public Vector3 InitialPosition { get => initialPosition; }
    public Vector3 InitialRotation { get => initialRotation; }
    public float ZoomOutCompletionTime { get => zoomInOutCompletionTime; }
    public float ZoomInOutYDamper { get => zoomInOutYDamper; }
    public float ZoomInOutZDamper { get => zoomInOutZDamper; }
    public float ZoomInOutRangeMinY { get => zoomInOutRangeMinY; }
    public float ZoomInOutRangeMaxY { get => zoomInOutRangeMaxY; }
    public float LerpSpeed { get => lerpSpeed; }
    public float MoveRange { get => moveRange; }
    public float ZoomInOutRangeMinZ { get => zoomInOutRangeMinZ; }
    public float ZoomInOutRangeMaxZ { get => zoomInOutRangeMaxZ; }
}
