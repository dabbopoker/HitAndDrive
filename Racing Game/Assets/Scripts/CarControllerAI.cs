using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControllerAI : MonoBehaviour
{
    [SerializeField] Rigidbody carRB;
    [SerializeField] Rigidbody sphereRB;

    [SerializeField] float speed;
    //[SerializeField] float revSpeed;
    [SerializeField] float turnSpeed;
    [SerializeField] LayerMask groundLayer;

    public float moveInput;
    public float turnInput;
    private bool isCarGrounded;

    private float normalDrag;
    [SerializeField] float modifiedDrag;

    [SerializeField] float alignToGroundTime;

    void Start()
    {
        // Detach Sphere from car
        sphereRB.transform.parent = null;
        carRB.transform.parent = null;

        normalDrag = sphereRB.drag;
    }

    public void setInputs(float turn, float move)
    {
        turnInput = turn;
        moveInput = move;
    }

    void Update()
    {
        // Get Input
        //moveInput = Input.GetAxisRaw("Vertical");
        //turnInput = Input.GetAxisRaw("Horizontal");

        // Calculate Turning Rotation
        float newRot = turnInput * turnSpeed * Time.deltaTime;

        if (isCarGrounded)
            transform.Rotate(0, newRot, 0, Space.World);

        // Set Cars Position to Our Sphere
        transform.position = sphereRB.transform.position;

        // Raycast to the ground and get normal to align car with it.
        RaycastHit hit;
        isCarGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 0.5f, groundLayer);

        // Rotate Car to align with ground
        Quaternion toRotateTo = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotateTo, alignToGroundTime * Time.deltaTime);

        // Calculate Movement Direction
        //moveInput *= moveInput > 0 ? fwdSpeed : revSpeed;

        // Calculate Drag
        sphereRB.drag = isCarGrounded ? normalDrag : modifiedDrag;
    }

    private void FixedUpdate()
    {
        if (isCarGrounded)
            sphereRB.AddForce(transform.forward * speed, ForceMode.Acceleration); // Add Movement
        else
            sphereRB.AddForce(transform.up * -200f); // Add Gravity

        if (carRB != null)
        {
            carRB.MoveRotation(transform.rotation);
        }
    }
}
