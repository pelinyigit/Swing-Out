using UnityEngine;
using DG.Tweening;
using System.Collections;

public class PipePartCutTest : MonoBehaviour
{
    public enum ForceDir { Right, Left }
    public ForceDir forceDir;
    public GameObject pipePartPrefab;
    public float forceValue;

    private Vector3 force;

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0f, 0f, 200f, 100f), "Test"))
        {
            StartCoroutine(OnPipePartCut());
        }
    }

    private IEnumerator OnPipePartCut()
    {
        GameObject pipePart = Instantiate(pipePartPrefab, transform.position, Quaternion.Euler(90f, 0f, 0f), transform);
        var pipePartBody = pipePart.GetComponent<Rigidbody>();
        var pipePartCollider = pipePart.GetComponent<BoxCollider>();
        pipePartBody.isKinematic = false;
        pipePartCollider.isTrigger = false;
        force = forceDir == ForceDir.Right ? Vector3.right : Vector3.left;
        pipePartBody.AddForce(force * forceValue);
        yield return new WaitForSeconds(.5f);
        pipePart.transform.DOScale(0f, .5f)
        .SetEase(Ease.InExpo)
        .OnComplete(delegate
        {
            Destroy(pipePart, .5f);
        });
    }
}