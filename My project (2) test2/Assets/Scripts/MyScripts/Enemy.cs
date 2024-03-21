using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    private Rigidbody enemyRb;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        transform.Rotate(0, 180, 0);

        if (gameObject.layer == LayerMask.NameToLayer("HighYYLayer"))
        {
            transform.position = new Vector3(transform.position.x, 1.3f, transform.position.z);
        }
    }

    void FixedUpdate()
    {
        float newYPosition = Mathf.Clamp(transform.position.y, 0f, 0.65f);
        transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);
        enemyRb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);

        if (transform.position.z < -100f)
        {
            GameManager.instance.ScoreUp();
            Destroy(gameObject);
        }
    }
}
