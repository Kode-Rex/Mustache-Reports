namespace Mustache.Reports.Boundary.Rendering.Word
{
    public interface IWordTemplaterGateway
    {
        IWordFileOutput Render(IWordFileInput template, object data);
    }
}