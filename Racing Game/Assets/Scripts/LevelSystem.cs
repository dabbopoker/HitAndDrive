using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSystem : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] Transform EndLine;
    [SerializeField] Slider slider;
    [SerializeField] GameObject finishLevel;

    bool hasFinished;

    float maxDistance;

    void Start()
    {
        maxDistance = getDistance();
    }

    void Update()
    {
        if (Player.position.z <= EndLine.position.z)
        {
            float distance = 1 - (getDistance() / maxDistance);
            SetProgress(distance);
        }
        else hasFinished = true;

        if (hasFinished == true)
        {
            FinishLevelUI();
        }
    }

    float getDistance()
    {
        return Vector3.Distance(Player.position, EndLine.position);
    }

    void SetProgress(float p)
    {
        slider.value = p;
    }

    void FinishLevelUI()
    {
        finishLevel.SetActive(true);
    }

    public void SaveLevel()
    {
        string activeScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("LevelSaved", activeScene);
        Debug.Log(activeScene);
        gameObject.SetActive(false);
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Menu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
