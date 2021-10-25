using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public Text score;
    public Text lives;

    private int scoreValue = 0;
    private int livesValue = 3;

    public GameObject winTextObject;
    public GameObject livesTextObject;
    public GameObject loseTextObject;

    public AudioSource musicSource;
    public AudioClip winSound;

    Animator anim;

    private bool facingRight = true;


    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        lives.text = livesValue.ToString();

        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);

        anim = GetComponent<Animator>();



    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        
        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            anim.SetInteger("State", 1);
            
        }
        
        if(collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);

            if(scoreValue == 4)
            {
                transform.position = new Vector2(44.0f, 1.0f);
                livesValue = 3;
                lives.text = livesValue.ToString();
            }

            if (scoreValue >= 8)
            {
                score.text = "";
                winTextObject.SetActive(true);

                musicSource.clip = winSound;
                musicSource.Play();
                musicSource.loop = false;

                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach(GameObject enemy in enemies)
                {
                    GameObject.Destroy(enemy);
                }
            }
        }

        if(collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = livesValue.ToString();
            Destroy(collision.collider.gameObject);

            if (livesValue == 0)
            {
            
                score.text = "";

                loseTextObject.SetActive(true);

                GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
                foreach(GameObject coin in coins)
                {
                    GameObject.Destroy(coin);
                }

            }
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            }


        }
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }

        if(Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State", 0);
        }
         
        if(Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }

        if(Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            anim.SetInteger("State", 2);
        }

    }

    void Flip()
   {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
   }
}
