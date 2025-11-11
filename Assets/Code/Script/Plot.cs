using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject towerObj;
    public Turret turret;
    // Keep track of which Tower data was used to build the current tower so we can refund on sell
    private Tower builtTower;
    private Color startColor;

    private void Start()
    {
        startColor = sr.color;
    }

    private void OnMouseEnter()
    {
        if (Time.timeScale == 0) return;
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        if (Time.timeScale == 0) return;
        if (UIManager.main.IsHoveringUI()) return;

        // Left-click behavior: if there's a tower, open upgrade UI
        if (towerObj != null)
        {
            turret.OpenUpgradeUI();
            return;
        }


        Tower towerToBuild = BuildManager.main.GetSelectedTower();

        if (towerToBuild.cost > LevelManager.main.currency)
        {
            Debug.Log("Not enough currency to build that!");
            return;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost);

        towerObj = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
        turret = towerObj.GetComponent<Turret>();
        builtTower = towerToBuild;
    }

    // Right-click selling handled per-plot so only the hovered plot is affected
    private void OnMouseOver()
    {
        if (Time.timeScale == 0) return;
        if (UIManager.main != null && UIManager.main.IsHoveringUI()) return;

        // Use legacy Input for reliability here (works regardless of Input System package state)
        if (Input.GetMouseButtonDown(1))
        {
            if (towerObj != null)
            {
                Debug.Log($"Selling tower on plot {gameObject.name}");
                SellTower();
            }
        }
    }

    private void SellTower()
    {
        if (towerObj == null) return;

        if (builtTower != null)
        {
            int refund = Mathf.RoundToInt(builtTower.cost * 0.5f);
            LevelManager.main.IncreaseCurrency(refund);
        }
        else
        {
            Debug.LogWarning("Selling tower but original Tower data is missing. No refund given.");
        }

        // Destroy tower and clear references
        Destroy(towerObj);
        towerObj = null;
        turret = null;
        builtTower = null;
    }
}
