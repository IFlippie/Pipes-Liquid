using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRing : MonoBehaviour
{
    Vector3[] pipeVertices;
    public float pipeRadius;
    public int verticesPerPoint;
    public List<GameObject> points = new List<GameObject>();
    public float x;
    public float y;
    public float z;
    // Start is called before the first frame update
    void Start()
    {
        //https://answers.unity.com/questions/764877/rotating-procedurally-generated-meshes.html
        for (int o = 0; o < verticesPerPoint; o++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            cube.transform.localScale = cube.transform.localScale * 0.1f;
            points.Add(cube);
        }

        float vStep = (2f * Mathf.PI) / verticesPerPoint;
        pipeVertices = new Vector3[verticesPerPoint];
        for (int o = 0; o < verticesPerPoint; o++)
        {
            Vector3 p;
            float r = pipeRadius * Mathf.Cos(o * vStep);
            p.x = transform.position.x + (r * Mathf.Sin(0f));
            p.y = transform.position.y + (r * Mathf.Cos(0f));
            p.z = transform.position.z + (pipeRadius * Mathf.Sin(o * vStep));
            var vPos = p;
            pipeVertices[o] = vPos;

            points[o].transform.position = vPos;
        }

    }

    // Update is called once per frame
    void Update()
    {
        for (int o = 0; o < verticesPerPoint; o++)
        {

            Quaternion q = Quaternion.Euler(0f, 1f, 0f);
            pipeVertices[o] = q * (pipeVertices[o] - transform.position) + transform.position;

            points[o].transform.position = pipeVertices[o];
        }
    }
}
