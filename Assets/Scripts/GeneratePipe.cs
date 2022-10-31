using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class GeneratePipe : MonoBehaviour
{
    Mesh me;
    MeshFilter mf;
    Vector3[] pipeVertices;
    public List<GameObject> pipePoints = new List<GameObject>();
    public List<GameObject> dirPoints = new List<GameObject>();
    int[] triangles;
    int layers;
    public Vector3 direction;

    [Header("Pipe")]
    public int verticesPerPoint;
    //Distance between the vertices in each layer
    public float pipeRadius;
    public GameObject endPoint;
    // Start is called before the first frame update
    void Start()
    {
        mf = GetComponent<MeshFilter>();
        me = new Mesh()
        {
            name = "Pipe Part"
        };

        mf.mesh = me;
        me.Clear();
        layers = pipePoints.Count;
        SmoothPipeSpawnPoints();
        //FlatPipeSpawnPoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SmoothPipeSpawnPoints() 
    {
        for (int i = 0; i < dirPoints.Count; i++)
        {
            //Debug.DrawRay(dirPoints[i].transform.position, dirPoints[i].transform.up * 4f, Color.red, 10000f);
            Debug.DrawRay(dirPoints[i].transform.position, -dirPoints[i].transform.up * 4f, Color.red, 10000f);
            //Debug.DrawRay(dirPoints[i].transform.position, -dirPoints[i].transform.right * 4f, Color.red, 10000f);
            //Debug.DrawRay(dirPoints[i].transform.position, dirPoints[i].transform.right * 4f, Color.red, 10000f);

            if (Physics.Raycast(dirPoints[i].transform.position, dirPoints[i].transform.up * 4f, out RaycastHit upHit)) { }
            if (Physics.Raycast(dirPoints[i].transform.position, -dirPoints[i].transform.up * 4f, out RaycastHit downHit)) { pipePoints[i + 1].transform.position += dirPoints[i].transform.up; }
            if (Physics.Raycast(dirPoints[i].transform.position, -dirPoints[i].transform.right * 4f, out RaycastHit leftHit)) { }
            if (Physics.Raycast(dirPoints[i].transform.position, dirPoints[i].transform.right * 4f, out RaycastHit rightHit)) { }
        }

        float vStep = (2f * Mathf.PI) / verticesPerPoint;


        pipeVertices = new Vector3[verticesPerPoint * layers];
        //print(pipeVertices.Length);
        for (int k = 0, j = 0; j < layers; j++)
        {
            for (int o = 0; o < verticesPerPoint; o++, k++)
            {
                Vector3 p;
                float r = pipeRadius * Mathf.Cos(o * vStep);
                p.x = pipePoints[j].transform.position.x + (r * Mathf.Sin(0f));
                p.y = pipePoints[j].transform.position.y + (r * Mathf.Cos(0f));
                p.z = pipePoints[j].transform.position.z + (pipeRadius * Mathf.Sin(o * vStep));
                var vPos = p;
                pipeVertices[k] = vPos;

                Quaternion q = pipePoints[j].transform.rotation;
                pipeVertices[k] = q * (pipeVertices[k] - pipePoints[j].transform.position) + pipePoints[j].transform.position;
            }
        }
        me.vertices = pipeVertices;

        triangles = new int[verticesPerPoint * layers * 6];
        for (int ti = 0, vi = 0, z = 0; z < layers - 1; z++, vi++)
        {
            for (int x = 0; x < verticesPerPoint; x++, ti += 6)
            {
                if (x < verticesPerPoint - 1)
                {
                    //how and why does the normal way not work
                    //so 2/3 and 1/4 switch to properly show the triangles
                    triangles[ti] = vi;
                    triangles[ti + 2] = triangles[ti + 3] = vi + 1;
                    triangles[ti + 1] = triangles[ti + 4] = vi + verticesPerPoint;
                    triangles[ti + 5] = vi + verticesPerPoint + 1;
                    vi++;
                    me.triangles = triangles;
                }
                else
                {
                    triangles[ti] = vi;
                    triangles[ti + 2] = vi - verticesPerPoint + 1;
                    triangles[ti + 1] = vi + verticesPerPoint;
                    triangles[ti + 4] = vi + verticesPerPoint;
                    triangles[ti + 3] = vi - verticesPerPoint + 1;
                    triangles[ti + 5] = vi + 1;
                    me.triangles = triangles;
                }
            }
        }
        me.triangles = triangles;
        me.RecalculateNormals();
    }

    public void FlatPipeSpawnPoints()
    {
        float vStep = (2f * Mathf.PI) / verticesPerPoint;

        //first part represents the amount of vertices in start and finish, second part represents all the vertices inbetween
        //is this correct???             5 * 3 = 15 * 2 = 30                       5 * 7 = 35 * 5 = 175
        pipeVertices = new Vector3[(2 * (verticesPerPoint * 3)) + ((verticesPerPoint * (layers-2)) * 5)];
        print("verticesperpoint " + verticesPerPoint);
        print("layers " + layers);
        print(pipeVertices.Length);//205
        for (int k = 0, c = 0; c < 4; c++)//5
        {
            for (int j = 0; j < layers; j++)//9
            {
                for (int o = 0; o < verticesPerPoint; o++, k++)//5
                {
                    //2 * 7 * 5 = 70
                    //3 * 9 * 5 = 135
                    if ((c == 3 && layers == 0) || (c == 4 && layers == layers - 1) || (c == 3 && layers == layers - 1) || (c == 4 && layers == 0)) { break; }
                    Vector3 p;
                    float r = pipeRadius * Mathf.Cos(o * vStep);
                    p.x = pipePoints[j].transform.position.x + (r * Mathf.Sin(0f));
                    p.y = pipePoints[j].transform.position.y + (r * Mathf.Cos(0f));
                    p.z = pipePoints[j].transform.position.z + (pipeRadius * Mathf.Sin(o * vStep));
                    var vPos = p;
                    pipeVertices[k] = vPos;
                    //print(k);

                    Quaternion q = pipePoints[j].transform.rotation;
                    pipeVertices[k] = q * (pipeVertices[k] - pipePoints[j].transform.position) + pipePoints[j].transform.position;
                }
            }
        }
        
        me.vertices = pipeVertices;

        //triangles = new int[verticesPerPoint * layers * 6];
        //for (int ti = 0, vi = 0, z = 0; z < layers - 1; z++, vi++)
        //{
        //    for (int x = 0; x < verticesPerPoint; x++, ti += 6)
        //    {
        //        if (x < verticesPerPoint - 1)
        //        {
        //            //how and why does the normal way not work
        //            //so 2/3 and 1/4 switch to properly show the triangles
        //            triangles[ti] = vi;
        //            triangles[ti + 2] = triangles[ti + 3] = vi + 1;
        //            triangles[ti + 1] = triangles[ti + 4] = vi + verticesPerPoint;
        //            triangles[ti + 5] = vi + verticesPerPoint + 1;
        //            vi++;
        //            me.triangles = triangles;
        //        }
        //        else
        //        {
        //            triangles[ti] = vi;
        //            triangles[ti + 2] = vi - verticesPerPoint + 1;
        //            triangles[ti + 1] = vi + verticesPerPoint;
        //            triangles[ti + 4] = vi + verticesPerPoint;
        //            triangles[ti + 3] = vi - verticesPerPoint + 1;
        //            triangles[ti + 5] = vi + 1;
        //            me.triangles = triangles;
        //        }
        //    }
        //}
        //me.triangles = triangles;
        //me.RecalculateNormals();
    }
}
