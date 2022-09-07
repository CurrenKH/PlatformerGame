using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    [SerializeField] public float maxSpeed;
    [SerializeField] float jumpForce = 7f;
    [SerializeField] Transform groundChecker;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float fireRate = 0.5f;
    [SerializeField] Transform gunTip;
    [SerializeField] GameObject bullet;
    [SerializeField] Boolean allowDoubleJump;
    [SerializeField] Boolean allowTripleJump;
    [SerializeField] Boolean allowRunning;
    [SerializeField] Boolean allowSlamming;
    [SerializeField] float staminaDepleteRate;
    [SerializeField] float staminaRechargeRate;
    [SerializeField] ParticleSystem dust;

    [SerializeField] int maxHealth = 10;
    public int currentHealth;
    [SerializeField] HealthBar healthBar;

    public HealthBar barScript;

    Rigidbody2D myRB;
    Animator myAnim;

    bool isGrounded;
    float checkGroundRadius = 0.2f;
    float nextFire = 0f;

    private int extraJumps;
    public int extraJumpsValue;

    public float stamina;
    public float totalStamina;
    bool playerIsRunning;

    // Start is called before the first frame update
    void Start()
    {
        extraJumps = extraJumpsValue;
        myRB = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        //slam = GetComponent<AudioSource>();
    }

    void Awake()
    {
        stamina = totalStamina;
        currentHealth = maxHealth;
        //healthBar.SetMaxHealth(currentHealth);

    }

    void Update()
    {
        Move();
        Flip();
        CheckIfGrounded();
        Jump();
        ExtraJumps();
        Stamina();
        GroundPound();
        barScript.Show();
        

        if (Input.GetAxisRaw("Fire1") > 0)
        {
            FireBullet();
        }
    }
    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController ec = collision.gameObject.GetComponent<EnemyController>();
        if (ec != null) ChangeHealth(-1 * ec.GetHitValue());
    }


    public void ChangeHealth(int changeValue)
    {
        currentHealth += changeValue;
        healthBar.SetHealth(currentHealth);
    }

    void GroundPound()
    {
        if (allowSlamming == true && isGrounded == false && Input.GetKey(KeyCode.S))
        {
            myRB.gravityScale = 20;
            dust.Play();
            //soundSource.PlayOneShot(landingSound, 0.5f);
            //Instantiate(dust, groundChecker.position, groundChecker.rotation);
        }
        else
        {
            myRB.gravityScale = 1;
        }
    }

    void Stamina()
    {
        if (playerIsRunning == true)
        {
            maxSpeed = 12;
        }

        if (playerIsRunning == false)
        {
            maxSpeed = 4;
        }

        if (allowRunning == true && Input.GetAxisRaw("Fire2") > 0 && stamina > 0)
        {
            playerIsRunning = true;
            stamina -= staminaDepleteRate;
        }
        else
        {
            playerIsRunning = false;
        }
        if (stamina < 100 && Input.GetAxisRaw("Fire2") <= 0)
        {
            stamina += staminaRechargeRate;
        }
    }

    void ExtraJumps()
    {
        //  Double jump true
        if (isGrounded == true && allowDoubleJump == true)
        {
            extraJumps = 2;
        }

        //  Triple jump true
        if (isGrounded == true && allowTripleJump == true)
        {
            extraJumps = 3;
        }

        //  If extra jumps available
        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            myRB.velocity = Vector2.up * jumpForce * 0.75f;
            extraJumps--;
        }

        //  If no extra jumps left
        else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && isGrounded == true)
        {
            myRB.velocity = Vector2.up * jumpForce;
        }
    }
    void Move()
    {
        float move = Input.GetAxis("Horizontal");

        if (Input.GetAxis("Horizontal") != 0)
        {
            myRB.velocity = new Vector2(move * maxSpeed, myRB.velocity.y);
        }

        myAnim.SetFloat("speed", Mathf.Abs(myRB.velocity.x));
    }

    void Flip()
    {
        if (transform.localScale.x * myRB.velocity.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    void Jump()
    {
        if (isGrounded && Input.GetAxis("Jump") > 0)
        {
            myRB.velocity = new Vector2(myRB.velocity.x, jumpForce);
            isGrounded = false;
            myAnim.SetBool("isGrounded", isGrounded);
        }
        myAnim.SetFloat("verticalSpeed", myRB.velocity.y);

    }

    void CheckIfGrounded()
    {
        Collider2D collider = Physics2D.OverlapCircle(groundChecker.position, checkGroundRadius, groundLayer);

        // Check if ground is touching player
        if (collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        myAnim.SetBool("isGrounded", isGrounded);
    }

    private void FireBullet()
    {
        //  If bullet is ready to fire
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            if (transform.localScale.x > 0)
            {
                Instantiate(bullet, gunTip.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            }
            else
            {
                Instantiate(bullet, gunTip.position, Quaternion.Euler(new Vector3(0, 0, 180f)));
            }
        }
    }
}

