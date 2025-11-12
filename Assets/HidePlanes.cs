using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class HidePlanes : MonoBehaviour
{
    public ARPlaneManager planeManager;

    void OnEnable()
    {
        planeManager.planesChanged += OnPlanesChanged;
    }

    void OnDisable()
    {
        planeManager.planesChanged -= OnPlanesChanged;
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (var plane in planeManager.trackables)
        {
            var renderer = plane.GetComponent<MeshRenderer>();
            if (renderer != null)
                renderer.enabled = false;
        }
    }
}
