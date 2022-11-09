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
    public GameObject extend;
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
            if (Input.GetMouseButtonUp(0) && previewPipe.GetComponent<PreviewPipe>().canBeCreated && previewPipe.GetComponent<PreviewPipe>().canBeLined) 
            {
                //Debug.DrawLine(StartPoint.transform.position, hit.point, Color.green, 10000f);

                //MakePipe(ray, hit);
                MakeCurve(ray, hit);
            }
                
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            if (Physics.Raycast(ray, out RaycastHit hit2))
            {
                if (hit2.transform.tag == "StartPoint")
                {
                    StartPoint = hit2.transform.gameObject;
                    previewPipe.SetActive(true);
                    previewPipe.GetComponent<PreviewPipe>().extender.SetActive(true);
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.R)) 
        {
            StartPoint = null;
            previewPipe.SetActive(false);
            previewPipe.GetComponent<PreviewPipe>().extender.SetActive(false);
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
        GameObject extender = Instantiate(extend);
        float dist = Vector3.Distance(StartPoint.transform.position, hit.point);
        float stepDist = dist / stepSize;
        Vector3 dir = (hit.point - StartPoint.transform.position).normalized;
        Vector3 dir2 = (hit.point - StartPoint.transform.position);
        Quaternion rotation;

        GameObject startPos = new GameObject();
        startPos.transform.position = StartPoint.transform.position + (StartPoint.transform.forward * 0.35f) + (StartPoint.transform.up * 0.75f);
        //startPos.transform.right = -StartPoint.transform.forward;
        startPos.transform.rotation = StartPoint.transform.rotation;
        startPos.transform.rotation *= Quaternion.Euler(0, 90, 0);
        pipePoints.Add(startPos);

        GameObject endPos = new GameObject();
        extender.transform.position = hit.point + (Vector3.up * 1.48f);
        extender.transform.forward = dir;
        endPos.transform.position = extender.transform.position - (extender.transform.forward * 0.35f) + (StartPoint.transform.up * 0.75f);
        rotation = Quaternion.LookRotation(dir2);
        rotation *= Quaternion.Euler(0, 90, 0);
        endPos.transform.rotation = rotation;


        GameObject anchorPos = new GameObject();
        anchorPos.transform.position = StartPoint.transform.position + dir * (dist / 2f);
        anchorPos.transform.position = new Vector3(anchorPos.transform.position.x, endPos.transform.position.y, anchorPos.transform.position.z);

        Vector3 relativeEnd = startPos.transform.InverseTransformPoint(endPos.transform.position);
        Vector3 relativePos = new Vector3(relativeEnd.x, 0, 0);
        Vector3 newWorldPos = startPos.transform.TransformPoint(relativePos);
        //print(newWorldPos);
        anchorPos.transform.position = newWorldPos;

        for (int i = 1; i < stepSize; i++)
        {
            Vector3 p0 = Vector3.Lerp(startPos.transform.position, anchorPos.transform.position, i * (1f / stepSize));
            Vector3 p1 = Vector3.Lerp(anchorPos.transform.position, endPos.transform.position, i * (1f / stepSize));
            Vector3 p2 = Vector3.Lerp(p0, p1, i * (1f / stepSize));
            dir = (hit.point - p2).normalized;
            //var dir3 = (hit.point - p2);
            GameObject pos = new GameObject();
            pos.transform.position = p2;
            rotation = Quaternion.LookRotation(dir2);
            rotation *= Quaternion.Euler(0, 90, 0);
            pos.transform.rotation = rotation;
            pipePoints.Add(pos);
            //GameObject dPos = new GameObject();
            //dPos.transform.position = p2;
            //dPos.transform.forward = dir * -1;
            //dirPoints.Add(dPos);
        }
        pipePoints.Add(endPos);
        //extender.transform.forward = endPos.transform.right;

        GameObject spawnedPipe = Instantiate(pipe);
        GeneratePipe gp = spawnedPipe.GetComponent<GeneratePipe>();
        gp.pipePoints.AddRange(pipePoints);
        gp.dirPoints.AddRange(dirPoints);
        pipePoints.Clear();
        dirPoints.Clear();
        StartPoint.tag = "Untagged";
        StartPoint = null;
        previewPipe.SetActive(false);
        previewPipe.GetComponent<PreviewPipe>().extender.SetActive(false);
    }
}
