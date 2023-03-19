using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public Vector2 destination;   //终点hook位置
    public float speed = 1;
    public float distance = 1;

    public GameObject nodePrefab;
    public GameObject endRopeObject;
    public GameObject lastNode;

    public LineRenderer lr;

    int vertexCount = 2;
    public List<GameObject> Nodes = new List<GameObject>();

    private bool done = false;

    void Start()
    {
        lr = GetComponent<LineRenderer>();

        //endRopeObject = GameObject.Find("Circle(Clone)");
        lastNode = transform.gameObject;
        Nodes.Add(transform.gameObject);

        destination = ThrowHook.destination;
    }


    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, speed);

        if ((Vector2)transform.position != destination)
        {
            if (Vector2.Distance(endRopeObject.transform.position, lastNode.transform.position) > distance)
            {
                CreateNode();
            }
        }
        else if (done == false)
        {
            done = true;

            while (Vector2.Distance(endRopeObject.transform.position, lastNode.transform.position) > distance)
            {
                CreateNode();
            }

            lastNode.GetComponent<HingeJoint2D>().connectedBody = endRopeObject.GetComponent<Rigidbody2D>();
        }

        RenderLine();
    }

    void RenderLine()
    {
        lr.SetVertexCount(vertexCount);
        int i;
        for (i = 0; i < Nodes.Count; i++)
        {
            lr.SetPosition(i, Nodes[i].transform.position);
        }
        lr.SetPosition(i, endRopeObject.transform.position);
    }


    void CreateNode()
    {
        Vector2 posCreate = endRopeObject.transform.position - lastNode.transform.position;
        posCreate.Normalize();
        posCreate *= distance;
        posCreate += (Vector2)lastNode.transform.position;

        GameObject go = (GameObject)Instantiate(nodePrefab, posCreate, Quaternion.identity);
        go.transform.SetParent(transform);

        lastNode.GetComponent<HingeJoint2D>().connectedBody = go.GetComponent<Rigidbody2D>();

        lastNode = go;

        Nodes.Add(lastNode);

        vertexCount++;
    }
}
