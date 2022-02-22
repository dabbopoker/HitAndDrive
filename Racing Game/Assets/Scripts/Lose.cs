using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lose : MonoBehaviour
{
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject racePositionText;
    [SerializeField] GameObject waterRiddle;    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Instantiate(waterRiddle, other.transform.position, Quaternion.Euler(-80, 0, 0));
            racePositionText.SetActive(false);
            gameOverScreen.SetActive(true);
        }
    }
}
