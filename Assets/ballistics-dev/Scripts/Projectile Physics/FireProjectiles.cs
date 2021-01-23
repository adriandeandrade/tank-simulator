using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace enjoiii.ballistics
{
    public class FireProjectiles : MonoBehaviour
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform projectilesParent;

        private void Start()
        {
            StartCoroutine(FireBullet());
        }

        private IEnumerator FireBullet()
        {
            while (true)
            {
                GameObject newBullet = Instantiate(projectilePrefab, transform.position, transform.rotation) as GameObject;
                newBullet.transform.parent = projectilesParent;
                newBullet.GetComponent<Projectile>().currentVelocity = ProjectileBallistics.ProjectileSpeed * transform.forward;

                yield return new WaitForSeconds(2f);
            }
        }
    }

}