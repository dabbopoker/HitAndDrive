using UnityEngine;
[System.Serializable]
public class CarChange 
{
    public int levelToReach;
    public GameObject newCarModel;
    [HideInInspector] public bool changed = false;
}
