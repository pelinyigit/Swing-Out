using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CameraData cameraData;
    [SerializeField] Transform pipeEnd;
    [SerializeField] TumblerController tumbler;

    [Header("Debug Values/MAKE THESE PRIVATE LATER")]
    public bool canMoveSideways = false;

    private void Awake()
    {
        InitStartPosition();
        InitStartRotation();
    }

    private void LateUpdate()
    {
        // if (canMoveSideways)
        {
            MoveSideways();
        }
        
        
    }

    private void OnDrawGizmos()
    {
        //InitStartPosition();
        //InitStartRotation();
    }

    private void InitStartPosition()
    {
        transform.position = cameraData.InitialPosition;
    }

    private void InitStartRotation()
    {
        transform.rotation = Quaternion.Euler(cameraData.InitialRotation);
    }

    public void MoveSideways()
    {
        var nextPosition = new Vector3(Mathf.Clamp(tumbler.transform.position.x, -1f, 1f) * cameraData.MoveRange, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, nextPosition, Time.deltaTime * cameraData.LerpSpeed);
    }

    public void DoZoomOut()
    {
        float nextCamPosY = transform.position.y + cameraData.ZoomInOutYDamper;
        float nextCamPosZ = transform.position.z + -cameraData.ZoomInOutZDamper;
        nextCamPosY = Mathf.Clamp(nextCamPosY, cameraData.ZoomInOutRangeMinY, cameraData.ZoomInOutRangeMaxY);
        nextCamPosZ = Mathf.Clamp(nextCamPosZ, cameraData.ZoomInOutRangeMinZ, cameraData.ZoomInOutRangeMaxZ);
        var nextPosition = new Vector3(tumbler.transform.position.x, nextCamPosY, nextCamPosZ);
        transform.DOMoveY(nextCamPosY, cameraData.ZoomOutCompletionTime)
            .SetEase(Ease.Linear)
            .OnStart(delegate
            {
                if (GameContanier.Instance.pipeEnd.currentPipeCount > 4)
                {
                    canMoveSideways = true;
                }
                transform.DOMoveZ(nextCamPosZ, cameraData.ZoomOutCompletionTime);
            })
            .OnComplete(delegate
            {

                #region OLD ZOOMOUT LOGIC
                //if (nextCamPosY == cameraData.ZoomInOutRangeMaxY)
                //{
                //    StartCoroutine(WaitRoutine());
                //}
                #endregion
            });
    }

    public void DoZoomIn()
    {
        float nextCamPosY = transform.position.y - cameraData.ZoomInOutYDamper;
        float nextCamPosZ = transform.position.z - -cameraData.ZoomInOutZDamper;
        nextCamPosY = Mathf.Clamp(nextCamPosY, cameraData.ZoomInOutRangeMinY, cameraData.ZoomInOutRangeMaxY);
        nextCamPosZ = Mathf.Clamp(nextCamPosZ, cameraData.ZoomInOutRangeMinZ, cameraData.ZoomInOutRangeMaxZ);
        var nextPosition = new Vector3(0f, nextCamPosY, nextCamPosZ);
        transform.DOMoveY(nextCamPosY, cameraData.ZoomOutCompletionTime)
            .SetEase(Ease.Linear)
            .OnStart(delegate
            {
                transform.DOMoveZ(nextCamPosZ, cameraData.ZoomOutCompletionTime);
            })
            .OnComplete(delegate
            {
                // if (GameContanier.Instance.pipeEnd.currentPipeCount <= 4)
                // {
                //     canMoveSideways = false;
                //     transform.DOMove(cameraData.InitialPosition, 1f);
                // }

                #region OLD ZOOMINLOGIC
                //if (GameContanier.Instance.pipeEnd.currentPipeCount >= 3)
                //{
                //    //transform.DOMoveX(0, 1f);
                //    nextPosition = new Vector3(transform.position.x, cameraData.ZoomInOutRangeMinY, cameraData.ZoomInOutRangeMaxZ);
                //    transform.DOMove(nextPosition, 1f).SetEase(Ease.Linear)
                //    .OnComplete(delegate
                //    {
                //        canMoveSideways = true;
                //    });
                //}
                //else if (GameContanier.Instance.pipeEnd.currentPipeCount == 1)
                //{
                //    nextPosition = new Vector3(transform.position.x, cameraData.ZoomInOutRangeMinY, cameraData.ZoomInOutRangeMaxZ);
                //    transform.DOMove(nextPosition, 1f).SetEase(Ease.Linear);
                //}
                #endregion
            });
    }

    private IEnumerator WaitRoutine()
    {
        yield return new WaitForSeconds(.5f);
        canMoveSideways = true;
    }
}
