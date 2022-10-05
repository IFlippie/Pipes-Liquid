using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubicCurves : MonoBehaviour
{
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
                Debug.DrawLine(transform.position, hit.point, Color.green, 1000f);
                //points.Add(hit.point);
                print("hit");
            }
        }
    }

    void MakeCurve() 
    {
        int length = points.Count;
        for (int  i = 0;  i <length;  i++)
        {

        }
    }
}
