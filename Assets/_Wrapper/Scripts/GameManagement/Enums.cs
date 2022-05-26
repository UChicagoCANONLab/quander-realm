
namespace Wrapper
{
    /// Dialogue System ///
    public enum Character
    {
        None,
        Char1,
        Char2,
        Char3,
        Char4,
        Char5,
        Char6
    }

    public enum Expression
    {
        Default,
        Positive,
        Negative,
        Confused
    }

    public enum Step
    {
        Forward = 1,
        Backward = -1
    };

    /// Save System ///
    public enum Game
    {
        BlackBox,
        Circuits,
        Labyrinth,
        QueueBits,
        Qupcakes
    }
}