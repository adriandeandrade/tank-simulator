
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using ChobiAssets.PTM;
using System;
using System.Linq;

[System.Serializable]
public class ArmorRegionData
{
    public float actualThickness;
}

public class ArmorRegion : MonoBehaviour
{
    [SerializeField] private ArmorRegionData armorData;

    // Debug
    private ImpactData lastImpactData;
    private bool firstImpact = true;

    public void HandleImpact(ImpactData impactData)
    {
        float a = armorData.actualThickness; // Actual Thickness
        float b = impactData.impactAngle; // Impact Angle
        // float nV = 5; // Shell normalization value. 5 Degrees for AP round

        float impactAngleCos = Mathf.Cos(b * Mathf.Deg2Rad);

        Debug.Log($"Impact Angle Cos: {impactAngleCos}");
        float relativeThickness = (int)armorData.actualThickness / impactAngleCos;

        Debug.Log($"Angle Hit: {b}, Thickness: <color=orange>{a}</color>, Relative Thickness: <color=orange>{relativeThickness}</color>");

        if (firstImpact)
        {
            firstImpact = false;
        }

        lastImpactData = impactData;

    }

    private void OnDrawGizmosSelected()
    {
        if (lastImpactData != null)
        {
            Vector3 hitPoint = lastImpactData.collision.GetContact(0).point;

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(hitPoint, 0.1f);

            // Draw the impact direction and surface normal debug rays.
            Debug.DrawRay(hitPoint, lastImpactData.collision.GetContact(0).normal, Color.yellow); // Draw the normal ray
            Debug.DrawRay(lastImpactData.collision.contacts[0].point, lastImpactData.collision.relativeVelocity.normalized * 100f, Color.green);
        }
    }
}
