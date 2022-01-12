using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Start() variables
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    public GameOverScreen GameOverScreen;


    //fsm
    private enum State
    {
        idle,
        run,
        jump,
        falling,
        hurt
    };
    private State state = State.idle;
    
    //inspector variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private AudioSource gem;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource jump;
    [SerializeField] private AudioSource hurt;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        if (PermanentUI.perm.health <= 0)
        {
            GameOver();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (state != State.hurt)
        {
            Movement();
        }
        AnimationState();
        anim.SetInteger("state",(int)state);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Diamond"))
        {
            gem.Play();
            Destroy(collision.gameObject);
            PermanentUI.perm.diamond += 1;
            PermanentUI.perm.diamondNumber.text = PermanentUI.perm.diamond.ToString();
        }

        if (collision.CompareTag("Powerup"))
        {
            Destroy(collision.gameObject);
            jumpForce = 25f;
            GetComponent<SpriteRenderer>().color = Color.yellow;
            StartCoroutine(ResetPower());
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (state == State.falling)
            {
                enemy.JumpedOn();
                Jump();
            }
            else
            {
                hurt.Play();
                state = State.hurt;
                HandleHealth();
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
            }
        }
    }

    private void HandleHealth()
    {
        PermanentUI.perm.health -= 1;
        PermanentUI.perm.healthAmount.text = PermanentUI.perm.health.ToString();
        if (PermanentUI.perm.health <= 0)
        {
            GameOver();
        }
    }
    private void Movement()
    {
        float hDirection = Input.GetAxisRaw("Horizontal");
        if (hDirection < 0) 
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale=new Vector2(-1,1);
        }
        else if (hDirection > 0) 
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale=new Vector2(1,1);
        }

        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground)) 
        {
            Jump();
        }
    }

    private void Jump()
    {
        jump.Play();
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jump;
    }
    private void AnimationState()
    {
        if (state == State.jump)
        {
            if (rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling)
        {
                if (coll.IsTouchingLayers(ground))
                {
                    state = State.idle;
                }
        }
        
        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            //right
            state = State.run;
        }
        else
        {
            state = State.idle;
        }
    }

    private void Footstep()
    {
        footstep.Play();
    }

    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(10);
        jumpForce = 18;
        GetComponent<SpriteRenderer>().color=Color.white;
    }

    public void GameOver()
    {
        GameOverScreen.Setup(PermanentUI.perm.diamond);
    }
}
