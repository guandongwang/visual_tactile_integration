using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextControl : MonoBehaviour
{
    TextMeshPro myTextMeshPro;
    // Start is called before the first frame update
    void Start()
    {
        myTextMeshPro = GetComponent<TextMeshPro>();
        myTextMeshPro.fontSize = 24;
        myTextMeshPro.color = Color.black;
        myTextMeshPro.text = "Hello, World!";
    }
}
