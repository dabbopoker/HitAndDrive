using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pivot : MonoBehaviour
{
    [SerializeField]
    GameObject standardCarModel, truckModel, bigTruckModel;
    [SerializeField]
    AudioSource motor;
    [SerializeField]
    AudioClip truckSound;
    Vector3 savedCanvasPos;
    [SerializeField]
    Transform canvas;
    // Start is called before the first frame update
    public void CanvasToPos()
    {
        canvas.localPosition = savedCanvasPos;
    }
    public void ChangeToTruck()
    {
        standardCarModel.SetActive(false);
        truckModel.SetActive(true);
    }

    public void ChangeToBigTruck()
    {
        truckModel.SetActive(false);
        bigTruckModel.SetActive(true);
    }

    public void SaveCanvasPos()
    {
        savedCanvasPos = canvas.localPosition;
       // StartCoroutine(stayatpos());
    }

    IEnumerator stayatpos()
    {
        while(false == false)
        {
            yield return null;
            print("dasdadgaudiwadGU");
            CanvasToPos();

        }
    }
}
