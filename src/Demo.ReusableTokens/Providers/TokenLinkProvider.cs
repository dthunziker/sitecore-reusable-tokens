using System;
using System.Linq;
using Demo.ReusableTokens.Config;
using Demo.ReusableTokens.Processors;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Reflection;

namespace Demo.ReusableTokens.Providers
{
    public sealed class TokenLinkProvider : LinkProvider
    {
        private const string TOKEN_IDENTIFIER = "_token";

        /// <summary>
        /// Detect if the item is a known token type and, if so, append the token identifyer
        /// </summary>
        public override string GetDynamicUrl(Item item, LinkUrlOptions options)
        {
            string dynamicUrl = base.GetDynamicUrl(item, options);

            if (TokenConfig.Processors.Any(x => x.TokenTemplates.Contains(item.TemplateID)))
            {
                dynamicUrl += TOKEN_IDENTIFIER;
            }

            return dynamicUrl;
        }

        public override string ExpandDynamicLinks(string text, bool resolveSites)
        {
            // Walk through the text expanding token links along the way
            int startIndex = text.IndexOf(TOKEN_IDENTIFIER, StringComparison.Ordinal);
            while (startIndex >= 0)
            {
                string result = String.Empty;

                int startOfLink = text.Substring(0, startIndex).LastIndexOf("<a href=\"~/link.aspx?", StringComparison.Ordinal);
                int endOfLink = text.IndexOf("</a>", startIndex, StringComparison.Ordinal) + 4;

                string linkText = text.Substring(startOfLink, endOfLink - startOfLink);
                DynamicLink dynamicLink = DynamicLink.Parse(linkText);

                Item tokenItem = Sitecore.Context.Database.GetItem(
                       dynamicLink.ItemId,
                       dynamicLink.Language ?? Sitecore.Context.Language);

                TokenProcessorConfig processorConfig = TokenConfig.Processors
                    .FirstOrDefault(x => x.TokenTemplates.Contains(tokenItem.TemplateID));

                if (processorConfig != null)
                {
                    BaseTokenProcessor processor = (BaseTokenProcessor)ReflectionUtil.CreateObject(processorConfig.Type);
                    if (processor != null)
                    {
                        result = processor.Process(tokenItem);
                    }
                    else
                    {
                        Log.Warn("No implementation found for token processor: " + processorConfig.Type, this);
                    }
                }
                else
                {
                    Log.Warn("No processor found for token: " + tokenItem.ID, this);
                }

                // Replace token and step forward
                text = text.Replace(linkText, result);
                startIndex = text.IndexOf(TOKEN_IDENTIFIER, StringComparison.Ordinal);
            }

            return base.ExpandDynamicLinks(text, resolveSites);
        }
    }
}
