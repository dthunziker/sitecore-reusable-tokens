using Sitecore.Data.Items;

namespace Demo.ReusableTokens.Processors
{
    public class ContentFieldTokenProcessor : BaseTokenProcessor
    {
        public override string Process(Item token)
        {
            // Assign a token replacement value
            // NOTE: Here's where you can perform complex logic to compute the replacement value.
            string result = token["Content"];

            // If the replacement content contains a nested link to itself, we will have a recusive loop.
            // To prevent this from occuring, we clear out the value.
            return result.Contains(token.ID.ToShortID().ToString()) ? null : result;
        }
    }
}
