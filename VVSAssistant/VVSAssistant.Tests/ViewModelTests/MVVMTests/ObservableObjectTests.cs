using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using VVSAssistant.ViewModels;

namespace VVSAssistant.Tests.ViewModelTests.MVVMTests
{
    [TestFixture]
    public class ObservableObjectTests
    {
        private StubObservableObject _stub = null;

        // Factory er anbefalet i stedet for Setup
        [SetUp]
        public void Setup()
        {
            _stub = new StubObservableObject();
        }

        [Test]
        public void OnPropertyChanged_PropertyChangedEventRaised_True()
        {
            bool raised = false;

            _stub.PropertyChanged += (sender, e) =>
            {
                Assert.IsTrue(e.PropertyName.Equals("TestOnPropertyChanged"));
                raised = true;
            };
            _stub.TestOnPropertyChanged = "Value";

            Assert.IsTrue(raised);
        }

        [Test]
        [TestCase(null, "Value", true)]
        [TestCase("Value", "Value", false)]
        [TestCase("Not value", "Value", true)]
        public void Setproperty_PropertyChangedEventRaised_True(string formerValue, string testValue, bool expected)
        {
            bool raised = false;
            _stub.TestSetProperty = formerValue;
            _stub.PropertyChanged += (sender, e) =>
            {
                Assert.IsTrue(e.PropertyName.Equals("TestSetProperty"));
                raised = true;
            };
            _stub.TestSetProperty = testValue;

            Assert.AreEqual(raised, expected);
        }

        [TearDown]
        public void TearDown()
        {
            _stub = null;
        }
    }

    class StubObservableObject : ObservableObject
    {
        private string _testOnpropertyChanged;
        public string TestOnPropertyChanged
        {
            get
            {
                return _testOnpropertyChanged;
            }
            set
            {
                _testOnpropertyChanged = value;
                OnPropertyChanged();
            }
        }
        private string _testSetProperty;
        public string TestSetProperty
        {
            get { return _testSetProperty; }
            set
            {
                SetProperty(ref _testSetProperty, value);
            }
        }
    }
}
