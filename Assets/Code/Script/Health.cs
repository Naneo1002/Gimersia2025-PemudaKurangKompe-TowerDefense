using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int currencyWorth = 50;

    [Header("UI")]
    [SerializeField] private GameObject healthBarPrefab; // assign prefab Canvas(World Space) here
    [SerializeField] private Vector3 barOffset = new Vector3(0, 2f, 0);
    [SerializeField] private float barLocalScale = 0.1f; // scale yang dipakai untuk localScale of bar

    private GameObject barInstance; // simpan whole prefab instance
    private Slider healthBarSlider;
    private int maxHitPoints;
    private bool isDestroyed = false;

    private void Start()
    {
        maxHitPoints = hitPoints;

        if (healthBarPrefab != null)
        {
            // Instantiate without world position issues, parent ke enemy agar mengikuti movement
            barInstance = Instantiate(healthBarPrefab, transform);
            // Set local position & scale supaya tidak mempengaruhi parent
            barInstance.transform.localPosition = barOffset;
            barInstance.transform.localRotation = Quaternion.identity;
            barInstance.transform.localScale = Vector3.one * barLocalScale;

            healthBarSlider = barInstance.GetComponentInChildren<Slider>();
            if (healthBarSlider != null) healthBarSlider.value = 1f;
        }
    }

    private void Update()
    {
        if (barInstance != null)
        {
            // agar selalu menghadap kamera meskipun jadi child
            barInstance.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
            // jika mau posisi tetap relatif terhadap kepala, gunakan localPosition saja (sudah diset di Start)
        }
    }

    public void TakeDamage(int dmg)
    {
        hitPoints -= dmg;

        if (healthBarSlider != null)
            healthBarSlider.value = (float)hitPoints / maxHitPoints;

        if (hitPoints <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(currencyWorth);
            isDestroyed = true;

            if (barInstance != null)
                Destroy(barInstance);

            Destroy(gameObject);
        }
    }
}
