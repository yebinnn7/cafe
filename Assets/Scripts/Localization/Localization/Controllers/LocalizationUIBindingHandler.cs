using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using TMPro;
using Cafe.Localization.Definitions;

namespace Cafe.Localization.Controllers
{
    /// <summary>
    /// This class is a bridge between the UI component and the Localization Controller.
    /// </summary>
    public class LocalizationUIBindingHandler : MonoBehaviour
    {
        // Properties
        // Variables
        TMP_Dropdown dropdown;

        // Configurations
        [SerializeField]
        public readonly SystemLanguage[] dropdownOptions = new SystemLanguage[2]
        {
            // The order of this array is the order of the dropdown list.
            // Since the dropdown list's values are set manually in Unity Editor,
            // if there are any changes in locales, you should update below and
            // dropdown list in Unity Editor MANUALLY.
            SystemLanguage.English,
            SystemLanguage.Korean
        };

        // Exports
        public void OnDropdownChanged(int index)
        {
            if (0 <= index && index < dropdownOptions.Length)
            {
                SystemLanguage selectedLanguage = dropdownOptions[index];
                LocalizationController.Instance.SetLocale(selectedLanguage);
            }
            else
            {
                Debug.LogWarning($"Irregular action(undefined behavior): Dropdown index {index} is out of range.");
            }
        }

        // Unity Bindings
        private void Awake()
        {
            dropdown = GetComponent<TMP_Dropdown>();
            // Below action is already configurated in Unity Editor manually.
            // dropdown.onValueChanged.AddListener(OnDropdownChanged);
        }

        private void Start()
        {
            // Below actions must be called after make sure the LocalizationController is initialized.
            InitDropdownValue();
        }

        // Internal Methods
        public void InitDropdownValue()
        {
            SystemLanguage currentLanguage = LocalizationController.Instance.CurrentLocale.ToSystemLanguage();
            int index = dropdownOptions.ToList().IndexOf(currentLanguage);
            dropdown.value = index;
        }
    }
}
