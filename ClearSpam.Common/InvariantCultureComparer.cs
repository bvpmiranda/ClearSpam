using System.Collections.Generic;
using System.Globalization;

namespace ClearSpam.Common
{
    public class InvariantCultureComparer : Comparer<string>
    {
        private readonly CompareInfo compareInfo;

        public InvariantCultureComparer()
        {
            compareInfo = CompareInfo.GetCompareInfo(CultureInfo.InvariantCulture.Name);
        }

        public override int Compare(string x, string y)
        {
            return compareInfo.Compare(x, y, CompareOptions.OrdinalIgnoreCase);
        }
    }
}
