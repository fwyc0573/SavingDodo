using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowHook : MonoBehaviour
{
    public GameObject hook;
    private GameObject currentHook;
    public static Vector2 destination;        //hook位置
    private bool can_kill;
    private bool isGenerate = false;
    private float RopeStartYPosition = 6f;
    public static int hearts;

    public GameObject PrefabsHammerAndRope;
    public GameObject hammerAndRope;
    void Start()
    {
        can_kill = true;

        hammerAndRope=(GameObject)Instantiate(PrefabsHammerAndRope, transform.position, Quaternion.identity);
    }

    private void OnEnable()
    {
        can_kill = true;
    }

    void Update()
    {
        if (!isGenerate)
        {
            destination = new Vector2(transform.position.x + 3f, RopeStartYPosition);
            //Debug.Log(destination);
            currentHook = (GameObject)Instantiate(hook, transform.position, Quaternion.identity);
            currentHook.GetComponent<Rope>().destination = destination;
            currentHook.GetComponent<Rope>().endRopeObject = gameObject;

            currentHook.transform.SetParent(hammerAndRope.transform);
            transform.SetParent(hammerAndRope.transform);
            isGenerate = true;
        }
        Invoke("destroyAll", 5f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("摆锤发生碰撞");
            if (can_kill&&!PlayerControl.isInBubble)
            {
                LightAndScore.Light -= 60;
                can_kill = false;
                Invoke("setCankill", 1f);
            }
        }
    }

    void setCankill()
    {
        can_kill = true;
    }

    void destroyAll()
    {
        Destroy(GameObject.Find("hammerAndRope(Clone)"));
    }
}
