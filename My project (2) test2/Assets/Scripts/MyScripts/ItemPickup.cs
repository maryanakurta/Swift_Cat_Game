using System.Collections;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public float flyingObjectHeight = 1000f;
    public float flyingDuration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FlyAndDisappear());
        }
    }

    IEnumerator FlyAndDisappear()
    {
        PlayerMove.canMove = false;

        Vector3 originalPosition = transform.position;
        Vector3 targetPosition = originalPosition + Vector3.up * flyingObjectHeight;

        float elapsedTime = 0f;

        while (elapsedTime < flyingDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / flyingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);

        transform.position = originalPosition;

        PlayerMove.canMove = true;
    }
}
