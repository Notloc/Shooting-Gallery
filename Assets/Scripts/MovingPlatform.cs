using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] GameObject platform;

    [SerializeField] Vector3 startingPos;
    [SerializeField] Vector3 endPos;

    [SerializeField] float travelTime = 5f;

    void Update()
    {
        platform.transform.localPosition = Vector3.Lerp(startingPos, endPos, (Mathf.Sin(Mathf.PI * (Time.time / travelTime)) + 1.0f) / 2.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var player = collision.collider.GetComponentInParent<Player>();
        if (player)
            player.transform.SetParent(platform.transform);
    }

    private void OnCollisionExit(Collision collision)
    {
        var player = collision.collider.GetComponentInParent<Player>();
        if (player)
            player.transform.SetParent(null);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        float numberOfDots = 6f;

        Vector3 start, end;
        start = this.transform.position + (this.transform.rotation * startingPos);
        end = this.transform.position + (this.transform.rotation * endPos);

        Gizmos.DrawSphere(start, 0.65f);
        for (float i=1f; i<numberOfDots; i++)
            Gizmos.DrawSphere(Vector3.Lerp(start, end, i/numberOfDots), 0.25f);
        Gizmos.DrawSphere(end, 0.65f);

    }
}
