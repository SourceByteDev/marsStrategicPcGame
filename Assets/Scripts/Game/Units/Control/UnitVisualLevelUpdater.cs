using System;
using System.Collections;
using System.Linq;
using Game.Units.Unit_Types;
using LogicHelper;
using Manager;
using Spine.Unity;
using UnityEngine;

namespace Game.Units.Control
{
    public class UnitVisualLevelUpdater : MonoBehaviour
    {
        private ValuesManage.LiveUnitData _myLiveUnit;

        private void UpdateAllUnits()
        {
            var allUpdateLevelUnits = UnitSpawner.Instance.currentUnits.Where(x => x is IUnitLevelUpdate).ToList();
            
            if (allUpdateLevelUnits.Count <= 0)
                return;
            
            allUpdateLevelUnits.ForEach(x =>
            {
                UpdateLevelUnit(x, false);
            });
        }

        private void UpdateLevelUnit(Unit unit, bool isOnNew)
        {
            if (!(unit is IUnitLevelUpdate))
                return;

            unit.OnUpdateIndexLevel(unit.gameParameters.currentLevel, isOnNew);

            print("update " + unit.gameParameters.currentLevel);

            StartCoroutine(WaitSelected());
        }

        private void SubscribeOnUpdate()
        {
            UnitSpawner.Instance.onUnitSpawned += delegate(Unit unit)
            {
                UpdateLevelUnit(unit, false);
            };
        }

        private void Awake()
        {
            SubscribeOnUpdate();
        }

        private void Start()
        {
            UnitLevelUpdater.Instance.OnUnitLevelUpdated += delegate(Unit unit)
            {
                UpdateLevelUnit(unit, true);
            }; 
            
            UpdateAllUnits();
        }

        private IEnumerator WaitSelected()
        {
            yield return new WaitForSeconds(.01f);
            
            UnitSelector.Instance.UpdateSelectedUnit();
        }
    }

    [Serializable]
    public struct VisualLevelData
    {
        [SerializeField] private Vector2 scale;

        [SerializeField] private Material meshMaterial;

        [SerializeField] private SkeletonDataAsset skeletonAsset;

        [SerializeField] private Vector2 positionOffsetOnSet;

        [SerializeField] private Vector2 colliderSize;

        public void UpdateUnit(Unit unit, bool isOnNew)
        {
            var unitRenderer = unit.MeshRenderer;

            var unitTransform = unit.transform;

            var unitSkeletonAnimation = unit.SkeletonAnimation;
            
            var liveUnitData = Managers.Values.GetLiveUnitByUnit(unit);

            var unitCollider = unit.Collider;

            // Set up material
            unitRenderer.material = meshMaterial;
            
            // Set up scale
            unitTransform.localScale = scale;

            // Set skeleton
            unitSkeletonAnimation.skeletonDataAsset = skeletonAsset;
            
            // Initialize
            unitSkeletonAnimation.Initialize(true);

            unitCollider.size = colliderSize;

            if (!isOnNew)
                return;

            unit.IsSelected = false;
            
            unit.IsSelected = true;
            
            liveUnitData.position += positionOffsetOnSet;

            unitTransform.position = (Vector2) unit.transform.position + positionOffsetOnSet;
        }
    }

    internal interface IUnitLevelUpdate
    {
        VisualLevelData[] Visuals { get; }
    }
}