using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;
        float thickness = 2.0f;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewRadius, thickness);

        Vector3 viewAngleLeft = fov.DirectionFormAngle(-fov.viewAngle / 2, false);
        Vector3 viewAngleRight = fov.DirectionFormAngle(fov.viewAngle / 2, false);

        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleLeft * fov.viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleRight * fov.viewRadius);
        Handles.color = Color.red;

        foreach(var fvisibleTarget in fov.VisibleTarget)
        {
            Handles.DrawLine(fov.transform.position, fvisibleTarget.position, thickness);
        }
    }
}
