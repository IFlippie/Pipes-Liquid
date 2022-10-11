using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class GeneratePipe : MonoBehaviour
{
    Mesh me;
    MeshFilter mf;
    Vector3[] pipeVertices;
    public List<Vector3> points = new List<Vector3>();

    [Header("Arms")]
    public int verticesPerPoint;
    //Distance between the vertices in each layer
    public float pipeRadius;
    public float ringDistance;
    //Distance from the middle of the torus, should be unrelated for us
    public float curveRadius;
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
        SpawnPoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPoints() 
    {
        for (int i = 0; i < points.Count; i++)
        {
            Debug.DrawRay(points[i], transform.up * 3f, Color.red, 10000f);
            Debug.DrawRay(points[i], -transform.up * 3f, Color.red, 10000f);
            Debug.DrawRay(points[i], -transform.right * 3f, Color.red, 10000f);
            Debug.DrawRay(points[i], transform.right * 3f, Color.red, 10000f);

            if (Physics.Raycast(points[i], transform.up * 3f, out RaycastHit upHit)) { }
            if (Physics.Raycast(points[i], -transform.up * 3f, out RaycastHit downHit)) { }
            if (Physics.Raycast(points[i], -transform.right * 3f, out RaycastHit leftHit)) { }
            if (Physics.Raycast(points[i], transform.right * 3f, out RaycastHit rightHit)) { }

            //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //cube.transform.position = points[i];
        }
      
        float vStep = (2f * Mathf.PI) / verticesPerPoint;
        //float uStep = ringDistance / curveRadius;
            
        pipeVertices = new Vector3[verticesPerPoint * points.Count];
        print(pipeVertices.Length);
        for (int k = 0, j = 0; j <= points.Count; j++)
        {
            for (int o = 0; o <= verticesPerPoint - 1; o++, k++)
            {
                Vector3 p;
                float r = pipeRadius * Mathf.Cos(o * vStep);
                p.x = points[j].x + (r * Mathf.Sin(j));
                p.y = points[j].y + (r * Mathf.Cos(j));
                p.z = points[j].z + (pipeRadius * Mathf.Sin(o * vStep));
                var vPos = p;
                pipeVertices[k] = vPos;
                print(k);
            }
        }
        me.vertices = pipeVertices;
    }
}
