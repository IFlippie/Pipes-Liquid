BEZIER CURVE IMPLEMENTATION:

how do you determine in which direction you want to do checks and when would you say that they are done?

so if startingpoint is higher than end point, the anchor will be lowered to the height of the endpoint and if endpoint is higher than startingpoint the anchor will be moved towards the endingpoint in length
this doesnt solve the issue of what if the curve should be curved upwards instead of the usual downwards

based on the anchor we will be able to set it to up, down, left, right and it won't be solely the middle point but we can start the anchor in the middle

add color for when it's possible or not possible to place said pipe, do this by setting a angle and length limit, (make mesh for starting and end points)
this helps with the anchor placement for example angle from startingpoint(forward) could decide at which angle the anchor should go in the opposite direction

CONNECTION IMPLEMENTATION:

should be easy as just spawning a new starting point at the end of the generated pipe

WRITE DOCUMENT
CODE CLEANUP
ONE BUG RELATED TO THE VERTICES ROTATION WHEN GOING STRAIGHT IN THE X DIRECTION

EXTRA FOR ART POINTS:

add texture rotation
double the vertices to get flat shading by making every triangle with unique vertices
add flowmap shader to simulate liquid inside pipe