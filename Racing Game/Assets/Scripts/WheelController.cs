using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{

    public GameObject[] wheelsToRotate;
    public TrailRenderer[] trails;
    public ParticleSystem[] tireSmokes;

    public float rotationSpeed;
    private Animator anim;

    public AudioSource tireScreechSound;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalAxis = Input.GetAxisRaw("Horizontal");

        foreach (var wheel in wheelsToRotate)
        {
            wheel.transform.Rotate(Time.deltaTime * rotationSpeed, 0, 0, Space.Self);
        }

        if (horizontalAxis > 0)
        {
            //turning right
            anim.SetBool("goingLeft", false);
            anim.SetBool("goingRight", true);
        }
        else if (horizontalAxis < 0)
        {
            //turning left
            anim.SetBool("goingRight", false);
            anim.SetBool("goingLeft", true);
        }
        else
        {
            //must be going straight
            anim.SetBool("goingRight", false);
            anim.SetBool("goingLeft", false);
        }

        if (horizontalAxis != 0)
        {
            foreach (var trail in trails)
            {
                trail.emitting = true;
            }

            foreach (var tireSmoke in tireSmokes)
            {
                tireSmoke.Play();
            }

            if (!tireScreechSound.isPlaying)
            {
                tireScreechSound.Play();
            }
        }
        else
        {
            foreach (var trail in trails)
            {
                trail.emitting = false;
            }

            foreach (var tireSmoke in tireSmokes)
            {
                tireSmoke.Stop();
            }

            tireScreechSound.Stop();
        }
    }
}