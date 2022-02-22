using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiCarPath : MonoBehaviour
{

    [SerializeField] Transform targetPositionTransform;
    [SerializeField] Transform goal;

    [SerializeField] Transform []linePoints;

    CarControllerAI carControllerAI;
    Vector3 targetPosition;

    NavMeshAgent agent;
    NavMeshPath path;
    
    [SerializeField] float nextPointReachDistance = 5f;
    //[SerializeField] PositionInRace pos;
    public int lineIndex = 0;
    private void Awake()
    {
        carControllerAI = GetComponent<CarControllerAI>();
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        goal.position = linePoints[0].position;
        lineIndex = 0;
    }

    
    private void Update()
    {
        NavMesh.CalculatePath(transform.position, goal.position, NavMesh.AllAreas, path);
        agent.SetPath(path);
        if(agent.path.corners.Length == 1)
        {
            targetPositionTransform.position = agent.path.corners[0];
        }
        else
        {
            targetPositionTransform.position = agent.path.corners[1];
        }
        
        #region drawDebugLine
      // for (int i = 0; i < path.corners.Length - 1; i++)
      //     Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        #endregion

        float goalDistance = Vector3.Distance(transform.position, goal.position);
        
        if(goalDistance < nextPointReachDistance)
        {
            lineIndex++;
            lineIndex = System.Convert.ToInt32(Mathf.Repeat(lineIndex, linePoints.Length));
            goal.position = linePoints[lineIndex].position;
        }
        //print(path.corners.Length);
        

        SetTargetPosition(targetPositionTransform.position);
        
        
        float moveAmount = 0;
        float turnAmount = 0;

        Vector3 dirToMovePosition = (targetPosition - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, dirToMovePosition);


        if(dot > 0)
        {
            moveAmount = 1f;
        }
        else
        {
            moveAmount = -1f;
        }

        float angleToDir = Vector3.SignedAngle(transform.forward, dirToMovePosition, Vector3.up);

        if(angleToDir > 0)
        {
            turnAmount = 1f;
        }
        else
        {
            turnAmount = -1f;
        }

        carControllerAI.setInputs(turnAmount, moveAmount);
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }


}
