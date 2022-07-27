using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PipeEndController : MonoBehaviour
{
    [Header("DELETE LATER ON")]
    public GameObject babyFatman;
    public int currentPipeCount { get; private set; }
    public List<GameObject> instantiatedButtons;

    [SerializeField] TumblerControlData controlData;
    [SerializeField] TumblerParameter tumblerParameter;
    [SerializeField] TumblerController tumbler;
    [SerializeField] SpringJoint botSpringJoint;
    [SerializeField] ParticleSystem splashParticle;
    [SerializeField] List<GameObject> instantiatedPipeParts;

    private GameContanier gameContanier;
    private CameraController mainCam;
    private Material nextPipePartMat;
    private Tween tween;
    private const float startPipePartY = .20f;
    private float nextPipePartY;
    private Vector3 nextForceDir;

    private void Awake()
    {
        currentPipeCount += 1;
        nextPipePartY = startPipePartY;
        nextPipePartMat = controlData.YellowMat;
        nextForceDir = Vector3.right;
        instantiatedPipeParts = new List<GameObject>();
        mainCam = Camera.main.GetComponent<CameraController>();
        botSpringJoint.spring = tumblerParameter.BotSpring;
        tumbler.GetComponent<Rigidbody>().mass = tumblerParameter.Mass;
    }

    private void Start()
    {
        gameContanier = GameContanier.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Button"))
        {
            HandleScaleTumbler(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Collided with ground");
        }
    }

    //private void OnGUI()
    //{
    //    if (GUI.Button(new Rect(0f, 0f, 200f, 100f), "Grow"))
    //    {
    //        StartCoroutine(ScaleUpTumblerRoutine(1));
    //    }
    //    if (GUI.Button(new Rect(400f, 0f, 200f, 100f), "Shrink"))
    //    {
    //        if (instantiatedPipeParts.Count > 0)
    //        {
    //            StartCoroutine(ScaleDownTumblerRoutine(1));
    //        }
    //    }
    //}

    private IEnumerator ScaleUpTumblerRoutine(int countToAdd)
    {
        for (int i = 0; i < countToAdd; i++)
        {
            GameObject instantiatedPipePart = Instantiate(controlData.PipePartPrefab, transform.parent);
            instantiatedPipeParts.Add(instantiatedPipePart);
            instantiatedPipePart.GetComponent<PipePartController>().index = instantiatedPipeParts.Count;
            instantiatedPipePart.GetComponent<Renderer>().material = nextPipePartMat;
            instantiatedPipePart.transform.localPosition = new Vector3(0f, 0f, nextPipePartY);
            tween = instantiatedPipePart.transform.DOScaleZ(1f, .5f)
                 .SetEase(Ease.InExpo)
                 .OnStart(delegate
                 {
                     transform.DOLocalMoveZ(nextPipePartY, .5f).SetEase(Ease.InExpo);
                     mainCam.DoZoomOut();
                 })
                 .OnComplete(delegate
                 {
                     transform.localPosition = new Vector3(0f, 0f, nextPipePartY);
                     HandleBotSpringJoint(true);
                     HandleTumblerMass();
                     gameContanier.lightBeam.UpdateLightBeam(true);
                     gameContanier.ReverseCurrentRingColor();
                 });
            nextPipePartY += .20f;
            nextPipePartMat = nextPipePartMat == controlData.GreenMat ? controlData.YellowMat : controlData.GreenMat;
            yield return tween.WaitForCompletion();
            currentPipeCount++;
            if (babyFatman != null)
            {
                GameObject go = Instantiate(babyFatman, instantiatedPipePart.transform.GetChild(1));
                go.GetComponent<CapsuleCollider>().enabled = false;
            }
            gameContanier.CheckGameStatus();
        }
    }

    public IEnumerator ScaleDownTumblerRoutine(int countToAdd)
    {
        currentPipeCount -= countToAdd;
        for (int i = 0; i < countToAdd; i++)
        {
            var lastInstantiatedPipePart = instantiatedPipeParts[instantiatedPipeParts.Count - 1];
            instantiatedPipeParts.Remove(lastInstantiatedPipePart);
            nextPipePartY -= .20f;
            tween = lastInstantiatedPipePart.transform.DOScaleZ(1f, .1f)
                .SetEase(Ease.OutExpo)
                .OnStart(delegate
                {
                    transform.DOLocalMoveZ(nextPipePartY, .5f).SetEase(Ease.OutExpo);
                    mainCam.DoZoomIn();
                    lastInstantiatedPipePart.GetComponent<MeshRenderer>().enabled = false;
                    StartCoroutine(OnPipePartCutRoutine(lastInstantiatedPipePart));
                })
                .OnComplete(delegate
                {
                    transform.localPosition = new Vector3(0f, 0f, nextPipePartY);
                    HandleBotSpringJoint(false);
                    HandleTumblerMass();
                    gameContanier.lightBeam.UpdateLightBeam(false);
                    gameContanier.TarnishCurrentRingColor();
                });
            nextPipePartMat = nextPipePartMat == controlData.GreenMat ? controlData.YellowMat : controlData.GreenMat;
            yield return tween.WaitForCompletion();
        }
    }

    private IEnumerator OnPipePartCutRoutine(GameObject pipePart)
    {
        var pipePartBody = pipePart.GetComponent<Rigidbody>();
        var pipePartCollider = pipePart.GetComponent<BoxCollider>();
        DisableRigidbody(pipePartBody);
        DisableCollider(pipePartCollider);
        pipePartBody.AddForce(nextForceDir * 300);
        SetNextForceDir();
        yield return new WaitForSeconds(.5f);
        PlayCutTween(pipePart);
    }

    private void PlayCutTween(GameObject pipePart)
    {
        pipePart.transform.DOScale(0f, .5f)
        .SetEase(Ease.InExpo)
        .OnStart(delegate
        {
            pipePart.transform.GetChild(1).gameObject.GetComponent<CapsuleCollider>().enabled = true;
        })
        .OnComplete(delegate
        {
            //Destroy(pipePart);


        });
    }

    private void SetNextForceDir()
    {
        nextForceDir = nextForceDir == Vector3.right ? (Vector3.left + Vector3.up * 1.5f) : (Vector3.right + Vector3.up * 1.5f);
    }

    private void DisableCollider(BoxCollider pipePartCollider)
    {
        pipePartCollider.isTrigger = false;
    }

    private void DisableRigidbody(Rigidbody pipePartBody)
    {
        pipePartBody.isKinematic = false;
    }

    private IEnumerator ProcessButtonOnTriggerEnterRoutine(ProcessButtonController processButton)
    {
        DisableCollider(processButton);
        PlayAnimation(processButton);
        PlayParticle(processButton);
        DisableText(processButton);
        yield return new WaitForSeconds(1.02f);
        EnableText(processButton);
        Scale(processButton);
    }

    private void Scale(ProcessButtonController processButton)
    {
        processButton.transform.DOScale(0, .5f)
            .SetEase(Ease.OutExpo)
            .OnComplete(delegate
            {
                processButton.gameObject.SetActive(false);
                instantiatedButtons.Add(processButton.gameObject);
            });
    }

    private void DisableText(ProcessButtonController processButton)
    {
        processButton.transform.GetChild(0).transform.gameObject.SetActive(false);
    }

    private void EnableText(ProcessButtonController processButton)
    {
        processButton.transform.GetChild(0).transform.gameObject.SetActive(true);
    }

    private void PlayParticle(ProcessButtonController processButton)
    {
        Instantiate(splashParticle, processButton.transform.position + Vector3.up * .15f, splashParticle.transform.rotation);
    }

    private void PlayAnimation(ProcessButtonController processButton)
    {
        processButton.GetComponent<Animator>().SetTrigger("Bump");
    }

    private void DisableCollider(ProcessButtonController processButton)
    {
        processButton.GetComponent<BoxCollider>().enabled = false;
    }

    private void HandleScaleTumbler(GameObject button)
    {
        var processButton = button.GetComponent<ProcessButtonController>();
        StartCoroutine(ProcessButtonOnTriggerEnterRoutine(processButton));
        DoublingPrefix prefix = processButton.doubling.Prefix;
        int pipeCountToAdd = 0;
        int value = (int)processButton.doubling.Value;
        if (prefix == DoublingPrefix.Plus)
        {
            pipeCountToAdd = value;
            StartCoroutine(ScaleUpTumblerRoutine(pipeCountToAdd));
        }
        else if (prefix == DoublingPrefix.Minus)
        {
            pipeCountToAdd = value;
            StartCoroutine(ScaleDownTumblerRoutine(pipeCountToAdd));
        }
        else if (prefix == DoublingPrefix.Multiply)
        {
            pipeCountToAdd = currentPipeCount * value - currentPipeCount;
            StartCoroutine(ScaleUpTumblerRoutine(pipeCountToAdd));
        }
        else if (prefix == DoublingPrefix.Divide)
        {
            pipeCountToAdd = currentPipeCount - currentPipeCount / value;
            StartCoroutine(ScaleDownTumblerRoutine(pipeCountToAdd));
        }

    }

    private void HandleBotSpringJoint(bool isTumbleGrowing)
    {
        var botSpring = botSpringJoint.spring;
        botSpring -= tumblerParameter.BotSpringDamper;
        botSpring = Mathf.Clamp(botSpringJoint.spring, tumblerParameter.BotSpringLowerLimit, tumblerParameter.BotSpringUpperLimit);
    }

    private void HandleTumblerMass()
    {
        var tumblerMass = tumbler.GetComponent<Rigidbody>().mass;
        tumblerMass -= tumblerParameter.MassDamper;
        tumblerMass = Mathf.Clamp(tumblerMass, tumblerParameter.MassLowerLimit, tumblerParameter.MassUpperLimit);
    }
}