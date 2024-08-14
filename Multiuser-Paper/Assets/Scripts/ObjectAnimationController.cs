using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnimationController : MonoBehaviour
{
    
    [SerializeField]
    private float animationProgress;

    [SerializeField]
    private AnimationClip animationClip;

    private Animator animator;

    private AnimatorOverrideController aoc;

    public void SetAnimProgress(float amount)
    {
        animationProgress = amount;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);

        AnimationClip[] defaultClips = aoc.animationClips;

        List<KeyValuePair<AnimationClip, AnimationClip>> clipOverrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        clipOverrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(defaultClips[0], animationClip));

        aoc.ApplyOverrides(clipOverrides);
        animator.runtimeAnimatorController = aoc;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        animator.SetFloat("AnimationProgress", animationProgress);
    }
}
