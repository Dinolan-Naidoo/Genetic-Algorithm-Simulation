using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GeneticPathFinder : MonoBehaviour
{
    public DNA dna;
    public bool hasBeenInitialized = false;
    public bool hasFinished = false;
    bool hasCrashed = false;
    Vector2 target;
    Vector2 nextPoint;
    public float creatureSpeed;
    public float PathMultiplier;
    public float rotationSpeed;
    int pathIndex = 0;
    Quaternion targetRotation;
    LineRenderer lr;
    List<Vector2> traveledPath = new List<Vector2>();
    public LayerMask obstacleLayer;

    //Initialize the agents with their DNA and a target destination
    public void InitCreature(DNA newDNA, Vector2 _target)
    {
        traveledPath.Add(transform.position);
        lr = GetComponent<LineRenderer>();
        dna = newDNA;
        target = _target;
        nextPoint = transform.position;
        traveledPath.Add(nextPoint);
        hasBeenInitialized = true;
    }

    private void Update()
    {
        //Runs if the agents are alive
        if(hasBeenInitialized && !hasFinished)
        {
            //Runs if the agents have found the target
            if (pathIndex == dna.genes.Count || Vector2.Distance(transform.position, target) < 0.5f)
            {
                hasFinished = true;
                
            }
            //Finds the next point of movement 
            if((Vector2)transform.position == nextPoint)
            {
                nextPoint = (Vector2)transform.position + dna.genes[pathIndex] * PathMultiplier;
                traveledPath.Add(nextPoint);
                targetRotation = lookAt2D(nextPoint);
                pathIndex++;
            }
            else
            {
                //Moves the agents towards the next point
                transform.position = Vector2.MoveTowards(transform.position, nextPoint, creatureSpeed * Time.deltaTime);
            }
            if(transform.rotation != targetRotation)
            {
                //Quaternion.Slerp interpolates rotation between the rotations "from" and "to"
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            //Line renderer to see the path taken
            RenderLine();
        }
    }

    //The following function renders a line for each agent's path
    public void RenderLine()
    {
        List<Vector3> linePoints = new List<Vector3>();
        if(traveledPath.Count > 2)
        {
            for (int i = 0; i < traveledPath.Count - 1; i++)
            {
                linePoints.Add(traveledPath[i]);
            }
            linePoints.Add(transform.position);
        }
        else
        {
            linePoints.Add(traveledPath[0]);
            linePoints.Add(transform.position);
        }

        lr.positionCount = linePoints.Count;
        lr.SetPositions(linePoints.ToArray());
    }

   

    //The following handles the fitness of the agents. Agents that get the closest to the target will be passed down to the next gen
    public float fitness
    {
        get
        {
            float Dist = Vector2.Distance(transform.position, target);
            if(Dist ==0)
            {
                Dist = 0.0001f;
            }
            RaycastHit2D[] obstacles = Physics2D.RaycastAll(transform.position, target, obstacleLayer);
            float obstacleMultiplier = 1f - (0.15f * obstacles.Length);
            return (60/Dist) * (hasCrashed ? 0.6f : 1f) * obstacleMultiplier;
        }
    }

    //Handles the movement of the agents
    public Quaternion lookAt2D(Vector2 target, float angleOffset = -90)
    {
        Vector2 fromTo = (target - (Vector2)transform.position).normalized;
        float zRotation = Mathf.Atan2(fromTo.y, fromTo.x) * Mathf.Rad2Deg;

        return Quaternion.Euler(0, 0, zRotation + angleOffset);
    }

    //Handles the collisions of the agents. Target collision = good ; Wall collision = bad 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Wall collision
        if (collision.gameObject.layer ==8)
        {
            hasFinished = true;
            hasCrashed = true;
        }
        
        //Target collision
        if(collision.gameObject.layer == 9)
        {
            hasFinished = true;
        }
    }

}
