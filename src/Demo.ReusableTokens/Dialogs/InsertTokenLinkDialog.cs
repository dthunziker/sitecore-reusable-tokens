using System;
using Demo.ReusableTokens.Config;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Shell.Controls.RichTextEditor.InsertLink;
using Sitecore.Web;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.WebControls;

namespace Demo.ReusableTokens.Dialogs
{
    public class InsertTokenLinkForm : InsertLinkForm
    {
        protected TreeviewEx TokenTreeview;

        protected DataContext TokenDataContext;

        protected Tab TokenTab;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            TokenDataContext.Root = TokenConfig.Root;
            TokenDataContext.GetFromQueryString();

            if (!String.IsNullOrEmpty(WebUtil.GetQueryString("databasename")))
            {
                TokenDataContext.Parameters = "databasename=" + WebUtil.GetQueryString("databasename");
            }
        }

        protected override void OnOK(object sender, EventArgs args)
        {
            if (Tabs.Active == 2)
            {
                Item selectionItem = TokenTreeview.GetSelectionItem();
                string displayName = selectionItem.DisplayName;

                LinkUrlOptions options = new LinkUrlOptions();
                string dynamicLink = LinkManager.GetDynamicUrl(selectionItem, options);

                string response = String.Format(
                    "scClose({0},{1})",
                    StringUtil.EscapeJavascriptString(dynamicLink),
                    StringUtil.EscapeJavascriptString(displayName));

                SheerResponse.Eval(response);
            }
            else
            {
                base.OnOK(sender, args);
            }
        }
    }
}
