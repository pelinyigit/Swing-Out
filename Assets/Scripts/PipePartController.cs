using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PipePartController : MonoBehaviour
{
    public int index;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Saw"))
        {
            StartCoroutine(ScaleUpButtons());
            StartCoroutine(SawOnTriggerEnterRoutine(other));
        }
    }

    private IEnumerator ScaleUpButtons()
    {
        var buttons = GameContanier.Instance.pipeEnd.instantiatedButtons;
        for (int i = 0; i < buttons.Count; i++)
        {
            var initialScale = buttons[i].GetComponent<ProcessButtonController>().initialScale;
            buttons[i].transform.DOScale(initialScale, .5f)
                 .OnStart(delegate
                 {
                     buttons[i].gameObject.SetActive(true);
                 });
                 buttons[i].GetComponent<BoxCollider>().enabled = true;
            yield return null;
        }
    }

    private IEnumerator SawOnTriggerEnterRoutine(Collider sawCollider)
    {
        sawCollider.enabled = false;
        transform.GetChild(0).gameObject.SetActive(true);
        int cutCount = GameContanier.Instance.pipeEnd.currentPipeCount - index;
        StartCoroutine(GameContanier.Instance.pipeEnd.ScaleDownTumblerRoutine(cutCount));
        yield return new WaitForSeconds(1f);
        sawCollider.enabled = true;
    }
}
