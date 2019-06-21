using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GradientEditor : EditorWindow
{
    private CustomGradient _gradient;
    private const int BorderSize = 10;
    private const int gradientPreviewRectHeight = 20;

    private void OnGUI()
    {
        Rect gradientPreviewRect = new Rect(BorderSize, BorderSize, position.width - (BorderSize * 2), gradientPreviewRectHeight);
        GUI.DrawTexture(gradientPreviewRect, _gradient.GetTexture((int)gradientPreviewRect.width));
    }

    public void SetGradient(CustomGradient gradient)
    {
        _gradient = gradient;
    }

    private void OnEnable()
    {
        titleContent.text = "Gradient Editor";
    }
}
