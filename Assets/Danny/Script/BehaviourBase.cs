using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourBase : MonoBehaviour
{
#if UNITY_EDITOR
        [ContextMenu("Bind Serialized Field _b")]
        protected void BindSerializedField()
        {
            UnityEditor.Undo.RecordObject(this, "Bind Serialized Field");
            OnBindSerializedField();
            UnityEditor.EditorUtility.SetDirty(this);
        }

        protected virtual void OnBindSerializedField() { }
        public virtual void OnInspectorGUI() { }
        public virtual void OnSceneGUI() { }
#endif // UNITY_EDITOR
}

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(BehaviourBase), true)]
    [UnityEditor.CanEditMultipleObjects]
    [ExecuteInEditMode]
    public class BehaviourBaseEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var targetList = serializedObject.targetObjects;
            if (null == targetList)
                return;

            for (int n = 0, cnt = targetList.Length; n < cnt; ++n)
            {
                var target = targetList[n] as BehaviourBase;
                target.OnInspectorGUI();
            }

            serializedObject.ApplyModifiedProperties();
        }

        void OnSceneGUI() 
        {
            if (null == target)
                return;

            var bb = target as BehaviourBase;
            bb.OnSceneGUI();
        }
    }
#endif // UNITY_EDITOR

