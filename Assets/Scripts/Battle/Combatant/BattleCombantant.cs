using System.Collections;
using UnityEngine;

public class BattleCombatant : MonoBehaviour
{
    private Animator animator;
    private CombatantInstance data;

    public void Setup(CombatantInstance instance)
    {
        data = instance;
        if (instance.Data.modelPrefab == null) return; // safe for testing
        var go = Instantiate(instance.Data.modelPrefab, transform);
        print(instance.Data.modelPrefab);
        animator = go.GetComponent<Animator>();
        if (animator != null && instance.Data.animatorController != null)
            animator.runtimeAnimatorController = instance.Data.animatorController;
    }

    public IEnumerator PlayAttack(string triggerName)
    {
        animator.SetTrigger(triggerName);
        yield return new WaitForSeconds(0.8f);
    }

    public IEnumerator PlayFaint()
    {
        animator.SetTrigger("Faint");
        yield return new WaitForSeconds(1f);
    }
}