using BattleTech;
using HarmonyLib;
using HBS.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CustomStatisticEffects {
  public class EffectDataDef {
    private static Dictionary<string, VersionManifestEntry> manifest = new Dictionary<string, VersionManifestEntry>();
    private static Dictionary<string, EffectDataDef> dataManager = new Dictionary<string, EffectDataDef>();
    public static void Register(string id,VersionManifestEntry entry) { manifest[id] = entry; }
    public static bool Request(string id, out EffectDataDef effect) {
      if(dataManager.TryGetValue(id, out var result)) {
        effect = result;
        return true;
      }
      if(manifest.TryGetValue(id, out var entry)) {
        try {
          string content = File.ReadAllText(entry.FilePath);
          JObject data = JObject.Parse(content);
          JObject jeffect = null;
          effect = new EffectDataDef();
          //Avoiding loop if name is not set
          if (data[nameof(EffectData)] == null) { jeffect = data; } else { jeffect = data[nameof(EffectData)] as JObject; }
          if (jeffect == null) { effect = null; Log.Debug?.WL($"EffectData {id} bad data"); return false; }
          if (jeffect["Description"] == null) { effect = null; Log.Debug?.WL($"EffectData {id} no Description"); return false; }
          if (jeffect["Description"]["Name"] == null) { effect = null; Log.Debug?.WL($"EffectData {id} no Description.Name"); return false; }
          if (string.IsNullOrEmpty(jeffect["Description"]["Name"].ToString())) { effect = null; Log.Debug?.WL($"EffectData {id} Description.Name is empty"); return false; }
          effect.EffectData = new EffectData();
          effect.EffectData.FromJSON(jeffect.ToString());
          effect.Custom = data[nameof(Custom)] as JObject;
          effect.sourceFile = entry.FilePath;
          dataManager[id] = effect;
          return true;
        }catch(Exception e) {
          effect = null;
          Log.Error?.TWL(0,entry.FilePath);
          Log.Error?.WL(0, e.ToString());
          return false;
        }
      } else {
        Log.Debug?.WL($"EffectData {id} is not in manifest");
      }
      effect = null;
      return false;
    }
    [JsonIgnore]
    public string sourceFile { get; set; } = string.Empty;
    [JsonIgnore]
    public EffectData EffectData;
    public JObject Custom;
  }
  [HarmonyPatch]
  public static class JSONSerializationUtility_RehydrateObjectFromDictionary_Patch {
    public static MethodBase TargetMethod() => (MethodBase)typeof(JSONSerializationUtility).GetMethod("RehydrateObjectFromDictionary", BindingFlags.Static | BindingFlags.NonPublic);

    public static void Postfix(ref object target) {
      try {
        if(target is BattleTech.EffectData effectData) {
          //Log.Debug?.WL($"JSONSerializationUtility.RehydrateObjectFromDictionary {effectData.Description.Id} Name:{effectData.Description.Name}");
          if (string.IsNullOrEmpty(effectData.Description.Name)) {
            if (string.IsNullOrEmpty(effectData.Description.Id) == false) {
              if (EffectDataDef.Request(effectData.Description.Id, out var result)) {
                target = result.EffectData;
                effectData.durationData = result.EffectData.durationData;
                effectData.targetingData = result.EffectData.targetingData;
                effectData.effectType = result.EffectData.effectType;
                effectData.Description = result.EffectData.Description;
                effectData.nature = result.EffectData.nature;
                effectData.statisticData = result.EffectData.statisticData;
                effectData.tagData = result.EffectData.tagData;
                effectData.floatieData = result.EffectData.floatieData;
                effectData.actorBurningData = result.EffectData.actorBurningData;
                effectData.vfxData = result.EffectData.vfxData;
                effectData.instantModData = result.EffectData.instantModData;
                effectData.poorlyMaintainedEffectData = result.EffectData.poorlyMaintainedEffectData;
                effectData.activeAbilityEffectData = result.EffectData.activeAbilityEffectData;
                Log.Debug?.WL(0, $"EffectData {effectData.Description.Id} loaded as {result.sourceFile}");
              }
            }
          }
        }
      } catch (Exception e) {
        Log.Error?.TWL(0,e.ToString());
      }
    }
  }
}