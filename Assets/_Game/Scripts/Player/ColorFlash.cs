using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFlash : MonoBehaviour
{
    [SerializeField]
    private Color color;

    [SerializeField]
    private float timeBetweenFlash = 0.25f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine flashRoutine;

    private float flashDuration;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color; 
    }

    public ColorFlash Flash(float flashDuration)
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        this.flashDuration = flashDuration;
        flashRoutine = StartCoroutine(FlashCoroutine());

        return this;
    }

    public void CancelFlash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        spriteRenderer.color = originalColor;
    }

    private IEnumerator FlashCoroutine()
    {
        int timesToFlash = Mathf.FloorToInt(flashDuration / timeBetweenFlash);

        for (int i = 0; i < timesToFlash; i++)
        {
            spriteRenderer.color = color;

            yield return new WaitForSeconds(timeBetweenFlash / 2);

            spriteRenderer.color = originalColor;

            yield return new WaitForSeconds(timeBetweenFlash / 2);
        }

        flashRoutine = null;
    }
}
