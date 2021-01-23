using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileParent;
    [SerializeField] private Transform barrelEndTransform;
    [SerializeField] private float muzzleVelocity = 1130f;
    [SerializeField] private LineRenderer lineRenderer;

    private float timeStep;

    private void Awake()
    {
        timeStep = Time.fixedDeltaTime * 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Fire();
        }

        DrawTrajectoryPath();
    }

    private void Fire()
    {
        GameObject newProjectile = Instantiate(projectilePrefab);
        newProjectile.transform.parent = projectileParent;
        newProjectile.transform.localScale = Vector3.one;

        Vector3 startPosition = barrelEndTransform.position;
        Vector3 startDirection = barrelEndTransform.transform.forward;

        newProjectile.GetComponent<MoveBullet>().SetStartValues(startPosition, startDirection);
    }

    void DrawTrajectoryPath()
    {
        //Start values
        Vector3 currentVel = barrelEndTransform.forward * muzzleVelocity;
        Vector3 currentPos = barrelEndTransform.position;

        Vector3 newPos = Vector3.zero;
        Vector3 newVel = Vector3.zero;

        List<Vector3> bulletPositions = new List<Vector3>();

        //Build the trajectory line
        bulletPositions.Add(currentPos);

        //I prefer to use a maxIterations instead of a while loop 
        //so we always avoid stuck in infinite loop and have to restart Unity
        //You might have to change this value depending on your values
        int maxIterations = 50000;

        for (int i = 0; i < maxIterations; i++)
        {
            //Calculate the bullets new position and new velocity
            CurrentIntegrationMethod(timeStep, currentPos, currentVel, out newPos, out newVel);

            //Set the new value to the current values
            currentPos = newPos;
            currentVel = newVel;

            //Add the new position to the list with all positions
            bulletPositions.Add(currentPos);

            //The bullet has hit the ground because we assume 0 is ground height
            //This assumes the bullet is fired from a position above 0 or the loop will stop immediately
            if (currentPos.y < 0f)
            {
                break;
            }

            //A warning message that something might be wrong
            if (i == maxIterations - 1)
            {
                Debug.Log("The bullet newer hit anything because we reached max iterations");
            }
        }


        //Display the bullet positions with a line renderer
        lineRenderer.positionCount = bulletPositions.Count;

        lineRenderer.SetPositions(bulletPositions.ToArray());
    }

    public static void CurrentIntegrationMethod(float timeStep, Vector3 currentPos, Vector3 currentVel, out Vector3 newPos, out Vector3 newVel)
    {
        //IntegrationMethods.BackwardEuler(timeStep, currentPos, currentVel, out newPos, out newVel);

        //IntegrationMethods.ForwardEuler(timeStep, currentPos, currentVel, out newPos, out newVel);

        //IntegrationMethods.Heuns(timeStep, currentPos, currentVel, out newPos, out newVel);

        IntegrationMethods.HeunsNoExternalForces(timeStep, currentPos, currentVel, out newPos, out newVel);
    }
}
