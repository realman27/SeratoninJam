using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeadacheState
{
    Right,
    Left,
    Idle,
    Attack,
    Follow,
    Dead,
    Null
}

public class HeadacheAI : Entity
{
    public HeadacheState currentState;

    private Rigidbody2D rb;

    public SpriteRenderer sprite;
    public Animator animator;

    private float IdleTime;

    private float WalkTime;
    public float walkSpeed = 4f;
    public float walkDirection;
    public HeadacheState forcedDirection;

    public float targetRange = 15;
    public Transform player;

    public float slamCooldown = 2f;
    public GameObject slamPrefab;
    public float attackRange = 2f;

    public CollideCheck cRight;
    public CollideCheck cLeft;
    public CollideCheck cRightLedge;
    public CollideCheck cLeftLedge;

    public override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();

        TransitionState(HeadacheState.Idle);

        player = PlayerEntity.instance.transform;
    }

    public override void Update()
    {
        base.Update();

        if (player == null)
        {
            if (PlayerEntity.instance != null)
            {
                player = PlayerEntity.instance.transform;
            }
            else
            {
                return;
            }
        }

        //Attacking
        switch(currentState)
        {
            case HeadacheState.Follow:
                if (Vector3.Distance(player.position, transform.position) < attackRange)
                {
                    TransitionState(HeadacheState.Attack);
                }

                if (Vector3.Distance(player.position, transform.position) > targetRange)
                {
                    TransitionState(HeadacheState.Idle);
                }
                break;
            case HeadacheState.Attack:
                break;
            default:
                if (Vector3.Distance(player.position, transform.position) <= targetRange)
                {
                    TransitionState(HeadacheState.Follow);
                }
                break;
        }

        //Passive
        switch (currentState)
        {
            case HeadacheState.Idle:
                IdleTime -= Time.deltaTime;
                if (IdleTime <= 0)
                {
                    if (forcedDirection != HeadacheState.Null)
                    {
                        TransitionState(forcedDirection);
                        forcedDirection = HeadacheState.Null;
                    }
                    else
                    {
                        HeadacheState newState = Random.Range(0, 2) == 0 ? HeadacheState.Right : HeadacheState.Left;
                        TransitionState(newState);
                    }
                }
                break;
            case HeadacheState.Attack:
                break;
            case HeadacheState.Follow:
                Movement();
                break;
            case HeadacheState.Dead:
                break;
            default: //Left or Right
                Movement();
                WalkTime -= Time.deltaTime;
                if (WalkTime <= 0)
                    TransitionState(HeadacheState.Idle);

                if (currentState == HeadacheState.Right)
                {
                    if (cRight.colliding == true || cRightLedge.colliding == false)
                    {
                        Vector3 move = new Vector2(0f, -1f);
                        rb.AddRelativeForce(move * walkSpeed, ForceMode2D.Impulse);
                        forcedDirection = HeadacheState.Left;
                        TransitionState(HeadacheState.Idle);
                    }
                } else if (currentState == HeadacheState.Left)
                {
                    if (cLeft.colliding == true || cLeftLedge.colliding == false)
                    {
                        Vector3 move = new Vector2(0f, 1f);
                        rb.AddRelativeForce(move * walkSpeed, ForceMode2D.Impulse);
                        forcedDirection = HeadacheState.Right;
                        TransitionState(HeadacheState.Idle);
                    }
                }
                break;
        }
    }

    void TransitionState(HeadacheState newState)
    {
        currentState = newState;
        switch(currentState)
        {
            case HeadacheState.Right:
                WalkTime = Random.Range(3, 5);
                walkDirection = 1f;
                sprite.flipX = false;
                animator.SetBool("walking", true);
                break;
            case HeadacheState.Left:
                WalkTime = Random.Range(3, 5);
                walkDirection = -1f;
                sprite.flipX = true;
                animator.SetBool("walking", true);
                break;
            case HeadacheState.Idle:
                IdleTime = Random.Range(4, 6);
                animator.SetBool("walking", false);
                break;
            case HeadacheState.Attack:
                StartCoroutine(Slam());
                break;
        }
    }

    void Movement()
    {
        switch(currentState)
        {
            case HeadacheState.Attack:
                break;
            case HeadacheState.Follow:
                float dirFollow = transform.position.x - player.transform.position.x;

                Vector3 moveFollow = new Vector3(0f, 0f);
                if (dirFollow > 0) //Left
                {
                    moveFollow = new Vector2(0f, -1);
                    sprite.flipX = true;
                } else if (dirFollow < 0) //Right
                {
                    moveFollow = new Vector2(0f, 1);
                    sprite.flipX = false;
                }
                rb.AddRelativeForce(moveFollow * walkSpeed, ForceMode2D.Impulse);
                break;
            default:
                Vector3 move = new Vector2(0f, walkDirection);
                rb.AddRelativeForce(move * walkSpeed, ForceMode2D.Impulse);
                break;
        }
    }

    public override void Death()
    {
        base.Death();

        currentState = HeadacheState.Dead;
        animator.SetTrigger("Dead");

        GetComponent<CircularGravity>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;

        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        healthbar.transform.gameObject.SetActive(false);

        GetComponent<HeadacheAI>().enabled = false;
    }

    IEnumerator Slam()
    {
        animator.SetTrigger("Slam");

        float cooldown = 1f;
        while (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            yield return null;
        }

        if (currentState == HeadacheState.Dead)
            yield break;

        GameObject slamO = Instantiate(slamPrefab, transform);
        slamO.transform.localPosition = new Vector3(1f, 0, 0);
        slamO.transform.rotation = transform.rotation * Quaternion.Euler(0, 0, 90);
        Destroy(slamO, 0.75f);

        float cooldown2 = slamCooldown;
        while(cooldown2 > 0)
        {
            cooldown2 -= Time.deltaTime;
            yield return null;
        }

        if (currentState == HeadacheState.Dead)
            yield break;

        TransitionState(HeadacheState.Idle);
    }

}
