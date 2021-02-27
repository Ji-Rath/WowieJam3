using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : DamageDealer
{
    public float minVelocity = 5;

    public override bool CanDealDamage(GameObject player)
    {
        // Only allow damage over a set y velocity
        return Mathf.Abs(player.GetComponent<PlayerController>().GetUpVelocity()) > minVelocity;
    }

    public override void OnPlayerCollide(GameObject player)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        int velocityPoints = (int)Mathf.Abs(player.GetComponent<PlayerController>().GetUpVelocity());
        playerStats.TakeDamage(new DamageInfo(damageInfo.damage, damageInfo.pointsPerDamage + velocityPoints));
    }
}
