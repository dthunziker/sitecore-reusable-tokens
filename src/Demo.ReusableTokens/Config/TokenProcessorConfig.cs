using System.Collections.Generic;
using Sitecore.Data;

namespace Demo.ReusableTokens.Config
{
    public sealed class TokenProcessorConfigs
    {
        public string Type { get; set; }

        public IEnumerable<ID> TokenTemplates { get; set; }
    }
}
