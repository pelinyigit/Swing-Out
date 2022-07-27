using System.Collections;
using UnityEngine;

public class SawCollider : MonoBehaviour
{

    public GameObject blueParticle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PipePart"))
        {
            StartCoroutine(DecreaseRate());
        }
    }

    private IEnumerator DecreaseRate()
    {
        ParticleSystem.EmissionModule main = gameObject.transform.parent.parent.GetChild(1).GetComponent<ParticleSystem>().emission;
     //   blueParticle.Play();
        GameObject particle = Instantiate(blueParticle, transform.position, Quaternion.identity);
        Destroy(particle, 2f);
        main.rateOverTime = 1500f;
        yield return new WaitForSeconds(.5f);
        main.rateOverTime = 100f;
    }
}
