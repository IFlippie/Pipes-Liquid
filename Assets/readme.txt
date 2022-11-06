BEZIER CURVE IMPLEMENTATION:

so if startingpoint is higher than end point, the anchor will be lowered to the height of the endpoint and if endpoint is higher than startingpoint the anchor will be moved towards the endingpoint in length
this doesnt solve the issue of what if the curve should be curved upwards instead of the usual downwards

based on the anchor we will be able to set it to up, down, left, right and it won't be solely the middle point but we can start the anchor in the middle

add color for when it's possible or not possible to place said pipe, do this by setting a angle and length limit
this helps with the anchor placement for example angle from startingpoint(forward) could decide at which angle the anchor should go in the opposite direction

for left and right the check would be to see if from the startingpoint the angle is and if it succeeds that then move the anchor based on that angle

if the angle increases increase the forward upto 90 at which point it should be equal 

JUST IMPLEMENTING THE ANGLE RELATED ANCHOR IS THE FINAL TASK

FULL FOCUS ON JUST THE EXTENDER AND BEZIER CURVE
HAVING THE BEZIER CURVE BEND SLIGHTLY SO IT CAN SOMEWHAT GO AROUND CORNERS
have the extender work with the generatepipe because now it just works with preview

WRITE DOCUMENT
CODE CLEANUP
ONE BUG RELATED TO THE VERTICES ROTATION WHEN GOING STRAIGHT IN THE X DIRECTION(gimbal lock) FIXED!!!!!!
maybe fixable by having transform.right replaced with forward or up but would require vertices code changes
or maybe use forward with quaternions(set)lookrotation

EXTRA FOR ART POINTS:

add texture rotation
double the vertices to get flat shading by making every triangle with unique vertices
add flowmap shader to simulate liquid inside pipe