using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float lookSpeedH = 2f;

    [SerializeField]
    private float lookSpeedV = 2f;

    [SerializeField]
    private float zoomSpeed = 2f;

    [SerializeField]
    private float dragSpeed = 3f;

    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        // Initialize the correct initial rotation
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    void Update()
    {
            //Look around with right Mouse
            if (Input.GetMouseButton(1))
            {
                yaw += lookSpeedH * Input.GetAxis("Mouse X");
                pitch -= lookSpeedV * Input.GetAxis("Mouse Y");

                transform.eulerAngles = new Vector3(pitch, yaw, 0f);
            }

            //Go Forward
            if (Input.GetKey("w")) 
            {
                transform.Translate(0, 0, 0.1f * zoomSpeed, Space.Self);
            }
            //Go Backwards
            if (Input.GetKey("s"))
            {
                transform.Translate(0, 0, 0.1f * -zoomSpeed, Space.Self);
            }
            //Go Left
            if (Input.GetKey("a"))
            {
                transform.Translate(0.1f * -zoomSpeed, 0, 0, Space.Self);
            }
            //Go Right
            if (Input.GetKey("d"))
            {
                transform.Translate(0.1f * zoomSpeed, 0, 0, Space.Self);
            }

    }
}
