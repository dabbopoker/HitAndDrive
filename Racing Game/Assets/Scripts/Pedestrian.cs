using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrian : MonoBehaviour
{
    [SerializeField] Animator animator;
    Rigidbody rb;

    bool collidedOnce = false;
    float force;
    private void Start()
    {
        force = TrackManager.instance.carCrashPower;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            rb = GetComponent<Rigidbody>();

            animator.enabled = false;
            rb.isKinematic = false;

            //Update the killbar       
            TrackManager.instance.killValue += 1;

            if (!collidedOnce)
            {
                print("yup;");
                FindObjectOfType<AudioManager>().Play("Hit");
                //Apply force
                Vector3 dirToMove = (other.transform.position - transform.position).normalized;
                bool rightOrLeft = Vector3.SignedAngle(other.transform.forward, dirToMove, Vector3.up) < 0;
                Vector3 moveDir;
                if(rightOrLeft)
                {
                    moveDir = other.transform.right;
                }else
                {
                    moveDir = other.transform.right * -1;
                }
                Vector3 forceDir = moveDir;
                rb.mass = 1f;
                rb.AddForce(forceDir * force, ForceMode.Impulse);
                
                collidedOnce = true;
            }

            //StartCoroutine(DestroySelf());
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Vector3 forceDir = collision.transform.forward + collision.transform.up;
            rb.mass = 1f;
            rb.AddForce(forceDir * force, ForceMode.Impulse);
        }
    }

    //IEnumerator DestroySelf()
    //{
    //    yield return new WaitForSeconds(5f);

    //    Destroy(gameObject.transform.parent.transform.parent.transform.parent.gameObject);
    //}
}
