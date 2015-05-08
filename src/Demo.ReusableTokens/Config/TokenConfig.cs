using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Xml;

namespace Demo.ReusableTokens.Config
{
    public static class TokenConfig
    {
        public static string Root
        {
            get { return Factory.GetConfigNode("reusableTokens/tokenRoot").InnerText; }
        }

        public static IEnumerable<TokenProcessorConfig> Processors
        {
            get
            {
                foreach (XmlNode node in Factory.GetConfigNodes("reusableTokens/tokenProcessors/*"))
                {
                    yield return new TokenProcessorConfig
                    {
                        Type = XmlUtil.GetAttribute("type", node),
                        TokenTemplates = XmlUtil.GetChildNodes("tokenTemplateIds", node).Select(x => ID.Parse(x.InnerText))
                    };
                }
            }
        }
    }

    public sealed class TokenProcessorConfig
    {
        public string Type { get; set; }

        public IEnumerable<ID> TokenTemplates { get; set; }
    }
}
