using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float speed = 2f;

    private int currentWaypointIndex = 0;

    // Update is called once per frame
    private void Update()
    {
        if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex].transform.position) < 0.1f) {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, speed * Time.deltaTime);
    }
}
