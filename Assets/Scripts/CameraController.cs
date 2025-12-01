using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 8f;          
    public float zoomSpeed = 5f;          
    public float minZoom = 3f;            
    public float maxZoom = 15f;           

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        MoveCamera();
        ZoomCamera();
    }

    void MoveCamera()
    {
        float h = Input.GetAxisRaw("Horizontal");  
        float v = Input.GetAxisRaw("Vertical");    

        Vector3 move = new Vector3(h, v, 0).normalized;
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    void ZoomCamera()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }
}
