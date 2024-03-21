using UnityEngine;

public class MeatCollectible : MonoBehaviour
{
    public AudioSource collectSoundSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectMeat();
        }
    }

    private void CollectMeat()
    {
        if (collectSoundSource != null)
        {
            collectSoundSource.Play();
        }

        gameObject.SetActive(false);
    }
}
