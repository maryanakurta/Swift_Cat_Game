using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDistance : MonoBehaviour
{
    public GameObject disDisplay;
    public GameObject disEndDisplay;
    public GameObject recordDisplay;
    public int disRun;
    public int recordDistance;
    public bool addingDis = false;
    public float disDelay = 0.35f;

    void Start()
    {
        recordDistance = PlayerPrefs.GetInt("RecordDistance", 0);
        UpdateRecordDisplay();
    }

    void Update()
    {
        if (addingDis == false)
        {
            addingDis = true;
            StartCoroutine(AddingDis());
        }
    }

    IEnumerator AddingDis()
    {
        disRun += 1;
        disDisplay.GetComponent<Text>().text = "" + disRun;
        disEndDisplay.GetComponent<Text>().text = "" + disRun;

        if (disRun > recordDistance)
        {
            recordDistance = disRun;
            PlayerPrefs.SetInt("RecordDistance", recordDistance);
            UpdateRecordDisplay();
        }

        yield return new WaitForSeconds(disDelay);
        addingDis = false;
    }

    void UpdateRecordDisplay()
    {
        if (recordDisplay != null && recordDisplay.GetComponent<Text>() != null)
        {
            recordDisplay.GetComponent<Text>().text = "YOUR RECORD: " + recordDistance;
        }
    }
}
