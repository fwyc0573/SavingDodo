using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**************本脚本代码全部由组员独立编写！！！！*************/

public class PaceCatch : MonoBehaviour
{
    public int count = 0; //开始计时（单位：帧）
    public int record_justBoom = 0;

    public int searchinterval;//采集节奏间隔
    public static int interval;//节奏相邻最小间隔

    public static int correct_area;//判定区域，用于判定敲击键盘时刻是否在节拍出现的一个合理的范围内


     int Keydown_beat_index;//记录上一次敲击对应帧序号
    public static int peatNmuber;//节拍数（以按删选采集到的为标准）
    public static int player_peat_rightNumber;//玩家实现的正确节拍数
    public static int player_peat_wrongNumber;           //错误
    public static int player_sum_pressNmuber;//玩家一共的敲击次数
    private int temp_sum_score;//记录连续表现分数

    int p;//记录上一个
    BoxCollider2D PlayerCollider;

    public GameObject Efft_fx0;
    public GameObject Efft_fx1;
    public GameObject Efft_fx2;

    public GameObject Good;
    public GameObject Great;
    public GameObject Perfect;
    //记录连续表现数组

    private GameObject peatObj;
    GameObject yunshi;
    GameObject yudi;

    public static GameObject newpeat;
    public AudioSource rightAudio;
    public AudioSource wrongAudio;
    GameObject Fx;
    public static int continue_rightPeat;

    public struct Godd_appear
    {
        public List<int> gdap_score;//记录良好表现的得分
        public List<int> gdap_index;//记录良好表现的节拍序号
    }
    Godd_appear apstruct;



    void Start()
    {
        PlayerCollider = GameObject.Find("Player").transform.GetComponent<BoxCollider2D>();
        searchinterval = 1;
        correct_area = 120;//50帧为最大正确范围
        //interval = 80; //50帧为最大节奏间隔
        interval = 100;
        peatNmuber = 0;
        player_peat_rightNumber = 0;
        player_peat_wrongNumber = 0;
        player_sum_pressNmuber = 0;
        temp_sum_score = 0;

        apstruct.gdap_score = new List<int>(3);
        apstruct.gdap_index = new List<int>(3);

        p = 0;
        Fx = (GameObject)Resources.Load("Prefabs/CorrectFx");
        peatObj = (GameObject)Resources.Load("Prefabs/Peat");
        yunshi = (GameObject)Resources.Load("Prefabs/陨石单");
        yudi = (GameObject)Resources.Load("Prefabs/雨");

        continue_rightPeat = 0;
    }

    void Update()
    {
        if (AudioPeer.can_play&&GameOver.gameOver==0&&PlayingUI.isStop==false)
        {
            PlayerDetect();

            if (count % searchinterval == 0)
            {
                audioArrayDetect();
            }
            count++;


            if (Input.GetKey(KeyCode.A))//player_peat_wrongNumber % 11 == 0 && player_peat_wrongNumber != 0
            {
                Instantiate(yunshi, new Vector3(18.5f, 4f, transform.position.z), transform.rotation);
                player_peat_wrongNumber++;
                //音效
            }
            if (Input.GetKey(KeyCode.B))//player_peat_rightNumber % 6 == 0 && continue_rightPeat != 0
            {
                Instantiate(yudi,new Vector3(8f, 4f, transform.position.z), transform.rotation);
                player_peat_rightNumber++;
                //音效
            }
            //Debug.Log("连续次数：" + continue_rightPeat);
        }
        

    }

    //玩家按键响应函数
    void PlayerDetect()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Keydown_beat_index = count;//获取敲击时对应的帧
            player_sum_pressNmuber++;
            //反应及时
            if (Keydown_beat_index - record_justBoom < correct_area / 2 && Keydown_beat_index - record_justBoom >= 0)
            {
                //Debug.Log("慢了些-> " + (record_justBoom - Keydown_beat_index));
                newpeat.GetComponent<PeatMotion>().can_boom = true;
                rightAudio.Play();
                LightAndScore.Score += 2;
                LightAndScore.Light += 2;

                //奖励判定
                Correct_award();

                player_peat_rightNumber++;
                record_justBoom = -100;//兑换成功，清空记录
                Keydown_beat_index = -100;

                //Debug.Log("敲对了，连续次数：" + continue_rightPeat);
            }
            //不及时
            else if (Keydown_beat_index - record_justBoom >= correct_area / 2 && record_justBoom != -100 && Keydown_beat_index - record_justBoom >= 0)
            {
                wrongAudio.Play();
                //错误计数+1
                player_peat_wrongNumber++;

                //Debug.Log("敲错了：" + continue_rightPeat);
            }
        }
    }

    //节奏点找寻函数
    void audioArrayDetect()
    {
        for (int i = p; i < AudioPeer.k; i++)
        {
            //Debug.Log(AudioPeer._subIndex[i]);
            if (AudioPeer._subIndex[i] == count && count - record_justBoom > interval)
            {
                p = i;
                // Debug.Log("节奏点出现");
                Debug.Log("A：" + AudioPeer._subDealValue[i]);

                peatNmuber++;

                record_justBoom = count;
                if (record_justBoom - Keydown_beat_index < correct_area / 2 && record_justBoom - Keydown_beat_index >= 0)
                {
                    Debug.Log("提前预知!!");
                    GameObject ww = Instantiate(Fx);
                    ww.transform.position = new Vector2(Random.Range(3.85f, 6.55f), Random.Range(-1.25f, 0.95f));
                    LightAndScore.Score += 2;
                    LightAndScore.Light += 2;
                    rightAudio.Play();

                    //Debug.Log("敲对了，连续次数：" + continue_rightPeat);
                }
                else if (record_justBoom - Keydown_beat_index >= correct_area / 2 && record_justBoom != -100 && record_justBoom - Keydown_beat_index >= 0)
                {
                    wrongAudio.Play();
                    //错误计数+1
                    player_peat_wrongNumber++;
                    newpeat = Instantiate(peatObj);
 
                    //Debug.Log("敲错了：" + continue_rightPeat);
                }
                else if(record_justBoom ==-100)
                {
                    newpeat = Instantiate(peatObj);
                }

                break;
            }
        }
    }

    private IEnumerator CloseText(GameObject text)
    {
        yield return new WaitForSeconds(1.5f);
        text.SetActive(false);

    }



    //奖励判定函数
    void Correct_award()
    {
        if (apstruct.gdap_score.Count == 0 || (apstruct.gdap_index[apstruct.gdap_index.Count - 1] + 1) == peatNmuber) //数组为0或者序号连接在一起则可以加入
        {
            apstruct.gdap_score.Add((record_justBoom - Keydown_beat_index));//记录帧判定差距
            apstruct.gdap_index.Add(peatNmuber);//记录节奏序号
            continue_rightPeat++;
            if (apstruct.gdap_score.Count == 3)
            {
                temp_sum_score = (-apstruct.gdap_score[0] -apstruct.gdap_score[1] -apstruct.gdap_score[2]) / 3;
                if (temp_sum_score <= 30)
                {
                    Debug.Log("超级牛逼!");
                    Perfect.SetActive(true);
                    StartCoroutine(CloseText(Perfect));
                }
                else if (temp_sum_score <= 45)
                {
                    Debug.Log("牛逼!");
                    Great.SetActive(true);
                    StartCoroutine(CloseText(Great));
                }
                else
                {
                    Debug.Log("不错！");
                    Good.SetActive(true);
                    StartCoroutine(CloseText(Good));

                }

                apstruct.gdap_score.Clear();
                apstruct.gdap_index.Clear();
            }
        }
        else
        {
            apstruct.gdap_score.Clear();
            apstruct.gdap_index.Clear();
            continue_rightPeat = 0;
        }
    }
    
    void CloseEff(GameObject ef)
    {
        ef.SetActive(false);
    }
}
