using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [Header("Configurations")]
    [SerializeField] private float speed = 5f; // Geschwindigkeit, mit der das Schiff einfliegt
    [SerializeField] private Vector2 waitTime = new Vector2(.6f, 1.5f); // Geschwindigkeit, mit der das Schiff einfliegt

    [Header("Others")]
    [SerializeField] public MonoBehaviour[] componentsToEnable; // Die Komponenten, die aktiviert werden sollen

    [SerializeField] float aimedXPosition;
    const float ENEMY_SPAWN_X_OFFSET = 50f;

    public void InitializeAtPosition(float aimedXPosition)
    {
        this.aimedXPosition = aimedXPosition;
    }

    private void Start()
    {
        if (GetComponent<Enemy>() != null) {
            Vector3 tempVector = RandomPointInsideBounds(GetComponent<Enemy>().Area.bounds);
            transform.position = new Vector3(ENEMY_SPAWN_X_OFFSET, tempVector.y);
            aimedXPosition = tempVector.x;
        }

        // Alle Komponenten deaktivieren
        foreach (var component in componentsToEnable)
            if (component.enabled) Debug.LogError("A Script, that will be enabled from " + this + " Script is enabled at the beginning");

        // Coroutine starten, die das Schiff einfliegen lässt
        StartCoroutine(EnterScene());
    }

    private IEnumerator EnterScene()
    {
        // Wait A Bit 
        yield return new WaitForSeconds(Random.Range(waitTime.x, waitTime.y));

        // Solange das Schiff noch nicht an der Zielposition ist...
        while (transform.position.x < aimedXPosition -0.1f || transform.position.x > aimedXPosition + .1f) // -.1f because the Lerp Command will infinitly lerp towards that position
        {
            // Bewege das Schiff in Richtung der Zielposition
            transform.position = Vector3.Lerp(transform.position, new Vector3(aimedXPosition, transform.position.y, transform.position.z), speed * Time.deltaTime);
            yield return null;
        }

        // Alle Komponenten aktivieren
        foreach (var component in componentsToEnable)
        {
            component.enabled = true;
        }
    }

    public Vector2 RandomPointInsideBounds(Bounds bounds)
    {
        // Determine a random position inside the bounds
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(randomX, randomY);
    }
}
