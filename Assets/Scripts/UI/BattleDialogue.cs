using System.Collections;
using TMPro;
using UnityEngine;

public class BattleDialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject actionMenu;
    [SerializeField] private float typeSpeed = 0.04f;

    void Awake()
    {
        if (dialogueBox == null) Debug.LogError("dialogueBox not assigned on BattleDialogue!");
        if (actionMenu == null) Debug.LogError("actionMenu not assigned on BattleDialogue!");
        if (dialogueText == null) Debug.LogError("dialogueText not assigned on BattleDialogue!");
        dialogueBox.SetActive(false);
        actionMenu.SetActive(false);
    }
    public IEnumerator TypeLine(string line)
    {
        dialogueBox.SetActive(true);
        actionMenu.SetActive(false);
        dialogueText.text = "";
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
        yield return new WaitForSeconds(0.8f);
    }
    public void ShowActionMenu()
    {
        actionMenu.SetActive(true);
    }
    public void HideAll()
    {
        dialogueBox.SetActive(false);
        actionMenu.SetActive(false);
    }
}