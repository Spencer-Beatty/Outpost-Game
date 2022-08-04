using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovementController))]
public class Unit : MonoBehaviour
{
    const float minPathUpdateTime = 0.2f;
    const float pathUpdateMoveThreshold = .25f;
    public Transform target;
    public float speed = 5f;
    public float turnDst = 5;
    public float turnSpeed = 3f;
    public float stoppingDistance = 10;
    private int waypointMovementPenalty;
    private bool completedPath = false;
    private bool movementFlag = false;

    Path path;
    Grids grid;
    private GameObject pathFindingController;
    private EnemyMovementController movementController;

    private Vector3[] oldWaypoints = { };

    

    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful )
    {
        if (pathSuccessful)
        {
            path = new Path(waypoints, transform.position, turnDst, stoppingDistance);
            foreach (Vector3 waypoint in oldWaypoints)
            {
                grid.NodeFromWorldPoint(waypoint).movementPenalty -= waypointMovementPenalty;
            }
            foreach (Vector3 waypoint in waypoints)
            {
                grid.NodeFromWorldPoint(waypoint).movementPenalty += waypointMovementPenalty;
            }
            oldWaypoints = waypoints;
            StopCoroutine("FollowPath");

            completedPath = false;
            StartCoroutine("FollowPath");
        }
    }

    public bool checkCompletion()
    {
        return completedPath;
    }

    public void setCompletion()
    {
        completedPath = false;
    }

    public void DisablePath()
    {
        StopAllCoroutines();
    }
    public void EnablePath()
    {
        StartCoroutine("UpdatePath");
    }
    void Start()
    {
        target = GameObject.Find("Player").transform;
        movementController = GetComponent<EnemyMovementController>();

       
        pathFindingController = GameObject.Find("PathFindingController");
        grid = pathFindingController.GetComponent<Grids>();
        waypointMovementPenalty = grid.waypointMovementPenalty;
        StartCoroutine("UpdatePath");
    }
    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < .3f && transform.position != target.position) 
        {
            yield return new WaitForSeconds(0.3f);
        }
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;
        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                targetPosOld = target.position;
            }
        }
    }
    IEnumerator FollowPath()
    {
       
        bool followingPath = true;
        int pathIndex = 0;
        float speedPercent = 1;

        if (path.lookPoints.Length < 1)
        {
            followingPath = false;
            
        }
        
        while (followingPath)
        {
            Vector2 pos2d = new Vector2(transform.position.x, transform.position.z);
            while(path.turnBoundaries [pathIndex].HasCrossedLine(pos2d))
            {
                if (pathIndex == path.finishLineIndex)
                {
                    followingPath = false;
                    break;
                }
                else
                {
                    pathIndex++;
                }
            }
            if (followingPath)
            {
                if (pathIndex >= path.slowDownIndex && stoppingDistance > 0)
                {
                    speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2d) / stoppingDistance);
                    /*if(GetComponent<GladiatorController>().distanceTo() <= GetComponent<GladiatorController>().walkingThreshold)
                    {
                        speedPercent = 0f;
                    }*/
                    if (speedPercent < 0.01)
                    {
                        followingPath = false;
                    }
                }                

               

                if (!movementFlag)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(
                   new Vector3(path.lookPoints[pathIndex].x, transform.position.y, path.lookPoints[pathIndex].z) - transform.position);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                    movementController.MoveCharacter(0, 1);
                }
                
               
                







            }
            yield return null;
        }
        
        completedPath = true;
    }

    public void grabMovementFlag()
    {
        movementFlag = true;
    }
    public void releaseMovementFlag()
    {
        movementFlag = false;
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            path.DrawWithGizmos();
        }
    }
}
