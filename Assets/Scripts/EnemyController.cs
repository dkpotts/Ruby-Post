using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2.0f;
    public bool vertical;
    public float changeTime = 3.0f;

    Rigidbody2D rigidbody2d;
    Animator animator;
    float timer;
    public int direction = 1;

    public bool pubBroken { get { return broken; }}
    private bool broken = true;

    public ParticleSystem smokeEffect;

    public AudioClip collectedClip;

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        timer = changeTime;
    }

    private void Update()
    {
        if (!broken)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;

        if (!broken)
        {
            return;
        }

        if (vertical)
        {
            position.y = position.y + speed * direction * Time.deltaTime;

            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + speed * direction * Time.deltaTime;

            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }
        
        rigidbody2d.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            if(broken != false)
            {
                player.ChangeHealth(-1);
                player.PlaySound(collectedClip);
            }
        }
    }

    public void Fix()
    {
        broken = false;
        //rigidbody2d.simulated = false;
        //makes robots immovable after they've been fixed, so you can talk to them without being hurt
        rigidbody2d.isKinematic = true;

        if(smokeEffect != null)
        {
            smokeEffect.Stop();
        }

        animator.SetTrigger("Fixed");
    }
}

