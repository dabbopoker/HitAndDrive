using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
public class PositionInRace : MonoBehaviour
{
    public string racerName = "CPURacer";
    public int position;
    public float remainingDistance;
    NavMeshAgent agent;
    NavMeshPath winPath;
    public int lineIndex;
    public int realLineIndex;
    float reloadPathDistance;
    [SerializeField] Transform currentTarget;
    [SerializeField] TextMeshProUGUI posText;
    [SerializeField] float posSmooth = 0.5f;
    float changeposTimer;
    int posStore;

    // Start is called before the first frame update
    void Start()
    {
        posStore = position;
        lineIndex = 0;
        realLineIndex = 0;
        
        TrackManager.instance.racers.Add(this);
        reloadPathDistance = TrackManager.instance.reloadPathDistance;
        //carPath = transform.GetComponentInParent<AiCarPath>();
        agent = GetComponent<NavMeshAgent>();
        winPath = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, currentTarget.position, NavMesh.AllAreas, winPath);
        agent.SetPath(winPath);
    }

    // Update is called once per frame
    void Update()
    {
        winPath = new NavMeshPath();    
        transform.localPosition = new Vector3(0, 0, 1.7f);
        //lineIndex = carPath.lineIndex;
        NavMesh.CalculatePath(transform.position, currentTarget.position, NavMesh.AllAreas, winPath);
        agent.SetPath(winPath);
        //remainingDistance = winPath.corners.Length;
        #region drawDebugLine
        remainingDistance = 0;
        for (int i = 0; i < winPath.corners.Length - 1; i++)
        {
            Debug.DrawLine(winPath.corners[i], winPath.corners[i + 1], Color.green);
            remainingDistance += Vector3.Distance(winPath.corners[i], winPath.corners[i + 1]);
        }
            
        #endregion
        
        if(remainingDistance <= reloadPathDistance && agent.hasPath)
        {
            lineIndex++;
            realLineIndex++;
            lineIndex = System.Convert.ToInt32(Mathf.Repeat(lineIndex, TrackManager.instance.trackPoints.Length));
            currentTarget.position = TrackManager.instance.trackPoints[lineIndex].position;

        }
        //set new currenttargetPosition
        currentTarget.position = TrackManager.instance.trackPoints[lineIndex].position;


        if (posText != null)
        {
            if (posStore == 1)
            {
                posText.text = posStore.ToString() + "st";
            }
            else if (posStore == 2)
            {
                posText.text = posStore.ToString() + "nd";
            }
            else if (posStore == 3)
            {
                posText.text = posStore.ToString() + "rd";
            }
            else
            {
                posText.text = posStore.ToString() + "th";
            }
        }

        if(posStore != position)
        {
            changeposTimer -= Time.deltaTime;
            if(changeposTimer <= 0)
            {
                posStore = position;
            }
        }
        else
        {
            changeposTimer = posSmooth;
        }

        




    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(currentTarget.position, 1.4f);
    }


}
