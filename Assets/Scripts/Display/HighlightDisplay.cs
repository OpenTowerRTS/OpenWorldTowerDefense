using UnityEngine;

public class HighlightDisplay : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject highlighter;
    public void SetHighlight(bool active)
    {
        if (highlighter != null)
        {
            highlighter.SetActive(active);
        }

    }
}
