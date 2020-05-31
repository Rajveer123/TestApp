using System;
namespace XFTest.Enums
{
    /// <summary>
    /// VisitStates Enum for car fit visit states
    /// </summary>
    public enum VisitStates
    {
        ToDo = 1,
        InProgress = 2,
        Done = 3,
        Rejected = 4

    }
    /// <summary>
    /// Below Class method will convert from strings to the Input enumeration type directly
    /// So we can use VisitStates enum in GetBackgroundThemColor utility mehtod 
    /// </summary>
    public class EnumHelper
    {
        public static T Parse<T>(string input)
        {
            return (T)Enum.Parse(typeof(T), input, true);
        }
    }


}
