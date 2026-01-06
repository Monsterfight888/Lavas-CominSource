using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform Player;
    bool first = true;
    readonly float cameraLag = .1f; // must be bigger than 0 and smaller than 1.  smaller numbers are more laggy
    void FixedUpdate()
    {
        if (first)
        {
            transform.position =
                new Vector3(Player.position.x, Player.position.y, transform.position.z);
            first = false;
        }
        else
        {

            float inverseLag = 1.0f - cameraLag;
            transform.position =
                new Vector3(Player.position.x * cameraLag + transform.position.x * inverseLag,
                            Player.position.y * cameraLag + transform.position.y * inverseLag,
                            transform.position.z);

        }

    }
}