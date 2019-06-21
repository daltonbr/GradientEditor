using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GradientEditor : EditorWindow
{
    private CustomGradient _gradient;
    private Rect _gradientPreviewRect;
    private const int BorderSize = 10;
    private const int gradientPreviewRectHeight = 20;
    private float _keyWidth = 10; 
    private float _keyHeight = 20;

    private Rect[] _keyRects;
    private bool _mouseIsDownOverKey;
    private int _selectedKeyIndex;
    private bool _needsRepaint;

    private void OnGUI()
    {
        Draw();
        HandleInput();

        if (_needsRepaint)
        {
            _needsRepaint = false;
            Repaint();
        }
    }

    private void Draw()
    {
        _gradientPreviewRect = new Rect(BorderSize, BorderSize, position.width - (BorderSize * 2), gradientPreviewRectHeight);
        GUI.DrawTexture(_gradientPreviewRect, _gradient.GetTexture((int)_gradientPreviewRect.width));
        _keyRects = new Rect[_gradient.NumKeys];

        for (int i = 0; i < _gradient.NumKeys; i++)
        {
            CustomGradient.ColorKey key = _gradient.GetKey(i);
            Rect keyRect = new Rect(_gradientPreviewRect.x + (_gradientPreviewRect.width * key.Time) - (_keyWidth/2f),
                                    _gradientPreviewRect.yMax + BorderSize, _keyWidth, _keyHeight);
            
            if (i == _selectedKeyIndex)
            {
                EditorGUI.DrawRect(new Rect(keyRect.x-3, keyRect.y-3, keyRect.width+6, keyRect.height+6), Color.black);
            }
    
            EditorGUI.DrawRect(keyRect, key.Color);
            _keyRects[i] = keyRect;
        }

        Rect settingsRect = new Rect(BorderSize, _keyRects[0].yMax + BorderSize,
                                     position.width - (BorderSize*2), position.height);
                                
        GUILayout.BeginArea(settingsRect);
        EditorGUI.BeginChangeCheck();
        Color newColor = EditorGUILayout.ColorField(_gradient.GetKey(_selectedKeyIndex).Color);
        if (EditorGUI.EndChangeCheck())
        {
            _gradient.UpdateKeyColor(_selectedKeyIndex, newColor);
        }
        GUILayout.EndArea();
    }

    private void HandleInput()
    {
        Event guiEvent = Event.current;

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
        {
            for (int i = 0; i < _keyRects.Length; i++)
            {
                if (_keyRects[i].Contains(guiEvent.mousePosition))
                {
                    // Hovering i-th keyRect
                    _selectedKeyIndex = i;
                    _mouseIsDownOverKey = true;
                    _needsRepaint = true;
                    break;
                }
            }

            if (!_mouseIsDownOverKey)
            {
                Color randomColor = new Color(Random.value, Random.value, Random.value);
                var keyTime = Mathf.InverseLerp(_gradientPreviewRect.x, _gradientPreviewRect.xMax, guiEvent.mousePosition.x);
                _selectedKeyIndex = _gradient.AddKey(randomColor, keyTime);
                _mouseIsDownOverKey = true;
                //_needsRepaint = true;
            }
        }

        if (guiEvent.type == EventType.MouseUp && guiEvent.button == 0)
        {
            _mouseIsDownOverKey = false;
        }

        if (_mouseIsDownOverKey && guiEvent.type == EventType.MouseDrag && guiEvent.button == 0)
        {
            var keyTime = Mathf.InverseLerp(_gradientPreviewRect.x, _gradientPreviewRect.xMax, guiEvent.mousePosition.x);
            _selectedKeyIndex = _gradient.UpdateKeyTime(_selectedKeyIndex, keyTime);
            _needsRepaint = true;
        }

        if (guiEvent.keyCode == KeyCode.Backspace && guiEvent.type == EventType.KeyDown)
        {
            _gradient.RemoveKey(_selectedKeyIndex);
            if (_selectedKeyIndex >= _gradient.NumKeys)
            {
                _selectedKeyIndex--;
            }
            _needsRepaint = true;
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
