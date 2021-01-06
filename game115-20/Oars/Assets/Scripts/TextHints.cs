using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextHints : MonoBehaviour
{
    // Text hint logic

    // Message content
    public static string message; // Will be accessed in PlayerCollisions.cs

    // Onscreen text
    static Text textHint;
    public static bool textOn = false;
    private float timer = 0.0f;
    [SerializeField] private float textOnTime = 5.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        textHint = GetComponent<UnityEngine.UI.Text>();
        timer = 0.0f;
        textOn = false;
        textHint.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(textOn)
        {
            textHint.enabled = true;
            textHint.text = message;
            timer += Time.deltaTime;
        }
        if (timer > textOnTime)
        {
            textOn = false;
            textHint.enabled = false;
            timer = 0.0f;
        }
    }
}
