using System.Collections.Generic;
using System.Globalization;

namespace ClearSpam.Common
{
    public class InvariantCultureComparer : Comparer<string>
    {
        private readonly CompareInfo _compareInfo;

        public InvariantCultureComparer()
        {
            _compareInfo = CompareInfo.GetCompareInfo(CultureInfo.InvariantCulture.Name);
        }

        public override int Compare(string x, string y)
        {
            return _compareInfo.Compare(x, y, CompareOptions.OrdinalIgnoreCase);
        }
    }
}
