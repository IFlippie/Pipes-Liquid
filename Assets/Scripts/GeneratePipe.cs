using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class GeneratePipe : MonoBehaviour
{
    Mesh me;
    MeshFilter mf;
    Vector3[] pipeVertices;
    public List<GameObject> points = new List<GameObject>();
    int[] triangles;
    int layers;
    public Vector3 direction;

    [Header("Pipe")]
    public int verticesPerPoint;
    //Distance between the vertices in each layer
    public float pipeRadius;
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
        layers = points.Count;
        SpawnPoints();
        //transform.rotation = points[0].transform.rotation;
        //transform.position = GetComponent<Renderer>().bounds.center;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPoints() 
    {
        for (int i = 0; i < points.Count; i++)
        {
            Debug.DrawRay(points[i].transform.position, points[i].transform.up * 3f, Color.red, 10000f);
            Debug.DrawRay(points[i].transform.position, -points[i].transform.up * 3f, Color.red, 10000f);
            Debug.DrawRay(points[i].transform.position, -points[i].transform.right * 3f, Color.red, 10000f);
            Debug.DrawRay(points[i].transform.position, points[i].transform.right * 3f, Color.red, 10000f);

            if (Physics.Raycast(points[i].transform.position, points[i].transform.up * 3f, out RaycastHit upHit)) { }
            if (Physics.Raycast(points[i].transform.position, -points[i].transform.up * 3f, out RaycastHit downHit)) { }
            if (Physics.Raycast(points[i].transform.position, -points[i].transform.right * 3f, out RaycastHit leftHit)) { }
            if (Physics.Raycast(points[i].transform.position, points[i].transform.right * 3f, out RaycastHit rightHit)) { }

            //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //cube.transform.position = points[i];
        }

        //StartCoroutine(GeneratePipeInSteps());
        float vStep = (2f * Mathf.PI) / verticesPerPoint;


        pipeVertices = new Vector3[verticesPerPoint * layers];
        //print(pipeVertices.Length);
        for (int k = 0, j = 0; j < layers; j++)
        {
            for (int o = 0; o < verticesPerPoint; o++, k++)
            {
                Vector3 p;
                float r = pipeRadius * Mathf.Cos(o * vStep);
                p.x = points[j].transform.position.x + (r * Mathf.Sin(0f));
                p.y = points[j].transform.position.y + (r * Mathf.Cos(0f));
                p.z = points[j].transform.position.z + (pipeRadius * Mathf.Sin(o * vStep));
                var vPos = p;
                pipeVertices[k] = vPos;

                Quaternion q = points[j].transform.rotation;
                pipeVertices[k] = q * (pipeVertices[k] - points[j].transform.position) + points[j].transform.position;

                //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //cube.transform.position = vPos;
                //cube.transform.localScale = cube.transform.localScale * 0.1f;
                //cube.transform.position = q * (cube.transform.position - points[j].transform.position) + points[j].transform.position;
                //print(k);
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

    private IEnumerator GeneratePipeInSteps() 
    {
        yield return new WaitForSeconds(1f);
        float vStep = (2f * Mathf.PI) / verticesPerPoint;

        pipeVertices = new Vector3[verticesPerPoint * layers];
        print(pipeVertices.Length);
        for (int k = 0, j = 0; j < layers; j++)
        {
            for (int o = 0; o < verticesPerPoint; o++, k++)
            {
                Vector3 p;
                float r = pipeRadius * Mathf.Cos(o * vStep);
                p.x = points[j].transform.position.x + (r * Mathf.Sin(0f));
                p.y = points[j].transform.position.y + (r * Mathf.Cos(0f));
                p.z = points[j].transform.position.z + (pipeRadius * Mathf.Sin(o * vStep));
                var vPos = p;
                pipeVertices[k] = vPos;

                //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //cube.transform.position = vPos;
                //cube.transform.localScale = cube.transform.localScale * 0.1f;
                yield return new WaitForSeconds(1f);
                print(k);
            }
        }
        me.vertices = pipeVertices;

        triangles = new int[verticesPerPoint * layers * 6];
        for (int ti = 0, vi = 0, z = 0; z < layers; z++, vi++)
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
                    print("standard" + vi);
                    yield return new WaitForSeconds(1f);
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
                    print("standard" + vi);
                    yield return new WaitForSeconds(1f);
                    me.triangles = triangles;
                }
            }
        }
        me.triangles = triangles;
        me.RecalculateNormals();
    }
}
