using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrian : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] int lvl = 1;
    [SerializeField] int lvlToKill = 0;
    Rigidbody rb;

    [HideInInspector] public bool collidedOnce = false;
    float force;
    private void Start()
    {
        foreach(BoxCollider b in GetComponents<BoxCollider>())
        {
            b.enabled = true;
        }
        
        force = TrackManager.instance.carCrashPower;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 && TrackManager.instance.currentlevel >= lvlToKill)
        {
            transform.parent.transform.parent.transform.parent.GetChild(2).GetComponent<Canvas>().enabled = false;

            rb = GetComponent<Rigidbody>();

            animator.enabled = false;
            rb.isKinematic = false;

            //Update the killbar       
            TrackManager.instance.killValue += 1;

            if (!collidedOnce)
            {
                TrackManager.instance.addLevel(lvl);
                FindObjectOfType<AudioManager>().Play("Hit");
                //Apply force
                Vector3 dirToMove = (other.transform.position - transform.position).normalized;
                bool rightOrLeft = Vector3.SignedAngle(other.transform.forward, dirToMove, Vector3.up) < 0;
                Vector3 moveDir;
                if(rightOrLeft)
                {
                    moveDir = other.transform.forward + (other.transform.up + other.transform.right * 0.5f);
                }else
                {
                    moveDir = other.transform.forward + (other.transform.up * 0.5f) + (other.transform.right * -0.5f);
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
        if (collision.gameObject.layer == 8 && !collidedOnce && rb != null)
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
