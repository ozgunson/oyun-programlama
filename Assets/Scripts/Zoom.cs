using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Zoom : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook freeLookCamera;
    [SerializeField] private float zoomSpeed = 1f;
    private float minZoom = 15f;
    private float maxZoom = 60f;

    private void Update()
    {
        
        float zoom = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        freeLookCamera.m_Lens.FieldOfView += zoom;

        // Minimum ve maksimum yakýnlaþtýrma mesafeleri
        freeLookCamera.m_Lens.FieldOfView = Mathf.Clamp(freeLookCamera.m_Lens.FieldOfView, minZoom, maxZoom);

    }


}

