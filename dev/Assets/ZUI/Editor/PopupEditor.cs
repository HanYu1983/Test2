using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Popup))]
public class PopupEditor : Editor {

    #region Inherited Properties
    SerializedProperty visible;
    SerializedProperty useSimpleActivation;

    SerializedProperty showingClip;
    SerializedProperty hidingClip;

    SerializedProperty onShow;
    SerializedProperty onHide;
    #endregion

    SerializedProperty animatedElements;
    SerializedProperty deactivateWhileInvisible;

    SerializedProperty titleHolder;
    SerializedProperty bodyHolder;

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

        animatedElements = serializedObject.FindProperty("AnimatedElements");
        deactivateWhileInvisible = serializedObject.FindProperty("DeactivateWhileInvisible");

        titleHolder = serializedObject.FindProperty("TitleHolder");
        bodyHolder = serializedObject.FindProperty("BodyHolder");
    }

    public override void OnInspectorGUI()
    {
        Popup myPopup = target as Popup;
        ZUIManager zM = FindObjectOfType<ZUIManager>();

        #region Activate Button
        if (GUILayout.Button("Activate", GUILayout.Height(30)))
        {

            foreach (Popup p in zM.AllPopups)
            {
                if (p == null) continue;

                Undo.RecordObject(p.gameObject, "Activate Pop-up");
                if (p == myPopup)
                {
                    p.gameObject.SetActive(true);
                }
                else
                {
                    p.gameObject.SetActive(false);
                }
            }
        }
        #endregion

        #region User Interface
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Is Visible?", visible.boolValue.ToString());
        EditorGUILayout.PropertyField(deactivateWhileInvisible);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Menu Elements", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(animatedElements, true);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Information", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(titleHolder);
        EditorGUILayout.PropertyField(bodyHolder);

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
            Undo.RecordObject(myPopup, "Update Animated Items");
            myPopup.AnimatedElements = GetAnimatedElements(myPopup.transform);
            UIElement popupUE = myPopup.GetComponent<UIElement>();
            if (popupUE)
                myPopup.AnimatedElements.Insert(0, popupUE);
        }
        #endregion

        if (!zM)
        {
            Debug.LogError("There's no ZUIManager script in the scene, you can have it by using the menu bar ZUI>Creation Window>Setup. Or by creating an empty GameObject and add ZUIManager script to it.");
            return;
        }

        #region Check Menu Independant Elements
        if (myPopup.AnimatedElements != null)
        {
            for (int i = 0; i < myPopup.AnimatedElements.Count; i++)
            {
                if (myPopup.AnimatedElements[i] == null) continue;

                if (!myPopup.AnimatedElements[i].MenuDependent)
                {
                    if (EditorUtility.DisplayDialog("Error", myPopup.AnimatedElements[i].gameObject.name + " is menu independant but is inside this Pop-up's elements list.", "Remove it from the list", "Switch it to menu dependant"))
                    {
                        Undo.RecordObject(myPopup, "Removing from list");
                        myPopup.AnimatedElements.RemoveAt(i);
                        i--;
                        continue;
                    }
                    else
                    {
                        Undo.RecordObject(myPopup, "Switch to menu dependant");
                        myPopup.AnimatedElements[i].MenuDependent = true;
                    }
                }
                if (myPopup.AnimatedElements[i].ControlledBy != myPopup)
                    myPopup.AnimatedElements[i].ControlledBy = myPopup;
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
            if (cUE && cUE.MenuDependent && (cUE.ControlledBy == null || cUE.ControlledBy == (Popup)target))
                ue.Add(cUE);

            ue.AddRange(GetAnimatedElements(c));
        }
        return ue;
    }

}
