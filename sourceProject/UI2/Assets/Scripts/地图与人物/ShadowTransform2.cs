using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowTransform2 : MonoBehaviour
{
    public Transform Hunter;
    public GameObject ob;
    void Start()
    {
        
    }

    void Update()
    {
        if (ob.activeInHierarchy)
            this.gameObject.SetActive(true);
        else
            this.gameObject.SetActive(false);
        this.transform.position = new Vector3(Hunter.position.x, this.transform.position.y, this.transform.position.z);
    }
}
