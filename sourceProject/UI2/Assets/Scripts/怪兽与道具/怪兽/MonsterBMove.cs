using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBMove : MonoBehaviour
{
    public float MspeedL;
    public float currentSpeed;
    private bool can_kill;
    AudioSource hit;
    private float XiangduiV;
    float timeSpend;
    GameObject Fx;

    void Start()
    {
        hit = GetComponent<AudioSource>();
        can_kill = true;
        timeSpend = 0;
        Fx = (GameObject)Resources.Load("Prefabs/pa");
    }

    private void OnEnable()
    {
        can_kill = true;
        GameObject player = GameObject.Find("Player");
        this.gameObject.transform.position = new Vector3(16.83f, transform.position.y, transform.position.z); 
    }

    void Update()
    {
        XiangduiV = 2.0f / 30.0f * timeSpend + 1.1f;
        MspeedL = BackgroundMove.bcSpeed - XiangduiV;
        currentSpeed = MspeedL;

        transform.position += Vector3.right * currentSpeed * Time.deltaTime;
        timeSpend += Time.deltaTime;

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
         //   Debug.Log("与怪物发生碰撞");
            if (can_kill && !PlayerControl.isInBubble)
            {
                GameObject ww = Instantiate(Fx);
                ww.transform.position = this.transform.position;
                Destroy(ww, 2f);
                LightAndScore.Light -= 35;
                can_kill = false;
                Invoke("setCankill", 1f);
            }
        }
    }

    void setCankill()
    {
        can_kill = true;
    }

}