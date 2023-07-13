using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private float targetZoom;
    [SerializeField]
    private float zoomFactor = 10f;
    [SerializeField]
    private float zoomLerpSpeed = 10f;

    private float yVelocity = 0.0f;

    private Vector3 dragOrigin;

    private void Start()
    {
        // cam = Camera;
        targetZoom = cam.orthographicSize;
    }

    private void Update()
    {
        float scrollData;
        scrollData = Input.GetAxis("Mouse ScrollWheel");

        targetZoom -= scrollData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, 4.5f, 24f);
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref yVelocity, Time.deltaTime * zoomLerpSpeed);
        PanCamera(targetZoom);
    }

    private void PanCamera(float zoomLevel)
    {
        if (zoomLevel >= 24f)
        {
            cam.transform.position = new Vector3(31.5f, 23.5f, -10f);
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = dragOrigin - mousePosition;

            //difference.x = Mathf.Clamp()

            cam.transform.position += difference;
        }
    }


}
