using UnityEngine;

/*
 * Uncomment below after GameAnalyticsSDK initialized.
 */

using GameAnalyticsSDK;

public class GameAnalyticsScr : MonoBehaviour
{


    private void Awake()
    {
        GameAnalytics.Initialize();
    }


}
