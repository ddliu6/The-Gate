using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public float runSpeed = 40f;
    public Animator animator;
    public GameObject key, door;
    public AudioClip collectItem, openDoor;
    private AudioSource source;

    float horizontalMove = 0f;
    bool jump = false, crouch = false;
    static List<bool> gotKey = new List<bool>() { false, false, false, false };
    static int keyCount = 0;
    static bool isGoal = false;

    int nextScene = 0;
    void Awake()
    {
        source = GetComponent<AudioSource>();
        if(isGoal && (SceneManager.GetActiveScene().buildIndex == 3))
            Instantiate(door, new Vector3(7.13f, 3.78f, 0), Quaternion.identity);

        if (!gotKey[SceneManager.GetActiveScene().buildIndex - 1])
        {
            DontDestroyOnLoad(key);
        }
        else
        {
            Destroy(key);
        }
    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (keyCount == 3 && (SceneManager.GetActiveScene().buildIndex == 3) && !isGoal)
        {
            Instantiate(door, new Vector3(7.13f, 3.78f, 0), Quaternion.identity);
            isGoal = true;
        }

        if (Input.GetButtonDown("Crouch"))
            crouch = true;
        else if(Input.GetButtonUp("Crouch"))
            crouch = false;
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.name == "Left" && gotKey[SceneManager.GetActiveScene().buildIndex - 1])
        {
            if (SceneManager.GetActiveScene().buildIndex < 3)
                nextScene = SceneManager.GetActiveScene().buildIndex + 1;
            else
                nextScene = 1;
            source.PlayOneShot(openDoor);
            StartCoroutine(LoadLevelAfterDelay(2));
        }
        else if(collision.collider.name == "Right" && gotKey[SceneManager.GetActiveScene().buildIndex - 1])
        {
            if (SceneManager.GetActiveScene().buildIndex > 1)
                nextScene = SceneManager.GetActiveScene().buildIndex - 1;
            else
                nextScene = 3;
            source.PlayOneShot(openDoor);
            StartCoroutine(LoadLevelAfterDelay(2));
        }
        else if(collision.collider.name == "Key")
        {
            Debug.Log("Got Key!");
            gotKey[SceneManager.GetActiveScene().buildIndex-1] = true;
            if(keyCount < 4)
                keyCount++;
            source.PlayOneShot(collectItem);
            Destroy(key);
        }
        else if (collision.collider.name == "Entrance")
        {
            Debug.Log("Last Stage!");
            nextScene = 4;
            source.PlayOneShot(openDoor);
            StartCoroutine(LoadLevelAfterDelay(2));
        }
        else if (collision.collider.name == "Goal" && gotKey[3])
        {
            Debug.Log("Goal!");
            nextScene = 5;
            source.PlayOneShot(openDoor);

            //reset data
            for(int i = 0; i < 4; ++i)
            {
                gotKey[i] = false;
            }
            isGoal = false;
            keyCount = 0;
            StartCoroutine(LoadLevelAfterDelay(2));
        }
    }

    IEnumerator LoadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextScene);
    }

    public void loadScene1()
    {
        SceneManager.LoadScene(1);
    }
    public void loadScene2()
    {
        SceneManager.LoadScene(2);
    }
    public void loadScene3()
    {
        SceneManager.LoadScene(3);
    }
}
