using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Project
{
    public class LanguageController : MonoBehaviour
    {
        [SerializeField] private Locale en;
        [SerializeField] private Locale tv;
        [SerializeField] private Locale kz;
        [SerializeField] private Locale sn;
        [SerializeField] private UnityEvent OnEn;
        [SerializeField] private UnityEvent OnMn;
        [SerializeField] private UnityEvent OnTv;
        [SerializeField] private UnityEvent OnKz;
        [SerializeField] private UnityEvent OnSn;

        public void OnEnable()
        {
            if (LocalizationSettings.SelectedLocale == en)
            {
                OnEn.Invoke();
            }
            else if (LocalizationSettings.SelectedLocale == tv)
            {
                OnTv.Invoke();
            }
            else if (LocalizationSettings.SelectedLocale == kz)
            {
                OnKz.Invoke();
            }
            else if (LocalizationSettings.SelectedLocale == sn)
            {
                OnSn.Invoke();
            }
            else
            {
                OnMn.Invoke();
            }
        }

        public void SetLocale(Locale locale)
        {
            LocalizationSettings.SelectedLocale = locale;
        }
    }
}
