using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CustomGradient))]
public class GradientDrawer : PropertyDrawer
{
    private const float MarginAfterLabel = 5f;

    /*
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        //return base.GetPropertyHeight(property, label);
        return customHeightInPixels;
    }
    */

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);

        Event guiEvent = Event.current;
        CustomGradient gradient = (CustomGradient)fieldInfo.GetValue(property.serializedObject.targetObject);
        float labelWidth = GUI.skin.label.CalcSize(label).x + MarginAfterLabel;
        Rect textureRect = new Rect(position.x + labelWidth, position.y, position.width - labelWidth, position.height);

        if (guiEvent.type == EventType.Repaint)
          {
            GUI.DrawTexture(textureRect, gradient.GetTexture((int)position.width));
            GUI.Label(position, label.text);

            // Workaround for background glitches (when moving cursor)
            // Doesn't appear to happen on 2019.3.0a4
#if !UNITY_2019_3_OR_NEWER
            GUIStyle gradientStyle = new GUIStyle();
            gradientStyle.normal.background = gradient.GetTexture((int) position.width);
            GUI.Label(textureRect, GUIContent.none, gradientStyle);
#endif
        }
        else
        {
            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
            {
                if (textureRect.Contains(guiEvent.mousePosition))
                {
                    EditorWindow.GetWindow<GradientEditor>();  
                }
            }
        }

    }
}
