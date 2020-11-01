using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public CharacterController2D controller;
    public Animator anim;
    public BoxCollider2D boxCol;
    public CircleCollider2D circleCol;


    public float movementSpeed;

    float horizontalMovementSpeed;
    bool isJumping = false;
    bool isHurt = false;

    public Text scoreText;
    int score;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();

        isJumping = false;
        isHurt = false;

        score = 0;

    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();

        horizontalMovementSpeed = Input.GetAxisRaw("Horizontal") * movementSpeed;

        anim.SetFloat("speed", Mathf.Abs(horizontalMovementSpeed));

        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            anim.SetBool("isJumping", true);

        }

        // Resets the game when pushing R
        if (Input.GetKeyDown(KeyCode.R) == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        // Resets the game when player gets below -6 on the y (when hitting frogs)
        if (this.gameObject.transform.position.y < -6)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
        // Exits to menu
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    public void OnLanding()
    {
        isJumping = false;
        anim.SetBool("isJumping", false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Gem")
        {
            score++;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Enemy")
        {
            if (isJumping)
            {
                Destroy(other.gameObject);
                score++;
            }
            else
            {
                anim.SetBool("isHurt", true);
                boxCol.enabled = false;
                circleCol.enabled = false;
            }
        }
    }
    void FixedUpdate()
    {
        controller.Move(horizontalMovementSpeed * Time.fixedDeltaTime, false, isJumping);
    }
}
