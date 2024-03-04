# Starbase Controller Design 
- The starbase controller will be driven by the state of the music system.
- Properties like BPM, Rhythmic Pulse, and Intensity will be exposed by the music system. 
- The BPM is the number of beats per minute of music. (The BPM will often vary. I don't plan on writing simple loops with a fixed BPM. I will write the music in segments that are then "stitched" together at runtime. This will allow for a great deal of variety and interest.) 
- The Rhythmic Pulse is determined by a musical concept called meter. Meter takes the BPM and applies a relative strength to each musical beat. There are two varieties of beat strength: Strong and Weak. It is very common for a meter to apply a pattern of beat strength that is one strong beat followed by some number of weak beats. For example, You may have a pattern like: Strong -> Weak -> Strong -> Weak. There are many variations of such patterns, though, the only important thing to understand is that the strong beats are directly mapped to the Rhythmic Pulses. These pulses will function as the "heartbeat" of our starbase.
- The Intensity parameter is determined by the music system and its interaction with the current game state. This will be an enum.

# Details
For now we can start an Intensity parameter that has three states. The first is associated with soft music and exploration and the second is associated with combat. There will be a third default state of the music system where the music is silent. 

The BPM and Rhythmic pulse parameters can control a huge variety of things like: The players abilities or state, The enemies abilities or state, and the starbase's abilities or state.

The Intensity could control the number of enemies, or the strength of the enemeies, or whether or not there are elite enemies. 

Varying the meter and BPM for each musical segment will have a great impact on how often the Rhythmic Pulse (heartbeat of the starbase) is triggered and that will give the starbase a very organic vibe/aesthetic. 
