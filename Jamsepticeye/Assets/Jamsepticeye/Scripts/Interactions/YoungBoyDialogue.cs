using UnityEngine;

public class YoungBoyDialogue : QuestInteraction
{
    // The game is in the wrong state, process different dialogues based on what state is wrong
    protected virtual void DialogueWrongState() { 
    }

    // The game is in the right state, process successful dialogue
    protected virtual void DialogueRightState() { 
    }
}
