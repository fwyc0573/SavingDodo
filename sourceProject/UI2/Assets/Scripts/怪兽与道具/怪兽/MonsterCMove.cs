using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCMove : MonoBehaviour
{
    public float MspeedL;
    public float currentSpeed;
    private bool can_kill;
    AudioSource hit;
    //  private float XiangduiV;
    GameObject Fx;

    private void OnEnable()
    {
        //XiangduiV = Random.Range(4.5f, 8.0f);
        MspeedL = BackgroundMove.bcSpeed;
        currentSpeed = MspeedL;
        can_kill = true;
        float Xoffset = 17f;
        GameObject player = GameObject.Find("Player");//Random.Range(-3f, 0.7f)
        this.gameObject.transform.position = new Vector3(Random.Range(19f, 21f), -3.52f , transform.position.z);
    }

    void Start()
    {
        hit = GetComponent<AudioSource>();
        //XiangduiV = Random.Range(4.5f, 8.0f);
        MspeedL = BackgroundMove.bcSpeed;
        currentSpeed = MspeedL;
        can_kill = true;
        //Fx = (GameObject)Resources.Load("Prefabs/kulou");
    }


    void Update()
    {
        transform.position += Vector3.right * currentSpeed * Time.deltaTime;
        //Debug.Log(timeSpend);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("发生碰撞");
            if (can_kill && !PlayerControl.isInBubble)
            {
                //音效
                //  hit.Play();

                //GameObject ww = Instantiate(Fx);
                //ww.transform.position = this.transform.position;
                //Destroy(ww, 2f);

                LightAndScore.Light -= 45;
                can_kill = false;
                Invoke("setCankill", 1f);
                //Destroy(this.gameObject);
            }
        }
    }

    void setCankill()
    {
        can_kill = true;
    }


}
