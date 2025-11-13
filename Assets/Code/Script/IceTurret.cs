using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class IceTurret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float bps = 0.25f; // bullets per second
    [SerializeField] private float freezeTime = 1f;
    [SerializeField] private float freezeMultiplier = 0.5f; // 0.5 => 50% speed

    [Header("Upgrade UI")]
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private int baseUpgradeCost = 100;

    private float bpsBase;
    private float rangeBase;

    private Transform target;
    private float timeUntilFire;
    private int level = 1;

    private void Update()
    {
        timeUntilFire += Time.deltaTime;

        if (timeUntilFire >= 1f / bps)
        {
            FreezeEnemies();
            timeUntilFire = 0f;
        }
    }

    private void FreezeEnemies()
    {
        // Use OverlapCircleAll to get all colliders inside radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);

        if (hits.Length == 0) return;

        for (int i = 0; i < hits.Length; i++)
        {
            Collider2D col = hits[i];
            if (col == null) continue;

            // Try get EnemyMovement on collider or parent (in case collider on child)
            EnemyMovement em = col.GetComponent<EnemyMovement>();
            if (em == null) em = col.GetComponentInParent<EnemyMovement>();
            if (em == null) continue;

            em.ApplyFreeze(freezeTime, freezeMultiplier);
        }
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

    public float Range { get { return targetingRange; } }
}