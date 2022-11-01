using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubicCurves : MonoBehaviour
{
    public float stepSize;
    public GameObject StartPoint;
    public List<GameObject> pipePoints = new List<GameObject>();
    public List<GameObject> dirPoints = new List<GameObject>();
    public GameObject pipe;
    public GameObject previewPipe;
    // Start is called before the first frame update
    void Start()
    {
        PreviewPipe prevPipe = previewPipe.GetComponent<PreviewPipe>();
    }

    // Update is called once per frame
    void Update()
    {
        //Set position      
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit) && StartPoint != null) 
        {
            if (Input.GetMouseButtonUp(0)) 
            {
                Debug.DrawLine(StartPoint.transform.position, hit.point, Color.green, 10000f);

                //MakePipe(ray, hit);
                MakeCurve(ray, hit);
            }
                
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            //Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit2))
            {
                if (hit2.transform.tag == "StartPoint")
                {
                    StartPoint = hit2.transform.gameObject;
                    previewPipe.SetActive(true);
                }
            }
        }
    }
    
    void MakePipe(Ray ray, RaycastHit hit) 
    {
        float dist = Vector3.Distance(StartPoint.transform.position, hit.point);
        float stepDist = dist / stepSize;
        Vector3 dir = (hit.point - StartPoint.transform.position).normalized;

        GameObject startPos = new GameObject();
        startPos.transform.position = StartPoint.transform.position + StartPoint.transform.forward;
        startPos.transform.right = dir * -1;
        pipePoints.Add(startPos);

        for (int  i = 1;  i < stepSize;  i++)
        {
            GameObject pos = new GameObject();
            pos.transform.position = StartPoint.transform.position + dir * (stepDist * i);
            pos.transform.right = dir* - 1;
            pipePoints.Add(pos);
            GameObject dPos = new GameObject();
            dPos.transform.position = StartPoint.transform.position + dir * (stepDist * i);
            dPos.transform.forward = dir * -1;
            dirPoints.Add(dPos);
        }

        GameObject endPos = new GameObject();
        endPos.transform.position = hit.point;
        endPos.transform.right = dir * -1;
        pipePoints.Add(endPos);

        GameObject spawnedPipe = Instantiate(pipe);
        GeneratePipe gp = spawnedPipe.GetComponent<GeneratePipe>();
        gp.pipePoints.AddRange(pipePoints);
        gp.dirPoints.AddRange(dirPoints);
        pipePoints.Clear();
        dirPoints.Clear();
        StartPoint = null;
    }

    void MakeCurve(Ray ray, RaycastHit hit)
    {
        float dist = Vector3.Distance(StartPoint.transform.position, hit.point);
        float stepDist = dist / stepSize;
        Vector3 dir = (hit.point - StartPoint.transform.position).normalized;

        GameObject startPos = new GameObject();
        startPos.transform.position = StartPoint.transform.position;
        //startPos.transform.position = StartPoint.transform.position + dir;
        startPos.transform.right = dir * -1;
        pipePoints.Add(startPos);
        //GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //cube1.transform.position = startPos.transform.position;
        //cube1.transform.localScale = cube1.transform.localScale * 0.1f;
        //print("startPos : " + startPos.transform.position);

        GameObject endPos = new GameObject();
        endPos.transform.position = hit.point;
        endPos.transform.right = dir * -1;
        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //cube.transform.position = endPos.transform.position;
        //cube.transform.localScale = cube.transform.localScale * 0.1f;
        //print("endPos : " + endPos.transform.position);

        GameObject anchorPos = new GameObject();
        anchorPos.transform.position = StartPoint.transform.position + dir * (dist / 2f);
        anchorPos.transform.position = new Vector3(anchorPos.transform.position.x, endPos.transform.position.y, anchorPos.transform.position.z);
        //GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //cube2.transform.position = anchorPos.transform.position;
        //print("anchorPos : " + anchorPos.transform.position);

        for (int i = 1; i < stepSize; i++)
        {
            Vector3 p0 = Vector3.Lerp(startPos.transform.position, anchorPos.transform.position, i*(1f/stepSize));
            //print("p0 " + p0);
            Vector3 p1 = Vector3.Lerp(anchorPos.transform.position, endPos.transform.position, i * (1f / stepSize));
            //print("p1 " + p1);
            Vector3 p2 = Vector3.Lerp(p0, p1, i * (1f / stepSize));
            //print("p2 " + p2);
            //GameObject cube3 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //cube3.transform.position = p2;
            //cube3.transform.localScale = cube3.transform.localScale * 0.5f;
            dir = (hit.point - p2).normalized;
            GameObject pos = new GameObject();
            pos.transform.position = p2;
            pos.transform.right = dir * -1;
            pipePoints.Add(pos);
            GameObject dPos = new GameObject();
            dPos.transform.position = p2;
            dPos.transform.forward = dir * -1;
            dirPoints.Add(dPos);
        }
        pipePoints.Add(endPos);

        GameObject spawnedPipe = Instantiate(pipe);
        GeneratePipe gp = spawnedPipe.GetComponent<GeneratePipe>();
        gp.pipePoints.AddRange(pipePoints);
        gp.dirPoints.AddRange(dirPoints);
        pipePoints.Clear();
        dirPoints.Clear();
        StartPoint = null;
        previewPipe.SetActive(false);
        //gp.SmoothPipeSpawnPoints();
    }
}
