using System.Collections;
using UnityEngine;

public class DoubleSpeedItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMove playerMove = other.GetComponent<PlayerMove>();

            if (playerMove != null)
            {
                StartCoroutine(ApplyDoubleSpeed(playerMove));
            }
        }
    }

    IEnumerator ApplyDoubleSpeed(PlayerMove playerMove)
    {
        playerMove.DisableCollider();

        playerMove.DoubleSpeed();

        yield return new WaitForSeconds(1f);

        playerMove.ResetSpeed();
        playerMove.EnableCollider();

        gameObject.SetActive(false);
    }
}
