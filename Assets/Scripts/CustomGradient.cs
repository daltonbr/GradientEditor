using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomGradient
{
    [SerializeField] private List<ColorKey> _keys = new List<ColorKey>();

    ///<summary>Represents a key in the CustomGradient.</summary>
    [System.Serializable]
    public struct ColorKey
    {
        [SerializeField] private Color _color;
        ///<summary>Normalized value position representing the position of the Key in the gradient.</summary>
        [SerializeField] private float _time; 

        public Color Color => _color;
        public float Time => _time;

        public ColorKey(Color color, float time)
        {
            _color = color;
            _time = time;
        }
    }

    ///<summary>Add a key to CustomGradient, and returns its index</summary>
    public int AddKey(Color color, float time)
    {
        ColorKey newColorKey = new ColorKey(color, time);
        for (int i = 0; i < _keys.Count ; i++)
        {
            if (newColorKey.Time < _keys[i].Time)
            {
                _keys.Insert(i, newColorKey);
                return i;
            }
        }
        _keys.Add(newColorKey);
        return _keys.Count - 1;
    }

    ///<summary>
    /// Remove the key when have at least two elements (keys) in the array. Otherwise don't do anything;
    ///</summary>
    public void RemoveKey(int index)
    {
        if (_keys.Count >= 2)
        {
            _keys.RemoveAt(index);
        }
    }

    ///<summary>
    /// Replace the key with a new one, keeping the same color.
    /// Returns the index of the updated key. This index can be changed.
    ///</summary>
    public int UpdateKeyTime(int index, float time)
    {
        // Removing the old key in this index, and then placing a new one in its place.
        // This way we can keep the array ordered.
        Color oldKeyColor = _keys[index].Color;
        RemoveKey(index);
        return AddKey(oldKeyColor, time);
    }

    public void UpdateKeyColor(int index, Color color)
    {
        _keys[index] = new ColorKey(color, _keys[index].Time);
    }

    ///<summary>The amount of keys in the CustomGradient</summary>
    public int NumKeys => _keys.Count;

    public ColorKey GetKey(int index)
    {
        return _keys[index];
    }

    public Color Evaluate(float time)
    {
        if (_keys.Count == 0)
        {
            return Color.white;
        }

        ColorKey keyLeft = _keys[0];
        ColorKey keyRight = _keys[_keys.Count - 1];

        for (int i = 0; i < _keys.Count - 1; i++)
        {
            if (_keys[i].Time <= time && _keys[i+1].Time >= time)
            {
                keyLeft = _keys[i];
                keyRight = _keys[i+1];
                break;
            }
        }

        float blendTime = Mathf.InverseLerp(keyLeft.Time, keyRight.Time, time);
        return Color.Lerp(keyLeft.Color, keyRight.Color, blendTime);
    }

    public Texture2D GetTexture(int width)
    {
        Texture2D texture = new Texture2D(width, 1);
        Color[] colors = new Color[width];
        for (int i = 0; i < width; i++)
        {
            colors[i] = Evaluate((float)i / (width - 1));
        }
        texture.SetPixels(colors);
        texture.Apply();
        return texture;
    }
}
