using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseReactSlow : MonoBehaviour
{
    public float sensitivity = 0.2f;
    [Range(0f, 1f)]
    public float speed = 0.2f;
    float rawPosX;
    float rawPosY;
    float rotX;
    float rotY;
    void Update()
    {
        rawPosX = Input.mousePosition.y;
        rawPosY = Input.mousePosition.x;

        rotX = - (rawPosX + Screen.height * -0.5f) * sensitivity;
        rotY = (rawPosY + Screen.width * -0.5f) * sensitivity;

        if (rotY > 25)
        {
            rotY = 25;
        }
        if (rotY < -25)
        {
            rotY = -25;
        }
        if (rotX > 25)
        {
            rotX = 25;
        }
        if (rotX < -25)
        {
            rotX = -25;
        }
        transform.rotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(rotX, rotY, 0), Time.deltaTime * speed);
    }
}