using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed = 5;
    public float speed = 60.0f;
    public float jumpPower = 500.0f;
    public bool grounded;
    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetButtonDown("Jump") && grounded)
        {
            rb2d.AddForce(Vector2.up * jumpPower * 4);
        }
    }

    private void FixedUpdate()
    {
        float leftRight = Input.GetAxis("Horizontal");
        rb2d.AddForce((Vector2.right * speed * 3) * leftRight);

        //player speed limiting
        if( rb2d.velocity.x > maxSpeed)
        {
            rb2d.velocity = new Vector2(maxSpeed, rb2d.velocity.y);
        }

        if( rb2d.velocity.x < -maxSpeed )
        {
            rb2d.velocity = new Vector2(-maxSpeed, rb2d.velocity.y);
        }
    }
}
