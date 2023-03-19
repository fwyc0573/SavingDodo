using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//audio signal   
/**************本脚本代码全部由组员独立编写！！！！*************/

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    public static int sampleFrebin;//采样数
    AudioSource _audioSource;


    public float[] _sample;//当前数组（用于记录当前帧的音频数据）
    public float[] _samlple_record;//记录数组（用于记录上一帧的音频数据）
    public static int MusicLength;
    public int Music_arraySize; //音频总时间* FPS = 需要开辟的数组
    public float[] _subEnergyList;//光谱通量记录数组

    public static float[] _subIndex;  //记录峰值光谱通量对应的帧数
    public static float[] _subDealValue; //记录峰值光谱通量对应的振幅

    public static int k; //当前帧的序号


    public int N;//当前滑动窗口序号
    float sum_value;
    public int special_index;
    public double average_value;//阈值

    public static bool can_play;//为实时处理分析而设置的布尔变量，为true时,PaceCatch脚本开始按帧搜寻已经产生的节拍，实现实时处理（即延迟8帧处理）
    public static int slider_size;//检测滑动窗口
    public static float thresholdMultiplier;//阈值调整系数
    public float peak_submin;//峰值特征（与两侧之差的最小值）
    double deal_sumValue;//临时处理变量
    public static float musiclength;


    public GameObject player;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        // _audioSource.mute = false;
        N = 0;
        sum_value = 0;
        special_index = 0;
        average_value = 0.0;
        sampleFrebin = 512;
        can_play = false;
        slider_size = 8;
        thresholdMultiplier = 2.0f;
        peak_submin = 0.3f;
        //thresholdMultiplier = 2.5f;
        //peak_submin = 0.3f;
        k = 0;
        _audioSource.volume = 1;

        _sample = new float[sampleFrebin];//512份
        _samlple_record = new float[sampleFrebin];//记录数组
        Music_arraySize = (int)_audioSource.clip.length * 150; //音频总时间*FPS
        _subEnergyList = new float[Music_arraySize];
        _subIndex = new float[Music_arraySize];
        _subDealValue = new float[Music_arraySize];
        musiclength = _audioSource.clip.length;

        Debug.Log("总时长：" + _audioSource.clip.length);
        Debug.Log("总帧数：" + _audioSource.clip.length * Time.deltaTime);
        MusicLength = (int)(_audioSource.clip.length + 0.5);

        //Debug.Log("初始化初始化初始化初始化初始化初始化初始化初始化初始化初始化");
        ;
    }


    void Update()
    {
        if(PlayingUI.isStop==false)
        {
            //k = Time.frameCount;
            k++;
            GetSpectrumAudioSource();
        }

    }

    void GetSpectrumAudioSource()
    {
        //当音频开始播放的时候开始分析处理
        if (_audioSource.isPlaying)
        {
            _audioSource.GetSpectrumData(_sample, 0, FFTWindow.Hanning);//傅里叶变换并使用汉宁窗口，_sample为变换后的频谱数据

            if (k == 0) //第0帧需要特殊处理（为了使记录数组_samlple_record起作用）
            {
                _samlple_record = (float[])_sample.Clone();
                _subEnergyList[k] = 0;//_subEnergyList[0]是无效值，应从[1]开始
            }
            else if (Music_arraySize > k && k > 0)// 防止越界并使得_samlple_record和_sample 两个数组的数据间隔一帧
            {

                SpectralFlux_Calcu();//计算相邻帧的光谱通量（相邻两帧的每个频率仓振幅差之和）

                SelectPositve();//将正变化光谱通量按原值保留，否则按0保留，记录在_subEnergyList[]中

                //对每8帧（滑动窗口的大小）的光谱通量进行节拍挑选，提升算法性能
                //在滑动窗口中处理音频数据，选择窗口中的平均值作为阈值
                if (k % slider_size == 0)
                {
                    Threshold_Calcu();//根据滑动窗口中数据挑选阈值，记录在average_value中

                    //再次遍历，对大于阈值的数据再次处理
                    for (int m = slider_size * N + 1; m < slider_size * (N + 1) + 1; m++)//N是当前滑动窗口的序号
                    {
                        //大于阈值
                        if (_subEnergyList[m] > (average_value + 0.6f))// +1是根据实际效果进行调整得到的
                        {
                            PeakSearch(m);//根据峰值特征寻找节拍
                        }
                    }
                    
                    //以上处理完一次间隔帧，记录当前帧为历史帧
                    _samlple_record = (float[])_sample.Clone();
                    N++;
                    can_play = true;//为实时处理分析而设置的布尔变量，为true时,PaceCatch脚本开始按帧搜寻已经产生的节拍，实现实时处理（即延迟8帧处理）
                }
                //对每8帧（滑动窗口的大小）的光谱通量进行节拍挑选，提升算法性能
                else
                {
                    //以上处理完一次间隔帧，记录当前帧为历史帧
                    _samlple_record = (float[])_sample.Clone();
                }
            }
        }

        else
        {
            Debug.Log("播放结束");
            //显示结束界面
        }
    }


    //光谱通量计算函数
    void SpectralFlux_Calcu()
    {
        sum_value = 0; //每一次计算重置sum_value为0
        for (int i = 0; i < 512; i++)
        {
            sum_value += _sample[i] - _samlple_record[i]; //光谱通量（相邻两帧的每个频率仓振幅差之和）
        }
    }

    //光谱通量修剪函数
    void SelectPositve()
    {
        if (sum_value > 0)//如果能量>0则记录，不然按0处理
        {
            _subEnergyList[k] = sum_value;
        }
        else
        {
            _subEnergyList[k] = 0;
        }
    }

    //阈值选择函数
    void Threshold_Calcu()
    {
        deal_sumValue = 0.0;

        for (int m = slider_size * N + 1; m < slider_size * (N + 1) + 1; m++)
        {
            //从平均值从而挑选节奏点
            deal_sumValue += _subEnergyList[m];
        }
        average_value = deal_sumValue / slider_size * thresholdMultiplier;//thresholdMultiplier是阈值调整系数

    }

    //根据峰值特征寻找节拍
    void PeakSearch(int m)
    {
        //大于左右相邻的修剪后的光谱通量（peak_submin为修剪光谱通量的最小差异）
        if (_subEnergyList[m] > (_subEnergyList[m - 1] + peak_submin) && _subEnergyList[m] > (_subEnergyList[m + 1] + peak_submin))//峰值特征
        {
            _subIndex[special_index] = m; //记录峰值光谱通量对应的帧数
            _subDealValue[special_index++] = _subEnergyList[m];//记录其振幅
        }
    }

}
