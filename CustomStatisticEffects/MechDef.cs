using BattleTech;
using BattleTech.Data;
using HarmonyLib;
using HBS.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CustomStatisticEffects {
  public class MechDefAdditional {
    public int ParentHashCode { get; private set; }
    private WeakReference<MechDef> f_parent = null;
    public MechDef parent {
      get {
        if(f_parent.TryGetTarget(out var result)) { return result; }
        return null;
      }
    }
    public StatCollection statCollection { get; private set; } = new StatCollection();
    public MechDefAdditional(MechDef parent) {
      this.f_parent = new WeakReference<MechDef>(parent);
      ParentHashCode = parent.GetHashCode();
    }

  }
  public static class MechDefHelper {
    private static Dictionary<int, HashSet<MechDefAdditional>> storage = new Dictionary<int, HashSet<MechDefAdditional>>();
    private static HashSet<MechDefAdditional> weakReferences = new HashSet<MechDefAdditional>();
    public static void Collect() {
      lock (weakReferences) {
        lock (storage) {
          int collected = 0;
          foreach (MechDefAdditional info in weakReferences) {
            if (info.parent == null) {
              ++collected;
              if (storage.TryGetValue(info.ParentHashCode, out var bucket)) {
                bucket.Remove(info);
              }
            }
          }
          weakReferences.RemoveWhere((a) => { return a.parent == null; });
          Log.Debug?.TWL(0, $"MechDefHelper.Collect collected:{collected}");
        }
      }
    }
    public static MechDefAdditional RegisterAddInfo(this MechDef mechDef) {
      lock (weakReferences) {
        lock (storage) {
          int hash = mechDef.GetHashCode();
          if (storage.TryGetValue(hash, out var bucket) == false) {
            bucket = new HashSet<MechDefAdditional>();
            storage.Add(hash, bucket);
          }
          foreach (var info in bucket) {
            if (info.parent == mechDef) { return info; }
          }
          var result = new MechDefAdditional(mechDef);
          bucket.Add(result);
          weakReferences.Add(result);
          return result;
        }
      }
    }
    public static StatCollection statCollection(this MechDef mechDef) {
      int hash = mechDef.GetHashCode();
      if (storage.TryGetValue(hash, out var bucket)) {
        foreach (var info in bucket) {
          if (info.parent == mechDef) {
            return info.statCollection;
          }
        }
      }
      return RegisterAddInfo(mechDef).statCollection;
    }
    public static void ApplyEffects(this MechDef mechDef) {
      if (mechDef == null) { return; }
      mechDef.ApplyEffects(mechDef.Inventory);
    }
    public static void ApplyEffects(this MechDef mechDef, IEnumerable<MechComponentRef> inventory) {
      foreach(var componentRef in inventory) {
        if (componentRef == null) { continue; }
        if (componentRef.Def == null) { continue; }
        foreach (var effectData in componentRef.Def.statusEffects) {
          if (effectData.targetingData.effectTriggerType != EffectTriggerType.Passive) { continue; }
          if (effectData.statisticData == null) { continue; }
          if (effectData.statisticData.targetCollection != StatisticEffectData.TargetCollection.NotSet) { continue; }
        }
      }
    }
    public static void InitStats(this MechDef mechDef) {
      var Constants = CombatGameConstants.GetInstance(UnityGameInstance.BattleTechGame);
      int Initiative = 0;
      StatCollection statCollection = mechDef.statCollection();
      statCollection.Reset(true);
      switch (mechDef.Chassis.weightClass) {
        case WeightClass.LIGHT:
        Initiative = Constants.Phase.PhaseLight;
        break;
        case WeightClass.MEDIUM:
        Initiative = Constants.Phase.PhaseMedium;
        break;
        case WeightClass.HEAVY:
        Initiative = Constants.Phase.PhaseHeavy;
        break;
        case WeightClass.ASSAULT:
        Initiative = Constants.Phase.PhaseAssault;
        break;
      }
      float ArmorMultiplier = 1f;
      float StructureMultiplier = 1f;
      switch (mechDef.Chassis.weightClass) {
        case WeightClass.LIGHT: ArmorMultiplier = Constants.CombatValueMultipliers.ArmorMultiplierLight; break;
        case WeightClass.MEDIUM: ArmorMultiplier = Constants.CombatValueMultipliers.ArmorMultiplierMedium; break;
        case WeightClass.HEAVY: ArmorMultiplier = Constants.CombatValueMultipliers.ArmorMultiplierHeavy; break;
        case WeightClass.ASSAULT: ArmorMultiplier = Constants.CombatValueMultipliers.ArmorMultiplierAssault; break;
      }
      switch (mechDef.Chassis.weightClass) {
        case WeightClass.LIGHT: StructureMultiplier = Constants.CombatValueMultipliers.StructureMultiplierLight; break;
        case WeightClass.MEDIUM: StructureMultiplier = Constants.CombatValueMultipliers.StructureMultiplierLight; break;
        case WeightClass.HEAVY: StructureMultiplier = Constants.CombatValueMultipliers.StructureMultiplierLight; break;
        case WeightClass.ASSAULT: StructureMultiplier = Constants.CombatValueMultipliers.StructureMultiplierLight; break;
      }
      statCollection.AddStatistic<int>("BaseInitiative", Initiative);
      statCollection.AddStatistic<int>("TurnRadius", mechDef.Chassis.TurnRadius);
      statCollection.AddStatistic<int>("MaxJumpjets", mechDef.Chassis.MaxJumpjets);
      statCollection.AddStatistic<float>("SpotterDistanceMultiplier", mechDef.Chassis.SpotterDistanceMultiplier);
      statCollection.AddStatistic<float>("SpotterDistanceAbsolute", 0.0f);
      statCollection.AddStatistic<float>("SpottingVisibilityMultiplier", mechDef.Chassis.VisibilityMultiplier);
      statCollection.AddStatistic<float>("SpottingVisibilityAbsolute", 0.0f);
      statCollection.AddStatistic<float>("SensorDistanceMultiplier", mechDef.Chassis.SensorRangeMultiplier);
      statCollection.AddStatistic<float>("SensorDistanceAbsolute", 0.0f);
      statCollection.AddStatistic<float>("SensorSignatureModifier", mechDef.Chassis.Signature);
      statCollection.AddStatistic<float>("MinStability", 0.0f);
      statCollection.AddStatistic<float>("MaxStability", mechDef.Chassis.Stability);
      statCollection.AddStatistic<float>("UnsteadyThreshold", Constants.ResolutionConstants.DefaultUnsteadyThreshold);
      statCollection.AddStatistic<int>("MaxHeat", Constants.Heat.MaxHeat);
      statCollection.AddStatistic<int>("OverheatLevel", (int)((double)Constants.Heat.MaxHeat * (double)Constants.Heat.OverheatLevel));
      statCollection.AddStatistic<int>("MinHeatNextActivation", -1);
      statCollection.AddStatistic<int>("HeatSinkCapacity", 0);
      statCollection.AddStatistic<bool>("IgnoreHeatToHitPenalties", false);
      statCollection.AddStatistic<bool>("IgnoreHeatMovementPenalties", false);
      statCollection.AddStatistic<int>("EndMoveHeat", 0);
      statCollection.AddStatistic<float>("WalkSpeed", mechDef.Chassis.MovementCapDef.MaxWalkDistance);
      statCollection.AddStatistic<float>("RunSpeed", mechDef.Chassis.MovementCapDef.MaxSprintDistance);
      statCollection.AddStatistic<float>("EngageRangeModifier", mechDef.Chassis.EngageRangeModifier);
      statCollection.AddStatistic<float>("DFASelfDamage", mechDef.Chassis.DFASelfDamage);
      statCollection.AddStatistic<bool>("DFACausesSelfUnsteady", true);
      statCollection.AddStatistic<int>("EvasivePipsGainedAdditional", 0);
      statCollection.AddStatistic<int>("MeleeHitPushBackPhases", 0);
      statCollection.AddStatistic<bool>("HeadShotImmunity", false);
      statCollection.AddStatistic<int>("CurrentHeat", 0);
      statCollection.AddStatistic<float>("Stability", 0.0f);
      statCollection.AddStatistic<bool>("IsProne", false);
      string[] names = Enum.GetNames(typeof(InstabilitySource));
      int length = names.Length;
      for (int index = 0; index < length; ++index)
        statCollection.AddStatistic<float>(string.Format("StabilityDefense.{0}", (object)names[index]), mechDef.Chassis.StabilityDefenses[index]);
      statCollection.AddStatistic<float>("ReceivedInstabilityMultiplier", 1f);
      statCollection.AddStatistic<float>("Head.Armor", mechDef.Head.AssignedArmor * ArmorMultiplier);
      statCollection.AddStatistic<float>("Head.Structure", mechDef.Head.CurrentInternalStructure * StructureMultiplier);
      statCollection.AddStatistic<LocationDamageLevel>("Head.DamageLevel", mechDef.Head.DamageLevel);
      statCollection.AddStatistic<float>("CenterTorso.Armor", mechDef.CenterTorso.AssignedArmor * ArmorMultiplier);
      statCollection.AddStatistic<float>("CenterTorso.RearArmor", mechDef.CenterTorso.AssignedRearArmor * ArmorMultiplier);
      statCollection.AddStatistic<float>("CenterTorso.Structure", mechDef.CenterTorso.CurrentInternalStructure * StructureMultiplier);
      statCollection.AddStatistic<LocationDamageLevel>("CenterTorso.DamageLevel", mechDef.CenterTorso.DamageLevel);
      statCollection.AddStatistic<float>("LeftTorso.Armor", mechDef.LeftTorso.AssignedArmor * ArmorMultiplier);
      statCollection.AddStatistic<float>("LeftTorso.RearArmor", mechDef.LeftTorso.AssignedRearArmor * ArmorMultiplier);
      statCollection.AddStatistic<float>("LeftTorso.Structure", mechDef.LeftTorso.CurrentInternalStructure * StructureMultiplier);
      statCollection.AddStatistic<LocationDamageLevel>("LeftTorso.DamageLevel", mechDef.LeftTorso.DamageLevel);
      statCollection.AddStatistic<float>("RightTorso.Armor", mechDef.RightTorso.AssignedArmor * ArmorMultiplier);
      statCollection.AddStatistic<float>("RightTorso.RearArmor", mechDef.RightTorso.AssignedRearArmor * ArmorMultiplier);
      statCollection.AddStatistic<float>("RightTorso.Structure", mechDef.RightTorso.CurrentInternalStructure * StructureMultiplier);
      statCollection.AddStatistic<LocationDamageLevel>("RightTorso.DamageLevel", mechDef.RightTorso.DamageLevel);
      statCollection.AddStatistic<float>("LeftArm.Armor", mechDef.LeftArm.AssignedArmor * ArmorMultiplier);
      statCollection.AddStatistic<float>("LeftArm.Structure", mechDef.LeftArm.CurrentInternalStructure * StructureMultiplier);
      statCollection.AddStatistic<LocationDamageLevel>("LeftArm.DamageLevel", mechDef.LeftArm.DamageLevel);
      statCollection.AddStatistic<float>("RightArm.Armor", mechDef.RightArm.AssignedArmor * ArmorMultiplier);
      statCollection.AddStatistic<float>("RightArm.Structure", mechDef.RightArm.CurrentInternalStructure * StructureMultiplier);
      statCollection.AddStatistic<LocationDamageLevel>("RightArm.DamageLevel", mechDef.RightArm.DamageLevel);
      statCollection.AddStatistic<float>("LeftLeg.Armor", mechDef.LeftLeg.AssignedArmor * ArmorMultiplier);
      statCollection.AddStatistic<float>("LeftLeg.Structure", mechDef.LeftLeg.CurrentInternalStructure * StructureMultiplier);
      statCollection.AddStatistic<LocationDamageLevel>("LeftLeg.DamageLevel", mechDef.LeftLeg.DamageLevel);
      statCollection.AddStatistic<float>("RightLeg.Armor", mechDef.RightLeg.AssignedArmor * ArmorMultiplier);
      statCollection.AddStatistic<float>("RightLeg.Structure", mechDef.RightLeg.CurrentInternalStructure * StructureMultiplier);
      statCollection.AddStatistic<LocationDamageLevel>("RightLeg.DamageLevel", mechDef.RightLeg.DamageLevel);
      Core.CallInitStatsPrefix(mechDef, statCollection);
    }
  }
  [HarmonyPatch(typeof(MechDef))]
  [HarmonyPatch(MethodType.Constructor)]
  [HarmonyPatch(new Type[] { })]
  public static class MechDef_Constructor {
    public static void Postfix(MechDef __instance) {
      try {
        //__instance.RegisterAddInfo();
      } catch (Exception e) {
        Log.Error?.TWL(0, e.ToString());
      }
    }
  }
  [HarmonyPatch(typeof(MechDef))]
  [HarmonyPatch(MethodType.Normal)]
  [HarmonyPatch("RefreshBattleValue")]
  [HarmonyPatch(new Type[] { })]
  public static class MechDef_RefreshBattleValue {
    public static void Postfix(MechDef __instance) {
      try {
        //Log.Debug?.TWL(0, $"MechDef.RefreshBattleValue id:{__instance.Description.Id} ChassisID:{__instance.ChassisID}");
        //__instance.RegisterAddInfo();
        //__instance.InitStats();
      } catch (Exception e) {
        Log.Error?.TWL(0, e.ToString());
      }
    }
  }
  [HarmonyPatch(typeof(GC))]
  [HarmonyPatch(MethodType.Normal)]
  [HarmonyPatch("Collect")]
  [HarmonyPatch(new Type[] { typeof(int), typeof(GCCollectionMode), typeof(bool), typeof(bool) })]
  public static class GC_Collect {
    public static void Postfix(int generation, GCCollectionMode mode, bool blocking, bool compacting) {
      try {
        //Log.Debug?.TWL(0, $"GC.Collect");
        MechDefHelper.Collect();
      } catch (Exception e) {
        Log.Error?.TWL(0, e.ToString());
      }
    }
  }
}