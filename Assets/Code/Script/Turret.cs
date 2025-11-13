using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Wizard Animation")]
    [SerializeField] private GameObject wizardObject; // Child with Animator
    private Animator wizardAnimator;

    [Header("Upgrade UI")]
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float bps = 1f; // bullets per second
    [SerializeField] private int baseUpgradeCost = 100;

    private float bpsBase;
    private float rangeBase;

    private Transform target;
    private float timeUntilFire;
    private int level = 1;

    private void Start()
    {
        // Save base stats for upgrade scaling
        bpsBase = bps;
        rangeBase = targetingRange;

        // Setup Upgrade button event
        if (upgradeButton != null)
            upgradeButton.onClick.AddListener(Upgrade);

        // Auto-detect animator if not assigned
        if (wizardObject == null)
        {
            wizardAnimator = GetComponentInChildren<Animator>();
            if (wizardAnimator != null)
            {
                wizardObject = wizardAnimator.gameObject;
            }
            else
            {
                Debug.LogError("No child object with Animator found! Assign wizardObject in Inspector.");
            }
        }
        else
        {
            wizardAnimator = wizardObject.GetComponent<Animator>();
            if (wizardAnimator == null)
                Debug.LogError("Assigned wizardObject has no Animator!");
        }
    }

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            if (wizardAnimator != null)
                wizardAnimator.SetBool("isAttacking", false);
            return;
        }

        //RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
            if (wizardAnimator != null)
                wizardAnimator.SetBool("isAttacking", false);
        }
        else
        {
            timeUntilFire += Time.deltaTime;

            // Ready to shoot → trigger attack animation only
            if (timeUntilFire >= 1f / bps)
            {
                if (wizardAnimator != null)
                    wizardAnimator.SetBool("isAttacking", true);

                timeUntilFire = 0f;
            }
        }
    }

    // Called by Animation Event
    public void Shoot()
    {
        if (target == null || !CheckTargetIsInRange()) return;

        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        
        // Scale bullet damage by turret level (50% increase per level)
        int scaledDamage = Mathf.RoundToInt(1 * Mathf.Pow(1.5f, level - 1));
        bulletScript.SetDamage(scaledDamage);
        
        bulletScript.SetTarget(target);
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);

        if (hits.Length > 0)
            target = hits[0].transform;
    }

    private bool CheckTargetIsInRange()
    {
        if (target == null) return false;
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }

    private void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

    public void Upgrade()
    {
        if (CalculateCost() > LevelManager.main.currency) return;

        LevelManager.main.SpendCurrency(CalculateCost());

        level++;
        bps = CalculateBPS();
        targetingRange = CalculateRange();

        CloseUpgradeUI();

        Debug.Log($"Upgraded to Level {level}. Damage: {bps}, Range: {targetingRange}, Next Cost: {CalculateCost()}");
    }

    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    private float CalculateBPS()
    {
        return bpsBase * Mathf.Pow(level, 0.6f);
    }

    private float CalculateRange()
    {
        return rangeBase * Mathf.Pow(level, 0.4f);
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Handles.color = Color.cyan;
    //    Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    //}

    // Public accessor so other components (e.g. range indicator) can read the turret's current range
    public float Range { get { return targetingRange; } }
}
