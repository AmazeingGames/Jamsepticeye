using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject animatedObject;
    [SerializeField]
    private IAnimatedSequence sequence;

    void StartAnimation()
    {
        sequence.StartAnimation(animatedObject);
    }

    void StopAnimation()
    {
        sequence.StopAnimation(animatedObject);
    }
}
