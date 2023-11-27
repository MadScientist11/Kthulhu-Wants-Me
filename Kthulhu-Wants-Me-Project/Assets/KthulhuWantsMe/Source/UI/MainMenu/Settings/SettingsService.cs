using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.Audio;
using Sirenix.Serialization;
using UnityEngine;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.UI.MainMenu.Settings
{
    public interface ISettingCommand
    {
        void Apply();
    }

    public class QualitySettingCommand : ISettingCommand
    {
        private SettingLowMediumHigh _valueToApply;

        public QualitySettingCommand Init(SettingLowMediumHigh value)
        {
            _valueToApply = value;
            return this;
        }

        public void Apply()
        {
            //QualitySettings.SetQualityLevel(0);
        }
    }

    public class WindowModeSettingCommand : ISettingCommand
    {
        private SettingWindowMode _valueToApply;

        public WindowModeSettingCommand Init(SettingWindowMode value)
        {
            _valueToApply = value;
            return this;
        }

        public void Apply()
        {
            switch (_valueToApply)
            {
                case SettingWindowMode.Fullscreen:
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height,
                        FullScreenMode.FullScreenWindow);
                    break;
                case SettingWindowMode.Windowed:
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height,
                        FullScreenMode.Windowed);
                    break;
                default:
                    break;
            }
        }
    }

    public class Settings
    {
        public IReadOnlyDictionary<SettingId, Enum> Table => _settings;
        private Dictionary<SettingId, Enum> _settings = new();

        public Enum this[SettingId i]
        {
            get { return _settings[i]; }
            set { _settings[i] = value; }
        }

        public void Clear()
        {
            _settings.Clear();
        }

        public Settings Clone()
        {
            Settings settings = MemberwiseClone() as Settings;
            settings._settings = _settings.ToDictionary(kv => kv.Key, kv => kv.Value);
            return settings;
        }

        public IEnumerable<KeyValuePair<SettingId, Enum>> GetDifferentEntries(Settings settings)
        {
            return _settings.Where(entry =>
                !settings.Table.ContainsKey(entry.Key) || !AreEnumsEqual(entry.Value, settings.Table[entry.Key]));
        }

        private bool AreEnumsEqual(Enum enum1, Enum enum2)
        {
            return enum1.Equals(enum2);
        }
    }

    public class SettingsService : IInitializableService
    {
        private readonly Dictionary<SettingId, ISettingCommand> _commands = new();

        private Settings _current;
        
        private readonly Settings _defaultSettings = new();

        private readonly Settings _sessionOverrides = new();
        private readonly IAudioService _audioService;

        public SettingsService(IAudioService audioService)
        {
            _audioService = audioService;

            _defaultSettings[SettingId.WindowMode] = SettingWindowMode.Fullscreen;
            _defaultSettings[SettingId.HudSetting] = SettingOnOff.On;
            _defaultSettings[SettingId.VSync] = SettingOnOff.On;
            _defaultSettings[SettingId.MasterVolume] = SettingGradation.Hundred;
            _defaultSettings[SettingId.SFXVolume] = SettingGradation.Hundred;
            _defaultSettings[SettingId.UIVolume] = SettingGradation.Hundred;
            _defaultSettings[SettingId.BGMVolume] = SettingGradation.Hundred;

            _commands.Add(SettingId.WindowMode, new WindowModeSettingCommand());
            
            _current = _defaultSettings.Clone();
            Debug.Log("ctor");

        }
        
        public bool IsInitialized { get; set; }
        
        public  UniTask Initialize()
        {
            IsInitialized = true;
            _current = null;
            Debug.Log("Init");
            return UniTask.CompletedTask;
        }
        

        public Enum Get(SettingId settingId)
        {
            Debug.Log("Get");

            return _current[settingId];
        }

        public void AddOverride(SettingId settingId, Enum value)
        {
            _sessionOverrides[settingId] = value;
        }

        public void ApplyOverrides()
        {
            foreach ((SettingId settingId, Enum value) in _sessionOverrides.Table)
            {
                if (!_current[settingId].Equals(value))
                {
                    ApplySetting(settingId, value);
                }
            }
            
            _sessionOverrides.Clear();
            
            WriteAsync(_current);
        }

        public void ApplySetting(SettingId settingId, Enum value)
        {
            switch (settingId)
            {
                case SettingId.None:
                    break;
                case SettingId.HudSetting:
                    break;
                case SettingId.WindowMode:
                    WindowModeSettingCommand windowModeSettingCommand =
                        (_commands[settingId] as WindowModeSettingCommand);
                    windowModeSettingCommand.Init((SettingWindowMode)value).Apply();
                    break;
                case SettingId.VSync:
                    QualitySettings.vSyncCount = (int)(SettingOnOff)value;
                    break;
                case SettingId.MasterVolume:
                case SettingId.UIVolume:
                case SettingId.SFXVolume:
                case SettingId.BGMVolume:
                    _audioService.SetVolume(settingId.ToString(), (int)(SettingGradation)value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(settingId), settingId, null);
            }

            _current[settingId] = value;
        }

        public static string SavesFolder
        {
            get { return Path.Combine(Application.persistentDataPath, "CthulhuWantsMe"); }
        }

        public static string SaveName
        {
            get { return Path.Combine(Application.persistentDataPath, "settings.save"); }
        }

        public static string SavePath
        {
            get { return Path.Combine(SavesFolder, SaveName); }
        }

        public async void WriteAsync(Settings settings)
        {
            Directory.CreateDirectory(SavesFolder);

            byte[] saveData = SerializationUtility.SerializeValue(settings, DataFormat.JSON);

            try
            {
                await File.WriteAllBytesAsync(SavePath, saveData);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save settings {e}");
                return;
            }
        }

        public async Task<Settings> ReadAsync()
        {
            if (!File.Exists(SavePath))
            {
                return _defaultSettings.Clone();
            }

            byte[] buffer = await File.ReadAllBytesAsync(SavePath);
            Settings settings = SerializationUtility.DeserializeValue<Settings>(buffer, DataFormat.JSON);
            return settings;
        }

      
    }
}