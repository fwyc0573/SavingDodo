using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using NAudio.Wave;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using UnityEngine.SceneManagement;

//本代码逻辑由组员编写，使用了NAudio库，用于将MP3转为WAV格式（Unity不支持动态加载MP3格式文件）
public class MusicLoad : MonoBehaviour
{
    public Button loadMp3;
    public   AudioSource MP3;
    public static AudioClip clickSfx;
    public string url;
    public AudioClip music1;
    public GameObject F;
    public GameObject X;
    bool rlength = true;
    void Start()
    {
        loadMp3.onClick.AddListener(LoadMusic);
        MP3 = gameObject.GetComponent<AudioSource>();
        clickSfx = MP3.clip;
        F.SetActive(false);
    }

    void Update()
    {
        if (rlength == false)
        {
            if (F.activeInHierarchy == false)
            {
                F.SetActive(true);
            }
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit2 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit2.collider != null)
                {
                    if (hit2.collider.gameObject.name == "XP")
                    {
                        if (X.activeInHierarchy)
                        {
                            X.SetActive(false);
                        }
                    }
                    //else
                    //{
                    //    X.SetActive(true);
                    //}

                }
               
            }
            if (Input.GetMouseButtonUp(0))//当抬起时
            {
                X.SetActive(true);
                //抬起响应
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null)
                {

                    if (hit.collider.gameObject.name == "XP")
                    {
                        rlength = true;
                        F.SetActive(false);
                    }
                }
            }
        }
    }

    //加载音乐
    void LoadMusic()
    {

        //获取到Assets文件夹的路径
        OpenFileName dialog = new OpenFileName();

        dialog.structSize = Marshal.SizeOf(dialog);

        dialog.filter = "mp3 files\0*.mp3";

        dialog.file = new string(new char[256]);

        dialog.maxFile = dialog.file.Length;

        dialog.fileTitle = new string(new char[64]);

        dialog.maxFileTitle = dialog.fileTitle.Length;

        dialog.initialDir = UnityEngine.Application.dataPath;  //默认路径

        dialog.title = "请选择导入的MP3歌曲";

        dialog.defExt = "mp3";//显示文件的类型
        //注意一下项目不一定要全选 但是0x00000008项不要缺少
        dialog.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;  //OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR

        //if (DialogShow.GetOpenFileName(dialog))
        //{
        //    Debug.Log(dialog.file);
        //}

        if (WindowDll.GetOpenFileName(dialog))
        {
            StartCoroutine(LoadMusic(dialog.file, @"D:\音频文件.wav"));
            //StartCoroutine(Load(ofn.file));
        }

        //MP3.Play();

    }

    private IEnumerator LoadMusic(string filepath, string savepath)
    {
        ChooseMusic.musicNum = 1;
        var stream = File.Open(filepath, FileMode.Open);
        var reader = new Mp3FileReader(stream);
        WaveFileWriter.CreateWaveFile(savepath, reader);
        var www = new WWW("file://" + savepath);
        yield return www;
        var clip = www.GetAudioClip();
        MP3.clip = clip;
        clickSfx = MP3.clip;
        Debug.Log(clickSfx.length);
        if(clickSfx.length> 60 && clickSfx.length<300) //歌曲长度应该在60s~300s之间
        {
            rlength = true;
            saveAudio.a = MP3;
            gameObject.SetActive(false);
            PlayerControl.MaskNum = 0;
            PlayerControl.LiquidNum = 0;
            PlayerControl.GlassNum = 0;
            PlayerControl.ClothesNum = 0;
            LightAndScore.Score = 0;
            Music.music1 = clickSfx;
            Invoke("backmusic1", 3f);
            SceneManager.LoadScene("GamingScene");
        }
        else
        {
            rlength = false;
        }
    }

    public IEnumerator LoadAudio(string recordPath)
    {
        WWW www = new WWW("file:///" + recordPath);
        yield return www;
        AudioClip clip = www.GetAudioClip();
        MP3.clip = clip;
        clickSfx = MP3.clip;
        MP3.Play();
    }

    void backmusic1()
    {
        Music.music1 = music1;
    }


}
