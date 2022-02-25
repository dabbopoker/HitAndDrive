using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    private Vector3 playerOffset;

    private Vector3 currentOffset;
    [SerializeField] public Vector3 offset = Vector3.zero;

    private void Start()
    {
        playerOffset = transform.position - target.position;

        currentOffset = playerOffset;
    }

    void Update()
    {
        //if (!player.isGrounded() || player.isStunned)
        //{
        //    return;
        //}

        if (target != null)
        {
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, currentOffset.z + target.position.z);
            transform.position = newPosition + offset;
        }
    }
}
