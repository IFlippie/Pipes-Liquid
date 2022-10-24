generated points between the 2 points and lowering or upping them until they are positioned
in a spot where their raycasts hit a specific threshold
this will also count as the check to see if a line between these points can even be created

always showing a preview when trying to create a pipe

calculating the direction again FOR EACH pipe part that has moved because of the curve

apply shader for texture and color preview

Figure out how to use the generated points because the current way has no implementation for
Bezier curves

So either continue with the points and use them or find a way to implement the bezier curve

stop thinking that you have to do a forward check from point a to B, its not necessary
