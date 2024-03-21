using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndRunSequence : MonoBehaviour
{
    public GameObject liveCoins;
    public GameObject liveDis;
    public GameObject endScreen;
    public GameObject fadeOut;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(EndSequence());
        }
    }

    IEnumerator EndSequence()
    {
        PlayerMove.canMove = false;
        yield return new WaitForSeconds(2);

        liveCoins.SetActive(false);
        liveDis.SetActive(false);
        endScreen.SetActive(true);

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(2);

        fadeOut.SetActive(true);

        yield return new WaitForSecondsRealtime(2);

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
