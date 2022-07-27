using UnityEngine;

public class RingController : MonoBehaviour
{
    public Material greyMat;

    private Material initialMaterial;

    private void Start()
    {
        MemorizeMaterial();
        TarnishColor();
    }

    private void MemorizeMaterial()
    {
        initialMaterial = GetComponent<MeshRenderer>().material;
    }

    public void TarnishColor()
    {
        GetComponent<MeshRenderer>().material = greyMat;
    }

    public void ReverseColor()
    {
        GetComponent<MeshRenderer>().material = initialMaterial;
    }
}
