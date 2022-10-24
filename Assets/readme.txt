generated points between the 2 points and lowering or upping them until they are positioned
in a spot where their raycasts hit a specific threshold
this will also count as the check to see if a line between these points can even be created

always showing a preview when trying to create a pipe, so do this when start point is selected

calculating the direction again FOR EACH pipe part that has moved because of the curve

apply shader for texture and color preview

Figure out how to use the generated points because the current way has no implementation for
Bezier curves

So either continue with the points and use them or find a way to implement the bezier curve

stop thinking that you have to do a forward check from point a to B, its not necessary

how do you determine in which direction you want to do checks and when would you say that they are done

so perhaps use vector3 up and down and determine angles at which you cannot place pipes

use vector3 up on endpoint
and make anchor the downwards or forward based on the startingpoint position

so if startingpoint is higher than end point, the anchor will be lowered to the height of the endpoint and if endpoint is higher than startingpoint the anchor will be moved towards the endingpoint in length
this doesnt solve the issue of what if the curve should be curved upwards instead of the usual downwards

based on the anchor we will be able to set it to up, down, left, right and it won't be solely the middle point but we can start the anchor in the middle
