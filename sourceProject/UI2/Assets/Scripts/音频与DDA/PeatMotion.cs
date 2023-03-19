using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeatMotion : MonoBehaviour
{
    GameObject player;
    float radian = 0; // 弧度
    float perRadian = 0.005f;
    float radius = 0.8f; // 半径
    Vector3 oldPos; // 开始时候的坐标
    private float tempTime;

    GameObject resourcePeat;
    GameObject peatcreat;
    public bool can_boom;
    public GameObject correctFx;

    void Start()
    {
        can_boom = false;
        player = GameObject.Find("Player");
        //new Vector2(Random.Range(3.85f, 6.55f), Random.Range(-1.25f, 0.95f));
        oldPos.x = Random.Range(3.85f, 6.55f); // 将最初的位置保存到oldPos
        oldPos.y = Random.Range(-1.25f, 0.95f);
        tempTime = 0;
        //获取材质本来的属性  
        this.GetComponent<SpriteRenderer>().material.color = new Color
        (
                this.GetComponent<SpriteRenderer>().material.color.r,
                this.GetComponent<SpriteRenderer>().material.color.g,
                this.GetComponent<SpriteRenderer>().material.color.b,
                //需要改的就是这个属性：Alpha值  
                1f
        );

    }

    void Update()
    {
        if (PlayingUI.isStop == false)
        {
            radian += perRadian; // 每次变化的弧度; // 弧度每次加0.03
            float dy = Mathf.Cos(radian) * radius; // dy定义的是针对y轴的变量，也可以使用sin，找到一个适合的值就可以
            float dx = Mathf.Sin(radian) * radius;
            if (Time.frameCount % 2 == 0)
            {

                transform.position = oldPos + new Vector3(dx, dy, 0);
            }

            Dispear();

            //当敲击正确的时候，调用CurrentPeat.Boom（）即可
            //Boom();
            if (can_boom)
            {
                Boom();
            }

            if (GameOver.gameOver == 0 && Time.frameCount % 30 == 0)
            {
                player.transform.position = new Vector2(7f, player.transform.position.y);
            }
        }
       
    }

    void Dispear()
    {
        if (tempTime < 1)
        {
            tempTime = tempTime + Time.deltaTime;
        }
        if (this.GetComponent<SpriteRenderer>().material.color.a <= 1)
        {
            this.GetComponent<SpriteRenderer>().material.color = new Color
            (
                this.GetComponent<SpriteRenderer>().material.color.r,
                this.GetComponent<SpriteRenderer>().material.color.g,
                this.GetComponent<SpriteRenderer>().material.color.b,
            //减小Alpha值，从1-30秒逐渐淡化 ,数值越大淡化越慢 
            gameObject.GetComponent<SpriteRenderer>().material.color.a - tempTime / 2.3f * Time.deltaTime
            );
        }
        if (this.gameObject != null)
        {
            Destroy(this.gameObject, 3f);//3秒后消除
        }

       
    }

    public void Boom()
    {
        //GameObject ww = Instantiate(correctFx);
        //ww.transform.position = this.transform.position;
        correctFx.SetActive(true);
        Destroy(this.gameObject,3f);//消除
       // Destroy(ww, 0.3f);
    }
}
