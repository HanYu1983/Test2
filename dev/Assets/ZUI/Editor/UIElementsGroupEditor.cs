using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIElementsGroup))]
public class UIElementsGroupEditor : Editor
{
    #region Inherited Properties
    SerializedProperty visible;
    SerializedProperty useSimpleActivation;

    SerializedProperty showingClip;
    SerializedProperty hidingClip;

    SerializedProperty onShow;
    SerializedProperty onHide;
    #endregion

    SerializedProperty prewarm;
    SerializedProperty animatedElements;
    SerializedProperty deactivateWhileInvisible;

    void OnEnable()
    {
        #region Inherited Properties
        visible = serializedObject.FindProperty("Visible");
        useSimpleActivation = serializedObject.FindProperty("UseSimpleActivation");

        showingClip = serializedObject.FindProperty("ShowingClip");
        hidingClip = serializedObject.FindProperty("HidingClip");

        onShow = serializedObject.FindProperty("OnShow");
        onHide = serializedObject.FindProperty("OnHide");
        #endregion

        prewarm = serializedObject.FindProperty("Prewarm");
        animatedElements = serializedObject.FindProperty("AnimatedElements");
        deactivateWhileInvisible = serializedObject.FindProperty("DeactivateWhileInvisible");

    }

    public override void OnInspectorGUI()
    {
        UIElementsGroup myElementsGroup = target as UIElementsGroup;

        #region User Interface
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(visible);
        EditorGUILayout.PropertyField(prewarm);
        EditorGUILayout.PropertyField(deactivateWhileInvisible);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Menu Elements", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(animatedElements, true);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Switching", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(useSimpleActivation);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Sounds", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(showingClip);
        EditorGUILayout.PropertyField(hidingClip);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(onShow);
        EditorGUILayout.PropertyField(onHide);

        EditorGUILayout.Space();
        #endregion

        #region Tools
        EditorGUILayout.LabelField("Tools", EditorStyles.boldLabel);

        #region Update Animated Elements Button
        if (GUILayout.Button("Update Animated Elements", GUILayout.Height(30)))
        {
            Undo.RecordObject(myElementsGroup, "Update Animated Items");
            myElementsGroup.AnimatedElements = GetAnimatedElements(myElementsGroup.transform);
            UIElement freeMenuUE = myElementsGroup.GetComponent<UIElement>();
            if (freeMenuUE)
                myElementsGroup.AnimatedElements.Insert(0, freeMenuUE);
        }
        #endregion

        #region Check Menu Independant Elements
        if (myElementsGroup.AnimatedElements != null)
        {
            for (int i = 0; i < myElementsGroup.AnimatedElements.Count; i++)
            {
                if (myElementsGroup.AnimatedElements[i] == null) continue;

                if (!myElementsGroup.AnimatedElements[i].MenuDependent)
                {
                    if (EditorUtility.DisplayDialog("Error", myElementsGroup.AnimatedElements[i].gameObject.name + " is menu independant but is inside this Element Group's elements list.", "Remove it from the list", "Switch it to menu dependant"))
                    {
                        Undo.RecordObject(myElementsGroup, "Removing from list");
                        myElementsGroup.AnimatedElements.RemoveAt(i);
                        i--;
                        continue;
                    }
                    else
                    {
                        Undo.RecordObject(myElementsGroup, "Switch to menu dependant");
                        myElementsGroup.AnimatedElements[i].MenuDependent = true;
                    }
                }
                if (myElementsGroup.AnimatedElements[i].ControlledBy != myElementsGroup)
                    myElementsGroup.AnimatedElements[i].ControlledBy = myElementsGroup;
            }
        }
        #endregion
        #endregion

        serializedObject.ApplyModifiedProperties();
    }

    List<UIElement> GetAnimatedElements(Transform holder)
    {
        List<UIElement> ue = new List<UIElement>();

        foreach (Transform c in holder)
        {
            UIElement cUE = c.GetComponent<UIElement>();
            if (cUE && cUE.MenuDependent && (cUE.ControlledBy == null || cUE.ControlledBy == (UIElementsGroup)target))
                ue.Add(cUE);

            ue.AddRange(GetAnimatedElements(c));
        }
        return ue;
    }
}
