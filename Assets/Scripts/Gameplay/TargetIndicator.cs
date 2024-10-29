using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    public Transform target;

    public void UpdateTarget(Transform newTarget)
    {
        target = newTarget;
        if (target != null)
        {
            gameObject.SetActive(true);
            Vector3 dir = target.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
