using System;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using VVSAssistant.Common.ViewModels;

namespace VVSAssistant.Tests.ViewModelTests.MVVMTests
{
    [TestFixture]
    public class ViewModelBaseTests
    {
        [Test]
        [TestCase(null, "Required")]
        [TestCase("Value", null)]
        public void ViewModelBaseIndexer_ValidateRequiredProperty(string value, string expectedError)
        {
            var vmstub = new ViewModelStub {RequiredPropertyTest = value};
            Assert.AreEqual(vmstub["RequiredPropertyTest"], expectedError);
        }
        [Test]
        [TestCase(0, "Range")]
        [TestCase(11, "Range")]
        [TestCase(5, null)]
        public void ViewModelBaseIndexer_ValidateRequiredProperty(int value, string expectedError)
        {
            var vmstub = new ViewModelStub {RangeTest = value};
            Assert.AreEqual(vmstub["RangeTest"], expectedError);
        }
    }

    internal class ViewModelStub : ViewModelBase
    {
        [Required(ErrorMessage ="Required")]
        public string RequiredPropertyTest { get; set; }

        [System.ComponentModel.DataAnnotations.Range(1,10, ErrorMessage="Range")]
        public int RangeTest { get; set; }

        public override void LoadDataFromDatabase()
        {
            throw new NotImplementedException();
        }
    }
}
