using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GradientEditor : EditorWindow
{
    private CustomGradient _gradient;
    private const int BorderSize = 10;
    private const int gradientPreviewRectHeight = 20;
    private float _keyWidth = 10; 
    private float _keyHeight = 20;

    private void OnGUI()
    {
        Event guiEvent = Event.current;

        Rect gradientPreviewRect = new Rect(BorderSize, BorderSize, position.width - (BorderSize * 2), gradientPreviewRectHeight);
        GUI.DrawTexture(gradientPreviewRect, _gradient.GetTexture((int)gradientPreviewRect.width));

        for (int i = 0; i < _gradient.NumKeys; i++)
        {
            CustomGradient.ColorKey key = _gradient.GetKey(i);
            Rect keyRect = new Rect(gradientPreviewRect.x + (gradientPreviewRect.width * key.Time) - (_keyWidth/2f),
            gradientPreviewRect.yMax + BorderSize, _keyWidth, _keyHeight);
            EditorGUI.DrawRect(keyRect, key.Color);
            Repaint();    
        }

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
        {
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            var keyTime = Mathf.InverseLerp(gradientPreviewRect.x, gradientPreviewRect.xMax, guiEvent.mousePosition.x);
            _gradient.AddKey(randomColor, keyTime); 
        }
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
