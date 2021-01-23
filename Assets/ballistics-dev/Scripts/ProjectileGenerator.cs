using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ChobiAssets.PTM;

public class ProjectileGenerator : Bullet_Generator_CS
{
    [SerializeField] private List<ProjectileData> projectiles = new List<ProjectileData>();
    [SerializeField] private int defaultProjectileTypeIndex = 0;

    private ProjectileData currentProjectileType;
    private int currentProjectileTypeIndex;

    protected override void Initialize()
    {
        // Set the default projectile
        if (projectiles.Count < 1 || projectiles == null)
        {
            Debug.LogError("No projectiles are assigned to this tank.");
            Debug.Break();
        }
        {
            currentProjectileTypeIndex = -1;

            Switch_Bullet_Type();
        }
    }

    public override void Switch_Bullet_Type()
    {
        if (projectiles.Count < 1 || projectiles == null)
        {
            Debug.LogError("No projectiles are assigned to this tank.");
            Debug.Break();
        }
        else
        {
            currentProjectileTypeIndex += 1;
            if (currentProjectileTypeIndex > 2)
            {
                currentProjectileTypeIndex = 0;
            }

            switch (currentProjectileTypeIndex)
            {
                case 0:
                    currentProjectileType = projectiles[0];

                    break;

                case 1:
                    currentProjectileType = projectiles[1];
                    break;

                case 2:
                    currentProjectileType = projectiles[2];
                    break;
            }

            Current_Bullet_Velocity = currentProjectileType.muzzleVelocity;
        }
    }

    protected override IEnumerator Generate_Bullet()
    {
        // Generate muzzle flash
        if (MuzzleFire_Object)
        {
            Instantiate(MuzzleFire_Object, transform.position, transform.rotation, transform);
        }

        // Generate the projectile
        if (currentProjectileType != null)
        {
            if (currentProjectileType.projectilePrefab == null)
            {
                Debug.LogError($"{currentProjectileType.name} does not have a projectile prefab assigned!");
                yield break;
            }

            GameObject newProjectile = Instantiate(currentProjectileType.projectilePrefab);
            Vector3 startPosition = transform.position;
            Vector3 startDirection = transform.forward;

            newProjectile.GetComponent<MoveBullet>().SetStartValues(startPosition, startDirection);
        }

        yield return null;
    }
}
