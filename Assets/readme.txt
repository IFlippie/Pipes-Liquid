
always showing a preview when trying to create a pipe, so do this when start point is selected and implement this by having a seperate preview mesh

bezier curve implementation:

how do you determine in which direction you want to do checks and when would you say that they are done?

use vector3 up on endpoint
and make anchor the downwards or forward based on the startingpoint position

so if startingpoint is higher than end point, the anchor will be lowered to the height of the endpoint and if endpoint is higher than startingpoint the anchor will be moved towards the endingpoint in length
this doesnt solve the issue of what if the curve should be curved upwards instead of the usual downwards

based on the anchor we will be able to set it to up, down, left, right and it won't be solely the middle point but we can start the anchor in the middle

calculating the direction again FOR EACH pipe part that has moved because of the curve

EXTRA FOR ART POINTS:

add texture rotation
add color for when it's possible or not possible to place said pipe, do this by setting a angle and length limit
double the vertices to get flat shading by making every triangle with unique vertices
add flowmap shader to simulate liquid inside pipe

