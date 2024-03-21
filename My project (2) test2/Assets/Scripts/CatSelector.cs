using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSelector : MonoBehaviour
{
    public int currentCatIndex = 0;
    public GameObject[] cats;

    void Start()
    {
        currentCatIndex = PlayerPrefs.GetInt("SelectedCat", 0);
        SetActiveCat(currentCatIndex);
    }

    private void SetActiveCat(int catIndex)
    {
        for (int i = 0; i < cats.Length; i++)
        {
            cats[i].SetActive(i == catIndex);
        }
    }

    public void ChangeCat(int newCatIndex)
    {
        currentCatIndex = newCatIndex;
        PlayerPrefs.SetInt("SelectedCat", currentCatIndex);
        SetActiveCat(currentCatIndex);
    }
}
