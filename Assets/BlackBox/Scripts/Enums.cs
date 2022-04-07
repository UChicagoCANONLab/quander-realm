namespace BlackBox
{
    public enum Dir
    {
        None,
        Left,
        Bot,
        Right,
        Top
    };

    public enum Marker
    {
        Miss,
        Hit,
        Reflect,
        Detour
    };

    public enum CellType
    {
        Node,
        EdgeNode,
        Nav
    };

    public enum GridSize
    {
        Small,
        Medium,
        Large
    }
}
