using Sitecore.Data.Items;

namespace Demo.ReusableTokens.Processors
{
    public abstract class BaseTokenProcessor
    {
        public abstract string Process(Item tokenItem);
    }
}
