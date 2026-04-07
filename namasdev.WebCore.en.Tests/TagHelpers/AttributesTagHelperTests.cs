using Microsoft.AspNetCore.Razor.TagHelpers;

using namasdev.WebCore.TagHelpers;

namespace namasdev.WebCore.Tests.TagHelpers
{
    public class AttributesTagHelperTests
    {
        // ── Helpers ────────────────────────────────────────────────────────────

        private static TagHelperContext BuildContext()
            => new(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString());

        private static TagHelperOutput BuildOutput(TagHelperAttributeList? existingAttributes = null)
            => new(
                "select",
                existingAttributes ?? new TagHelperAttributeList(),
                (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // ── Process ────────────────────────────────────────────────────────────

        [Fact]
        public void Process_EmptyAspAttributes_AddsNoAttributes()
        {
            var tagHelper = new AttributesTagHelper();
            var output = BuildOutput();

            tagHelper.Process(BuildContext(), output);

            Assert.Empty(output.Attributes);
        }

        [Fact]
        public void Process_SingleAttribute_IsAddedToOutput()
        {
            var tagHelper = new AttributesTagHelper
            {
                AspAttributes = new Dictionary<string, string>
                {
                    ["data-placeholder"] = "Select an option"
                }
            };
            var output = BuildOutput();

            tagHelper.Process(BuildContext(), output);

            var attr = Assert.Single(output.Attributes);
            Assert.Equal("data-placeholder", attr.Name);
            Assert.Equal("Select an option", attr.Value);
        }

        [Fact]
        public void Process_MultipleAttributes_AllAddedToOutput()
        {
            var tagHelper = new AttributesTagHelper
            {
                AspAttributes = new Dictionary<string, string>
                {
                    ["data-url"] = "/api/items",
                    ["data-min-length"] = "2",
                    ["disabled"] = "disabled"
                }
            };
            var output = BuildOutput();

            tagHelper.Process(BuildContext(), output);

            Assert.Equal(3, output.Attributes.Count);
            Assert.Equal("/api/items", output.Attributes["data-url"].Value);
            Assert.Equal("2", output.Attributes["data-min-length"].Value);
            Assert.Equal("disabled", output.Attributes["disabled"].Value);
        }

        [Fact]
        public void Process_AttributeAlreadyExists_IsOverwritten()
        {
            var existing = new TagHelperAttributeList
            {
                { "class", "form-select" }
            };
            var tagHelper = new AttributesTagHelper
            {
                AspAttributes = new Dictionary<string, string>
                {
                    ["class"] = "form-select is-invalid"
                }
            };
            var output = BuildOutput(existing);

            tagHelper.Process(BuildContext(), output);

            var attr = Assert.Single(output.Attributes);
            Assert.Equal("class", attr.Name);
            Assert.Equal("form-select is-invalid", attr.Value);
        }

        [Fact]
        public void Process_PreservesExistingUnrelatedAttributes()
        {
            var existing = new TagHelperAttributeList
            {
                { "id", "my-select" }
            };
            var tagHelper = new AttributesTagHelper
            {
                AspAttributes = new Dictionary<string, string>
                {
                    ["data-url"] = "/api/items"
                }
            };
            var output = BuildOutput(existing);

            tagHelper.Process(BuildContext(), output);

            Assert.Equal(2, output.Attributes.Count);
            Assert.Equal("my-select", output.Attributes["id"].Value);
            Assert.Equal("/api/items", output.Attributes["data-url"].Value);
        }

        // ── Default state ──────────────────────────────────────────────────────

        [Fact]
        public void AspAttributes_DefaultsToEmptyDictionary()
        {
            var tagHelper = new AttributesTagHelper();
            Assert.NotNull(tagHelper.AspAttributes);
            Assert.Empty(tagHelper.AspAttributes);
        }
    }
}
