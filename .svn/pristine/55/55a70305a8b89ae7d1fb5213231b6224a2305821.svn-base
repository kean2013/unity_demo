using System;
using System.Collections;
using Malee.List;
using System.Linq;
using UnityEngine;
using System.Reflection;
using ILRuntime.CLR.Utils;
using ILRuntime.Reflection;
using Sirenix.OdinInspector;
using ILRuntime.CLR.TypeSystem;
using UnityEngine.Serialization;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
using Object = UnityEngine.Object;
using Extensions = ILRuntime.CLR.Utils.Extensions;

namespace JEngine.Core
{
    [HelpURL("https://xgamedev.uoyou.com/classbind-v0-6.html")]
    public class ClassBind : SerializedMonoBehaviour
    {
        [Searchable] [FormerlySerializedAs("ScriptsToBind")]
        public ClassData[] scriptsToBind;

        /// <summary>
        /// Bind itself, call it after when instantiating a prefab with ClassBind in main solution
        /// 激活ClassBind，在主工程Instantiate带有ClassBind的prefab后调用
        /// </summary>
        public void BindSelf()
        {
            ClassBindMgr.DoBind(this);
        }
        
        /// <summary>
        /// Add class
        /// </summary>
        /// <param name="classData"></param>
        /// <returns></returns>
        public object AddClass(ClassData classData)
        {
            //添加脚本
            string classType = classData.GetClassTypeName;
            if (!InitJEngine.Appdomain.LoadedTypes.TryGetValue(classType, out var type))
            {
                Log.PrintError($"自动绑定{name}出错：{classType}不存在，已跳过");
                return null;
            }

            Type t = type.ReflectionType; //获取实际属性
            classData.ClassType = t;
            Type baseType =
                t.BaseType is ILRuntimeWrapperType wrapperType
                    ? wrapperType.RealType
                    : t.BaseType; //这个地方太坑了 你一旦热更工程代码写的骚 就会导致ILWrapperType这个问题出现 一般人还真不容易发现这个坑
            Type monoType = typeof(MonoBehaviour);

            //JBehaviour需自动赋值一个值
            bool isMono = t.IsSubclassOf(monoType) || (baseType != null && baseType.IsSubclassOf(monoType));
            bool needAdapter = baseType != null &&
                               baseType.GetInterfaces().Contains(typeof(CrossBindingAdaptorType));

            ILTypeInstance instance = isMono
                ? new ILTypeInstance(type as ILType, false)
                : InitJEngine.Appdomain.Instantiate(classType);

            instance.CLRInstance = instance;

            /*
             * 这里是ClassBind的灵魂，我都佩服我自己这么写，所以别乱改这块
             * 非mono的跨域继承用特殊的，就是用JEngine提供的一个mono脚本，来显示字段，里面存ILTypeInstance
             * 总之JEngine牛逼
             * ClassBind只支持挂以下2种热更类型：纯热更类型，继承了Mono的类型（无论是主工程多重继承后跨域还是跨域后热更工程多重继承都可以）
             * 主工程多重继承后再跨域多重继承的应该还不支持
             */
            //主工程多重继承后跨域继承的生成适配器后用这个
            if (needAdapter && isMono && baseType != typeof(MonoBehaviourAdapter.Adaptor))
            {
                Type adapterType = Type.GetType(baseType.FullName ?? string.Empty);
                if (adapterType == null)
                {
                    Log.PrintError($"{t.FullName}, need to generate adapter");
                    return null;
                }

                //直接反射赋值一波了
                var clrInstance = gameObject.AddComponent(adapterType) as MonoBehaviour;

                var clrILInstance = t.GetFields(
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                    .First(f => f.Name == "instance" && f.FieldType == typeof(ILTypeInstance));
                var clrAppDomain = t.GetFields(
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                    .First(f => f.Name == "appdomain" && f.FieldType == typeof(AppDomain));
                if (!(clrInstance is null))
                {
                    clrInstance.enabled = false;
                    clrILInstance.SetValue(clrInstance, instance);
                    clrAppDomain.SetValue(clrInstance, InitJEngine.Appdomain);
                    instance.CLRInstance = clrInstance;
                    classData.ClrInstance = (CrossBindingAdaptorType)clrInstance;
                }
            }
            //直接继承Mono的，热更工程多层继承mono的，非继承mono的，或不需要继承的，用这个
            else
            {
                //挂个适配器到编辑器（直接继承mono，非继承mono，无需继承，都可以用这个）
                var clrInstance = gameObject.AddComponent<MonoBehaviourAdapter.Adaptor>();
                clrInstance.enabled = false;
                clrInstance.ILInstance = instance;
                clrInstance.AppDomain = InitJEngine.Appdomain;
                classData.ClrInstance = clrInstance;


                //是MonoBehaviour继承，需要指定CLRInstance
                if (isMono)
                {
                    instance.CLRInstance = clrInstance;
                }

                //判断类型
                clrInstance.isMonoBehaviour = isMono;

                classData.Added = true;

                //JBehaviour额外处理
                var go = t.GetField("_gameObject", BindingFlags.Public);
                go?.SetValue(clrInstance.ILInstance, gameObject);
            }

            if (isMono)
            {
                var m = type.GetConstructor(Extensions.EmptyParamList);
                if (m != null)
                {
                    InitJEngine.Appdomain.Invoke(m, instance, null);
                }
            }

            return instance;
        }

        /// <summary>
        /// Set value
        /// </summary>
        /// <param name="classData"></param>
        public void SetVal(ClassData classData)
        {
            string classType = classData.GetClassTypeName;
            var t = classData.ClassType;
            var clrInstance = classData.ClrInstance;
            //绑定数据
            classData.BoundData = false;
            var fields = classData.fields.ToArray();
            object obj = null;
            const BindingFlags bindingAttr = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance |
                                             BindingFlags.Static;

            foreach (var field in fields)
            {
                try
                {
                    Type fieldType = null;
                    if (field.fieldType != FieldType.Bool)
                    {
                        fieldType = t.GetField(field.fieldName, bindingAttr).FieldType ??
                                    (t.BaseType.GetField(field.fieldName, bindingAttr).FieldType ??
                                     (t.GetProperty(field.fieldName, bindingAttr).PropertyType ??
                                      t.BaseType.GetProperty(field.fieldName, bindingAttr).PropertyType));
                    }

                    switch (field.fieldType)
                    {
                        case FieldType.NotSupported:
                            continue;
                        case FieldType.Number:
                        {
                            fieldType = ((ILRuntimeWrapperType) fieldType).RealType;
                            obj = fieldType.GetPrimitiveFromString(field.value);
                            classData.BoundData = true;
                            break;
                        }
                        case FieldType.String:
                            fieldType = typeof(string);
                            obj = fieldType.GetPrimitiveFromString(field.value);
                            classData.BoundData = true;
                            break;
                        case FieldType.Bool:
                            fieldType = typeof(bool);
                            obj = fieldType.GetPrimitiveFromString(field.value);
                            classData.BoundData = true;
                            break;
                        case FieldType.GameObject:
                            obj = field.gameObject is GameObject
                                ? field.gameObject
                                : (field.gameObject as MonoBehaviour).gameObject;
                            classData.BoundData = true;
                            break;
                        case FieldType.UnityComponent when field.gameObject is GameObject go:
                        {
                            if (fieldType != null)
                            {
                                obj = fieldType.GetInstanceFromGO(go);
                                classData.BoundData = true;
                            }
                            else
                            {
                                Log.PrintError(
                                    $"自动绑定{name}出错：{classType}.{field.fieldName}赋值出错：{field.fieldName}不存在");
                            }

                            break;
                        }
                        case FieldType.UnityComponent:
                            obj = field.gameObject;
                            classData.BoundData = true;
                            break;
                        case FieldType.HotUpdateResource:
                            obj = AssetMgr.Load(field.path ?? field.value, typeof(Object));
                            classData.BoundData = true;
                            break;
                        case FieldType.PrimitiveTypeList:
                            string[] pLst = field.primitiveTypeList;
                            if (!fieldType.MakeHotArray(true, ref obj, pLst))
                            {
                                Log.PrintError(
                                    $"自动绑定{name}出错：{classType}.{field.fieldName}赋值出错：{field.fieldName}不是List或Array，已跳过");
                                continue;
                            }

                            classData.BoundData = true;
                            break;
                        case FieldType.UnityObjectTypeList:
                            var lst = field.unityObjectTypeList;
                            if (!fieldType.MakeHotArray(false, ref obj, null, lst))
                            {
                                Log.PrintError(
                                    $"自动绑定{name}出错：{classType}.{field.fieldName}赋值出错：{field.fieldName}不是List或Array，已跳过");
                                continue;
                            }

                            classData.BoundData = true;
                            break;
                        case FieldType.Color:
                            obj = field.color;
                            classData.BoundData = true;
                            break;
                        case FieldType.Vector2:
                            obj = field.v2;
                            classData.BoundData = true;
                            break;
                        case FieldType.Vector3:
                            obj = field.v3;
                            classData.BoundData = true;
                            break;
                        case FieldType.Vector4:
                            obj = field.v4;
                            classData.BoundData = true;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                catch (Exception except)
                {
                    Log.PrintError(
                        $"自动绑定{name}出错：{classType}.{field.fieldName}获取值{field.value}出错：{except.Message},{except.StackTrace}，已跳过");
                }

                //如果有数据再绑定
                if (!classData.BoundData) continue;

                void SetField(MemberInfo mi)
                {
                    try
                    {
                        switch (mi)
                        {
                            case null:
                                throw new NullReferenceException();
                            case FieldInfo info:
                                info.SetValue(clrInstance.ILInstance, obj);
                                break;
                            case PropertyInfo inf:
                                inf.SetValue(clrInstance.ILInstance, obj);
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Log.PrintError(
                            $"自动绑定{name}出错：{classType}.{field.fieldName}赋值出错：{e.Message}，已跳过");
                    }
                }

                var fi = t.GetField(field.fieldName, bindingAttr);
                if (fi == null) fi = t.BaseType?.GetField(field.fieldName, bindingAttr);
                if (fi != null)
                {
                    SetField(fi);
                }
                else
                {
                    var pi = t.GetProperty(field.fieldName, bindingAttr);
                    if (pi == null) pi = t.BaseType?.GetProperty(field.fieldName, bindingAttr);
                    SetField(pi);
                }
            }
        }

        /// <summary>
        /// Active
        /// </summary>
        /// <param name="classData"></param>
        public void Active(ClassData classData)
        {
            string classType = classData.GetClassTypeName;
            var t = classData.ClassType;
            var clrInstance = classData.ClrInstance;
            //是否激活
            if (classData.activeAfter)
            {
                if (classData.BoundData == false && classData.fields != null &&
                    classData.fields.Count > 0)
                {
                    Log.PrintError($"自动绑定{name}出错：{classType}没有成功绑定数据，自动激活成功，但可能会抛出空异常！");
                }

                //Mono类型能设置enabled
                if (clrInstance.GetType().IsSubclassOf(typeof(MonoBehaviour)))
                {
                    ((MonoBehaviour)clrInstance).enabled = true;
                }

                //不管是啥类型，直接invoke这个awake方法
                var flags = BindingFlags.Default | BindingFlags.Public
                                                | BindingFlags.Instance | BindingFlags.FlattenHierarchy |
                                                BindingFlags.NonPublic | BindingFlags.Static;
                var awakeMethod = clrInstance.GetType().GetMethod("Awake",flags);
                if (awakeMethod == null)
                {
                    awakeMethod = t.GetMethod("Awake", flags);
                }
                else
                {
                    awakeMethod.Invoke(clrInstance, null);
                    classData.Activated = true;
                }

                if (awakeMethod == null)
                {
                    Log.PrintError($"{t.FullName}不包含Awake方法，无法激活，已跳过");
                }
                else if (!classData.Activated)
                {
                    awakeMethod.Invoke(t, null);
                }

                classData.Activated = true;
            }
            Remove();
        }

        /// <summary>
        /// Remove cb
        /// </summary>
        private void Remove()
        {
            //添加后删除
            Destroy(this);
        }
    }

    [Serializable]
    public class ClassData
    {
        [ValueDropdown("Namespace")] [FoldoutGroup("$className")] [FormerlySerializedAs("Namespace")]
        public string classNamespace = "HotUpdateScripts";

        [ValueDropdown("Classes")] [FoldoutGroup("$className")] [FormerlySerializedAs("Class")]
        public string className = "";

        [FoldoutGroup("$className")] [FormerlySerializedAs("ActiveAfter")]
        public bool activeAfter = true;

        [Searchable]
        [ListDrawerSettings(ShowPaging = true, NumberOfItemsPerPage = 6, Expanded = true)]
        [FoldoutGroup("$className")]
        [PropertySpace(0, 15)]
        [FormerlySerializedAs("Fields")]
        public FieldList fields = new FieldList();

        public bool BoundData { get; set; }
        public bool Added { get; set; }
        public bool Activated { get; set; }
        
        public CrossBindingAdaptorType ClrInstance { get; set; }
        public Type ClassType { get; set; }

        public string GetClassTypeName =>
            $"{classNamespace + (classNamespace == "" ? "" : ".")}{className}";

        public IEnumerable Namespace =>
            Tools.Domain.LoadedTypes.Select(d => d.Value).ToList().FindAll(v => !(v is CLRType))
                .Select(v => v.ReflectionType.Namespace).Distinct().ToList().FindAll(s=>!string.IsNullOrEmpty(s));

        public IEnumerable Classes =>
            Tools.Domain.LoadedTypes.Select(d => d.Value).ToList()
                .FindAll(v => v.FullName.StartsWith($"{classNamespace}."))
                .Select(s => s.FullName.Replace($"{classNamespace}.", ""));
    }

    [Serializable]
    public class ClassField
    {
        [HideLabel]
        [Tooltip("需要赋值的字段的名字")]
        [HorizontalGroup("$fieldName/Split", 0.25f)]
        [BoxGroup("$fieldName/Split/Member", centerLabel: true)]
        public string fieldName;

        [HideLabel]
        [BoxGroup("$fieldName",false)]
        [HorizontalGroup("$fieldName/Split", 0.3f)]
        [BoxGroup("$fieldName/Split/Type", centerLabel: true)]
        public FieldType fieldType;

        [HideLabel]
        [Tooltip("基本数据的字符串值")]
        [BoxGroup("$fieldName/Split/Value", centerLabel: true)]
        [ShowIf("@this.fieldType == FieldType.Number ||" +
                "this.fieldType == FieldType.String || " +
                "this.fieldType == FieldType.Bool")]
        public string value;

        [FilePath]
        [HideLabel]
        [Tooltip("热更资源的路径")]
        [BoxGroup("$fieldName/Split/Value", centerLabel: true)]
        [ShowIf("@this.fieldType == FieldType.HotUpdateResource")]
        public string path;

        [HideLabel]
        [Tooltip("GameObject或UnityComponent所对应的游戏物体")]
        [BoxGroup("$fieldName/Split/Value", centerLabel: true)]
        [ShowIf(
            "@this.fieldType == FieldType.GameObject || " +
            "this.fieldType == FieldType.UnityComponent")]
        public Object gameObject;

        [Tooltip("基础类型数组或列表的值")]
        [BoxGroup("$fieldName/Split/Value", centerLabel: true)]
        [ShowIf("@this.fieldType == FieldType.PrimitiveTypeList")]
        [ListDrawerSettings(ShowPaging = true, NumberOfItemsPerPage = 5)]
        public string[] primitiveTypeList;

        [Tooltip("UnityObject类型数组或列表的值")]
        [BoxGroup("$fieldName/Split/Value", centerLabel: true)]
        [ShowIf("@this.fieldType == FieldType.UnityObjectTypeList")]
        [ListDrawerSettings(ShowPaging = true, NumberOfItemsPerPage = 5)]
        public Object[] unityObjectTypeList;
        
        [HideLabel]
        [Tooltip("颜色字段")]
        [BoxGroup("$fieldName/Split/Value", centerLabel: true)]
        [ShowIf(
            "@this.fieldType == FieldType.Color")]
        public Color color;
        
        [HideLabel]
        [Tooltip("Vector2字段")]
        [BoxGroup("$fieldName/Split/Value", centerLabel: true)]
        [ShowIf(
            "@this.fieldType == FieldType.Vector2")]
        public Vector2 v2;

        [HideLabel]
        [Tooltip("Vector3字段")]
        [BoxGroup("$fieldName/Split/Value", centerLabel: true)]
        [ShowIf(
            "@this.fieldType == FieldType.Vector3")]
        public Vector3 v3;
        
        [HideLabel]
        [Tooltip("Vector4字段")]
        [BoxGroup("$fieldName/Split/Value", centerLabel: true)]
        [ShowIf(
            "@this.fieldType == FieldType.Vector4")]
        public Vector4 v4;
    }

    [Serializable]
    public enum FieldType
    {
        Number = 0,
        String = 1,
        Bool = 2,
        GameObject = 3,
        UnityComponent = 4,
        HotUpdateResource = 5,
        NotSupported = 6,
        PrimitiveTypeList = 7,
        UnityObjectTypeList = 8,
        Color = 9,
        Vector2 = 10,
        Vector3 = 11,
        Vector4 = 12,
    }

    [Serializable]
    public class FieldList : ReorderableArray<ClassField>
    {
    }
    
    /// <summary>
    /// Ignore the following field/property while matching fields in the editor
    /// 在编辑器下进行自动匹配时忽略该字段/属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ClassBindIgnoreAttribute : Attribute
    {
    }
}