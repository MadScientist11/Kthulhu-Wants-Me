using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
                    break;
                case SettingWindowMode.Windowed:
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.Windowed);
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
    public class SettingsService
    {
        private readonly Dictionary<SettingId, ISettingCommand> _commands = new();

        private readonly Settings _defaultSettings = new();
        private readonly Settings _current = new();
        
        private readonly Settings _sessionOverrides = new();

        public SettingsService()
        {
            _defaultSettings[SettingId.WindowMode] = SettingWindowMode.Fullscreen;
            _defaultSettings[SettingId.HudSetting] = SettingOnOff.On;

            _current = _defaultSettings.Clone();
            
            _commands.Add(SettingId.WindowMode, new WindowModeSettingCommand());
        }

        public Enum Get(SettingId settingId)
        {
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
                    WindowModeSettingCommand windowModeSettingCommand = (_commands[settingId] as WindowModeSettingCommand);
                    windowModeSettingCommand.Init((SettingWindowMode)value).Apply();
                    break;
                case SettingId.VSync:
                    QualitySettings.vSyncCount = (int)(SettingOnOff)value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(settingId), settingId, null);
            }
            
            _current[settingId] = value;
        }
    }
}