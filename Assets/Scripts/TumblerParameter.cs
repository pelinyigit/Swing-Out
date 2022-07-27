using UnityEngine;

[CreateAssetMenu(fileName = "Tumbler Parameters", menuName = "Swing Out/Tumbler Parameters")]
public class TumblerParameter : ScriptableObject
{
    [Header("Torque")]
    [SerializeField] float torqueSpeed = 10f;

    [Header("Angular Drag")]
    [SerializeField] float angularDrag = 50f;
    [SerializeField] float angularDragDamper = 6f;

    [Header("Mass")]
    [SerializeField] float mass = 2f;
    [SerializeField] float massDamper = .2f;
    [SerializeField] float massLowerLimit = 1f;
    [SerializeField] float massUpperLimit = 2f;

    [Header("Spring")]
    [SerializeField] float botSpring = 500f;
    [SerializeField] float botSpringDamper = 15f;
    [SerializeField] float botSpringLowerLimit = 300f;
    [SerializeField] float botSpringUpperLimit = 500f;

    public float AngularDrag { get => angularDrag; set => angularDrag = value; }
    public float Mass { get => mass; set => mass = value; }
    public float BotSpringDamper { get => botSpringDamper; set => botSpringDamper = value; }
    public float BotSpringLowerLimit { get => botSpringLowerLimit; set => botSpringLowerLimit = value; }
    public float BotSpringUpperLimit { get => botSpringUpperLimit; set => botSpringUpperLimit = value; }
    public float TorqueSpeed { get => torqueSpeed; set => torqueSpeed = value; }
    public float MassDamper { get => massDamper; set => massDamper = value; }
    public float AngularDragDamper { get => angularDragDamper; set => angularDragDamper = value; }
    public float MassLowerLimit { get => massLowerLimit; }
    public float MassUpperLimit { get => massUpperLimit; }
    public float BotSpring { get => botSpring; }
}
