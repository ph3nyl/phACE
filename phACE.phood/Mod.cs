using ACE.Entity.Enum;
using ACE.Server.Command;
using ACE.Server.Mods;
using ACE.Server.Network;
using HarmonyLib;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace phACE.phood
{
    public class Settings
    {
        #region Content Control
        public bool ImportContentOnStart { get; set; } = true;
        public bool RevertContentOnStop { get; set; } = true;
        #endregion

        #region Patch_VitalSaturation
        public bool EnableVitalSaturation { get; set; } = true;
        public static double BonusRegenHealth { get; set; } = 2.5d;
        public static double BonusRegenStamina { get; set; } = 2.5d;
        public static double BonusRegenMana { get; set; } = 2.5d;
        public static double MaxRegenPoolValue { get; set; } = 3600d;
        public static double BoostToSatConversionFactor { get; set; } = 6.0d;
        public static double SpiceOfLife { get; set; } = 10.0d;
        #endregion

        #region Patch_ScalingAttributeBeers
        public bool EnableScalingAttributeBeers { get; set; } = true;
        #endregion
    }
    public class Mod : IHarmonyMod
    {
        #region Members
        private static string modPath = Path.Combine(ModManager.ModPath, "phACE.phood");
        public const string ID = "com.ACE.ACEmulator.phACE.phood";
        public const string shortName = "phood";
        public static Harmony Harmony { get; set; } = new(ID);
        private bool disposedValue;
        public static Mod? Instance { get; private set; }
        public static string ModPath { get => modPath; set => modPath = value; }
        public static ModState State { get => state; set => state = value; }

        const int SETTINGS_RETRIES = 10;
        private FileSystemWatcher? _settingsWatcher;
        private DateTime _lastChange = DateTime.Now;
        private readonly TimeSpan _reloadInterval = TimeSpan.FromSeconds(3);
        private static ModState state = ModState.None;
        private static Settings? settings = new();
        static string SettingsPath => Path.Combine(Mod.ModPath, "Settings.json");
        public static Settings? Settings { get => settings; set => settings = value; }
        private readonly FileInfo settingsInfo = new(SettingsPath);
        private readonly JsonSerializerOptions _serializeOptions = new()
        {
            WriteIndented = true,
            AllowTrailingCommas = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };
        #endregion

        #region Initialize / Dispose (called by ACE)
        public void Initialize()
        {
            Instance = this;

            _settingsWatcher = new FileSystemWatcher()
            {
                Path = ModPath,
                Filter = $"Settings.json",
                EnableRaisingEvents = true,
                NotifyFilter = NotifyFilters.LastWrite
            };

            _settingsWatcher.Changed += Settings_Changed;
            _settingsWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName;

            Start();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Stop();

                    if (_settingsWatcher != null)
                        _settingsWatcher.Changed -= Settings_Changed;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Start / Stop
        private void Start()
        {
            try
            {
                Mod.State = ModState.Loading;
                LoadSettings();

                if (Settings != null && Settings.ImportContentOnStart)
                {
                    ContentManager.RevertContent(modPath);
                    ContentManager.ImportContent(modPath);
                }

                if (Settings != null && Settings.EnableVitalSaturation)
                {
                    Patch_VitalSaturation.Init();
                    Harmony.PatchCategory("VitalSaturation");
                }

                if (Settings != null && Settings.EnableVitalSaturation)
                {
                    Patch_ScalingAttributeBeers.Init();
                    Harmony.PatchCategory("ScalingAttributeBeers");
                }

                if (Mod.State == ModState.Error)
                {
                    ModManager.DisableModByPath(Mod.ModPath);
                    return;
                }

                Mod.State = ModState.Running;
            }
            catch (Exception ex)
            {
                ModManager.Log($"Failed to start.  Unpatching {ID}: {ex.Message}");
                ModManager.DisableModByPath(ModPath);
                //Dispose();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        private void Stop()
        {
            if (Settings != null && Settings.RevertContentOnStop)
            {
                ContentManager.RevertContent(modPath);
            }

            if (Mod.State == ModState.Error)
                ModManager.Log($"Improper shutdown: {Mod.ModPath}", ModManager.LogLevel.Error);
            //CustomCommands.Unregister();

            if (Settings != null && Settings.EnableScalingAttributeBeers)
            {
                Patch_ScalingAttributeBeers.Fini();
            }
            if (Settings != null && Settings.EnableVitalSaturation)
            {
                Patch_VitalSaturation.Fini();
            }
            Harmony.UnpatchAll(ID);
        }
        #endregion

        #region Settings
        private void SaveSettings()
        {
            string jsonString = JsonSerializer.Serialize(Settings, _serializeOptions);

            if (!settingsInfo.RetryWrite(jsonString, SETTINGS_RETRIES))
            {
                ModManager.Log($"Failed to save settings to {SettingsPath}...", ModManager.LogLevel.Warn);
                Mod.State = ModState.Error;
            }
        }

        private void LoadSettings()
        {
            if (!settingsInfo.Exists)
            {
                ModManager.Log($"Creating {settingsInfo}...");
                SaveSettings();
            }
            else
                ModManager.Log($"Loading settings from {SettingsPath}...");

            if (!settingsInfo.RetryRead(out string jsonString, SETTINGS_RETRIES))
            {
                Mod.State = ModState.Error;
                return;
            }

            try
            {
                Settings = JsonSerializer.Deserialize<Settings>(jsonString, _serializeOptions);
            }
            catch (Exception)
            {
                ModManager.Log($"Failed to deserialize Settings: {SettingsPath}", ModManager.LogLevel.Warn);
                Mod.State = ModState.Error;
                return;
            }
        }


        private void Settings_Changed(object sender, FileSystemEventArgs e)
        {
            //Only reload if currently running
            if (State != ModState.Running)
                return;

            var delta = DateTime.Now - _lastChange;
            if (delta < _reloadInterval)
                return;
            _lastChange = DateTime.Now;

            Stop();
            Start();
            ModManager.Log($"Settings reloaded.");
        }
        #endregion

        #region Commands
        [CommandHandler($"{shortName}", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 2,
            $"configure parts of the {shortName} mod",
            "<system> <function>\n" +
            $"@{shortName} content <import/revert> -- affects *all* content included in mod\n" +
            $"can target specific content type by using <weenies/landblocks/spells/recipes/quests>")]
        public static void ContentCommands(Session session, params string[] parameters)
            => ContentManager.ContentCommands(session, modPath, parameters);
        #endregion
    }

    public enum ModState
    {
        None,       // Mod instance freshly created
        Loading,    // Patch class has been started
        Error,      // An error has occurred (loading/saving/etc.)
        Running     // Mod successfully started
    }
}