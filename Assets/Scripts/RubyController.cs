using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    //allows currentHealth to be seen outside this class, but not be edited without using ChangeHealth function in order to prevent errors
    public int health { get { return currentHealth; }}
    private int currentHealth;

    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    private float horizontal;
    private float vertical;
    
    Animator animator;
    private Vector2 lookDirection = new Vector2(1, 0);

    public GameObject projectilePrefab;
    public ParticleSystem rubyHitEffect;
    public ParticleSystem healEffect;

    private AudioSource audiosource;
    public AudioClip[] rubySounds;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audiosource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //grab and store axis movement values
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 10.0f;
            Debug.Log("sprint");
        }
        else
        {
            moveSpeed = 5.0f;
        }

        
        if (Input.GetKeyDown(KeyCode.X))
        {
            DialogCheck();
        }

        //storing vector for direction of movement
        Vector2 move = new Vector2(horizontal, vertical);

        //if both conditions true, then character is not moving
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            //if character not moving, store and normalize its direction so you face correct position while idle
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        //set animator's looking direction
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        //decrement invincibility timer and set false if time is done
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;        
        position.x = position.x + moveSpeed * horizontal * Time.deltaTime;
        position.y = position.y + moveSpeed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            rubyHitEffect.Play();
            

            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            PlaySound(rubySounds[0]);
        }
        else
        {
            healEffect.Play();
        }
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        //instantiate creates a copy of projectilePrefab at the rigidbody location shifted up a little with no rotation b/c quaternion is set to identity
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        PlaySound(rubySounds[1]);
    }

    public void PlaySound(AudioClip clip)
    {
        audiosource.PlayOneShot(clip);
    }

    public void DialogCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));

        if (hit.collider != null)
        {
            DialogTrigger dialogTrigger = hit.collider.GetComponent<DialogTrigger>();

            if (dialogTrigger != null)
            {
                int count = dialogTrigger.characterDialog.sentences.Count();

                FindObjectOfType<DialogManager>().StartDialog(dialogTrigger.characterDialog);             
            }
        }
        else
        {
            FindObjectOfType<DialogManager>().dialogBox.SetActive(false);
        }
    }
}
