using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewPipe : MonoBehaviour
{
    Mesh me;
    MeshFilter mf;
    Vector3[] pipeVertices;
    int[] triangles;
    int layers;
    [HideInInspector]
    public List<GameObject> pipePoints = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> dirPoints = new List<GameObject>();
    public LayerMask ignore;

    [Header("Pipe")]
    public int verticesPerPoint;
    //Distance between the vertices in each layer
    public float pipeRadius;
    public CubicCurves cubicCurves;
    public GameObject extender;
    public bool canBeCreated;
    GameObject previewStartPoint;
    float stepSize;
    Renderer rend;
    Color green;
    Color red;
    //public GameObject endPoint;
    // Start is called before the first frame update
    void Start()
    {
        mf = GetComponent<MeshFilter>();
        me = new Mesh()
        {
            name = "Pipe Preview"
        };

        mf.mesh = me;
        me.Clear();
        stepSize = cubicCurves.stepSize;
        rend = GetComponent<Renderer>();
        green = new Color(0.3272619f, 1f, 0.03301889f, 0.6235294f);
        red = new Color(1f, 0f, 0f, 0.6235294f);
        //layers = pipePoints.Count;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);      
        previewStartPoint = cubicCurves.StartPoint;
        if (Physics.Raycast(ray, out RaycastHit hit) && previewStartPoint != null)
        {
            Vector3 dir = (hit.point - previewStartPoint.transform.position).normalized;
            if (Physics.Raycast(previewStartPoint.transform.position, dir, out RaycastHit upHit, 100f)) 
            {
                //Debug.DrawRay(previewStartPoint.transform.position, dir*1000f, Color.green, 1000f);
                if (upHit.point != hit.point)
                {
                    rend.material.SetColor("_Color", red);
                }
                else { rend.material.SetColor("_Color", green); }
            }
            MakeCurve(ray, hit);
        }
    }

    public void SmoothPipeSpawnPoints()
    {
        me.Clear();
        layers = pipePoints.Count;
        for (int i = 0; i < dirPoints.Count; i++)
        {
            //Debug.DrawRay(dirPoints[i].transform.position, -dirPoints[i].transform.up * 4f, Color.red, 10000f);

            //if (Physics.Raycast(dirPoints[i].transform.position, -dirPoints[i].transform.up * 4f, out RaycastHit downHit)) { pipePoints[i + 1].transform.position += dirPoints[i].transform.up; }
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

    void MakeCurve(Ray ray, RaycastHit hit)
    {
        float dist = Vector3.Distance(previewStartPoint.transform.position, hit.point);
        float stepDist = dist / stepSize;
        Vector3 dir = (hit.point - previewStartPoint.transform.position).normalized;
        Vector3 dir2 = (hit.point - previewStartPoint.transform.position);
        Quaternion rotation;

        GameObject startPos = new GameObject();
        startPos.transform.position = previewStartPoint.transform.position + (previewStartPoint.transform.forward *0.35f) + (previewStartPoint.transform.up * 0.75f);
        startPos.transform.right = -previewStartPoint.transform.forward;
        pipePoints.Add(startPos);

        GameObject endPos = new GameObject();
        extender.transform.position = hit.point + (Vector3.up * 1.48f);
        extender.transform.forward = dir;
        endPos.transform.position = extender.transform.position - (extender.transform.forward * 0.35f) + (previewStartPoint.transform.up * 0.75f);
        rotation = Quaternion.LookRotation(dir2);
        rotation *= Quaternion.Euler(0, 90, 0);
        endPos.transform.rotation = rotation;


        GameObject anchorPos = new GameObject();
        anchorPos.transform.position = previewStartPoint.transform.position + dir * (dist / 2f);
        anchorPos.transform.position = new Vector3(anchorPos.transform.position.x, endPos.transform.position.y, anchorPos.transform.position.z);
        Vector3 relative = startPos.transform.InverseTransformPoint(endPos.transform.position);
        float angle = (Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg) + 90f;
        //print("Angle: " + angle);

        if (angle < 0f || angle > 90f)
        {
            canBeCreated = false;
        }
        Vector3 relativeEnd = startPos.transform.InverseTransformPoint(endPos.transform.position);
        Vector3 relativePos = new Vector3(relativeEnd.x, 0, 0);
        Vector3 newWorldPos = startPos.transform.TransformPoint(relativePos);
        print(newWorldPos);
        anchorPos.transform.position = newWorldPos;

        for (int i = 1; i < stepSize; i++)
        {
            Vector3 p0 = Vector3.Lerp(startPos.transform.position, anchorPos.transform.position, i * (1f / stepSize));
            Vector3 p1 = Vector3.Lerp(anchorPos.transform.position, endPos.transform.position, i * (1f / stepSize));
            Vector3 p2 = Vector3.Lerp(p0, p1, i * (1f / stepSize));
            dir = (hit.point - p2).normalized;
            GameObject pos = new GameObject();
            pos.transform.position = p2;
            rotation = Quaternion.LookRotation(dir2);
            rotation *= Quaternion.Euler(0, 90, 0);
            pos.transform.rotation = rotation;
            pipePoints.Add(pos);
            GameObject dPos = new GameObject();
            dPos.transform.position = p2;
            dPos.transform.forward = dir * -1;
            dirPoints.Add(dPos);
        }
        pipePoints.Add(endPos);
        SmoothPipeSpawnPoints();
        for (int p = 0; p < pipePoints.Count; p++)
        {
            Destroy(pipePoints[p]);
        }
        for (int o = 0; o < dirPoints.Count; o++)
        {
            Destroy(dirPoints[o]);
        }
        Destroy(anchorPos);
        pipePoints.Clear();
        dirPoints.Clear();      
    }

}
