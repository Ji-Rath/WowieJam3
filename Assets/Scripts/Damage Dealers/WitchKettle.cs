using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchKettle : DamageDealer
{
    public float upForce = 1000f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool CanDealDamage(GameObject player)
    {
        return true;
    }

    public override void OnPlayerCollide(GameObject player)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        playerStats.isPoisoned = true;
        player.GetComponent<Rigidbody>().AddForce(0f, upForce, 0f);
        playerStats.TakeDamage(GetDamageInfo());
    }
}
