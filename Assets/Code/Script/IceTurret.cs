using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;
using UnityEngine.UI;

public class IceTurret : MonoBehaviour
{
    [Header("References")]
    //[SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    //[SerializeField] private GameObject bulletPrefab;
    //[SerializeField] private Transform firingPoint;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float bps = 1f; // bullets per second
    // Start is called before the first frame update
    [SerializeField] private float freezeTime = 1f;

    //[Header("Wizard Animation")]
    //[SerializeField] private GameObject wizardObject; // Child with Animator
    //private Animator wizardAnimator;

    [Header("Upgrade UI")]
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;

    private float timeUntilFire;

    // Update is called once per frame
    private void Update()
    {
            timeUntilFire += Time.deltaTime;

            // Ready to shoot trigger attack animation only
            if (timeUntilFire >= 1f / bps)
            {
            FreezeEnemies();
            timeUntilFire = 0f;
            }
    }

    private void FreezeEnemies()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();
                em.UpdateSpeed(0.5f);

                StartCoroutine(ResetEnemySpeed(em));
            }
        }
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime);

        em.ResetSpeed();
    }
}
