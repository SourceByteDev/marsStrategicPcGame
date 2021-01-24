using System;
using System.Collections;
using Data;
using Game.Units.Control;
using Manager;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Units.Unit_Types
{
    [RequireComponent(typeof(SkeletonAnimation))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class Unit : MonoBehaviour
    {
        public UnitGameParameters gameParameters;

        private Shader _normalShader;

        private Shader _outLineShader;

        private bool _isSelected;

        private ValuesManage.LiveUnitData _myLiveUnitData;
        
        public MeshRenderer MeshRenderer { get; private set; }

        public SkeletonAnimation SkeletonAnimation { get; private set; }
        
        public BoxCollider2D Collider { get; private set; }
        
        public UnityAction OnDead { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;

                MeshRenderer.material.shader = value ? _outLineShader : _normalShader;
            }
        }

        public virtual void OnSpawnedSome(BuildUnitParameters unit)
        {
            print("From " + name + " spawned " + unit.toBuildUnit.parameters.unitName);
        }

        public virtual void OnUpdateIndexLevel(int index, bool isOnNew)
        {
            print($"Update {name} for {index} level {isOnNew}");
        }

        public void Damage(int value)
        {
            gameParameters.currentHealth -= value;

            _myLiveUnitData.parameters.currentHealth -= value;
            
            if (UnitSelector.Instance.SelectedUnit == this)
                UnitSelector.Instance.UpdateSelectedUnit();
            
            if (gameParameters.currentHealth <= 0)
                Kill();
        }
        
        public void Kill()
        {
            

            OnDead?.Invoke();
            
            if (gameParameters.controlType == ControlType.House)
            {
                GameControl.OnGameOver();
            }
            
            UnitSpawner.Instance.RemoveUnit(this);
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
            MeshRenderer = GetComponent<MeshRenderer>();

            SkeletonAnimation = GetComponent<SkeletonAnimation>();

            Collider = GetComponent<BoxCollider2D>();
            
            _normalShader = Shader.Find("Spine/Skeleton");

            _outLineShader = Shader.Find("Spine/Outline/Skeleton");
        }

        private void Start()
        {
            _myLiveUnitData = Managers.Values.GetLiveUnitByUnit(this);
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
        Worker,
        Laboratory,
        Factory,
        FlyPort,
        WithoutSale
    }
}