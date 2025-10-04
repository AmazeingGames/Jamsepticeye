using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class InkExternalFunctions
{
    // Plays a little emote over the head of the speaker in the game view
    public void BindEmoteFunction(Story story)
    {
        story.BindExternalFunction("playEmote", (string emoteName) 
            => PlayEmote(emoteName));
    }

    public void UnbindEmote(Story story) 
    {
        story.UnbindExternalFunction("playEmote");
    }

    public void PlayEmote(string emoteName)
    {
        // Play an emote over the head of the speaker
        Debug.Log($"Play emote: {emoteName}");
    }
    
}
