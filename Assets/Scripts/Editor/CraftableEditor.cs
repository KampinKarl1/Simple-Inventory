using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace SimpleInventory.Crafting
{
    public class CraftableEditor : EditorWindow
    {
        [MenuItem("Crafting/Crafting Tool")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow(typeof(CraftableEditor));
        }
        
        bool init = false;

        bool inspectingAllCraftables = false;
        Craftable inspectedCraftable = null;
        Craftable[] craftables = null;
        [SerializeField] Sprite selectSprite = null;

        private void InspectAllCraftables()
        {
            inspectingAllCraftables = true;
            craftables = Resources.FindObjectsOfTypeAll<Craftable>();
        }

        private void SetInspectedCraftable(Craftable c)
        {
            inspectedCraftable = c;
            inspectingAllCraftables = false;
        }

        void OnGUI()
        {
            if (!init) 
            {
                InitializeSelectedCraftableStyle();
                init = true;
            }

            GUILayout.Space(64);
            if (!inspectingAllCraftables && GUILayout.Button("Find Craftables"))
            {
                InspectAllCraftables();
            }

            if (inspectingAllCraftables && craftables != null && craftables.Length > 0)
            {
                ShowCraftables();
            }

            else if (inspectedCraftable != null)
            {
                ShowCraftable(inspectedCraftable);
            }

            GUILayout.Space(64);
        }


        private void ShowCraftables()
        {
            foreach (Craftable c in craftables)
            {
                GUILayout.BeginHorizontal();

                //----------Select Craftable Button-----------------
                GUILayoutOption[] options = { GUILayout.Width(50f), GUILayout.Height(50f) };
                if (GUILayout.Button(selectSprite.texture, options))
                {
                    SetInspectedCraftable(c);
                }

                //---------Show the Craftable with its name, icon, and tooltip info-----------
                GUIContent content = new GUIContent(c.ItemName, c.Icon.texture, c.Description);
                GUILayout.Label(content);

                GUILayout.EndHorizontal();
            }
        }

        #region Style for 
        int numColumns = 2;
        GUIStyle craftableInfoStyle = new GUIStyle();
        GUIStyle ingredientStyle = new GUIStyle();

        private void InitializeSelectedCraftableStyle() 
        {
            craftableInfoStyle.fontSize = 36;
            craftableInfoStyle.fontStyle = FontStyle.Bold;

            ingredientStyle.fontSize = 24;
            ingredientStyle.fontStyle = FontStyle.Italic;
        }

        Object newIngredient = null;

        private void ShowCraftable(Craftable c) 
        {
            GUILayout.Space(24f);
            GUIContent content = new GUIContent(c.ItemName, c.Icon.texture, c.Description);
            GUILayout.Label(content, craftableInfoStyle);
            GUILayout.Label("The ingredients needed to make " + c.ItemName + ": ", craftableInfoStyle);

            GUILayout.Space(24f);

            GUILayout.BeginHorizontal();

            for (int i = 0; i < c.NumberOfIngredients; ++i)
            {
                Item ingreditent = c.GetIngredientAt(i);

                GUILayout.BeginVertical();

                GUILayout.Label(ingreditent.Icon.texture);
                GUILayout.Label(ingreditent.ItemName + ": " + c.GetNumberNeededAt(i).ToString(), ingredientStyle);

                GUILayout.EndVertical();
            }

            GUILayout.EndHorizontal();

            newIngredient = EditorGUILayout.ObjectField(newIngredient, typeof(Item), true);

            if (newIngredient != null && newIngredient as Item)
            {
                if (GUILayout.Button("Add Ingredient")) 
                {
                    c.AddIngredient(newIngredient as Item, 1);
                }
            }

        }
        #endregion
    }
}