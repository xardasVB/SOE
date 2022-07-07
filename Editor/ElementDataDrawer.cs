#if UNITY_EDITOR
using System.Collections.Generic;
using SOE.Elements;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace SOE.Editor {
  public class ElementDataDrawer : OdinValueDrawer<List<ElementData>> {
    protected override void DrawPropertyLayout(GUIContent label) {
      
      EditorGUI.BeginChangeCheck();
      
      ValueEntry.SmartValue.ForEach(e => {
        e.DrawEditor();
      });
      
            
      if (EditorGUI.EndChangeCheck()) {
        ValueEntry.Property.MarkSerializationRootDirty();
      }
      
    }
  }
}
#endif