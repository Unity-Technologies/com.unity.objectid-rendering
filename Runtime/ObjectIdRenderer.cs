using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.SelectionGroups;

namespace Unity.ObjectIdRendering
{
    [ExecuteAlways]
    public class ObjectIdRenderer : ReplacementShaderRenderer
    {
        public Color backgroundColor = Color.black;
        public Color defaultColor = Color.white;

        protected override void Start()
        {
            base.Start();
            camera.backgroundColor = backgroundColor;
            ConfigureRendererComponents();
        }

        void ConfigureRendererComponents()
        {
            var unselectedRenderers = new HashSet<Renderer>(FindObjectsOfType<Renderer>());
            var selectedRenderers = new HashSet<Renderer>();
            foreach (var selectionGroup in SelectionGroupUtility.GetGroupNames())
            {
                var group = SelectionGroupUtility.GetFirstGroup(selectionGroup);
                foreach (var i in SelectionGroupUtility.GetGameObjects(selectionGroup))
                {
                    foreach (var r in ((GameObject)i).GetComponents<Renderer>())
                    {
                        selectedRenderers.Add(r);
                        AddPropertyBlock(r, group.color);
                    }
                }
            }
            unselectedRenderers.ExceptWith(selectedRenderers);
            foreach (var r in unselectedRenderers)
            {
                AddPropertyBlock(r, defaultColor);
            }
        }

        void AddPropertyBlock(Renderer renderer, Color color)
        {
            var mpb = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(mpb);
            mpb.SetColor("_IdColor", color);
            renderer.SetPropertyBlock(mpb);
        }

    }
}