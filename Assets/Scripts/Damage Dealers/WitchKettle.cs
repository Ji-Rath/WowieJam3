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
        Vector3 direction = Quaternion.Euler(transform.rotation.eulerAngles) * Vector3.up;
        GetComponent<Collider>().enabled = false;
        player.transform.position = transform.position;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().AddForce(direction * upForce);
        playerStats.TakeDamage(GetDamageInfo());

        StartCoroutine(EnableKettle());
    }

    public IEnumerator EnableKettle()
    {
        yield return new WaitForSeconds(1);
        GetComponent<Collider>().enabled = true;
    }
}
