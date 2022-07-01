using System;
using Sirenix.OdinInspector;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace SOE.Core {
  public partial class UniqueId : ScriptableObject {
    [ReadOnly] [HorizontalGroup("ID")]
    public string uniqueId = "";

    public string GetUniqueId => uniqueId;
    
#if UNITY_EDITOR
    
    public bool NeedNewId() {
      string[] guids = AssetDatabase.FindAssets("t:"+ nameof(UniqueId),new[] {"Assets/ScriptableData"});

      UniqueId checkId;
      for(int i =0;i<guids.Length;i++){
        string path = AssetDatabase.GUIDToAssetPath(guids[i]);
        checkId = AssetDatabase.LoadAssetAtPath<UniqueId>(path);
        if (checkId.uniqueId == uniqueId && checkId != this) {
          return true;
        }
      }
      return false;
    }

    
    [HorizontalGroup("ID")]
    [Button(ButtonStyle.Box), LabelText("Generate"), LabelWidth(15)]
    public void GenerateID() {
      uniqueId = Guid.NewGuid().ToString();
      EditorUtility.SetDirty(this);
    }
#endif
    public string GetId() {
      return uniqueId;
    }
    
  }
}