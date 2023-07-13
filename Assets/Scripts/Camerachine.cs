using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class Camerachine : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera virtualCamera;
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private float targetZoom;
    [SerializeField]
    private float zoomFactor = 100f;
    [SerializeField]
    private float zoomLerpSpeed = 10f;

    private float yVelocity = 0.0f;

    private Vector3 dragOrigin;

    private void Start()
    {
        // cam = Camera;
        targetZoom = 500f;
    }

    private void Update()
    {

        float scrollData;
        scrollData = Input.GetAxis("Mouse ScrollWheel");

        targetZoom -= scrollData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, 4.5f, 1000f);

        virtualCamera.m_Lens.OrthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref yVelocity, Time.deltaTime * zoomLerpSpeed); ;

        PanCamera();
    }

    private void PanCamera()
    {

        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = dragOrigin - mousePosition;

            //difference.x = Mathf.Clamp()

            virtualCamera.transform.position += difference;
        }
    }
}
