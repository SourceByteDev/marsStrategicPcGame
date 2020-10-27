using System;
using System.Collections;
using Data;
using Game.Units.Control;
using UnityEngine;

namespace Game.Units.Unit_Types
{
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class Unit : MonoBehaviour
    {
        public UnitGameParameters gameParameters;

        private Shader normalShader;

        private Shader outLineShader;

        private MeshRenderer meshRenderer;

        private bool isSelected;

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;

                meshRenderer.material.shader = value ? outLineShader : normalShader;
            }
        }

        public void InitParameters(UnitData data)
        {
            gameParameters = new UnitGameParameters(data);
        }

        public void InitParameters(UnitGameParameters parameters)
        {
            gameParameters = parameters;
        }

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();

            normalShader = Shader.Find("Spine/Skeleton");

            outLineShader = Shader.Find("Spine/Outline/Skeleton");
        }

        private void OnMouseDown()
        {
            UnitSelector.Instance.OnUnitNeedSelect(this);
        }
    }

    public enum ControlType
    {
        None,
        House,
        Barracks,
        Soldier,
        Worker
    }
}