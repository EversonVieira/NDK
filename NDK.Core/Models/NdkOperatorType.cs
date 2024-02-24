namespace NDK.Core.Models
{
    public enum NDKOperatorType
    {

        EQUAL = 100,
        NOTEQUAL = 101,
        ISNULL = 102,
        ISNOTNULL = 103,

        LESSTHANOREQUAL = 104,
        LESSTHAN = 105,

        GREATERTHAN = 106,
        GREATERTHANOREQUAL = 107,

        BETWEEN = 201,

        STARTSWITH = 202,
        ENDSWITH = 203,
        CONTAINS = 204,

        IN = 301,
        NOTIN = 302,
    }
}
