using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [Header("Level stuff")]
    [SerializeField] int startLevel = 0;
    [SerializeField] public int currentlevel;
    [SerializeField] float showLevelTime;
    float showaddedLevelCountdown;
    [SerializeField] GameObject carLevelText;
    [SerializeField] string prefix = "Lvl. ";
    [SerializeField] AnimationCurve textPopupCurve;
    [SerializeField] float curveMultiplier;
    Coroutine showupLevel;
    [SerializeField] Transform camera;
    Quaternion lvltxtwrldrot;
    int currentPlus;
    [SerializeField] TextMeshProUGUI lvlText;
    [SerializeField] float textRefreshSpeed;
    Coroutine updateTxtCoroutine;
    [SerializeField] float carSizePlus = 0.1f;
    [SerializeField] float currentCarSize;
    [SerializeField] public Transform car;
    [SerializeField] GameObject standardCarModel;
    public List<CarChange> carChanges;
    List<GameObject> usedCarModels = new List<GameObject>();

    List<TextMeshPro> tmps = new List<TextMeshPro>();
    // Start is called before the first frame update
    void Awake()
    {
        
        currentCarSize = car.localScale.y;
        lvltxtwrldrot = carLevelText.transform.rotation;
        currentlevel = startLevel;
        if(instance != null)
        {
            Debug.LogWarning("Too many Trackmanagers in Scene!");
            return;
        }
        instance = this;
    }
    private void Start()
    {
        usedCarModels.Add(standardCarModel);
        

    }
    public void addLevel(int lvl)
    {
        currentCarSize += carSizePlus;
        car.localScale = Vector3.one * currentCarSize; 
        currentlevel += lvl;
        if(updateTxtCoroutine == null)
        {
            updateTxtCoroutine = StartCoroutine(updateLevelTxt());
        }
        else
        {
            StopCoroutine(updateTxtCoroutine);
            updateTxtCoroutine = StartCoroutine(updateLevelTxt());
        }
        
        currentPlus += lvl;
        showaddedLevelCountdown = showLevelTime;
        TextMeshPro tmp = Instantiate(carLevelText, car).GetComponent<TextMeshPro>();
        tmp.text = prefix + lvl.ToString();
        tmp.sortingOrder = currentlevel;
        StartCoroutine(holdRotation(tmp.transform));
        Destroy(tmp, 5);

        if (showupLevel == null)
        {
            showupLevel = StartCoroutine(ShowUpText());
        }
        bool carChanged = false;
        foreach (CarChange c in carChanges)
        {
            if(c.levelToReach <= currentlevel && c.changed == false)
            {
                //changeCarModel(c.newCarModel);
                c.changed = true;
                carChanged = true;
            }
        }
        Animator carAnim = car.GetComponent<Animator>();
        if(carChanged)
        {
            if (currentlevel >= 30)
            {
                carAnim.SetFloat("Car", 2);
                carAnim.Play("ChangeToBigTruck");
            }
            else
            carAnim.Play("LevelUpCar");
        }
        else
        {
            if(currentlevel >= 30)
            {
                carAnim.SetFloat("Car", 2);
                carAnim.Play("bigtrucklvlupanim");
            }
            else if(currentlevel >= 10)
            {
                carAnim.SetFloat("Car", 1);
                carAnim.Play("LevelUpTruck");
            }
            else
            {
                carAnim.SetFloat("Car", 0);
                carAnim.Play("LevelUp");
            }
            
        }

    }

    void changeCarModel(GameObject model)
    {
        foreach(GameObject g in usedCarModels)
        {
            g.SetActive(false);
        }
        model.SetActive(true);
        usedCarModels.Add(model);

    }
    IEnumerator updateLevelTxt()
    {
        Transform t = lvlText.transform;
        while(t.localScale.y > 0)
        {
            t.localScale -= Vector3.up * Time.deltaTime * textRefreshSpeed;
            yield return new WaitForEndOfFrame();
        }
        lvlText.text = "Lv." + currentlevel.ToString();

        while (t.localScale.y < 1)
        {
            t.localScale += Vector3.up * Time.deltaTime * textRefreshSpeed;
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }

    IEnumerator ShowUpText()
    {
        bool stop = false;
        float time = 0;
        while(stop == false)
        {
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime * curveMultiplier;
            carLevelText.transform.localScale = Vector3.one * textPopupCurve.Evaluate(time);
            if(time >= 1)
            {
                carLevelText.transform.localScale = Vector3.one;
                stop = true;
                showaddedLevelCountdown = showLevelTime;
            }
        }
        while(showaddedLevelCountdown > 0)
        {
            showaddedLevelCountdown -= Time.deltaTime;
            yield return new WaitForEndOfFrame();

        }
        time = 1;
        while(stop)
        {
            time -= Time.deltaTime * curveMultiplier;
            carLevelText.transform.localScale = Vector3.one * time;
            yield return new WaitForEndOfFrame();

            if (time <= 0f)
            {
                carLevelText.transform.localScale = Vector3.zero;
                stop = false;
            }
        }
        currentPlus = 0;
        showupLevel = null;
        yield break;
    }
    IEnumerator holdRotation(Transform t)
    {
        while(t !=null)
        {
            t.rotation = lvltxtwrldrot;
            yield return new WaitForEndOfFrame();
        }
        yield break;
        
        
    }

    private void LateUpdate()
    {
        
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
