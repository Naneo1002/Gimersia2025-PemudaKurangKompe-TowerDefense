using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAnim : MonoBehaviour
{
    private Turret turretScript;

    private void Start()
    {
        // Get the Turret script from the parent object
        turretScript = GetComponentInParent<Turret>();
        if (turretScript == null)
        {
            Debug.LogError("Turret script not found in parent! Make sure this GameObject is a child of the Turret.");
        }
    }

    // This function will be called by the Animation Event
    public void ShootEvent()
    {
        if (turretScript != null)
        {
            turretScript.Shoot();
        }
    }
}
