using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class Curves : MonoBehaviour
{
    public Vector3[] points;

    public void Reset()
    {
        points = new Vector3[] {
            new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f)
        };
    }
    public static class Bezier
    {
        public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            return Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);
        }
    }
    public Vector3 GetPoint(float t)
    {
        return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], t));
    }
    // Start is called before the first frame update
    void Start()
    {
        //https://catlikecoding.com/unity/tutorials/curves-and-splines/
        //https://www.youtube.com/watch?v=RF04Fi9OCPc
        //https://docs.unity3d.com/ScriptReference/ExecuteInEditMode.html
        //https://youtu.be/yDFQIyEpJfw
        //https://en.wikibooks.org/wiki/Cg_Programming/Unity/B%C3%A9zier_Curves
        //https://catlikecoding.com/unity/tutorials/swirly-pipe/
        //https://catlikecoding.com/unity/tutorials/advanced-rendering/flat-and-wireframe-shading/
        //https://hextantstudios.com/unity-flat-low-poly-shader/
        //https://blog.nobel-joergensen.com/2010/12/25/procedural-generated-mesh-in-unity/
    }

    // Update is called once per frame
    void Update()
    {

    }

    [CustomEditor(typeof(Curves))]
    public class BezierEditor : Editor
    {

        private Curves curve;
        private Transform handleTransform;
        private Quaternion handleRotation;
        private const int lineSteps = 10;

        public void OnSceneGUI()
        {
            curve = target as Curves;

            handleTransform = curve.transform;
            handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

            Vector3 p0 = ShowPoint(0);
            Vector3 p1 = ShowPoint(1);
            Vector3 p2 = ShowPoint(2);

            Handles.color = Color.white;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p1, p2);

            Handles.color = Color.white;
            Vector3 lineStart = curve.GetPoint(0f);
            for (int i = 1; i <= lineSteps; i++)
            {
                Vector3 lineEnd = curve.GetPoint(i / (float)lineSteps);
                Handles.DrawLine(lineStart, lineEnd);
                lineStart = lineEnd;
            }
        }
            private Vector3 ShowPoint(int index)
            {
                Vector3 point = handleTransform.TransformPoint(curve.points[index]);
                EditorGUI.BeginChangeCheck();
                point = Handles.DoPositionHandle(point, handleRotation);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(curve, "Move Point");
                    EditorUtility.SetDirty(curve);
                    curve.points[index] = handleTransform.InverseTransformPoint(point);
                }
                return point;
            }

        }
    }
