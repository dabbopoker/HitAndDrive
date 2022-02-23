using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDestroy : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject collisionParticle;
    [SerializeField] GameObject explosionParticle;
    [SerializeField] GameObject emoji;
    [SerializeField] GameObject gameOverScreen;

    bool zooming;
    [SerializeField] GameObject speedParticle;
    [SerializeField] AudioSource engineSound;
    [SerializeField] GameObject car;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            Instantiate(collisionParticle, collision.GetContact(0).point, Quaternion.identity);
        }

        if (collision.gameObject.layer == 11)
        {
            Destroy(car);
            Instantiate(explosionParticle, gameObject.transform.position + new Vector3(0, 0.3f, 0), gameObject.transform.rotation);
            engineSound.Stop();
            FindObjectOfType<AudioManager>().Play("Fail");

            gameOverScreen.SetActive(true);
        }

        if (collision.gameObject.layer == 12)
        {
            Instantiate(collisionParticle, collision.GetContact(0).point, Quaternion.identity);
            Instantiate(emoji, collision.GetContact(0).point, transform.rotation * Quaternion.Euler(40f, 0, 0f));
        }

        if (collision.gameObject.layer == 13)
        {
            zooming = true;
        }
    }

    private void Update()
    {
        if (zooming)
        {
            StartCoroutine(LerpFunction(64, 0.3f));
            zooming = false;
        }
    }

    IEnumerator LerpFunction(float endValue, float duration)
    {
        StartCoroutine(LerpSound(2f, 0.3f));
        float time = 0;
        float startValue = mainCamera.fieldOfView;
        speedParticle.SetActive(true);
        while (time < duration)
        {
            mainCamera.fieldOfView = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        mainCamera.fieldOfView = endValue;
        
        yield return new WaitForSeconds(2.5f);

        time = 0;
        speedParticle.SetActive(false);
        while (time < duration)
        {
            mainCamera.fieldOfView = Mathf.Lerp(endValue, startValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        mainCamera.fieldOfView = startValue;
    }

    IEnumerator LerpSound(float endValue, float duration)
    {
        float time = 0;
        float startValue = engineSound.pitch;
        while (time < duration)
        {
            engineSound.pitch = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        engineSound.pitch = endValue;

        yield return new WaitForSeconds(2.5f);

        time = 0;
        while (time < duration)
        {
            engineSound.pitch = Mathf.Lerp(endValue, startValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        engineSound.pitch = startValue;
    }
}
