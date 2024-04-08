using System.Collections.Generic;

namespace NfiCodeAssignment.Models
{
    public class RobotsInputDTO
    {
        public RobotsInputDTO()
        {
            RobotInformations = new List<RobotInformationDTO>();
            Movements = new List<int>();
        }

        public int RobotsCount { get; set; }

        public List<int> Movements { get; set; }

        public List<RobotInformationDTO> RobotInformations { get; set; }
    }
}
