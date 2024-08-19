using Growers.io.Models;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: April 29, 2024
 * Course: Application Development III
 * Description: Data repository for user information related to a given Firebase user.
 */

namespace Growers.io.Enums
{
    /// <summary>
    /// Enum for command type for an <see cref="Actuator"/> command.
    /// </summary>
    public enum CommandType
    {
        Buzzer,
        Servo,
        Led,
        Fan
    }
}
