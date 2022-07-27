using UnityEngine;

[CreateAssetMenu(fileName = "Tumbler Control Data", menuName = "Swing Out/Tumbler Control Data")]
public class TumblerControlData : ScriptableObject
{
    [SerializeField] GameObject pipePartPrefab;
    [SerializeField] Material yellowMat;
    [SerializeField] Material greenMat;

    public GameObject PipePartPrefab { get => pipePartPrefab; }
    public Material YellowMat { get => yellowMat; }
    public Material GreenMat { get => greenMat; }
}
