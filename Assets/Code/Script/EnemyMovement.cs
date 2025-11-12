using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;

    private float baseSpeed;
    private Coroutine freezeRoutine;

    private void Start()
    {
        baseSpeed = moveSpeed;
        target = LevelManager.main.path[pathIndex];
    }

    private void Update()
    {
        if (target == null) return;

        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex == LevelManager.main.path.Length)
            {
                if (LifeSystem.main != null)
                {
                    LifeSystem.main.LoseLife(1);
                }
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }
            else
            {
                target = LevelManager.main.path[pathIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        if (target == null) return;
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    // Public API turret panggil — lebih aman daripada turret ng-start coroutine sendiri
    public void ApplyFreeze(float duration, float multiplier)
    {
        // stop previous freeze so duration refreshes (prevents race)
        if (freezeRoutine != null) StopCoroutine(freezeRoutine);

        moveSpeed = baseSpeed * multiplier;
        freezeRoutine = StartCoroutine(UnfreezeAfter(duration));
    }

    private IEnumerator UnfreezeAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        moveSpeed = baseSpeed;
        freezeRoutine = null;
    }

    // optional legacy methods — tetap ada kalau dipakai di tempat lain
    public void UpdateSpeed(float newSpeed)
    {
        moveSpeed = newSpeed; 
    }

    public void ResetSpeed()
    {
        moveSpeed = baseSpeed;
    }
}