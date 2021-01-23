using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Projectiles/New Projectile Data", fileName = "New Projectile Data")]
public class ProjectileData : ScriptableObject
{
    //Data belonging to this bullet type
    [Tooltip("The initial speed [m/s]")]
    public float muzzleVelocity = 10f;

    [Tooltip("Mass [kg]")]
    public float mass = 0.2f;
    [Tooltip("Radius [m]")]

    public float radius = 0.05f;

    [Tooltip("Coefficients, which is a value you can't calculate - you have to simulate it in a wind tunnel and they also depends on the speed, so we pick some average value. Drag coefficient (Tesla Model S has the drag coefficient 0.24) 1")]
    public float dragCoefficient = 0.5f;

    [Tooltip("Lift coefficient")]
    public float liftCoefficient = 0.5f;


    //External data (shouldn't maybe be here but is easier in this tutorial)
    [Tooltip("Wind speed [m/s] (THIS VALUE WILL BE PULLED FROM A WIND IMPLEMENTATION CLASS SOONA)")]
    public Vector3 windSpeedVector = new Vector3(0f, 0f, 0f);

    [Tooltip("The density of the medium the bullet is travelling in, which in this case is air at 15 degrees [kg/m^3]")]
    public float rho = 1.225f;

    public GameObject projectilePrefab;
}
