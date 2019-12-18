using UnityEngine;

public sealed class AnimationManager : MonoBehaviour
{
    private Animation _anim;
    private int _animCount = -1;

    private void Awake()
    {
        _anim = animation;
    }

    public int GetClipCount()
    {
        if (_animCount > -1) return _animCount;

        var i = 0;
        foreach (AnimationState animState in _anim) i++;

        _animCount = i;

        return _animCount;
    }

    public AnimationClip this[int index]
    {
        get
        {
            var i = 0;

            foreach (AnimationState animState in _anim)
            {
                if (i == index) return animState.clip;
                i++;
            }

            return null;
        }
    }

    public AnimationClip this[string clipName]
    {
        get
        {
            var i = 0;

            foreach (AnimationState animState in _anim)
            {
                if (animState.name.ToUpper().Equals(clipName.ToUpper())) return animState.clip;
                i++;
            }

            return null;
        }
    }
}