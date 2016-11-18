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
        public void Setproperty_PropertyChangedEventRaised_True()
        {
            bool raised = false;

            _stub.PropertyChanged += (sender, e) =>
            {
                Assert.IsTrue(e.PropertyName.Equals("TestSetProperty"));
                raised = true;
            };
            _stub.TestSetProperty = "Some value";

            Assert.IsTrue(raised);
        }
        [Test]
        public void Setproperty_PropertyChangedEventRaised_False()
        {
            bool raised = false;
            string value = "Value";
            _stub.TestSetProperty = value;

            _stub.PropertyChanged += (sender, e) =>
            {
                Assert.IsTrue(e.PropertyName.Equals("TestSetProperty"));
                raised = true;
            };
            _stub.TestSetProperty = "Value";

            Assert.IsFalse(raised);
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
