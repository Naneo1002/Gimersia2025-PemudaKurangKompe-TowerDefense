using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private GameObject pausePanel;
    private bool _isGamePaused = false;

    private void Start()
    {
        if (livesText == null)
        {
            Debug.LogError("Assign a TMP_Text to UIController.livesText in the inspector.");
            return;
        }

        if (LifeSystem.main != null)
        {
            // initialize display
            UpdateLivesText(LifeSystem.main.GetLives());
            // subscribe to changes
            LifeSystem.main.onLivesChanged += UpdateLivesText;
        }
        else
        {
            Debug.LogWarning("LifeSystem not found in scene. Add LifeSystem to a GameObject.");
        }
    }

    private void OnDestroy()
    {
        if (LifeSystem.main != null)
            LifeSystem.main.onLivesChanged -= UpdateLivesText;
    }

    private void UpdateLivesText(int currentLives)
    {
        if (livesText != null)
            livesText.text = $"Lives: {currentLives}";
    }

    private void ShowPausePanel()
    {
        pausePanel.SetActive(true);
    }

    public void HidePausePanel()
    {
        pausePanel.SetActive(false);
    }

    public void TogglePause() {
        if (_isGamePaused) {
            HidePausePanel();
            _isGamePaused = false;
        } else {
            ShowPausePanel();
            _isGamePaused = true;
        }
    }
} 
