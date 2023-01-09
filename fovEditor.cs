using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Navigation))]
public class fovEditor : Editor
{


    private void OnSceneGUI()
    {
        Navigation fov = (Navigation)target;
        Handles.color = Color.blue;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

        Vector3 viewAngle01 = DirFromAngles(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle02 = DirFromAngles(fov.transform.eulerAngles.y, fov.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.radius);


        if (fov.CanSeePlayer)
        {
            Handles.color = Color.red;
            Handles.DrawLine(fov.transform.position, fov.target.transform.position );
        }
    }

    private Vector3 DirFromAngles(float eulerY, float AngleDegree)
    {
        AngleDegree += eulerY;
        return new Vector3(Mathf.Sin(AngleDegree * Mathf.Deg2Rad), 0, Mathf.Cos(AngleDegree * Mathf.Deg2Rad));
    }
}
