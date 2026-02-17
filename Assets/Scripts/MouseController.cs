using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    private float speedHor = 3;
    private float speedVer = 3;

    private float minVert = -45;
    private float maxVert = 45;

    private float rotationX;
    private float rotationY;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        rotationX -= Input.GetAxis("Mouse Y") * speedVer;
        rotationX = Mathf.Clamp(rotationX, minVert, maxVert);

        float delta = Input.GetAxis("Mouse X") * speedHor;
        rotationY = transform.localEulerAngles.y + delta;

        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
    }
}