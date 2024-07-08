using UnityEngine;

public class ConsoleAnimationPlayingCheck : MonoBehaviour
{
    [HideInInspector] public bool animationPlaying;
    
    //Checks if animation is played to stop sparkle spelunk from playing while in this animation
    //This is because it works a lot with transforms and while this animation plays, the transforms are different than expected
    
    private void AnimationPlaying()
    {
        animationPlaying = true;
    }
    
    private void AnimationNotPlaying()
    {
        animationPlaying = false;
    }
}
