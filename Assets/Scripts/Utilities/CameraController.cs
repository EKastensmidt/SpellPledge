using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PostProcessVolume postProcessVolume;
    private ColorGrading colorGrading;

    private void Start()
    {
        postProcessVolume.profile.TryGetSettings(out colorGrading);
    }

    public void SetColorGradingOnOff(bool value)
    {
        colorGrading.active = value;
    }
}
