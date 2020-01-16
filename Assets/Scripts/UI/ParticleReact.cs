using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleReact : MonoBehaviour
{
    float moveX;
    float moveY;

    void Update()
    {
        if (GetComponent<Transform>().name == "Foreground")
        {
            moveX = -Input.mousePosition.x + (Screen.width);
            moveY = Input.mousePosition.y;
        }
        else if (GetComponent<Transform>().name == "Background")
        {
            moveX = Input.mousePosition.x;
            moveY = -Input.mousePosition.y + Screen.height;
        }


        transform.position = Vector2.Lerp(transform.position, new Vector2(moveX, moveY), Time.deltaTime * 0.1f);
    }
}
