using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldSpaceText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text displayText;

    private Rigidbody2D rb;
    private Rigidbody2D rigidbody
    {
        get
        {
            if (rb == null) //lazy initialise
                rb = GetComponent<Rigidbody2D>();
            return rb;
        }
    }

    private Vector3 direction;

    private float duration;
    private float timer = 0;

    public void DisplayText(string text, Color color, Vector2 startPosition, Vector2 direction, float duration)
    {
        this.direction = direction;
        this.duration = duration;

        displayText.text = text;

        transform.position = startPosition;
        gameObject.SetActive(true);

        timer = 0;
        rigidbody.AddForce(direction, ForceMode2D.Impulse);
    }

    public void DisplayText(string text, Color color, Vector3 startPosition, float force, float duration)
    {
        Vector2 direction = new Vector2(Random.Range(-1.0f, 1.0f), 1);
        DisplayText(text, color, startPosition, direction * force, duration);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        float normTiem = timer / duration;

        if (normTiem > 1.0f)
        {
            gameObject.SetActive(false);
        }
    }
}
