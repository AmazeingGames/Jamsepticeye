using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MainMenuAnimator : MonoBehaviour
{
    [SerializeField] float initialDelay;
    [SerializeField] float timeBetweenEach;
    [SerializeField] List<GameObject> objectsToEnableSequentially;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("gs");
    }

    private void OnEnable()
    {
        foreach (var gameObject in objectsToEnableSequentially)
            gameObject.SetActive(false);
        StartCoroutine(EnableObjectsSequentially());
    }

    IEnumerator EnableObjectsSequentially()
    {
        yield return new WaitForSeconds(initialDelay);

        for (int i = 0; i < objectsToEnableSequentially.Count; i++)
        {
            GameObject gameObject = objectsToEnableSequentially[i];
            float timeDelay = i * this.timeBetweenEach;
            StartCoroutine(EnableObject_CO(gameObject, timeDelay));
        }
    }

    IEnumerator EnableObject_CO(GameObject gameObject, float delay_Seconds)
    {
        yield return new WaitForSeconds(delay_Seconds);

        gameObject.SetActive(true);
    }
}
