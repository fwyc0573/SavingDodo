using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAndScore : MonoBehaviour
{
    public static int Light;//光能量，即生命值
    public static int Score;//得分
    public static bool still_alive;//存活状态
    public static float MusicLength;

    public static int progress_index;

    public const int stageone = 15;
    public const int stagetwo = 35;
    public const int stagethree = 80;

    public GameObject fx1;

    void Start()
    {
        progress_index  = 1;//对应关卡号
        Light = 150;
        Score = 0;
        still_alive = true;
        MusicLength = AudioPeer.musiclength;
    }
  
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    Light += 50;
        //}

        if (Light>150 || Light ==150)
        {
            Light = 150;
            fx1.SetActive(true);
        }
       else
        {
            fx1.SetActive(false);
        }

        if (Light < 0)
        {
            Light = 0;
            still_alive = false;

        }
        if (GameOver.gameOver != 0)
        {
            fx1.SetActive(false);
        }
    }


}
