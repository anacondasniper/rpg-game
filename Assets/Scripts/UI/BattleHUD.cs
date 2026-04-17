using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider hpBar;
    public void SetData(CombatantInstance c)
    {
        nameText.text = c.Name;
        levelText.text = $"Lv. {c.Level}";
        hpBar.value = (float)c.CurrentHp / c.MaxHp;
    }

    public IEnumerator AnimateHP(CombatantInstance c)
    {
        float target = (float)c.CurrentHp / c.MaxHp;
        float t = 0f;
        float start = hpBar.value;
        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            hpBar.value = Mathf.Lerp(start, target, t);
            yield return null;
        }
        hpBar.value = target;
    }
}