using BattleTech;
using HarmonyLib;
using HBS.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace CustomStatisticEffects {
  public static class StatisticEffectDataHelper {
    public static readonly string REMOVE_EFFECT_STATISTIC_NAME = "_CANCEL_EFFECT";
    private delegate string d_Field_get(BattleTech.StatisticEffectData src);
    private delegate void d_Field_set(BattleTech.StatisticEffectData src, string value);
    private static d_Field_get i_Location_get = null;
    private static d_Field_set i_Location_set = null;
    private static d_Field_get i_ShouldNotHaveTags_get = null;
    private static d_Field_set i_ShouldNotHaveTags_set = null;
    private static d_Field_get i_ShouldHaveTags_get = null;
    private static d_Field_set i_ShouldHaveTags_set = null;
    private static d_Field_get i_abilifierId_get = null;
    private static d_Field_set i_abilifierId_set = null;
    public static string abilifierId(this BattleTech.StatisticEffectData src) {
      if (i_abilifierId_get == null) { return string.Empty; }
      return i_abilifierId_get(src);
    }
    public static void abilifierId(this BattleTech.StatisticEffectData src, string value) {
      if (i_abilifierId_set == null) { return; }
      i_abilifierId_set(src, value);
    }
    public static string Location(this BattleTech.StatisticEffectData src) {
      if (i_Location_get == null) { return string.Empty; }
      return i_Location_get(src);
    }
    public static void Location(this BattleTech.StatisticEffectData src, string value) {
      if (i_Location_set == null) { return; }
      i_Location_set(src, value);
    }
    public static string ShouldNotHaveTags(this BattleTech.StatisticEffectData src) {
      if (i_ShouldNotHaveTags_get == null) { return string.Empty; }
      return i_ShouldNotHaveTags_get(src);
    }
    public static void ShouldNotHaveTags(this BattleTech.StatisticEffectData src, string value) {
      if (i_ShouldNotHaveTags_set == null) { return; }
      i_ShouldNotHaveTags_set(src, value);
    }
    public static string ShouldHaveTags(this BattleTech.StatisticEffectData src) {
      if (i_ShouldHaveTags_get == null) { return string.Empty; }
      return i_ShouldHaveTags_get(src);
    }
    public static void ShouldHaveTags(this BattleTech.StatisticEffectData src, string value) {
      if (i_ShouldHaveTags_set == null) { return; }
      i_ShouldHaveTags_set(src, value);
    }
    public static void Prepare() {
      FieldInfo Location = typeof(BattleTech.StatisticEffectData).GetField("Location", BindingFlags.Public | BindingFlags.Instance);
      Log.Error?.WL(1, $"StatisticEffectData.Location {(Location == null ? "not found" : "found")}");
      if (Location != null) {
        {
          var dm = new DynamicMethod("get_Location", typeof(string), new Type[] { typeof(BattleTech.StatisticEffectData) });
          var gen = dm.GetILGenerator();
          gen.Emit(OpCodes.Ldarg_0);
          gen.Emit(OpCodes.Ldfld, Location);
          gen.Emit(OpCodes.Ret);
          i_Location_get = (d_Field_get)dm.CreateDelegate(typeof(d_Field_get));
        }
        {
          var dm = new DynamicMethod("set_Location", null, new Type[] { typeof(BattleTech.StatisticEffectData), typeof(string) });
          var gen = dm.GetILGenerator();
          gen.Emit(OpCodes.Ldarg_0);
          gen.Emit(OpCodes.Ldarg_1);
          gen.Emit(OpCodes.Stfld, Location);
          gen.Emit(OpCodes.Ret);
          i_Location_set = (d_Field_set)dm.CreateDelegate(typeof(d_Field_set));
        }
      }
      FieldInfo ShouldHaveTags = typeof(BattleTech.StatisticEffectData).GetField("ShouldHaveTags", BindingFlags.Public | BindingFlags.Instance);
      Log.Error?.WL(1, $"StatisticEffectData.ShouldHaveTags {(ShouldHaveTags == null ? "not found" : "found")}");
      if (ShouldHaveTags != null) {
        {
          var dm = new DynamicMethod("get_ShouldHaveTags", typeof(string), new Type[] { typeof(BattleTech.StatisticEffectData) });
          var gen = dm.GetILGenerator();
          gen.Emit(OpCodes.Ldarg_0);
          gen.Emit(OpCodes.Ldfld, ShouldHaveTags);
          gen.Emit(OpCodes.Ret);
          i_ShouldHaveTags_get = (d_Field_get)dm.CreateDelegate(typeof(d_Field_get));
        }
        {
          var dm = new DynamicMethod("set_ShouldHaveTags", null, new Type[] { typeof(BattleTech.StatisticEffectData), typeof(string) });
          var gen = dm.GetILGenerator();
          gen.Emit(OpCodes.Ldarg_0);
          gen.Emit(OpCodes.Ldarg_1);
          gen.Emit(OpCodes.Stfld, ShouldHaveTags);
          gen.Emit(OpCodes.Ret);
          i_ShouldHaveTags_set = (d_Field_set)dm.CreateDelegate(typeof(d_Field_set));
        }
      }
      FieldInfo ShouldNotHaveTags = typeof(BattleTech.StatisticEffectData).GetField("ShouldNotHaveTags", BindingFlags.Public | BindingFlags.Instance);
      Log.Error?.WL(1, $"StatisticEffectData.ShouldNotHaveTags {(ShouldNotHaveTags == null ? "not found" : "found")}");
      if (ShouldNotHaveTags != null) {
        {
          var dm = new DynamicMethod("get_ShouldNotHaveTags", typeof(string), new Type[] { typeof(BattleTech.StatisticEffectData) });
          var gen = dm.GetILGenerator();
          gen.Emit(OpCodes.Ldarg_0);
          gen.Emit(OpCodes.Ldfld, ShouldNotHaveTags);
          gen.Emit(OpCodes.Ret);
          i_ShouldNotHaveTags_get = (d_Field_get)dm.CreateDelegate(typeof(d_Field_get));
        }
        {
          var dm = new DynamicMethod("set_ShouldNotHaveTags", null, new Type[] { typeof(BattleTech.StatisticEffectData), typeof(string) });
          var gen = dm.GetILGenerator();
          gen.Emit(OpCodes.Ldarg_0);
          gen.Emit(OpCodes.Ldarg_1);
          gen.Emit(OpCodes.Stfld, ShouldNotHaveTags);
          gen.Emit(OpCodes.Ret);
          i_ShouldNotHaveTags_set = (d_Field_set)dm.CreateDelegate(typeof(d_Field_set));
        }
      }
      FieldInfo abilifierId = typeof(BattleTech.StatisticEffectData).GetField("abilifierId", BindingFlags.Public | BindingFlags.Instance);
      Log.Error?.WL(1, $"StatisticEffectData.abilifierId {(abilifierId == null ? "not found" : "found")}");
      if (abilifierId != null) {
        {
          var dm = new DynamicMethod("get_abilifierId", typeof(string), new Type[] { typeof(BattleTech.StatisticEffectData) });
          var gen = dm.GetILGenerator();
          gen.Emit(OpCodes.Ldarg_0);
          gen.Emit(OpCodes.Ldfld, abilifierId);
          gen.Emit(OpCodes.Ret);
          i_abilifierId_get = (d_Field_get)dm.CreateDelegate(typeof(d_Field_get));
        }
        {
          var dm = new DynamicMethod("set_abilifierId", null, new Type[] { typeof(BattleTech.StatisticEffectData), typeof(string) });
          var gen = dm.GetILGenerator();
          gen.Emit(OpCodes.Ldarg_0);
          gen.Emit(OpCodes.Ldarg_1);
          gen.Emit(OpCodes.Stfld, abilifierId);
          gen.Emit(OpCodes.Ret);
          i_abilifierId_set = (d_Field_set)dm.CreateDelegate(typeof(d_Field_set));
        }
      }
    }
  }
  public static class EffectDurationDataHelper {
    private delegate string d_Field_get(BattleTech.EffectDurationData src);
    private delegate void d_Field_set(BattleTech.EffectDurationData src, string value);
    private static d_Field_get i_stackId_get = null;
    private static d_Field_set i_stackId_set = null;
    public static string stackId(this BattleTech.EffectDurationData src) {
      if (i_stackId_get == null) { return string.Empty; }
      return i_stackId_get(src);
    }
    public static void stackId(this BattleTech.EffectDurationData src, string value) {
      if (i_stackId_set == null) { return; }
      i_stackId_set(src, value);
    }
    public static void Prepare() {
      FieldInfo stackId = typeof(BattleTech.EffectDurationData).GetField("stackId", BindingFlags.Public | BindingFlags.Instance);
      Log.Error?.WL(1, $"EffectDurationData.stackId {(stackId == null ? "not found" : "found")}");
      if (stackId != null) {
        {
          var dm = new DynamicMethod("get_stackId", typeof(string), new Type[] { typeof(BattleTech.EffectDurationData) });
          var gen = dm.GetILGenerator();
          gen.Emit(OpCodes.Ldarg_0);
          gen.Emit(OpCodes.Ldfld, stackId);
          gen.Emit(OpCodes.Ret);
          i_stackId_get = (d_Field_get)dm.CreateDelegate(typeof(d_Field_get));
        }
        {
          var dm = new DynamicMethod("set_stackId", null, new Type[] { typeof(BattleTech.EffectDurationData), typeof(string) });
          var gen = dm.GetILGenerator();
          gen.Emit(OpCodes.Ldarg_0);
          gen.Emit(OpCodes.Ldarg_1);
          gen.Emit(OpCodes.Stfld, stackId);
          gen.Emit(OpCodes.Ret);
          i_stackId_set = (d_Field_set)dm.CreateDelegate(typeof(d_Field_set));
        }
      }
    }
  }

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
        if (string.IsNullOrEmpty(effect.effectData.durationData.stackId()) == false) {
          if (effect_id_cache.TryGetValue(effect.effectData.durationData.stackId(), out var list2) == false) {
            list2 = new List<StatisticEffect>();
            effect_id_cache.Add(effect.effectData.durationData.stackId(), list2);
          }
          list2.Add(effect);
          activeEffectCache[effect.statCollection][effect.effectData.durationData.stackId()] = list2;
        }
      } catch (Exception e) {
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
        if (string.IsNullOrEmpty(effect.effectData.durationData.stackId()) == false) {
          if (effect_id_cache.TryGetValue(effect.effectData.durationData.stackId(), out var list2) == false) {
            list2 = new List<StatisticEffect>();
            effect_id_cache.Add(effect.effectData.durationData.stackId(), list2);
          }
          list2.Remove(effect);
          activeEffectCache[effect.statCollection][effect.effectData.durationData.stackId()] = list2;
        }
      } catch (Exception e) {
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
      return effect.effectData.EffectsByDataID(effect.statCollection);
    }
    public static List<StatisticEffect> EffectsByDataID(this EffectData effectData, StatCollection statCollection) {
      if (statCollection == null) { return new List<StatisticEffect>(); }
      string id = effectData.durationData.stackId();
      if (string.IsNullOrEmpty(id)) { id = effectData.Description.Id; }
      if (string.IsNullOrEmpty(id)) { return new List<StatisticEffect>(); }
      if (activeEffectCache.TryGetValue(statCollection, out var effect_id_cache) == false) {
        effect_id_cache = new Dictionary<string, List<StatisticEffect>>();
        activeEffectCache.Add(statCollection, effect_id_cache);
      }
      if (effect_id_cache.TryGetValue(id, out var list) == false) {
        list = new List<StatisticEffect>();
        effect_id_cache.Add(id, list);
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
  [HarmonyPatch(typeof(AbstractActor), "OnActivationEnd")]
  public static class AbstractActor_OnActivationEnd {
    public static void Prefix(AbstractActor __instance, ref bool __state) {
      if (__instance == null) { return; }
      __state = __instance.HasActivatedThisRound;
      try {
        Log.TWL(0, $"AbstractActor.OnActivationEnd {__instance.PilotableActorDef.ChassisID}:{__instance.GUID} HasActivatedThisRound:{__instance.HasActivatedThisRound}");
      }catch(Exception e) {
        Log.TWL(0,e.ToString(),true);
      }
    }
    public static void Postfix(AbstractActor __instance, ref bool __state) {
      if (__instance == null) { return; }
      try {
        if (__state == true) { return; }
        Log.TWL(0, $"AbstractActor.OnActivationEnd {__instance.PilotableActorDef.ChassisID}:{__instance.GUID} HasActivatedThisRound:{__state}");
        __instance.Combat.EffectManager.NotifyEndOfObjectActivation(__instance.GUID);
      } catch (Exception e) {
        Log.TWL(0, e.ToString(), true);
      }
    }
  }
  [HarmonyPatch(typeof(EffectManager), "OnTurnActorActivateComplete")]
  public static class EffectManager_OnTurnActorActivateComplete {
    public static void Prefix(EffectManager __instance, ref bool __runOriginal) {
      if (__instance == null) { return; }
      __runOriginal = false;
      try {

      } catch (Exception e) {
        Log.TWL(0, e.ToString(), true);
      }
    }
  }
  [HarmonyPatch(typeof(EffectManager), "CancelEffect")]
  public static class EffectManager_CancelEffect {
    public static void Postfix(EffectManager __instance, Effect e) {
      if (e is StatisticEffect seffect) {
        seffect.UncacheEffectID();
        //Log.Debug?.TWL(0, $"EffectManager.CancelEffect descriptionId:{e.effectData.Description.Id} stackId:{e.EffectData.durationData.stackId()} abilifierId:{e.EffectData.durationData.abilifierId()} name:'{e.EffectData.Description.Name}' target:{seffect.Target.PilotableActorDef.ChassisID} stack:{seffect.CountApplied(false)}");
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
        //Log.Debug?.TWL(0, $"EffectManager.EffectComplete descriptionId:{e.effectData.Description.Id} stackId:{e.EffectData.durationData.stackId()} abilifierId:{e.EffectData.durationData.abilifierId()} name:'{e.EffectData.Description.Name}' target:{seffect.Target.PilotableActorDef.ChassisID} stack:{seffect.CountApplied(false)}");
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
        Log.Debug?.TWL(0, $"StatisticEffect.OnEffectBegin descriptionId:{__instance.EffectData.Description.Id} stackId:{__instance.EffectData.durationData.stackId()} abilifierId:{__instance.EffectData.statisticData.abilifierId()} name:'{__instance.EffectData.Description.Name}' target:{__instance.Target.PilotableActorDef.ChassisID}");
        Log.Debug?.WL(1, $"rejected by stack limit");
        __runOriginal = false; __instance.SetApplicationStatus(false);
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
        List<StatCollection> targetStatCollections = __instance.GetTargetStatCollections(effectData, target).ToHashSet().ToList();
        //Log.Debug?.TWL(0, $"EffectManager.CreateEffect team descriptionId:{effectData.Description.Id} stackId:{effectData.durationData.stackId()} abilifierId:{effectData.durationData.abilifierId()} name:'{effectData.Description.Name}' target:{target.PilotableActorDef.ChassisID} collections:{targetStatCollections.Count}");
        for (int index = 0; index < targetStatCollections.Count; ++index) {
          if (targetStatCollections[index].ContainsStatistic(effectData.statisticData.statName) == false) { continue; }
          var effect = new StatisticEffect(__instance.Combat, effectID, stackItemUID, creator, target, targetStatCollections[index], effectData, hitInfo, attackIndex);
          __instance.AddEffect(effect, skipLogging);
          if (EffectManager.AbilityLogger.IsLogEnabled) {
            LogLevel level = skipLogging ? LogLevel.Debug : LogLevel.Log;
            EffectManager.AbilityLogger.LogAtLevel(level, string.Format("{0} gains effect {1} from team {2}", target.DisplayName, effect.EffectData.Description.Name, creator.DisplayName));
          }
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
  [HarmonyAfter("io.mission.activatablecomponents")]
  public static class EffectManager_CreateEffect_Actor {
    public static void RemoveEffect(this EffectManager effectManager, StatCollection statCollection, EffectData effectData, bool skipLogging) {
      var effectsToRemove = effectData.EffectsByDataID(statCollection);
      foreach(var effect in effectsToRemove) {
        effectManager.CancelEffect(effect, skipLogging);
      }
    }
    public static void Prefix(ref bool __runOriginal, EffectManager __instance, EffectData effectData, string effectID, int stackItemUID, ICombatant creator, ICombatant target, WeaponHitInfo hitInfo, int attackIndex, bool skipLogging, ref List<Effect> __result) {
      try {
        if (effectData.effectType != EffectType.StatisticEffect) { return; }
        __runOriginal = false;
        __result = new List<Effect>();
        List<StatCollection> targetStatCollections = __instance.GetTargetStatCollections(effectData, target).ToHashSet().ToList();
        //Log.Debug?.TWL(0, $"EffectManager.CreateEffect actor descriptionId:{effectData.Description.Id} stackId:{effectData.durationData.stackId()} abilifierId:{effectData.statisticData.abilifierId()} statName:{effectData.statisticData.statName} name:'{effectData.Description.Name}' target:{target.PilotableActorDef.ChassisID} collections:{targetStatCollections.Count}");
        //Log.Debug?.WL(0, Environment.StackTrace);
        for (int index = 0; index < targetStatCollections.Count; ++index) {
          if(StatisticEffectDataHelper.REMOVE_EFFECT_STATISTIC_NAME == effectData.statisticData.statName) {
            __instance.RemoveEffect(targetStatCollections[index], effectData, skipLogging);
            continue;
          }
          if (targetStatCollections[index].ContainsStatistic(effectData.statisticData.statName) == false) { continue; }
          var effect = new StatisticEffect(__instance.Combat, effectID, stackItemUID, creator, target, targetStatCollections[index], effectData, hitInfo, attackIndex);
          __instance.AddEffect(effect, skipLogging);
          if (EffectManager.AbilityLogger.IsLogEnabled && !skipLogging) {
            LogLevel level = skipLogging ? LogLevel.Debug : LogLevel.Log;
            if (creator != target) {
              EffectManager.AbilityLogger.LogAtLevel(level, string.Format("{0} gains effect {1} from {2}", target.DisplayName, effect.EffectData.Description.Name, creator.DisplayName));
            } else {
              EffectManager.AbilityLogger.LogAtLevel(level, string.Format("{0} gains effect {1} from self", target.DisplayName, effect.EffectData.Description.Name));
            }
          }
          __result.Add(effect);
        }
      } catch (Exception e) {
        Log.Error?.TWL(0, e.ToString());
      }
    }
  }
}