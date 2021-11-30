namespace Snittlistan.Web.ViewModels;

public class QuillData
{
    public QuillData(string htmlFieldName, string stringFieldName, string placeholder)
    {
        HtmlFieldName = htmlFieldName;
        StringFieldName = stringFieldName;
        Placeholder = placeholder;
    }

    public string HtmlFieldName { get; }

    public string StringFieldName { get; }

    public string Placeholder { get; }
}
