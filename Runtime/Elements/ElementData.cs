using System;
using System.Collections.Generic;
using System.Linq;
using SOE.Core;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
using UnityEditor;
#endif
using UnityEngine;
using Object = UnityEngine.Object;

namespace SOE.Elements {
  
  [Serializable]
  public class ElementData {
    
    protected internal class IgnoreDraw : Attribute { }
    
    //[IgnoreDraw] public string Type;
    [SerializeField]
    [IgnoreDraw] public SerializableType ValueType;
    [IgnoreDraw] public FieldID Id;

    //public string BlackBoardValue = "";
    [IgnoreDraw] public bool RestrictBlackBoard = false;
    [IgnoreDraw] public bool UseBlackboard = false;
    [IgnoreDraw] public int DropDownValue = 0;
    [IgnoreDraw] public List<string> BlackBoardFields = new();

    public int Int; 
    public bool Bool;
    public float Float;
    public String String; 
    public Vector2 Vector2;
    public Vector3 Vector3;
    public AnimationCurve AnimationCurve;
    public Object Object;
    
    [IgnoreDraw] public int EnumValue;
    
#if UNITY_EDITOR
    
    public void DrawEditor() {
      //if (!String.IsNullOrEmpty(Type)) 
      //  ParseLegacy();

      EditorGUILayout.BeginHorizontal();
      
      bool isBlackboard = ValueType.type?.Name == "ElBValue";

      if (UseBlackboard || isBlackboard) {
        //List<String> nameList = new List<string>();
        //BlackBoardFields.ForEach(e=>nameList.Add(e.Name));
        DropDownValue = SirenixEditorFields.Dropdown(Id.name, DropDownValue, BlackBoardFields?.ToArray());
      }
      else {
        DrawDefaultEditor();
      }

      if (!isBlackboard && !RestrictBlackBoard) {
        UseBlackboard = EditorGUILayout.Toggle(UseBlackboard, GUILayout.MaxWidth(30.0f));
      }
      else {
         UseBlackboard = RestrictBlackBoard ? false : true;
      }

      EditorGUILayout.EndHorizontal();
    }
    
    private void DrawDefaultEditor() {
      if (ValueType.type == null) return;
      
      string name = "";
      if (Id != null)
        name = Id.name;

      if (ValueType.type == typeof(String)) 
        String = EditorGUILayout.TextField(name, String);
      if (ValueType.type == typeof(Int32))
        Int = EditorGUILayout.IntField(name, Int);
      if (ValueType.type == typeof(float)) 
        Float = EditorGUILayout.FloatField(name, Float);
      if (ValueType.type == typeof(Vector3)) 
        Vector3 = EditorGUILayout.Vector3Field(name, Vector3);
      if (ValueType.type == typeof(Vector2)) 
        Vector2 = EditorGUILayout.Vector2Field(name, Vector2);
      if (ValueType.type == typeof(bool)) 
        Bool = EditorGUILayout.Toggle(name, Bool);
      if (ValueType.type == typeof(AnimationCurve)) 
        AnimationCurve = EditorGUILayout.CurveField(name, AnimationCurve);
      if (ValueType.type.IsEnum) {
        var values = new Queue<string>();
        foreach (var v in ValueType.type.GetEnumValues()) values.Enqueue(v.ToString());
        EnumValue = SirenixEditorFields.Dropdown(name, EnumValue, values.ToArray());
      }
      if (typeof(Object).IsAssignableFrom(ValueType.type))
        Object = EditorGUILayout.ObjectField(name, Object, ValueType.type, false);
    }
#endif
    
    public string GetBBoardName() {
      return BlackBoardFields[DropDownValue];
    }
    
    public ElementData(Type type, FieldID id, object value) {
      ValueType = new(type);
      Id = id;
      SetValue(value);
    }

    public void SetValue(object value) {
      var drawFields = GetType().GetFields().Where(method => !Attribute.IsDefined(method, typeof(IgnoreDraw)));
      foreach (var field in drawFields) {
        //if (Type.Name != field.FieldType.Name) continue;
        if (!ValueType.type.IsAssignableFrom(field.FieldType)) continue;
        field.SetValue(this, value);
        break;
      }
    }

    public T GetValue<T>() where T : Object  {
      if (Object is T)
        return (T) Object;
      return default;
    }

    #region Legacy
    //private class Fix : System.Attribute { }
    
    //[IgnoreDraw] [Fix] public ChunkType ChunkType;
    //[IgnoreDraw] [Fix] public BiomeType BiomeType;
    //[IgnoreDraw] [Fix] public RuneData RuneData;
    //[IgnoreDraw] [Fix] public SpellData SpellData;
    //[IgnoreDraw] [Fix] public SpellModifier SpellModifier;
    //[IgnoreDraw] [Fix] public UOData UobjectData;
    //[IgnoreDraw] [Fix] public WindowData WindowData;
    //[IgnoreDraw] [Fix] public EnumTypes.OperationE Operation;
    //[IgnoreDraw] [Fix] public EnumTypes.SignE Sign;
    //[IgnoreDraw] [Fix] public EnumTypes.MathOperationE MathOperation;
    //[IgnoreDraw] [Fix] public PerkData PerkData;
    //[IgnoreDraw] [Fix] public CharacterDataEx.LoyaltyE Loyalty;
    //[IgnoreDraw] [Fix] public CharacterData CharacterData;
    //[IgnoreDraw] [Fix] public DamageData DamageData;
    //[IgnoreDraw] [Fix] public PoolDataBase PoolData;
    //[IgnoreDraw] [Fix] public RuneGroup RuneGroup;
    //[IgnoreDraw] [Fix] public SpellGroup SpellGroup;
    //[IgnoreDraw] [Fix] public ScenarioGroup ScenarioGroup;
    
    //public void ParseLegacy() {
      //if (System.String.IsNullOrEmpty(Type)) return;
      //
      //ValueType = new(System.Type.GetType("System." + Type));
      //if (ValueType.type == null)
      //  ValueType = new(typeof(Vector3).Assembly.GetType("UnityEngine." + Type));
      //if (ValueType.type == null)
      //  ValueType = new(System.Type.GetType("Modules.SOE.Elements." + Type));
      //if (ValueType.type == null) {
      //  
      //  var fixFields = GetType().GetFields().Where(method => System.Attribute.IsDefined(method, typeof(Fix)));
      //  foreach (var field in fixFields) {
      //    if (Type != field.FieldType.Name) continue;
      //    ValueType = new(field.FieldType);
      //    if (field.FieldType.IsEnum) EnumValue = (int)field.GetValue(this);
      //    else Object = (Object) field.GetValue(this);
      //    Debug.LogError(field.FieldType + " ||| " + Type);
      //    break;
      //  }
      //}
      //if (ValueType.type != null) {
      //  Type = null; //Debug.LogError(ValueType.type.Name + " ||| " + Type);
      //}
      //else
      //  Debug.LogError("NULL ||| " + Type);

    //}
    
    //private bool IsDirty = true;
    //public void OnBeforeSerialize() {
    //  if (!IsDirty) return;
    //  foreach (var ef in _action.ActionList.ActionList) {
    //    foreach (var c in ef.ConditionList.ConditionList)
    //      foreach (var el in c.ElementsDataList)
    //        el.ParseLegacy();
    //    foreach (var el in ef.ElementsDataList)
    //      el.ParseLegacy();
    //  }
    //  EditorUtility.SetDirty(this);
    //  IsDirty = false;
    //}
    //public void OnAfterDeserialize() { }
    
    #endregion
    
  }
}
