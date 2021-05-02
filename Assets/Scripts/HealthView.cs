using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthView : MonoBehaviour
{
    TextMesh text;
    [SerializeField]
    Health health;
    
    void Start()
    {
        text = GetComponent<TextMesh>();
        health.onDamageTaken.AddListener(Damaged);
    }

    private void Damaged()
    {
        text.text = "Health " + health.HealthRatio.ToString();
    }
}
