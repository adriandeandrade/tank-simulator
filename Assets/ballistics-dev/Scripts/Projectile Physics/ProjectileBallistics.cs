using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace enjoiii.ballistics
{
    public class ProjectileBallistics : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Transform gun;

        public static float ProjectileSpeed { get; } = 30f;

        private static float h;

        private LineRenderer lineRenderer;

        private void Awake()
        {
            h = Time.fixedDeltaTime * 1f;
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            RotateGun();
            DrawProjectileTrajectory();
        }

        private void RotateGun()
        {
            float? highAngle = 0f;
            float? lowAngle = 0f;

            CalculateAngleToHitTarget(out highAngle, out lowAngle);

            // Artillery
            float angle = (float)highAngle;

            // Gun
            // float angle = (float)lowAngle;

            // Rotate gun
            gun.localEulerAngles = new Vector3(360f - angle, 0f, 0f);

            // Rotate turret towards target
            transform.LookAt(target);
            transform.eulerAngles = new Vector3(0f, transform.rotation.eulerAngles.y, 0f);
        }

        private void CalculateAngleToHitTarget(out float? theta1, out float? theta2)
        {
            // Initial speed
            float v = ProjectileSpeed;
            Vector3 targetVector = target.position - gun.position;

            // Vertical distance
            float y = targetVector.y;

            // Reset y so we can get the horizontal distance x
            targetVector.y = 0;

            // Horizontal distance
            float x = targetVector.magnitude;

            // Gravity
            float g = 9.81f;

            // Calculate the angles
            float vSqr = v * v;
            float underTheRoot = (vSqr * vSqr) - g * (g * x * x + 2 * y * vSqr);

            // Check if we are within range
            if (underTheRoot >= 0f)
            {
                float rightSide = Mathf.Sqrt(underTheRoot);
                float top1 = vSqr + rightSide;
                float top2 = vSqr - rightSide;

                float bottom = g * x;

                theta1 = Mathf.Atan2(top1, bottom) * Mathf.Rad2Deg;
                theta2 = Mathf.Atan2(top2, bottom) * Mathf.Rad2Deg;
            }
            else
            {
                theta1 = null;
                theta2 = null;
            }
        }

        private float CalculateTimeToHitTarget()
        {
            // Init values
            Vector3 currentVelocity = gun.transform.forward * ProjectileSpeed;
            Vector3 currentPosition = gun.transform.position;

            Vector3 newPosition = Vector3.zero;
            Vector3 newVelocity = Vector3.zero;

            // The total time it will take before we hit the target
            float time = 0f;

            // Limit to 30 seconds to avoid infinite loop if we never reach the target
            for (time = 0f; time < 30f; time += h)
            {
                ProjectileBallistics.IntegrationMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity);

                // If we are moving downwards and are below the target, then we have hit
                if (newPosition.y < currentPosition.y && newPosition.y < target.position.y)
                {
                    // Add 2 times to make sure we end up below the target when we display the trajectory
                    time += h * 2f;
                    break;
                }

                currentPosition = newPosition;
                currentVelocity = newVelocity;
            }

            return time;
        }

        private void DrawProjectileTrajectory()
        {
            // How long did it take to hit the target
            float timeToHitTarget = CalculateTimeToHitTarget();

            // How many segments we will have
            int maxIndex = Mathf.RoundToInt(timeToHitTarget / h);

            lineRenderer.positionCount = maxIndex;

            // Start Values
            Vector3 currentVelocity = gun.transform.forward * ProjectileSpeed;
            Vector3 currentPosition = gun.transform.position;

            Vector3 newPosition = Vector3.zero;
            Vector3 newVelocity = Vector3.zero;

            // Build the trajectory line
            for (int index = 0; index < maxIndex; index++)
            {
                lineRenderer.SetPosition(index, currentPosition);

                // Calculate the new position of the bullet
                ProjectileBallistics.IntegrationMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity);

                currentPosition = newPosition;
                currentVelocity = newVelocity;
            }
        }

        public static void IntegrationMethod(float h, Vector3 currentPos, Vector3 currentVel, out Vector3 newPos, out Vector3 newVel)
        {
            // IntegrationMethods.BackwardEuler(h, currentPos, currentVel, out newPos, out newVel);
            // IntegrationMethods.BackwardEuler(h, currentPos, currentVel, out newPos, out newVel);
            IntegrationMethods.Heuns(h, currentPos, currentVel, out newPos, out newVel);
        }

    }

}