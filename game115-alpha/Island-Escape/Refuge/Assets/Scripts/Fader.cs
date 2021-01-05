using UnityEngine;

public class Fader : MonoBehaviour
{
    public void OnFadeComplete()
    {
        Debug.Log("The End.");
        Application.Quit();
    }
}
