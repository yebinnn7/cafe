using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Cafe.Localization.Definitions;

namespace Cafe.Localization.Controllers
{
    public class LocalizationController : MonoBehaviour
    {
        public static LocalizationController Instance { get; private set; }

        private void Awake()
        {
            _MakeThisUnique();
            InitWithSystemLanguage();
        }

        private void _MakeThisUnique() {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitWithSystemLanguage()
        {
            SystemLanguage systemLanguage = Application.systemLanguage;
            SetLocale(systemLanguage);
        }

        public void SetLocale(SystemLanguage systemLanguage)
        {
            Locale locale;
            try
            {
                locale = systemLanguage.ToLocale();
            }
            catch (KeyNotFoundException _e)
            {
                Debug.LogWarning($"SystemLanguage {systemLanguage} is not supported. Defaulting to English.");
                locale = LocalizationDefaults.DefaultLanguage.ToLocale();
            }
            CurrentLocale = locale;
        }

        [SerializeField]
        private Locale _currentLocale;

        public Locale CurrentLocale
        {
            get => _currentLocale;
            set
            {
                _currentLocale = value;
                LocalizationSettings.SelectedLocale = _currentLocale;
            }
        }
    }
}
