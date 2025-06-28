using Foundation.SerializationWarnings.Services;
using Sitecore.Pipelines.GetContentEditorWarnings;
using static Sitecore.Pipelines.GetContentEditorWarnings.GetContentEditorWarningsArgs;

namespace Foundation.SerializationWarnings.Pipelines
{
    public class SerializedItemWarningProcessor
    {
        public void Process(GetContentEditorWarningsArgs args)
        {
            if (args?.Item == null)
                return;

            SerializedItemChecker sic = new SerializedItemChecker();
            var (isSerialized, moduleFile) = sic.IsItemSerialized(args.Item);
            if (isSerialized)
            {
                args.Warnings.Add(new ContentEditorWarning
                {
                    Title = "SCS Item Notice",
                    Text = $"This item is tracked in <b>{moduleFile}</b>. Ensure changes are committed to source control."
                });
            }
        }
    }
}