using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**************本脚本代码全部由组员独立编写！！！！*************/

public class RamdonMapObject : MonoBehaviour
{
    public GameObject obj;
    private Vector3 objPosition;
    public float Xoffset = 17f;
    public float Yoffset = 0;
    private int ramdom;
    private int lastp;
    public  int[] P_obj;//每个obj对应的生成概率数组
    public  float[] G_finalp;//最后确定范围的数组
    private  int size;
    public static bool can_Generate1;
    public static bool can_Generate2;
    //第一关设定，size = 4
    public const int fruit_1 = 20;
    public const int leaf_1 = 20;
    public const int snake_1 = 20;
    public const int mokey_1 = 10;
    int[] store_1 = { fruit_1, leaf_1, snake_1, mokey_1 };

    //第二关设定,对应场景增加响应节拍表现惩罚奖赏（雨露、陨石、沼泽）；size = 7
    public const int fruit_2 = 20;
    public const int leaf_2 = 20;
    public const int snake_2 = 15;
    public const int mokey_2 = 8;
    public const int speedup_2 = 6;
    public const int slowdown_2 = 6;
    public const int softfruit_2 = 15;
    int[] store_2 = { fruit_2, leaf_2, snake_2, mokey_2, speedup_2 , slowdown_2 , softfruit_2 };

    //第三关设定；size = 10
    public const int fruit_3 = 16;
    public const int leaf_3 = 16;
    public const int snake_3 = 5;
    public const int mokey_3 = 8;
    public const int speedup_3 = 6;
    public const int slowdown_3 = 6;
    public const int softfruit_3 = 10;
    public const int bullet_3 = 15;
    public const int trap_3 = 15;
    public const int bubble_3 = 10;
    int[] store_3 = { fruit_3, leaf_3, snake_3, mokey_3, speedup_3, slowdown_3, softfruit_3, bullet_3 , trap_3 , bubble_3 };

    //第四关设定；size = 10
    public const int fruit_4 = 17;
    public const int leaf_4 = 15;
    public const int snake_4 = 18;
    public const int mokey_4 = 8;
    public const int speedup_4 = 7;
    public const int slowdown_4 = 7;
    public const int softfruit_4 = 12;
    public const int bullet_4 = 18;
    public const int trap_4 = 15;
    public const int bubble_4 = 6;
    int[] store_4 = { fruit_4, leaf_3, snake_4, mokey_4, speedup_4, slowdown_4, softfruit_4, bullet_4, trap_4, bubble_4 };


    public int pro_sum;
    public int index;
    private static float current_add;//难度调整变动

    private ObjectPool m_TestPool;
    private bool can_putback;//清除功能延迟控制变量
    public Queue<GameObject> instance_List;//已生成的obj集合
    private GameObject instance;//临时生成obj变量
    private int current_Pindex;


    void Start()
    {
        can_Generate1 = false;
        can_Generate2 = false;
        m_TestPool = ObjectPool.GetInstance();
        current_Pindex = LightAndScore.progress_index;
        Debug.Log("当前是关卡：" + current_Pindex);

        ramdom = 0;
        current_add = 0f;
        lastp = 0;

        if (current_Pindex == 1)
        {
            size = 4;
            P_obj = store_1;
        }
        else if (current_Pindex == 2)
        {
            size = 7;
            P_obj = store_2;
        }
        else if (current_Pindex == 3)
        {
            size = 10;
            P_obj = store_3;
        }
        else//4,5关
        {
            size = 10;
            P_obj = store_4;
        }

        G_finalp = new float[size * 2]; //size+1个

        for (int i = 0; i < size; i++)
        {
            pro_sum += P_obj[i];
        }

        pro_sum += size * 5;

        can_putback = false;
        instance_List = new Queue<GameObject>();
        instance = null;
    }

    void Update()
    {
        if (LightAndScore.still_alive && !PlayingUI.isStop)
        {
            if (Time.frameCount % (int)(-107 / Music.musicTime * Music.musicCurTime +150-index*10) == 0)
                Generate();
        }

        if (GameOver.gameOver == 1 || GameOver.gameOver == 2)
        {
            OrderCleanObj();
        }
    }


    void Generate()
    {
        current_add = 5 * DDA.Diffculty_degree;
        ramdom = Random.Range(1, pro_sum);//XX + size*5 = XXX （每个机关有5%的可调空间）

        lastp = 0;

        //各obj的范围生成
        for (int m = 0; m < size; m++)
        {
            G_finalp[m * 2] = lastp;
            if(m<2)
            {
                G_finalp[m * 2 + 1] = G_finalp[m * 2] + P_obj[m];
            }
            else
            {
                G_finalp[m * 2 + 1] = G_finalp[m * 2] + P_obj[m] + current_add;
            }
            lastp += P_obj[m] + 5;

        }


        for (int m = 0; m < size; m++)
        {
            //Debug.Log("G_finalp[m * 2]:" + G_finalp[m * 2]+"  G_finalp[m * 2 + 1]:" + G_finalp[m * 2 + 1]);
            //Debug.Log("m：" + m+"  size："+size);
            if (G_finalp[m * 2] <= ramdom && ramdom <= G_finalp[m * 2 + 1])
            {

                if (current_Pindex == 1 || current_Pindex == 5)
                {
                    Ge_1(m);
                }
                else if (current_Pindex == 2)
                {
                    Ge_2(m);
                }
                else if (current_Pindex == 3)
                {
                    Ge_3(m);
                }
                else
                {
                    Ge_3(m);
                }

                //按生成顺序清理
                instance_List.Enqueue(instance);
                Invoke("PutReBack", 12f);
                break;
            }
        }

        //定期清理池中杂物
        m_TestPool.CleanPool("Mask");
        m_TestPool.CleanPool("Glass");
        m_TestPool.CleanPool("Liquid");
        m_TestPool.CleanPool("Clothes");
        m_TestPool.CleanPool("MonsterC");
        m_TestPool.CleanPool("MonsterB");
        m_TestPool.CleanPool("MonsterA");
        m_TestPool.CleanPool("SpeedUp");
        m_TestPool.CleanPool("SpeedDown");
        m_TestPool.CleanPool("果实");
        m_TestPool.CleanPool("树叶");

        //按照生成顺序消除已出现的机关
        if (can_putback)
        {
            OrderCleanObj();
        }
    }

    void Ge_1(int m)
    {
        //fruit_2
        if (m == 0)
        {
            objPosition = new Vector3(20f, Random.Range(0, 3.4f), transform.position.z);
            instance = (GameObject)m_TestPool.GetObj("Glass", objPosition, transform.rotation);
        }
        //leaf_2
        else if (m == 1)
        {
            objPosition = new Vector3(Random.Range(14f, 20.1f), Random.Range(-1.7f, 3.4f), transform.position.z);
            instance = (GameObject)m_TestPool.GetObj("Clothes", objPosition, transform.rotation);
        }
        //snake_1
        else if (m == 2)
        {
            objPosition = new Vector3(17f, -3.05f, transform.position.z);
            instance = (GameObject)m_TestPool.GetObj("MonsterB", objPosition, transform.rotation);
        }
        //mokey_1
        else if (m == 3)
        {
            objPosition = new Vector3(7f, 2.4f, transform.position.z);
            instance = (GameObject)m_TestPool.GetObj("Monkey", objPosition, transform.rotation);
        }
    }

    void Ge_2(int m)
    {
        //fruit_2
        if (m == 0)
        {
            objPosition = new Vector3(20f, Random.Range(0, 3.4f), transform.position.z);
            instance = (GameObject)m_TestPool.GetObj("Glass", objPosition, transform.rotation);
        }
        //leaf_2
        else if (m == 1)
        {
            objPosition = new Vector3(Random.Range(14f, 20.1f), Random.Range(-1.7f, 3.4f), transform.position.z);
            instance = (GameObject)m_TestPool.GetObj("Clothes", objPosition, transform.rotation);
        }
        //snake_2
        else if (m == 2)
        {
            objPosition = new Vector3(17f, -3.05f, transform.position.z);
            instance = (GameObject)m_TestPool.GetObj("MonsterB", objPosition, transform.rotation);
        }
        //mokey_2
        else if (m == 3)
        {
            objPosition = new Vector3(7f, 2.4f, transform.position.z);
            instance = (GameObject)m_TestPool.GetObj("Monkey", objPosition, transform.rotation);
        }
        //speedup_2
        else if (m == 4)
        {
            objPosition = new Vector3(Random.Range(12.3f, 20.1f), Random.Range(-2.5f, 3f), transform.position.z);
            instance = (GameObject)m_TestPool.GetObj("SpeedUp", objPosition, transform.rotation);
        }
        //slowdown_2
        else if (m == 5)
        {
            objPosition = new Vector3(Random.Range(12.3f, 20.1f), Random.Range(-2.5f, 3f), transform.position.z);
            instance = (GameObject)m_TestPool.GetObj("SpeedDown", objPosition, transform.rotation);
        }
        //softfruit_2
        else if (m == 6)
        {
            objPosition = new Vector3(Random.Range(15f, 21f), 4f, transform.position.z-1f);
            instance = (GameObject)m_TestPool.GetObj("Fruit", objPosition, transform.rotation);
        }
    }

    void Ge_3(int m)
    {
        //fruit_2
        if (m == 0)
        {
            objPosition = new Vector3(20f, Random.Range(0, 3.4f), transform.position.z);
            instance = (GameObject)m_TestPool.GetObj("Glass", objPosition, transform.rotation);
        }
        //leaf_2
        else if (m == 1)
        {
            objPosition = new Vector3(Random.Range(14f, 20.1f), Random.Range(-1.7f, 3.4f), transform.position.z);
            instance = (GameObject)m_TestPool.GetObj("Clothes", objPosition, transform.rotation);
        }
        //snake_3
        else if (m == 2)
        {
            objPosition = new Vector3(17f, -3.05f, transform.position.z);
            instance = (GameObject)m_TestPool.GetObj("MonsterB", objPosition, transform.rotation);
        }
        //mokey_3
        else if (m == 3)
        {
            objPosition = new Vector3(7f, 2.4f, transform.position.z);
            instance = (GameObject)m_TestPool.GetObj("Monkey", objPosition, transform.rotation);
        }
        //speedup_3
        else if (m == 4)
        {
            objPosition = new Vector3(Random.Range(12.3f, 20.1f), Random.Range(-2.5f, 3f), transform.position.z);
            instance = (GameObject)m_TestPool.GetObj("SpeedUp", objPosition, transform.rotation);
        }
        //slowdown_3
        else if (m == 5)
        {
            objPosition = new Vector3(Random.Range(12.3f, 20.1f), Random.Range(-2.5f, 3f), transform.position.z);
            instance = (GameObject)m_TestPool.GetObj("SpeedDown", objPosition, transform.rotation);
        }
        //softfruit_2
        else if (m == 6)
        {
            objPosition = new Vector3(Random.Range(15f, 21f), 4f, transform.position.z-1f);
            instance = (GameObject)m_TestPool.GetObj("Fruit", objPosition, transform.rotation);
        }
        //bullet_3
        else if (m == 7)
        {
            objPosition = new Vector3(17f, Random.Range(-2.0f, 3f), transform.position.z);
            instance = (GameObject)m_TestPool.GetObj("MonsterA", objPosition, transform.rotation);
        }
        //trap_3
        else if (m == 8)
        {
            objPosition = new Vector3(Random.Range(19f, 21f), -3.52f, transform.position.z);
            instance = (GameObject)m_TestPool.GetObj("MonsterC", objPosition, transform.rotation);
        }
        //bubble_3
        else if (m == 9)
        {
            objPosition = new Vector3(17f, Random.Range(-2.9f, 3f), transform.position.z);
            instance = (GameObject)m_TestPool.GetObj("Bubble", objPosition, transform.rotation);
        }
    }


    //延迟清除控制
    void PutReBack()
    {
        can_putback = true;
    }

    //顺序清除函数
    void OrderCleanObj()
    {
        if (instance_List.Count > 0)
        {
            m_TestPool.RecycleObj(instance_List.Dequeue());
            //  instance_List.Remove(instance_List[0]);
        }

        can_putback = false;
    }
}
