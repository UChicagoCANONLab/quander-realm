
namespace Wrapper
{
    /// Dialogue System ///
    
    public enum Character
    {
        Molly,
        Tangle,
        Byte,
        Wolfie,
        Batty,
        TwinA,
        TwinB,
        Chef,
        None
    };

    public enum Expression
    {
        Default,
        Positive,
        Negative,
        Confused
    };

    /// <summary>
    /// Translates Step.Forward to +1 and Step.Backward to -1
    /// </summary>
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
    };

    public enum LoginStatus
    {
        Success,
        FormatError,
        DatabaseError,
        RetrievalError,
        NonExistentUserError
    };

    /// Rewards ///
    
    public enum CardType
    {
        Character,
        ComputerPart,
        Visual,
        Concept
    };

    public enum DisplayType
    {
        Featured,
        InJournal
    }
}