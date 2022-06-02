
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
        BlackBox = 0,
        Circuits = 1,
        Labyrinth = 2,
        QueueBits = 3,
        Qupcakes = 4
    }

    public enum LoginStatus
    {
        Success,
        FormatError,
        DatabaseError,
        RetrievalError,
        NonExistentUserError
    }
}