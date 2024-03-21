using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public int currentCatIndex = 0;
    public GameObject[] catModels;
    public CatBlueprinnt[] cats;

    void Start()
    {
        PlayerPrefs.SetInt("NumberOfCoins", 0);

        foreach (CatBlueprinnt cat in cats)
        {
            cat.isUnlocked = true;
        }

        currentCatIndex = PlayerPrefs.GetInt("SelectedCat", 0);
        foreach (GameObject cat in catModels)
        {
            cat.SetActive(false);
        }
        catModels[currentCatIndex].SetActive(true);
    }

    void Update()
    {
        UpdateUI();
    }

    public void ChangeNext()
    {
        catModels[currentCatIndex].SetActive(false);

        currentCatIndex++;
        if (currentCatIndex == catModels.Length)
            currentCatIndex = 0;

        catModels[currentCatIndex].SetActive(true);
        CatBlueprinnt c = cats[currentCatIndex];

        PlayerPrefs.SetInt("SelectedCat", currentCatIndex);
    }

    public void ChangePrevious()
    {
        catModels[currentCatIndex].SetActive(false);

        currentCatIndex--;
        if (currentCatIndex == -1)
            currentCatIndex = catModels.Length - 1;

        catModels[currentCatIndex].SetActive(true);

        PlayerPrefs.SetInt("SelectedCat", currentCatIndex);
    }

    public void UnlockCat()
    {
        // Remove this method as it's no longer needed
    }

    private void UpdateUI()
    {
        Debug.Log("currentCatIndex: " + currentCatIndex);
        Debug.Log("catModels.Length: " + catModels.Length);

        if (currentCatIndex < 0 || currentCatIndex >= catModels.Length)
        {
            currentCatIndex = 0;
        }

        CatBlueprinnt c = cats[currentCatIndex];

        int currentCoinBalance = PlayerPrefs.GetInt("NumberOfCoins", 0);
    }
}
