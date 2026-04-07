using namasdev.WebCore.ViewModels;

namespace namasdev.WebCore.Tests.ViewModels
{
    public class ViewModelBaseTests
    {
        [Fact]
        public void MessageOk_DefaultsToNull()
        {
            var vm = new ViewModelBase();
            Assert.Null(vm.MessageOk);
        }

        [Fact]
        public void MessageError_DefaultsToNull()
        {
            var vm = new ViewModelBase();
            Assert.Null(vm.MessageError);
        }

        [Fact]
        public void MessageOk_CanBeSet()
        {
            var vm = new ViewModelBase { MessageOk = "Saved successfully." };
            Assert.Equal("Saved successfully.", vm.MessageOk);
        }

        [Fact]
        public void MessageError_CanBeSet()
        {
            var vm = new ViewModelBase { MessageError = "Something went wrong." };
            Assert.Equal("Something went wrong.", vm.MessageError);
        }
    }
}
