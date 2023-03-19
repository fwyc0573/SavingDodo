using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    public float MspeedL;
    public float currentSpeed;

    private void OnEnable()
    {
        MspeedL = BackgroundMove.bcSpeed;
        currentSpeed = MspeedL;
        GameObject player = GameObject.Find("Player");
        this.gameObject.transform.position = new Vector3(Random.Range(12.3f, 20.1f), Random.Range(-3.0f, 2.85f), transform.position.z);
    }

    void Start()
    {
        MspeedL = BackgroundMove.bcSpeed;
        currentSpeed = MspeedL;
    }


    void Update()
    {
        transform.position += Vector3.right * currentSpeed * Time.deltaTime;
    }


}

