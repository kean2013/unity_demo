using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix.Tool
{
    using Mgr;

    public static class UITools
    {
        public static GObject FguiGetChild(GObject view, string name)
        {
            if (view == null)
            {
                Debug.LogError($"Error  fgui get child parent ,view == null,gview.asCom = GRoot et child = {name}");
                return null;
            }
            var child = view.asCom.GetChild(name);
            if (child == null)
            {
                var tbl = name.Split("/");
                child = view;
                for (int i = 0; i < tbl.Length; i++)
                {
                    child = child.asCom.GetChild(tbl[i]);
                    if (child == null)
                    {
                        Debug.LogError($"error childName = {tbl[i]}");
                        break;
                    }
                }
            }
            if (child == null)
            {
                Debug.LogError($" no such child {name}");
            }
            return child;
        }

        public static NTexture FguiLoadTexture(string packageName, string textureName)
        {
            UIPackage.AddPackage($"Assets/Resources/UIPackage/{packageName}");
            return UIPackage.GetItemAsset(packageName, textureName) as NTexture;
        }
        public static void FguiSetVisible(GObject view, string childName, bool visible)
        {
            var child = FguiGetChild(view, childName);
            if (child != null) child.visible = visible;
        }
        public static void FguiSetTexture(GObject view, string childName, NTexture texture)
        {
            if (texture == null)
            {
                Debug.LogError("fgui set texture == null");
                return;
            }
            var child = FguiGetChild(view, childName);
            if (child != null)
            {
                child.asImage.texture = texture;
            }
        }
        public static void FguiSetIcon(GObject view, string childName, string texture)
        {
            var child = FguiGetChild(view, childName);
            if (child != null)
            {
                child.icon = texture;
            }
        }
        public static void FguiSetText(GObject view, string childName, string text)
        {
            var child = FguiGetChild(view, childName);
            if (child != null)
            {
                child.asTextField.text = text;
            }
        }
        public static void FguiSetControlerIndex(GObject view, string name, int index)
        {
            var ctrl = view.asCom.GetController(name);
            if (ctrl != null)
            {
                ctrl.selectedIndex = index;
            }
        }
        public static void FguiSetColor(GObject view, string childName, Color color)
        {
            var child = FguiGetChild(view, childName);
            if (child != null)
            {
                child.asGraph.color = color;
            }
        }
        public static void FguiSetBtnData(GObject view, string childName, object data)
        {
            var child = FguiGetChild(view, childName);
            if (child != null)
            {
                child.asButton.data = data;
            }
        }

        public static void FguiAddBtnClick(GObject view, string childName, EventCallback1 func)
        {
            var child = FguiGetChild(view, childName);
            if (child != null)
            {
                child.asButton.onClick.Add(func);
            }
        }

        public static void FguiAddBtnClick(GObject view, string childName, EventCallback0 func)
        {
            var child = FguiGetChild(view, childName);
            if (child != null)
            {
                child.asButton.onClick.Add(func);
            }
        }

        public static void FguiSetFillAmount(GObject view, string childName, float amount)
        {
            var child = FguiGetChild(view, childName);
            if (child != null)
            {
                child.asImage.fillAmount = amount;
            }
        }
        public static void FguiSetEnabled(GObject view, string childName, bool enabled)
        {
            var child = FguiGetChild(view, childName);
            if (child != null)
            {
                child.enabled = enabled;
            }
        }
        public static void FguiAddBtnOnTouchBegin(GObject view, string childName, EventCallback0 func)
        {
            var child = FguiGetChild(view, childName);
            if (child != null)
            {
                child.onTouchBegin.Add(func);
            }
        }

        public static void FguiAddBtnOnTouchEnd(GObject view, string childName, EventCallback0 func)
        {
            var child = FguiGetChild(view, childName);
            if (child != null)
            {
                child.onTouchEnd.Add(func);
            }
        }

        public static void FguiAddBtnOnChanged(GObject view, string childName, EventCallback1 func)
        {
            
               var child = FguiGetChild(view, childName);
            if (child != null)
            {
                child.asButton.onChanged.Add(func);
            }
        }

        public static void FguiAddBtnOnChanged(GObject view, string childName, EventCallback0 func)
        {

            var child = FguiGetChild(view, childName);
            if (child != null)
            {
                child.asButton.onChanged.Add(func);
            }

            UITools.GetLanguage(1);
        }

        public static string GetLanguage(int languageId)
        {
            var cfg = DataMgr.Instance.CfgTables.Language.GetOrDefault(languageId);
            if (cfg == null)
                return "";
            return cfg.Cn;
        }

    }

}