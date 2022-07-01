using System;
using UnityEditor;
using UnityEngine;

namespace SOE.Core {
  [Serializable]
  public class BlackBoardData  {

    public string Name = "";
    [SerializeField] private object _obj;
    public object Obj => _obj;

    public BlackBoardData(string name) {
      Name = name;
    }

    public void SetObj(object obj) {
      _obj = null;
      _obj = obj;
    }

#if UNITY_EDITOR
    public void DrawEditor() {
      
      EditorGUILayout.BeginHorizontal();

      EditorGUILayout.LabelField(Name,Obj?.ToString());
      
      EditorGUILayout.EndHorizontal();
    }
#endif
    
  }
}