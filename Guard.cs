public class Guard
{
    static public bool Only(bool value)
    {
        return !value;
    }

    static public bool From(bool value)
    {
        return value;
    }
}