##  REPOSITORY DETAILS

* A set of 1 or more robots. Each robot has an origin point, which is an (x, y) coordinate.
* A set of 1 or more time steps. In each time step, each of the robots will visit a specific (x, y) position.
For example, a schedule with 3 robots and 2 time steps could look as follows:

![image](https://github.com/hafizeucmak/rectangle-collision-detection/assets/23457562/72019301-c124-4d1c-a7b9-57d5f7493a19)

A schedule is valid if there are no collision between any of the robots for all time steps. A collision is defined
as follows:

* Each robot occupies a rectangular area. The origin of the robot and the point that is being visited by
the robot are the two corners of this rectangle.

* When the occupied areas of two robots overlap or touch in an edge or point, then there is a collision.
See (Tip 1) for a formula that you can use to determine whether two rectangles overlap or touch.

![image](https://github.com/hafizeucmak/rectangle-collision-detection/assets/23457562/0fc45d49-1da1-4ea8-9de3-ff2231ba373a)
