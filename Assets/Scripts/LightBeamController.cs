using UnityEngine;
using DG.Tweening;

public class LightBeamController : MonoBehaviour
{
    [Header("Scale Params")]
    public Vector3 initialScale;
    [Tooltip("Must be set to .288f by default")] public float scaleDamper;

    [Header("Other Params")]
    public bool canDoColorLerp;

    private GameContanier gameContanier;
    private Sequence colorSequence;
    public int scaleNumber;


    private void Awake()
    {
        ResetLightBeam();
    }

    private void Start()
    {
        colorSequence = DOTween.Sequence();
        gameContanier = GameContanier.Instance;
        if (canDoColorLerp)
        {
            DoColorFadeAnimation();
        }
    }

    //private void OnGUI()
    //{
    //    if (GUI.Button(new Rect(0f, 0f, 100f, 200f), "Scale Light Beam"))
    //    {
    //        UpdateLightBeam();
    //    }
    //}

    public void UpdateLightBeam(bool scaleUp)
    {
        if (scaleNumber >= gameContanier.maxPipeCount)
        {
            return;
        }
        var currentScale = transform.localScale;
        var scaleVector = scaleUp ? new Vector3(scaleDamper, scaleDamper, 0f) : new Vector3(-scaleDamper, -scaleDamper, 0f);
        transform.localScale = currentScale + scaleVector;
        scaleNumber = scaleUp ? ++scaleNumber : --scaleNumber;
    }

    private void ResetLightBeam()
    {
        transform.localScale = initialScale;
    }

    private void DoColorFadeAnimation()
    {
        var material = GetComponent<MeshRenderer>().material;
        var initialAlpha = material.color.a;
        colorSequence.Append(material.DOFade(0f, 1f));
        colorSequence.Append(material.DOFade(initialAlpha, 1f));
        colorSequence.SetLoops(-1, LoopType.Restart);
    }
}
