using System.Collections.Generic;

namespace ReportUtils
{
    public class AkaFileData : FileData
    {
        public HashSet<string> ContainedAkaLinks { get; } = new HashSet<string>();
    }
}
