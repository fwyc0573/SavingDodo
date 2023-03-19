using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/**************本脚本代码全部由组员独立编写！！！！*************/

public class DDA : MonoBehaviour
{
    public static float Melody_accuracy;//玩家旋律感知正确率
    public float Press_accuracy;//玩家按键正确率
    public static float Diffculty_degree;//难度系数归一化值
    public static float Diffculty_degree_max;//难度系数函数最大值

    //public GameObject cube;

    void Start()
    {
        Melody_accuracy = 0;
        Press_accuracy = 0;
        Diffculty_degree = 0;
        Diffculty_degree_max = AudioPeer.MusicLength * AudioPeer.MusicLength;
        //Debug.Log(Diffculty_degree_max);
    }


    void Update()
    {
        if (Time.frameCount % 100 == 0)//每百帧进行一次难度系数调整
        {
            AdjustDiff();
        }
    }

    void AdjustDiff()
    {
        //对难度系数进行归一化处理，难度系数原始模型为 y = x * x
        // Debug.Log(Time.time);
        Diffculty_degree = Time.time * Time.time / (Diffculty_degree_max);
        //Debug.Log(Diffculty_degree);
        if (PaceCatch.peatNmuber > 0 && PaceCatch.player_sum_pressNmuber > 0)
        {
            //节奏感知正确率
            Melody_accuracy = (float)PaceCatch.player_peat_rightNumber / (float)PaceCatch.peatNmuber;
            //按键正确率
            Press_accuracy = (float)PaceCatch.player_peat_rightNumber / (float)PaceCatch.player_sum_pressNmuber;
            //修正难度系数数值

            if (Diffculty_degree > 0.0f && Diffculty_degree < 1.0f)
            {
                Diffculty_degree += 1.1f * 0.1f * (Melody_accuracy - 0.5f) + 4.1f * 0.1f * (Press_accuracy - 0.5f);

                if (Diffculty_degree < 0.0f)
                {
                    Diffculty_degree = 0;
                    //Debug.Log("小于0，变成0");
                }
                else if (Diffculty_degree > 1.0f)
                {
                    Diffculty_degree = 1;
                    Debug.Log("超过1，变成1");
                }
            }

            //具体进行调整参数
            Adjust_parameter();
        }

    }

    //调整各项具体的难度控制系数
    void Adjust_parameter()
    {
        //调整节奏阈值系数
        if (0.5f <= AudioPeer.thresholdMultiplier && AudioPeer.thresholdMultiplier <= 3.0f)
            AudioPeer.thresholdMultiplier = 3.0f - 2.5f * Diffculty_degree;

        //调整判定范围
        if (20 <= PaceCatch.correct_area && PaceCatch.correct_area <= 50)
            PaceCatch.correct_area = (int)(50.0f - 30.0f * Diffculty_degree);

        //调整节奏最小间隔，该部分宜微调不然会明显破坏节奏感
        if (PaceCatch.correct_area / 2 <= PaceCatch.interval && PaceCatch.interval <= 50)
            PaceCatch.interval = (int)(50.0f - (50.0f - PaceCatch.correct_area / 2) * Diffculty_degree);
    }

}

