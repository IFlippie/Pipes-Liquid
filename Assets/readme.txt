first step will be to determine a starting point which will be the first vector3 somewhere

step two will be looking at something with the game camera and get the position and angle
of that raycasted point
https://answers.unity.com/questions/377506/how-i-can-know-surface-angle.html

step three will be to generate additional points between these 2 points this can be done by
generating points between the 2 points and then lowering or upping them until they are positioned
in a spot where their raycasts hit a specific threshold
this will also count as the check to see if a line between these points can even be created