using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubicCurves : MonoBehaviour
{
    public float stepSize;
    public GameObject mainCamera;
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
                Debug.DrawLine(mainCamera.transform.position, hit.point, Color.green, 10000f);

                //print(Vector3.Angle(hit.normal, ray.direction));
                MakeCurve(ray, hit);
            }
        }
    }
    
    void MakeCurve(Ray ray, RaycastHit hit) 
    {
        float dist = Vector3.Distance(mainCamera.transform.position, hit.point);
        float stepDist = dist / stepSize;
        Vector3 dir = (hit.point - mainCamera.transform.position).normalized;

        for (int  i = 1;  i < stepSize;  i++)
        {
            GameObject pos = new GameObject();
            pos.transform.position = mainCamera.transform.position + dir * (stepDist * i);
            //pos.transform.rotation = Quaternion.FromToRotation(mainCamera.transform.position,hit.point);
            pos.transform.right = dir* - 1;
            //pos.transform.rotation = Quaternion.LookRotation(hit.point, Vector3.up);
            points.Add(pos);
        }

        GameObject endPos = new GameObject();
        endPos.transform.position = hit.point;
        //endPos.transform.rotation = Quaternion.FromToRotation(mainCamera.transform.position, hit.point);
        endPos.transform.right = dir * -1;
        //endPos.transform.rotation = Quaternion.LookRotation(hit.point, Vector3.up);
        points.Add(endPos);

        GameObject spawnedPipe = Instantiate(pipe);
        //spawnedPipe.transform.position = mainCamera.transform.position;
        GeneratePipe gp = spawnedPipe.GetComponent<GeneratePipe>();
        gp.points.AddRange(points);
        points.Clear();
    }
}
