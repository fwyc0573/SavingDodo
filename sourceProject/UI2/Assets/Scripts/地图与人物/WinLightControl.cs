using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLightControl : MonoBehaviour
{
    public GameObject Player;
    private float speed;
    public bool light_change;
    private float lastTime;
    private float curTime;

    void Start()
    {
        speed = 2f;
        light_change = true;
    }

    void Update()
    {
        if (GameOver.gameOver == 1)
        {
            Debug.Log("游戏结束：" + GameOver.gameOver);
            if (light_change)
            {
                light_change = false;
                lastTime = Time.time;
            }

            curTime = Time.time;
            Debug.Log("curTime：" + curTime);
            Player.transform.position += Vector3.right * speed * Time.deltaTime;
            if ((curTime - lastTime) >= 7f)
            {
                GameOver.isOut = true;
            }
        }
    }

}
