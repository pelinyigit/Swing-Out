using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;

public class RingSelection : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Color originalColor;
    private GameObject selectedRing;
    
    void Start()
    {
        //SelectRing(1);
    }
    
    public void SelectRing(int ringCounter)
    {
        ResetColor();
        
        selectedRing = transform.GetChild(ringCounter - 1).gameObject;
        meshRenderer = selectedRing.GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.color;
        
        ColorChange();
    }

    private void ColorChange()
    {
        meshRenderer.material.DOColor(Color.black, 1f).OnComplete(ColorChangeBack);
    }

    private void ColorChangeBack()
    {
        meshRenderer.material.DOColor(originalColor, 1f).OnComplete(ColorChange);
    }

    private void ResetColor()
    {
        if (selectedRing != null)
        {
            meshRenderer.material.DOKill();
            meshRenderer.material.DOColor(originalColor, 1f);
        }
    }
}
