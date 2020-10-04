using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 5f;
    public float jumpSpeed = 5f;
    float difference;

    public SpriteRenderer sprite;
    public Animator animator;

    public CollideCheck ground;

    public float direction;

    Rigidbody2D rb;

    private float jumpCharge = 1f;
    public float jumpMaxCharge = 2f;
    private bool jumping = false;
    private bool startCharge = false;
    public AudioClip jumpSound;

    public bool crouching = false;
    public Transform point;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        difference = jumpSpeed / Speed;
    }

    private void Update()
    {
        JumpCharge();
        Crouch();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        float hor = Input.GetAxis("Horizontal");
        if (hor > 0)
        {
            direction = (hor / hor);
            sprite.flipX = false;
        } else if (hor < 0)
        {
            direction = (hor / hor) * -1f;
            sprite.flipX = true;
        }

        if (hor > 0 || hor < 0)
        {
            animator.SetBool("Walking", true);
        } else
        {
            animator.SetBool("Walking", false);
        }

        if (startCharge)
            return;

        Vector3 move = new Vector2(0f, hor);
        rb.AddRelativeForce(move * Speed, ForceMode2D.Impulse);
    }

    void JumpCharge()
    {
        if (Input.GetButton("Jump") && jumping == false && crouching == false)
        {
            if (startCharge == false)
            {
                animator.SetTrigger("JumpCharge");
                startCharge = true;
            }

            jumpCharge += Time.deltaTime;
            if (jumpCharge >= jumpMaxCharge)
                Jump();
        }

        if (Input.GetButtonUp("Jump") && jumping == false)
        {
            Jump();
        }
    }

    void Jump()
    {
        startCharge = false;
        jumping = true;

        AudioManager.instance.Play(jumpSound, transform);

        rb.AddForce(transform.right * -jumpSpeed * 50 * jumpCharge, ForceMode2D.Impulse);
        StartCoroutine(GroundCheck());

        animator.SetTrigger("Jump");
    }

    IEnumerator GroundCheck()
    {
        yield return new WaitForSeconds(0.5f);

        while (jumping)
        {
            if (ground.colliding == true)
            {
                jumping = false;
                jumpCharge = 1f;
                animator.SetTrigger("Grounded");
            }

            yield return null;
        }
    }

    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && jumping == false) {
            crouching = true;
            animator.SetBool("Crouch", true);
            GetComponent<BoxCollider2D>().offset = new Vector2(0.05f, 0f);
            GetComponent<BoxCollider2D>().size = new Vector2(0.55f, 0.36f);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            crouching = false;
            animator.SetBool("Crouch", false);
            GetComponent<BoxCollider2D>().offset = new Vector2(0f, 0f);
            GetComponent<BoxCollider2D>().size = new Vector2(0.69f, 0.36f);
        }
    }
}
