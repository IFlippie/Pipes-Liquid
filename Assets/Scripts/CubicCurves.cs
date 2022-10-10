using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubicCurves : MonoBehaviour
{
    public float stepSize;
    public List<Vector3> points = new List<Vector3>();
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
                Debug.DrawLine(transform.position, hit.point, Color.green, Vector3.Distance(transform.position, hit.point));

                //print(Vector3.Angle(hit.normal, ray.direction));
                MakeCurve(ray, hit);
            }
        }
    }
    
    void MakeCurve(Ray ray, RaycastHit hit) 
    {
        float dist = Vector3.Distance(transform.position, hit.point);
        float stepDist = dist / stepSize;
        print(stepDist);
        for (int  i = 0;  i < stepSize;  i++)
        {
            Vector3 pos = transform.position + (hit.point - transform.position).normalized * (stepDist * i);
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = pos;
            print("cube point is: " + cube.transform.position);
            points.Add(pos);         
        }
        //print("end point is: " + hit.point);
    }
}
