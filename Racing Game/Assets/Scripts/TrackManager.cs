using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackManager : MonoBehaviour
{
    public Transform[] trackPoints;

    public float reloadPathDistance;

    public static TrackManager instance;

    public List<PositionInRace> racers;

    [SerializeField] Image killBar;
    [SerializeField] float levelLongness = 5;
    [SerializeField] float decreaseMultiplicator;
    public float killValue;
    bool fever;
    float feverBurnTime;
    public float carCrashPower = 5;


    // Start is called before the first frame update
    void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Too many Trackmanagers in Scene!");
            return;
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //print(fever);
        int posi = racers.Count;
        racers.Sort(SortByPos);
        foreach(PositionInRace p in racers)
        {
            //print(p.name);
            p.position = posi;
            posi--;
        }
        if(killValue >= levelLongness && !fever)
        {
            feverBurnTime = killValue;
            fever = true;
        }
        if(fever)
        {
            feverBurnTime -= Time.deltaTime * decreaseMultiplicator;
            fever = feverBurnTime > 0;
            killBar.fillAmount = feverBurnTime / levelLongness;
            killValue = feverBurnTime;
        }
        else
        {
            killBar.fillAmount = killValue / levelLongness;
            killValue -= Time.deltaTime * decreaseMultiplicator;
            killValue = Mathf.Clamp(killValue, 0, levelLongness);
        }
            
        
        

    }

    static int SortByPos(PositionInRace r1, PositionInRace r2)
    {
        if(r1.realLineIndex.CompareTo(r2.realLineIndex) == 0)
        {
            return r1.remainingDistance.CompareTo(r2.remainingDistance) * -1;
        }
        return r1.realLineIndex.CompareTo(r2.realLineIndex);
    }
}
