using System.Collections.Generic;
using TARE.Engine.Parser;

namespace TARE.Engine.Flags
{
    internal interface IFlagCondition
    {
        bool IsMatch(IEnumerable<Word> input);
    }
}
