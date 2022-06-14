using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Prov.App.Extensions
{
    [HtmlTargetElement("a", Attributes = "supress-by-action")]
    public class ApagaByActionTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ApagaByActionTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        [HtmlAttributeName("supress-by-action")]
        public string actionName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (output == null) throw new ArgumentNullException(nameof(output));

            var action = _contextAccessor.HttpContext.GetRouteValue("action").ToString();

            if (actionName.Contains(action)) return;

            output.SuppressOutput();
        }
    }
}
