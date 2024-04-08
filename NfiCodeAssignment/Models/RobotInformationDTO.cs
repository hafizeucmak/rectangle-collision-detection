using System.Collections.Generic;

namespace NfiCodeAssignment.Models
{
    public class RobotInformationDTO
    {
        public RobotInformationDTO()
        {
            RectangleCoordinates = new List<RectangleCoordinate>();
        }

        public string RobotName { get; set; }

        public Position InitialPosition { get; set; }

        public List<RectangleCoordinate> RectangleCoordinates { get; set; }


        public void CalculateRectangleCoordinates(int nextXPoint, int nextYPoint, int movement)
        {
            var rectangleCoordinate = new RectangleCoordinate
            {
                Movement = movement
            };

            // When the next X position of the rectangle is to the right of the initial X position, XPointLeft is set to the initial X position; otherwise, it equals the next X point.
            rectangleCoordinate.XPointLeft = (nextXPoint > this.InitialPosition.XPoint) ? this.InitialPosition.XPoint : nextXPoint;
            // If the next X position of the rectangle is to the right of the initial X position, XPointRight equals the next X point; otherwise, it is set to the initial X position.
            rectangleCoordinate.XPointRight = (nextXPoint > this.InitialPosition.XPoint) ? nextXPoint : this.InitialPosition.XPoint;
            // When the next Y position of the rectangle is below the initial Y position, YPointBottom equals the initial Y point; otherwise, it is set to the next Y point.
            rectangleCoordinate.YPointBottom = (nextYPoint > this.InitialPosition.YPoint) ? this.InitialPosition.YPoint : nextYPoint;
            // If the next Y position of the rectangle is above the initial Y position, YPointTop equals the next Y point; otherwise, it is set to the initial Y position.
            rectangleCoordinate.YPointTop = (nextYPoint > this.InitialPosition.YPoint) ? nextYPoint : this.InitialPosition.YPoint;

            this.RectangleCoordinates.Add(rectangleCoordinate);
        }
    }

    public class Position
    {
        public Position(int xPoint, int yPoint)
        {
            XPoint = xPoint;
            YPoint = yPoint;
        }

        public int XPoint { get; set; }

        public int YPoint { get; set; }

    }

    public class RectangleCoordinate
    {
        public int Movement { get; set; }

        public int XPointLeft { get; set; }

        public int XPointRight { get; set; }

        public int YPointBottom { get; set; }

        public int YPointTop { get; set; }
    }
}
