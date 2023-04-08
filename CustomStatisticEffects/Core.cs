using BattleTech;
using HarmonyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomStatisticEffects {
  internal class Settings {
    public bool debugLog { get; set; } = false;
  }
  public static class Core {
    internal static string BaseDir { get; set; } = string.Empty;
    internal static Settings settings { get; set; } = new Settings();
    private static List<Action<MechDef, StatCollection>> InitStatsPrefix = new List<Action<MechDef, StatCollection>>();
    public static void CallInitStatsPrefix(MechDef mechDef, StatCollection statCollection) {
      foreach(var callback in InitStatsPrefix) { try { callback(mechDef, statCollection); } catch(Exception e) { Log.TWL(0,e.ToString(),true); } }
    }
    public static void RegisterInitStatsPrefix(Action<MechDef, StatCollection> callback) {
      InitStatsPrefix.Add(callback);
    }
    public static void FinishedLoading(List<string> loadOrder, Dictionary<string, Dictionary<string, VersionManifestEntry>> customResources) {
      Log.Error?.TWL(0, "FinishedLoading");
      try {
        foreach (var customResource in customResources) {
          Log.Debug?.WL(1, "customResource:" + customResource.Key);
          if (customResource.Key == nameof(EffectDataDef)) {
            foreach (var resource in customResource.Value) {
              Log.Debug?.WL(2, "resource:" + resource.Key + "=" + resource.Value.FilePath);
              EffectDataDef.Register(resource.Key, resource.Value);
            }
          }
        }
      } catch (Exception e) {
        Log.TWL(0, e.ToString(), true);
      }
    }
    public static T FindObject<T>(this GameObject go, string name) where T : Component {
      T[] arr = go.GetComponentsInChildren<T>(true);
      foreach (T component in arr) { if (component.gameObject.transform.name == name) { return component; } }
      return null;
    }
    public static void Init(string directory, string settingsJson) {
      Log.BaseDirectory = directory;
      Log.InitLog();
      Core.BaseDir = directory;
      Core.settings = JsonConvert.DeserializeObject<CustomStatisticEffects.Settings>(settingsJson);
      Log.TWL(0, "Initing... " + directory + " version: " + Assembly.GetExecutingAssembly().GetName().Version, true);
      try {
        var harmony = new Harmony("ru.kmission.customstatisticeffects");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
      } catch (Exception e) {
        Log.TWL(0, e.ToString(), true);
      }
    }
  }
}

