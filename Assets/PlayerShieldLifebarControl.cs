using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShieldLifebarControl : MonoBehaviour
{
    // The physical images and text values on display
    public Image energyShield;
    public Image lifebar;
    public Text energyShieldText;
    public Text lifebarText;

    // Max "health" variables
    public float energyShieldMaxHealth;
    public float lifebarMaxHealth;

    // Current "health" values and "calculate" variables
    private float energyShieldCurrentHealth;
    private float lifebarCurrentHealth;
    private float calculateEnergyShield;
    private float calculateLifebar;

    // Recharge times and delays
    private double energyShieldRechargeSpeed;
    private double energyShieldRechargeDelay;
    private double lifebarRechargeSpeed;
    private double lifebarRechargeDelay;
    private float timeUncharged;
    private bool medkitHealed = false;
    private bool wasDamaged = false;
    private float damageAmount;

    // "Deal Damage" and medkit healing functions
    public float Damage(float damage)
    {
        if(damage < 0)
        {
            wasDamaged = true;
            damageAmount = damage;
            if(energyShieldCurrentHealth > 0)
            {
                energyShieldCurrentHealth = energyShieldCurrentHealth - damage;
            }
            else if(lifebarCurrentHealth > 0)
            {
                lifebarCurrentHealth = lifebarCurrentHealth - damage;
            }

        }
        return damage;
    }
    public int Medkit(int healing)
    {
        if(healing != 0)
        {
            lifebarCurrentHealth = lifebarMaxHealth;
            medkitHealed = true;
        }
        return healing;
    }

    // Shows the enery shield health and lifebar health values
    void updateShieldText()
    {
        energyShieldText.text = "" + (int)energyShieldCurrentHealth;
    }
    void updateLifebarText()
    {
        lifebarText.text = "" + (int)lifebarCurrentHealth;
    }


    //

    // Start is called before the first frame update
    void Start()
    {
        energyShieldCurrentHealth = energyShieldMaxHealth;
        lifebarCurrentHealth = lifebarMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        calculateEnergyShield = energyShieldCurrentHealth / energyShieldMaxHealth;
        calculateLifebar = lifebarCurrentHealth / lifebarMaxHealth;
        
        timeUncharged += Time.deltaTime;

        // If the player presses the x key, and the player takes damage, the timeUncharged counter is delayed
        if(Input.GetKeyDown(KeyCode.X))
        {
            Damage(Random.Range(1, 20));
            if(wasDamaged == true)
            {
                timeUncharged = 0;
            }
        }

        // If the player presses the c key, they will be healed with a medkit that instantly heals all HP and nullifies the energy shield recharge delay
        if(Input.GetKeyDown(KeyCode.C))
        {
            Damage(Random.Range(1, 20));
            if(medkitHealed == true)
            {
                timeUncharged = (float)energyShieldRechargeDelay;
            }
        }

        updateLifebarText();
        updateShieldText();
        
        // Checks if the current shield health is beneath its chosen max health then recharges
        if(energyShieldCurrentHealth < energyShieldMaxHealth)
        {
            if(energyShieldRechargeDelay == timeUncharged)
            {
                timeUncharged = 0;
                // Fills the energy shield independent of the framerate (to my understanding) and dictated by the set recharge speed
                energyShield.fillAmount = Mathf.MoveTowards(energyShield.fillAmount, calculateEnergyShield, Time.deltaTime / (float)energyShieldRechargeSpeed);
                updateShieldText();
            }

        }
        // Recharges the lifebar if the energy shields maxed or overshielded and the regarge delay has passed
        if(energyShieldCurrentHealth >= energyShieldMaxHealth)
        {
            if(lifebarRechargeDelay == timeUncharged)
            {
                timeUncharged = 0;
                // Fills the lifebar independent of the framerate (to my understanding) and dictated by the set recharge speed
                lifebar.fillAmount = Mathf.MoveTowards(lifebar.fillAmount, calculateLifebar, Time.deltaTime * (float)lifebarRechargeSpeed);
                updateLifebarText();
            }
        }
        // Recharges the lifebar only if the energy shield is at max capacity or overshielded and the recharge delay has passed
        if(lifebarCurrentHealth < lifebarMaxHealth && energyShieldCurrentHealth >= energyShieldMaxHealth)
        {
            if(lifebarRechargeDelay == timeUncharged)
            {
                timeUncharged = 0;
                // Fills the lifebar independent of the framerate (to my understanding) and dictated by the set recharge speed
                lifebar.fillAmount = Mathf.MoveTowards(lifebar.fillAmount, calculateLifebar, Time.deltaTime * (float)lifebarRechargeSpeed);
                updateLifebarText();
            }
        }

        // Makes sure these values are false to prevent these player states from carying over across frames
        wasDamaged = false;
        medkitHealed = false;
        
        // Checks to make sure that the HP are never under a minimum of 0, because if it is, the player is dead
        if(lifebarCurrentHealth < 0)
        {
            lifebarCurrentHealth = 0;
            energyShieldCurrentHealth = 0;
        }
    }
}
