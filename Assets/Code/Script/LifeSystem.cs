using System;
using UnityEngine;

/// <summary>
/// Simple singleton life manager. Tracks player lives and exposes LoseLife(int)
/// and an event for UI or other systems can react to changes.
/// Place this on a persistent GameObject in the scene (eg. GameManager).
/// </summary>
public class LifeSystem : MonoBehaviour
{
    public static LifeSystem main;

    [Header("Life Settings")]
    [SerializeField] private int startingLives = 10;

    // current lives (read-only from other classes)
    private int lives;

    // event that broadcasts the current lives after a change
    public event Action<int> onLivesChanged;

    private void Awake()
    {
        if (main == null)
        {
            main = this;
            lives = startingLives;
        }
        else if (main != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Decrease lives by amount (default 1). Triggers onLivesChanged.
    /// </summary>
    public void LoseLife(int amount = 1)
    {
        if (amount <= 0) return;

        lives -= amount;
        if (lives < 0) lives = 0;

        onLivesChanged?.Invoke(lives);

        if (lives == 0)
        {
            // TODO: handle game over (reload, show UI, etc.)
            Debug.Log("Game Over - lives reached zero.");
        }
    }

    public int GetLives()
    {
        return lives;
    }

    /// <summary>
    /// Optional helper to add life.
    /// </summary>
    public void AddLife(int amount = 1)
    {
        if (amount <= 0) return;
        lives += amount;
        onLivesChanged?.Invoke(lives);
    }
}
