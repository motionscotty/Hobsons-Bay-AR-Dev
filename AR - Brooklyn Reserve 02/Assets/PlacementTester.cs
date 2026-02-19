using UnityEngine;
using Imagine.WebAR;

public class PlacementTester : MonoBehaviour
{
    private WorldTracker tracker;

    void Start()
    {
        tracker = GetComponent<WorldTracker>();
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 160, 50), "PLACE ORIGIN"))
        {
            tracker.PlaceOrigin();
        }

        if (GUI.Button(new Rect(10, 70, 160, 50), "RESET ORIGIN"))
        {
            tracker.ResetOrigin();
        }
    }
}
