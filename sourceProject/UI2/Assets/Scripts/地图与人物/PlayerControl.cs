using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    public Animator anim;

    public static int MaskNum = 0;
    public static int GlassNum = 0;
    public static int LiquidNum = 0;
    public static int ClothesNum = 0;

    

    public GameObject bubble;
    public static bool isInBubble = false;

    [Header("跳跃参数")]
    public float jumpForce = 7f;
    public float lessJumpForce = 0.2f;

    [Header("状态参数")]
    public bool isOnGround;
    public bool isJump;
    public int jumpCount;
    public bool isHurt;

    bool jumpPressed;

    [Header("地面参数")]
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("音频参数")]
    public AudioSource jumpAudio;
    public AudioSource hurtAudio;
    public AudioSource goodsAudio;
    public AudioSource elementsAudio;//拾取元素音效
    public AudioSource guluguluAudio;//获取泡泡音效
    public AudioSource slowDown;
    public AudioSource biger;
    public AudioSource smaller;
    public AudioSource duangduang;
    public AudioSource bubleBoom;
    public AudioSource snake;
    public AudioSource speedup;

    GameObject p;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

        bubble = GameObject.Find("bb");

        p = GameObject.Find("Player");
    }

    void Update()
    {
        //Debug.Log("主角的y速度："+p.GetComponent<Rigidbody2D>().velocity.y);
        if (Input.GetButtonDown("Jump") && jumpCount >= 0)
        {
            jumpPressed = true;
        }

        if(isInBubble == true)
        {
            bubble.SetActive(true);
        }
        else
        {
            bubble.SetActive(false);
        }

        anim.SetBool("isOnGround", isOnGround);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isHurt", isHurt);
        //Debug.Log("速度：" + rb.velocity.y);
    }

    private void FixedUpdate()
    {
        isOnGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        //PhysicsCheck();
        Jump();
    }

    void PhysicsCheck()
    {
        if (coll.IsTouchingLayers(groundLayer))
            isOnGround = true;
        else
            isOnGround = false;
    }

    void Jump()
    {
        if (isOnGround)
        {
            jumpCount = 2;
            isJump = false;
        }
        if (jumpPressed && isOnGround)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
            jumpAudio.Play();
            Debug.Log("第二个if：" + jumpCount);
        }

        else if (jumpPressed && isJump)
        {
            if (jumpCount > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpCount--;
                //Debug.Log("1:" + jumpCount);
                jumpPressed = false;
                jumpAudio.Play();
            }
            else
            {
                Debug.Log("2:" + jumpCount);
                rb.velocity = new Vector2(rb.velocity.x, lessJumpForce);
                jumpPressed = false;
                jumpAudio.Play();
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("名字:" + collision.gameObject.name);
        if (collision.gameObject.tag == "Monster")
        {
            if (!isInBubble)
            {
                isHurt = true;
                hurtAudio.Play();
                Invoke("setHurtFalse", 0.1f);
            }
            if (collision.gameObject.name == "Monkey")
            {
                //Destroy(GameObject.Find("hammerAndRope(Clone)"));
                duangduang.Play();
            }
            else if (collision.gameObject.name == "MonsterB")
            {
                //Destroy(GameObject.Find("hammerAndRope(Clone)"));
                snake.Play();
            }
            else
                collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "Goods")
        {
            if (collision.gameObject.name == "Glass")//天空元素
            {
                LightAndScore.Light += 10;
                LightAndScore.Score += 10;
                GlassNum++;
                elementsAudio.Play();
            }
            else if (collision.gameObject.name == "果子" )//野果
            {
                LightAndScore.Light += 10;
                LightAndScore.Score += 10;
                GlassNum++;
                elementsAudio.Play();
            }
            else if (collision.gameObject.tag == "rain")//水滴元素
            {
                LightAndScore.Light += 20;
                LightAndScore.Score += 20;
                LiquidNum++;
                elementsAudio.Play();
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.name == "Clothes")//叶子元素
            {
                LightAndScore.Light += 30;
                LightAndScore.Score += 30;
                ClothesNum++;
                elementsAudio.Play();
            }
            else if (collision.gameObject.name == "Big")
            {
                if (this.transform.localScale.x < 0.45f)
                  this.transform.localScale = new Vector3(this.transform.localScale.x + 0.08f, this.transform.localScale.y + 0.08f, this.transform.localScale.z);
                goodsAudio.Play();
                biger.Play();
            }
            else if (collision.gameObject.name == "Small")
            {
                if (this.transform.localScale.x > 0.15f)
                    this.transform.localScale = new Vector3(this.transform.localScale.x - 0.08f, this.transform.localScale.y - 0.08f, this.transform.localScale.z);
                goodsAudio.Play();
                smaller.Play();
            }
            else if (collision.gameObject.name == "SpeedUp")
            {
                BackgroundMove.t *= 1.7f;
                BackgroundMove2.t *= 1.7f;
                BackgroundMove3.t *= 1.7f;
                Invoke("setNormalTimeScale", 3.5f);
                speedup.Play();
            }
            else if (collision.gameObject.name == "SpeedDown")
            {
                BackgroundMove.t *= 0.6f;
                BackgroundMove2.t *= 0.6f;
                BackgroundMove3.t *= 0.6f;
                Invoke("setNormalTimeScale", 1.7f);
                goodsAudio.Play();
                slowDown.Play();
            }
            else if (collision.gameObject.name == "Bubble")
            {
                isInBubble = true;
                Invoke("bubbleBreak", 4f);
                goodsAudio.Play();
                guluguluAudio.Play();
            }

            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "Fruit")
        {
            LightAndScore.Light += 10;
            LightAndScore.Score += 10;
            GlassNum++;
            elementsAudio.Play();
            Destroy(GameObject.Find("Fruit"));
            Destroy(GameObject.Find("Fruit Reference Points"));
        }
    }

    void setHurtFalse()
    {
        isHurt = false;
    }
    void setNormalTimeScale()
    {
        BackgroundMove.t = 1f;
        BackgroundMove2.t = 1f;
        BackgroundMove3.t = 1f;
    }
    void bubbleBreak()
    {
        isInBubble = false;
        bubleBoom.Play();
    }
}
