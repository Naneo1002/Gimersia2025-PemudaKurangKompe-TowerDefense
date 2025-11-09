using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple UI helper that displays current lives. Attach to a UI Text element and assign the Text.
/// It subscribes to LifeSystem.onLivesChanged to update automatically.
/// </summary>
public class LifeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text livesText;

    private void Start()
    {
        if (livesText == null)
        {
            Debug.LogError("Assign a UI Text component to LifeUI in the inspector.");
            return;
        }

        if (LifeSystem.main != null)
        {
            // initialize
            livesText.text = LifeSystem.main.GetLives().ToString();
            LifeSystem.main.onLivesChanged += OnLivesChanged;
        }
        else
        {
            Debug.LogWarning("LifeSystem not found in scene. Add LifeSystem component to a GameObject.");
        }
    }

    private void OnDestroy()
    {
        if (LifeSystem.main != null)
            LifeSystem.main.onLivesChanged -= OnLivesChanged;
    }

    private void OnLivesChanged(int newLives)
    {
        if (livesText != null)
            livesText.text = newLives.ToString();
    }
}
