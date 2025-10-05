using UnityEngine;

public class BakerGreeting : IAnimatedSequence
{
    void IAnimatedSequence.StartAnimation(GameObject obj)
    {
        var animator = obj.GetComponent<Animator>();
        animator.SetFloat("LookX", 0);
        animator.SetFloat("LookY", -1);
        animator.SetFloat("Speed", 3);
    }

    void IAnimatedSequence.StopAnimation(GameObject obj)
    {
        var animator = obj.GetComponent<Animator>();
        animator.SetFloat("LookX", 1);
        animator.SetFloat("LookY", 0);
        animator.SetFloat("Speed", 0);
    }
}
