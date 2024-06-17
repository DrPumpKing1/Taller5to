using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public float scrollSpeed = 20f; 
    public RectTransform creditsTransform; 

    private void Update()
    {
        // Mueve el objeto de créditos hacia arriba
        creditsTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
       
        
    }
}
