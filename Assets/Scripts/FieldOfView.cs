using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)] public float viewAngle;

    public LayerMask _targetMask;
    public LayerMask _obstracleMask;

    public List<Transform> VisibleTarget;
    public float MeshResolution;

    private void Start()
    {
        StartCoroutine(nameof(FindTargetWithDelay), 0.2);
    }

    IEnumerator FindTargetWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            GetVisibleTarget();
        }
    }

    private void Update()
    {
        DrawFieldOfView();
    }

    private void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * MeshResolution);
        float stepAngle = viewAngle / stepCount;
        for(int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngle * i;
            Debug.DrawLine(transform.position, transform.position + DirectionFormAngle(angle, true) * viewRadius, Color.red);
        }
    }


    public void GetVisibleTarget()
    {
        VisibleTarget.Clear();
        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, _targetMask);
        for (int i = 0; i < targetInViewRadius.Length; i++)
        {
            Transform target = targetInViewRadius[i].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            
            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstracleMask))
                {
                    VisibleTarget.Add(target);
                }
            }
        }
    }

    public Vector3 DirectionFormAngle(float angleInDegrees, bool isGlobalAngles)
    {
        if (!isGlobalAngles)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3 (Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
