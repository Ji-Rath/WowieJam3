using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatsUI : MonoBehaviour
{
    public PlayerStats playerStats;
    public TMP_Text healthText;
    public TMP_Text potentialPointsText;
    public TMP_Text totalScoreText;

    Animator ppAnimator;
    int cachedPoints;

    // Start is called before the first frame update
    public void Initialize()
    {
        playerStats.playerDamage += UpdateUI;
        cachedPoints = playerStats.Points;
        ppAnimator = potentialPointsText.gameObject.GetComponent<Animator>();
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateUI()
    {
        healthText.text = "Health: " + playerStats.Health;
        totalScoreText.text = "Score: " + playerStats.Points;

        if (playerStats.airbornePoints == 0)
        {
            ppAnimator.SetTrigger("FinishPoints");
        }
        else
        {
            ppAnimator.ResetTrigger("FinishPoints");
            potentialPointsText.text = "PP: " + Mathf.RoundToInt(playerStats.airbornePoints) + " x " + playerStats.GetMultiplier();
        }
        

        cachedPoints = playerStats.Points;
        ppAnimator.SetFloat("Points", playerStats.airbornePoints);
    }

    void OnDestroy()
    {
        playerStats.playerDamage -= UpdateUI;
    }
}
