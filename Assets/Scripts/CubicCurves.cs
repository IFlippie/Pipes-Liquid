using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubicCurves : MonoBehaviour
{
    public float stepSize;
    public GameObject StartPoint;
    public List<GameObject> points = new List<GameObject>();
    public GameObject pipe;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Set position
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit)) 
            {
                Debug.DrawLine(StartPoint.transform.position, hit.point, Color.green, 10000f);

                MakeCurve(ray, hit);
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.tag == "StartPoint")
                {
                    StartPoint = hit.transform.gameObject;
                }
            }
        }
    }
    
    void MakeCurve(Ray ray, RaycastHit hit) 
    {
        float dist = Vector3.Distance(StartPoint.transform.position, hit.point);
        float stepDist = dist / stepSize;
        Vector3 dir = (hit.point - StartPoint.transform.position).normalized;

        GameObject startPos = new GameObject();
        startPos.transform.position = StartPoint.transform.position + StartPoint.transform.forward;
        startPos.transform.right = dir * -1;
        points.Add(startPos);

        for (int  i = 1;  i < stepSize;  i++)
        {
            GameObject pos = new GameObject();
            pos.transform.position = StartPoint.transform.position + dir * (stepDist * i);
            pos.transform.right = dir* - 1;
            points.Add(pos);
        }

        GameObject endPos = new GameObject();
        endPos.transform.position = hit.point;
        endPos.transform.right = dir * -1;
        points.Add(endPos);

        GameObject spawnedPipe = Instantiate(pipe);
        GeneratePipe gp = spawnedPipe.GetComponent<GeneratePipe>();
        gp.points.AddRange(points);
        points.Clear();
    }
}
