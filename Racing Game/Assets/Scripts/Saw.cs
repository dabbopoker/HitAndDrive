using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    [SerializeField] float rotationSpeed;

    [SerializeField] Vector3 pos1;
    [SerializeField] Vector3 pos2;
    [SerializeField] float speed;

    void Update()
    {
        transform.Rotate(0, Time.deltaTime * rotationSpeed, 0, Space.Self);

        transform.position = Vector3.Lerp(pos1, pos2, Mathf.PingPong(Time.time * speed, 1.0f));
    }
}
