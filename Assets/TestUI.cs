using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    [SerializeField] private Vector2 Rect;
    [SerializeField] private Vector2 Transform;
    [SerializeField] private Image View;


    private void Update()
    {
        Rect = View.rectTransform.localPosition;
        Transform = transform.position;
    }
}
