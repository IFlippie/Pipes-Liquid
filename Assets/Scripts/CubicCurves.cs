using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubicCurves : MonoBehaviour
{
    public float stepSize;
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
                Debug.DrawLine(transform.position, hit.point, Color.green, 10000f);

                //print(Vector3.Angle(hit.normal, ray.direction));
                MakeCurve(ray, hit);
            }
        }
    }
    
    void MakeCurve(Ray ray, RaycastHit hit) 
    {
        float dist = Vector3.Distance(transform.position, hit.point);
        float stepDist = dist / stepSize;
        Vector3 dir = (hit.point - transform.position).normalized;

        for (int  i = 1;  i < stepSize;  i++)
        {
            GameObject pos = new GameObject();
            pos.transform.position = transform.position + dir * (stepDist * i);
            pos.transform.rotation = Quaternion.FromToRotation(transform.position,hit.point);
            points.Add(pos);
        }

        GameObject spawnedPipe = Instantiate(pipe);
        spawnedPipe.transform.InverseTransformPoint(transform.position);
        GeneratePipe gp = spawnedPipe.GetComponent<GeneratePipe>();
        gp.points.AddRange(points);
        points.Clear();
        //print("CC"+points.Count);
        //print("end point is: " + hit.point);
    }
}
