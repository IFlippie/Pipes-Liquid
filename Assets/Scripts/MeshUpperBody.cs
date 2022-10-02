using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshUpperBody : MonoBehaviour
{
    Mesh me;
    MeshFilter mf;
    Vector3[] armLVertices;
    Vector3[] armRVertices;
    Vector3[] bodyVertices;
    int[] triangles;

    [Header("Arms")]
    public int vertsPerLayer;
    public int layers;
    public float pipeRadius;
    public float curveRadius;
    public float ringDistance;
    public float bodyOffset;

    // Start is called before the first frame update
    void Start()
    {
        mf = GetComponent<MeshFilter>();
        me = new Mesh()
        {
            name = "Upper Body"
        };

        mf.mesh = me;
    }

    // Update is called once per frame
    void Update()
    {
        //GeneratePipe();
        StartCoroutine(GenerateArmsInSteps());
    }

    //shows generated vertices in the editor
    private void OnDrawGizmos()
    {
        //for (int i = 0; i < armLVertices.Length; i++)
        //{
        //    Gizmos.DrawSphere(armLVertices[i], .1f);
        //}
        //for (int i = 0; i < armRVertices.Length; i++)
        //{
        //    Gizmos.DrawSphere(armRVertices[i], .1f);
        //}
        //for (int i = 0; i < bodyVertices.Length; i++)
        //{
        //    Gizmos.DrawSphere(bodyVertices[i], .1f);
        //}
    }

    private void GeneratePipe() 
    {
        me.Clear();
        float vStep = (2f * Mathf.PI) / vertsPerLayer;
        float uStep = ringDistance / curveRadius;
        // 3 armlengths + 10 verts will mean 40 vertices or 4 rings
        armLVertices = new Vector3[vertsPerLayer * (layers + 1)];
        for (int k = 0, j = 0; j <= layers; j++)
        {
            for (int i = 0; i <= vertsPerLayer - 1; i++, k++)
            {
                Vector3 p;
                float r = (curveRadius + pipeRadius * Mathf.Cos(i * vStep));
                p.x = r * Mathf.Sin(j * uStep);
                p.y = r * Mathf.Cos(j * uStep);
                p.z = pipeRadius * Mathf.Sin(i * vStep);
                var pos = p;
                armLVertices[k] = pos;
            }
        }
        me.vertices = armLVertices;

        int[] triangles = new int[vertsPerLayer * layers * 6];
        for (int ti = 0, vi = 0, z = 0; z < layers; z++, vi++)
        {
            for (int x = 0; x < vertsPerLayer; x++, ti += 6)
            {
                if (x < vertsPerLayer - 1)
                {
                    //how and why does the normal way not work
                    //so 2/3 and 1/4 switch to properly show the triangles
                    triangles[ti] = vi;
                    triangles[ti + 4] = triangles[ti + 1] = vi + 1;
                    triangles[ti + 3] = triangles[ti + 2] = vi + vertsPerLayer;
                    triangles[ti + 5] = vi + vertsPerLayer + 1;
                    vi++;
                    //yield return new WaitForSeconds(1f);
                    me.triangles = triangles;
                }
                else
                {
                    triangles[ti] = vi;
                    triangles[ti + 1] = vi - vertsPerLayer + 1;
                    triangles[ti + 2] = vi + vertsPerLayer;
                    triangles[ti + 3] = vi + vertsPerLayer;
                    triangles[ti + 4] = vi - vertsPerLayer + 1;
                    triangles[ti + 5] = vi + 1;
                    //yield return new WaitForSeconds(1f);
                    me.triangles = triangles;
                }
            }
        }
        me.triangles = triangles;
        me.RecalculateNormals();
    }

    private IEnumerator GenerateArmsInSteps()
    {
        yield return new WaitForSeconds(1f);
        me.Clear();
        float vStep = (2f * Mathf.PI) / vertsPerLayer;
        float uStep = ringDistance / curveRadius;
        // 3 armlengths + 10 verts will mean 40 vertices or 4 rings
        armLVertices = new Vector3[vertsPerLayer * (layers + 1)];
        for (int k = 0, j = 0; j <= layers; j++)
        {
            for (int i = 0; i <= vertsPerLayer - 1; i++, k++)
            {
            Vector3 p;
            float r = (curveRadius + pipeRadius * Mathf.Cos(i * vStep));
            p.x = r* Mathf.Sin(j* uStep);
            p.y = r* Mathf.Cos(j* uStep);
            p.z = pipeRadius* Mathf.Sin(i* vStep);
            var pos = p;
            armLVertices[k] = pos;
            }
        }
        me.vertices = armLVertices;

        int[] triangles = new int[vertsPerLayer * layers * 6];
        for (int ti = 0, vi = 0, z = 0; z < layers; z++, vi++)
        {
            for (int x = 0; x < vertsPerLayer; x++, ti += 6)
            {
                if (x < vertsPerLayer - 1)
                {
                    //how and why does the normal way not work
                    //so 2/3 and 1/4 switch to properly show the triangles
                    triangles[ti] = vi;
                    triangles[ti + 4] = triangles[ti + 1] = vi + 1;
                    triangles[ti + 3] = triangles[ti + 2] = vi + vertsPerLayer;
                    triangles[ti + 5] = vi + vertsPerLayer + 1;
                    vi++;
                    //yield return new WaitForSeconds(1f);
                    me.triangles = triangles;
                }
                else
                {
                    triangles[ti] = vi;
                    triangles[ti + 1] = vi - vertsPerLayer + 1;
                    triangles[ti + 2] = vi + vertsPerLayer;
                    triangles[ti + 3] = vi + vertsPerLayer;
                    triangles[ti + 4] = vi - vertsPerLayer + 1;
                    triangles[ti + 5] = vi + 1;
                    //yield return new WaitForSeconds(1f);
                    me.triangles = triangles;
                }
            }
        }
        me.triangles = triangles;
        me.RecalculateNormals();
    }

}
