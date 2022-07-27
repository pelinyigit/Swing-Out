using UnityEngine;
using TMPro;

[SelectionBase]
public class ProcessButtonController : MonoBehaviour
{
    public enum ButtonShape { Small, Big }

    public Doubling doubling;
    [SerializeField] TextMeshPro doublingText;
    public Material negative;
    public ButtonShape buttonShape;

    [HideInInspector]
    public Vector3 initialScale;

    private void Awake()
    {
        SetDoublingText();
        HandleInitialScale();
    }

    private void SetDoublingText()
    {
        doublingText.text = $"{(char)doubling.Prefix}{(int)doubling.Value}";

        if (doubling.Prefix == DoublingPrefix.Minus || doubling.Prefix == DoublingPrefix.Divide)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

            Material[] materials = new Material[skinnedMeshRenderer.materials.Length];

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = skinnedMeshRenderer.materials[i];
            }

            materials[1] = negative;

            skinnedMeshRenderer.materials = materials;
        }
    }

    private void HandleInitialScale()
    {
        if (buttonShape == ButtonShape.Small)
        {
            initialScale = Vector3.one;
        }
        else if (buttonShape == ButtonShape.Big)
        {
            initialScale = new Vector3(2, 2, 1);
        }
    }
}