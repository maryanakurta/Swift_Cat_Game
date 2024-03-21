using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public float destroyRangeMin = 248.2709f;
    public float destroyRangeMax = 263.961f;

    void Start()
    {
        StartCoroutine(DestroyClone());
    }

    IEnumerator DestroyClone()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null && IsPlayerInRange(player.transform.position.z))
            {
                Destroy(gameObject);
                yield break;
            }
        }
    }

    bool IsPlayerInRange(float playerZ)
    {
        return playerZ >= destroyRangeMin && playerZ <= destroyRangeMax;
    }
}
