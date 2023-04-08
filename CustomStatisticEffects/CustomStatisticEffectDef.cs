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
          effect = new EffectDataDef();
          if (data[nameof(EffectData)] == null) { effect = null; return false; }
          if (data[nameof(EffectData)]["Description"] == null) { effect = null; return false; }
          if (data[nameof(EffectData)]["Description"]["Name"] == null) { effect = null; return false; }
          if (string.IsNullOrEmpty(data[nameof(EffectData)]["Description"]["Name"].ToString())) { effect = null; return false; }
          effect.EffectData.FromJSON(data[nameof(EffectData)].ToString());
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
        if(target is EffectData effectData) {
          if (string.IsNullOrEmpty(effectData.Description.Name)) {
            if (string.IsNullOrEmpty(effectData.Description.Id) == false) {
              if (EffectDataDef.Request(effectData.Description.Id, out var result)) {
                target = result.EffectData;
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