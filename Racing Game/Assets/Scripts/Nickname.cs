using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nickname : MonoBehaviour
{
    new Transform camera;
    [SerializeField] Transform container;

    private void Start()
    {
        camera = FindObjectOfType<Camera>().GetComponent<Transform>();
        //transform.parent = null;
    }

    private void Update()
    {
        transform.position = container.transform.position;
        transform.LookAt(camera);
    }
}
