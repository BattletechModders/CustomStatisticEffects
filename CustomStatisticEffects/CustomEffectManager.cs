using BattleTech;
using HarmonyLib;
using System;
using System.Collections.Generic;

namespace CustomStatisticEffects {
  public static class EffectManagerHelper {
    private static Dictionary<StatCollection, Dictionary<string, List<StatisticEffect>>> activeEffectCache = new Dictionary<StatCollection, Dictionary<string, List<StatisticEffect>>>();
    public static void Clear() { activeEffectCache.Clear(); }
    public static void CacheEffectID(this StatisticEffect effect) {
      if (effect == null) { return; }
      if (effect.statCollection == null) { return; }
      try {
        if (activeEffectCache.TryGetValue(effect.statCollection, out var effect_id_cache) == false) {
          effect_id_cache = new Dictionary<string, List<StatisticEffect>>();
          activeEffectCache.Add(effect.statCollection, effect_id_cache);
        }
        if (string.IsNullOrEmpty(effect.id) == false) {
          if (effect_id_cache.TryGetValue(effect.id, out var list0) == false) {
            list0 = new List<StatisticEffect>();
            effect_id_cache.Add(effect.id, list0);
          }
          list0.Add(effect);
          activeEffectCache[effect.statCollection][effect.id] = list0;
        }
        if (string.IsNullOrEmpty(effect.effectData.Description.Id) == false) {
          if (effect_id_cache.TryGetValue(effect.effectData.Description.Id, out var list1) == false) {
            list1 = new List<StatisticEffect>();
            effect_id_cache.Add(effect.effectData.Description.Id, list1);
          }
          list1.Add(effect);
          activeEffectCache[effect.statCollection][effect.effectData.Description.Id] = list1;
        }
      }catch(Exception e) {
        Log.Error?.TWL(0,e.ToString());
      }
    }
    public static void UncacheEffectID(this StatisticEffect effect) {
      if (effect == null) { return; }
      if (effect.statCollection == null) { return; }
      try {
        if (activeEffectCache.TryGetValue(effect.statCollection, out var effect_id_cache) == false) {
          effect_id_cache = new Dictionary<string, List<StatisticEffect>>();
          activeEffectCache.Add(effect.statCollection, effect_id_cache);
        }
        if (string.IsNullOrEmpty(effect.id) == false) {
          if (effect_id_cache.TryGetValue(effect.id, out var list0) == false) {
            list0 = new List<StatisticEffect>();
            effect_id_cache.Add(effect.id, list0);
          }
          list0.Remove(effect);
          activeEffectCache[effect.statCollection][effect.id] = list0;
        }
        if (string.IsNullOrEmpty(effect.effectData.Description.Id) == false) {
          if (effect_id_cache.TryGetValue(effect.effectData.Description.Id, out var list1) == false) {
            list1 = new List<StatisticEffect>();
            effect_id_cache.Add(effect.effectData.Description.Id, list1);
          }
          list1.Remove(effect);
          activeEffectCache[effect.statCollection][effect.effectData.Description.Id] = list1;
        }
      }catch(Exception e) {
        Log.Error?.TWL(0, e.ToString());
      }
    }
    public static List<StatisticEffect> EffectsByID(this StatisticEffect effect) {
      if (effect.statCollection == null) { return new List<StatisticEffect>(); }
      if (string.IsNullOrEmpty(effect.id)) { return new List<StatisticEffect>(); }
      if (activeEffectCache.TryGetValue(effect.statCollection, out var effect_id_cache) == false) {
        effect_id_cache = new Dictionary<string, List<StatisticEffect>>();
        activeEffectCache.Add(effect.statCollection, effect_id_cache);
      }
      if (effect_id_cache.TryGetValue(effect.id, out var list) == false) {
        list = new List<StatisticEffect>();
        effect_id_cache.Add(effect.id, list);
      }
      return list;
    }
    public static List<StatisticEffect> EffectsByDataID(this StatisticEffect effect) {
      if (effect.statCollection == null) { return new List<StatisticEffect>(); }
      if (string.IsNullOrEmpty(effect.effectData.Description.Id)) { return new List<StatisticEffect>(); }
      if (activeEffectCache.TryGetValue(effect.statCollection, out var effect_id_cache) == false) {
        effect_id_cache = new Dictionary<string, List<StatisticEffect>>();
        activeEffectCache.Add(effect.statCollection, effect_id_cache);
      }
      if (effect_id_cache.TryGetValue(effect.effectData.Description.Id, out var list) == false) {
        list = new List<StatisticEffect>();
        effect_id_cache.Add(effect.effectData.Description.Id, list);
      }
      return list;
    }
    public static int CountApplied(this StatisticEffect effect, bool unique) {
      List<StatisticEffect> effects = unique ? effect.EffectsByID() : effect.EffectsByDataID();
      int result = 0;
      foreach(var othereffect in effects) {
        if (othereffect.IsEffectApplied()) { ++result; }
      }
      return result;
    }
    public static bool IsEffectApplied(this StatisticEffect effect) {
      if ((effect.activationsRemaining < 0) && (effect.phasesRemaining < 0)) { return false; }
      if ((effect.activationsRemaining < 0) && (effect.roundsRemaining < 0)) { return false; }
      if ((effect.activationsRemaining < 0) && (effect.movesRemaining < 0)) { return false; }

      if ((effect.phasesRemaining < 0) && (effect.roundsRemaining < 0)) { return false; }
      if ((effect.phasesRemaining < 0) && (effect.movesRemaining < 0)) { return false; }

      if ((effect.roundsRemaining < 0) && (effect.movesRemaining < 0)) { return false; }
      return true;
    }
    public static void ReapplyStack(this StatisticEffect effect) {
      foreach(var othereffect in effect.EffectsByID()) {
        if (othereffect.IsEffectApplied()) { continue; }
        othereffect.OnEffectBegin(true);
        if (othereffect.IsEffectApplied()) {
          Log.Debug?.WL(0,$"Effect {effect.effectData.Description.Id}:{effect.id} now satisfy stack limit");
        }
      }
      foreach (var othereffect in effect.EffectsByDataID()) {
        if (othereffect.IsEffectApplied()) { continue; }
        othereffect.OnEffectBegin(true);
        if (othereffect.IsEffectApplied()) {
          Log.Debug?.WL(0, $"Effect {effect.effectData.Description.Id}:{effect.id} now satisfy stack limit");
        }
      }
    }
    public static void Dump(this EffectManager manager) {
      Log.Debug?.TWL(0,$"Effects Dump round:{manager.Combat.TurnDirector.CurrentRound} phase:{manager.Combat.TurnDirector.CurrentPhase}");
      foreach(var target in activeEffectCache) {
        Log.Debug?.WL(1, $"{(target.Key as ICombatant).PilotableActorDef.ChassisID}");
        foreach(var effects in target.Value) {
          foreach(var effect in effects.Value) {
            if (effect.effectData.Description.Id != effects.Key) { continue; }
            StatisticEffect seffect = effect as StatisticEffect;
            Log.Debug?.WL(2,$"{effect.effectData.Description.Id}:{effect.id} applied:{effect.IsEffectApplied()} statvalue:{seffect.modVariant.statName}:{seffect.statCollection.GetStatistic(seffect.modVariant.statName).CurrentValue.ToString()} activationsRemaining:{effect.activationsRemaining} phasesRemaining:{effect.phasesRemaining} roundsRemaining:{effect.roundsRemaining} movesRemaining:{effect.movesRemaining}");
          }
        }
      }
    }
  }
  [HarmonyPatch(typeof(EffectManager), "Hydrate")]
  public static class EffectManager_Hydrate {
    public static void Postfix(EffectManager __instance, List<Effect> ___effects) {
      try {
        EffectManagerHelper.Clear();
        foreach (Effect effect in ___effects) {
          if (effect is StatisticEffect seffect) {
            seffect.CacheEffectID();
          }          
        }          
      }catch(Exception e) {
        Log.TWL(0,e.ToString(),true);
      }
    }
  }
  [HarmonyPatch(typeof(CombatGameState), "_Init")]
  public static class CombatGameState_Init {
    public static void Postfix() => EffectManagerHelper.Clear();
  }
  [HarmonyPatch(typeof(CombatGameState), "OnCombatGameDestroyed")]
  public static class H_CombatGaemState_OnCombatGameDestroyed {
    public static void Postfix() => EffectManagerHelper.Clear();
  }
  [HarmonyPatch(typeof(EffectManager), "AddEffect")]
  public static class EffectManager_AddEffect {
    public static void Postfix(EffectManager __instance, Effect effect) {
      if (effect is StatisticEffect seffect) { seffect.CacheEffectID(); }
      
    }
  }
  [HarmonyPatch(typeof(EffectManager), "CancelEffect")]
  public static class EffectManager_CancelEffect {
    public static void Postfix(EffectManager __instance, Effect e) {
      if (e is StatisticEffect seffect) {
        seffect.UncacheEffectID();
        seffect.ReapplyStack();
        //__instance.Dump();
      }
    }
  }
  [HarmonyPatch(typeof(EffectManager), "EffectComplete")]
  public static class EffectManager_Effectcomplete {
    public static void Postfix(EffectManager __instance, Effect e) {
      if (e is StatisticEffect seffect) {
        seffect.UncacheEffectID();
        seffect.ReapplyStack();
        //__instance.Dump();
      }
    }
  }
  [HarmonyPatch(typeof(StatisticEffect), "OnEffectPhaseBegin")]
  public static class StatisticEffect_OnEffectPhaseBegin {
    public static void Prefix(StatisticEffect __instance, ref bool __runOriginal) {
      if (__instance is StatisticEffect seffect) {
        if (seffect.IsEffectApplied() == false) { __runOriginal = false; }
      }
    }
  }
  [HarmonyPatch(typeof(StatisticEffect), "OnEffectTakeDamage")]
  public static class StatisticEffect_OnEffectTakeDamage {
    public static void Prefix(StatisticEffect __instance, ref bool __runOriginal) {
      if (__instance is StatisticEffect seffect) {
        if (seffect.IsEffectApplied() == false) { __runOriginal = false; }
      }
    }
  }
  [HarmonyPatch(typeof(StatisticEffect), "OnEffectActivationEnd")]
  public static class StatisticEffect_OnEffectActivationEnd {
    public static void Prefix(StatisticEffect __instance, ref bool __runOriginal) {
      if (__instance is StatisticEffect seffect) {
        if (seffect.IsEffectApplied() == false) { __runOriginal = false; }
      }
    }
  }
  [HarmonyPatch(typeof(StatisticEffect), "OnEffectBegin")]
  public static class StatisticEffect_OnEffectBegin {
    public static bool CheckStackLimit(this StatisticEffect effect) {
      if(effect.EffectData.durationData.uniqueEffectIdStackLimit > 0) {
        if (effect.CountApplied(true) >= effect.EffectData.durationData.uniqueEffectIdStackLimit) { return false; }
      }
      if (effect.EffectData.durationData.stackLimit > 0) {
        if (effect.CountApplied(false) >= effect.EffectData.durationData.stackLimit) { return false; }
      }
      return true;
    }
    public static void SetApplicationStatus(this StatisticEffect effect, bool state) {
      effect.activationsRemaining = effect.eTimer.numActivationsRemaining;
      effect.phasesRemaining = effect.eTimer.numPhasesRemaining;
      effect.roundsRemaining = effect.eTimer.numRoundsRemaining;
      effect.movesRemaining = effect.eTimer.numMovementsRemaining;
      if (state == false) {
        if (effect.activationsRemaining <= 0) { effect.activationsRemaining = -1; }
        if (effect.phasesRemaining <= 0) { effect.phasesRemaining = -1; }
        if (effect.roundsRemaining <= 0) { effect.roundsRemaining = -1; }
        if (effect.movesRemaining <= 0) { effect.movesRemaining = -1; }
      } else {
        if (effect.activationsRemaining < 0) { effect.activationsRemaining = 0; }
        if (effect.phasesRemaining < 0) { effect.phasesRemaining = 0; }
        if (effect.roundsRemaining < 0) { effect.roundsRemaining = 0; }
        if (effect.movesRemaining < 0) { effect.movesRemaining = 0; }
      }
      effect.eTimer.numActivationsRemaining = effect.activationsRemaining;
      effect.eTimer.numPhasesRemaining = effect.phasesRemaining;
      effect.eTimer.numRoundsRemaining = effect.roundsRemaining;
      effect.eTimer.numMovementsRemaining = effect.movesRemaining;
    }
    public static void Prefix(StatisticEffect __instance, ref bool __runOriginal) {
      //if (__instance is StatisticEffect seffect) {
        if (__instance.CheckStackLimit() == false) {
          __runOriginal = false; __instance.SetApplicationStatus(false);
          Log.Debug?.TWL(0, $"StatisticEffect.OnEffectBegin effect:{__instance.EffectData.Description.Id} target:{__instance.Target.PilotableActorDef.ChassisID} rejected by stack limit");
        } else {
          __instance.SetApplicationStatus(true);
        }
     // }
    }
  }
  [HarmonyPatch(typeof(EffectManager))]
  [HarmonyPatch(MethodType.Normal)]
  [HarmonyPatch("CreateEffect")]
  [HarmonyPatch(new Type[] { typeof(EffectData), typeof(string), typeof(int), typeof(Team), typeof(ICombatant), typeof(WeaponHitInfo), typeof(int), typeof(bool) })]
  [HarmonyPriority(0)]
  public static class EffectManager_CreateEffect_Team {
    public static void Prefix(ref bool __runOriginal, EffectManager __instance, EffectData effectData, string effectID, int stackItemUID, Team creator, ICombatant target, WeaponHitInfo hitInfo, int attackIndex, bool skipLogging, ref List<Effect> __result) {
      try {
        if (effectData.effectType != EffectType.StatisticEffect) { return; }
        __runOriginal = false;
        __result = new List<Effect>();
        List<StatCollection> targetStatCollections = __instance.GetTargetStatCollections(effectData, target);
        for (int index = 0; index < targetStatCollections.Count; ++index) {
          if (targetStatCollections[index].ContainsStatistic(effectData.statisticData.statName) == false) { continue; }
          var effect = new StatisticEffect(__instance.Combat, effectID, stackItemUID, creator, target, targetStatCollections[index], effectData, hitInfo, attackIndex);
          __instance.AddEffect(effect, skipLogging);
          __result.Add(effect);
        }
      } catch (Exception e) {
        Log.Error?.TWL(0, e.ToString());
      }
    }
  }
  [HarmonyPatch(typeof(EffectManager))]
  [HarmonyPatch(MethodType.Normal)]
  [HarmonyPatch("CreateEffect")]
  [HarmonyPatch(new Type[] { typeof(EffectData), typeof(string), typeof(int), typeof(ICombatant), typeof(ICombatant), typeof(WeaponHitInfo), typeof(int), typeof(bool) })]
  [HarmonyPriority(0)]
  public static class EffectManager_CreateEffect_Actor {
    public static void Prefix(ref bool __runOriginal, EffectManager __instance, EffectData effectData, string effectID, int stackItemUID, ICombatant creator, ICombatant target, WeaponHitInfo hitInfo, int attackIndex, bool skipLogging, ref List<Effect> __result) {
      try {
        if (effectData.effectType != EffectType.StatisticEffect) { return; }
        __runOriginal = false;
        __result = new List<Effect>();
        List<StatCollection> targetStatCollections = __instance.GetTargetStatCollections(effectData, target);
        for (int index = 0; index < targetStatCollections.Count; ++index) {
          if (targetStatCollections[index].ContainsStatistic(effectData.statisticData.statName) == false) { continue; }
          var effect = new StatisticEffect(__instance.Combat, effectID, stackItemUID, creator, target, targetStatCollections[index], effectData, hitInfo, attackIndex);
          __instance.AddEffect(effect, skipLogging);
          __result.Add(effect);
        }
      } catch (Exception e) {
        Log.Error?.TWL(0, e.ToString());
      }
    }
  }
}