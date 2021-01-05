using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextHints : MonoBehaviour
{
    // Text hint variables
    public static string message; // Message content
    static Text textHint; // Onscreen text

    // Text timer
    public static bool textOn = false;
    private static float timer = 0.0f;
    public float textOnTime = 5.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        textHint = GetComponent<UnityEngine.UI.Text>();
        timer = 0.0f;
        textOn = false;
        textHint.text = "";
        textHint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (textOn)
        {
            textHint.enabled = true;
            textHint.text = message;
            timer += Time.deltaTime;
        }
        if(timer >= textOnTime)
        {
            textOn = false;
            textHint.enabled = false;
            timer = 0.0f;
        }
    }
}
