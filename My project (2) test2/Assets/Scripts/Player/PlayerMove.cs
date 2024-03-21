using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public float leftRightSpeed = 4;
    public static bool canMove = false;
    public bool isJumping = false;
    public bool comingDown = false;
    public GameObject playerObject;
    public float moveSpeed = 5;
    public float forwardSpeed = 5;

    private Collider playerCollider;
    private AudioSource audioSource;
    public AudioClip deathSound;

    private bool deathSoundPlayed = false;

    private bool isFalling = false;
    private Vector3 originalPosition;

    public float accelerationFactor = 3f;
    public float flyingJumpHeight = 5f;
    public float forwardJumpDistance = 2f;

    public Text meatCounterText;
    private int meatCounter = 0;

    private bool isPlayingFoodAnimation = false;
    private float foodAnimationDuration = 1f;
    private float foodAnimationTimer = 0f;

    private bool isDoubleSpeedActive = false;

    public GameObject deathCanvas;

    private static int deathCount = 0;

    void Start()
    {
        playerCollider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Flying"))
        {
            other.gameObject.SetActive(false);
            StartCoroutine(JumpAndResume(flyingJumpHeight, forwardJumpDistance));
        }
        else if (other.CompareTag("doubleSpeed") && !isDoubleSpeedActive)
        {
            other.gameObject.SetActive(false);
            StartCoroutine(ApplyDoubleSpeed());
        }
        else if (other.CompareTag("hereEnemy"))
        {
            Die();
        }
        else if (other.CompareTag("bigMeat"))
        {
            CollectMeat(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("hereEnemy"))
        {
            Die();
        }
    }

    private void Die()
    {
        if (!deathSoundPlayed && deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
            deathSoundPlayed = true;

            deathCount++;

            StartCoroutine(ReloadSceneAfterDelay(1.5f));
        }

        canMove = false;
        playerObject.GetComponent<Animator>().Play("Stumble Backwards");

        deathCanvas.gameObject.SetActive(true);

        StartCoroutine(DisableDeathCanvasAfterDelay(2f));
    }

    IEnumerator JumpAndResume(float jumpHeight, float forwardDistance)
    {
        originalPosition = transform.position;

        isJumping = true;
        playerObject.GetComponent<Animator>().Play("Standing Jump");

        float elapsedTime = 0f;

        while (elapsedTime < 0.45f)
        {
            float progress = elapsedTime / 0.45f;
            transform.position = Vector3.Lerp(originalPosition, originalPosition + Vector3.up * jumpHeight, progress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        comingDown = true;

        elapsedTime = 0f;

        while (elapsedTime < 0.45f)
        {
            float progress = elapsedTime / 0.45f;
            transform.position = Vector3.Lerp(originalPosition + Vector3.up * jumpHeight, originalPosition, progress);
            elapsedTime += Time.deltaTime;

            if (!isFalling && transform.position.y <= jumpHeight - 0.1f)
            {
                StartCoroutine(FallToSandFloor());
                isFalling = true;
            }

            yield return null;
        }

        isJumping = false;
        comingDown = false;
        playerObject.GetComponent<Animator>().Play("Running");
        canMove = true;

        transform.position = new Vector3(transform.position.x, originalPosition.y, originalPosition.z + forwardDistance);
    }

    IEnumerator ApplyDoubleSpeed()
    {
        isDoubleSpeedActive = true;

        DisableCollider();
        DoubleSpeed();
        yield return new WaitForSeconds(1.5f);
        ResetSpeed();
        EnableCollider();

        isDoubleSpeedActive = false;
    }

    IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canMove = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator DisableDeathCanvasAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        deathCanvas.gameObject.SetActive(false);
    }

    public void DisableCollider()
    {
        playerCollider.enabled = false;
    }

    public void EnableCollider()
    {
        playerCollider.enabled = true;
    }

    public void DoubleSpeed()
    {
        moveSpeed *= accelerationFactor;
        forwardSpeed *= accelerationFactor;
    }

    public void ResetSpeed()
    {
        moveSpeed /= accelerationFactor;
        forwardSpeed /= accelerationFactor;
    }

    IEnumerator FallToSandFloor()
    {
        while (transform.position.y > 1.0f)
        {
            transform.Translate(Vector3.down * Time.deltaTime * 5f, Space.World);
            yield return null;
        }
        isFalling = false;
    }

    void Update()
    {
        if (canMove)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                if (transform.position.x > LevelBoundary.leftSide)
                {
                    transform.Translate(Vector3.left * Time.deltaTime * leftRightSpeed);
                }
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                if (transform.position.x < LevelBoundary.rightSide)
                {
                    transform.Translate(Vector3.left * Time.deltaTime * leftRightSpeed * -1);
                }
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space))
            {
                if (isJumping == false)
                {
                    isJumping = true;
                    playerObject.GetComponent<Animator>().Play("Standing Jump");
                    StartCoroutine(JumpAndResume(flyingJumpHeight, forwardJumpDistance));
                }
            }

            foreach (Touch touch in Input.touches)
            {
                if (touch.position.x < Screen.width / 2)
                {
                    if (transform.position.x > LevelBoundary.leftSide)
                    {
                        transform.Translate(Vector3.left * Time.deltaTime * leftRightSpeed);
                    }
                }
                else
                {
                    if (transform.position.x < LevelBoundary.rightSide)
                    {
                        transform.Translate(Vector3.left * Time.deltaTime * leftRightSpeed * -1);
                    }
                }

                if (touch.position.y > Screen.height / 2)
                {
                    if (isJumping == false)
                    {
                        isJumping = true;
                        playerObject.GetComponent<Animator>().Play("Standing Jump");
                        StartCoroutine(JumpAndResume(flyingJumpHeight, forwardJumpDistance));
                    }
                }
            }

            if (isJumping == true)
            {
                if (comingDown == false)
                {
                    transform.Translate(Vector3.up * Time.deltaTime * 5, Space.World);
                }
                if (comingDown == true)
                {
                    transform.Translate(Vector3.up * Time.deltaTime * -5, Space.World);
                }
            }

            if (!isPlayingFoodAnimation)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * forwardSpeed);
            }
            else
            {
                foodAnimationTimer += Time.deltaTime;

                if (foodAnimationTimer >= foodAnimationDuration)
                {
                    isPlayingFoodAnimation = false;
                    foodAnimationTimer = 0f;
                    playerObject.GetComponent<Animator>().Play("Running");
                }
            }
        }
    }


    private void CollectMeat(GameObject meat)
    {
        if (!isPlayingFoodAnimation)
        {
            playerObject.GetComponent<Animator>().Play("GetFood");
            isPlayingFoodAnimation = true;
            StartCoroutine(ResetFoodAnimationFlag());
        }

        meatCounter++;
        meatCounterText.text = " " + meatCounter;
        meat.SetActive(false);
    }

    IEnumerator ResetFoodAnimationFlag()
    {
        yield return new WaitForSeconds(foodAnimationDuration);
        isPlayingFoodAnimation = false;
    }
}
