using Moq;
using NUnit.Framework;
using System.Diagnostics;
using FamilyTreeLogic.Enums;
using FamilyTreeLogic.Interfaces;
using FamilyTreeLogic.Models;
using FamilyTreeLogic.Services;

namespace FamilyTreeTests
{
    [TestFixture]
    public class FamilyTreeServiceTests
    {

        private const string _fileName = "familytrees.json";

        private IFamilyTreeService _familyTreeService;

        private Mock<IFileWrapper> _fileWrapperMock;


        [SetUp]
        public void SetUp()
        {
            string json = string.Empty;

            try
            {
                json = File.ReadAllText(_fileName);
            }
            catch (Exception)
            {
                Debug.WriteLine($"It was not possible to read the {_fileName} file!");
                Assert.Fail();
            }

            _fileWrapperMock = new Mock<IFileWrapper>();
            _fileWrapperMock.Setup(x => x.ReadAll(_fileName)).Returns(() => json);

            _familyTreeService = new FamilyTreeService(_fileWrapperMock.Object);
        }

        [Test]
        public void Test_Initialize_NullParam_Exception()
        {
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(
                () => _familyTreeService.Initialize(null));

            Assert.That(ex.Message, Is.EqualTo("Value cannot be null. (Parameter 'request')"));
        }

        [Test]
        public void Test_Initialize_Request_FirstNameIsSet_NoPeopleData()
        {
            string testStr = "test";
            SortParam sortParam = SortParam.Age;

            UserRequest request = new UserRequest()
            {
                FirstName = testStr,
                SortParam = sortParam,
                SearchParam = testStr,
                InputFile = _fileName
            };

            var result = _familyTreeService.Initialize(request);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.People.Count);
            Assert.AreEqual(6, result.PersonNodes.ToList().Count); //6 family tree is setup in the json file

            Assert.AreEqual(testStr, result.UserRequest.FirstName);
            Assert.AreEqual(_fileName, result.UserRequest.InputFile);
            Assert.AreEqual(sortParam, result.UserRequest.SortParam);
        }

        [Test]
        public void Test_Initialize_Request_FirstNameIsUnSet_HasPeopleData()
        {
            string testStr = "test";
            SortParam sortParam = SortParam.Age;

            UserRequest request = new UserRequest()
            {
                FirstName = string.Empty,
                SortParam = sortParam,
                SearchParam = testStr,
                InputFile = _fileName
            };

            var result = _familyTreeService.Initialize(request);

            Assert.IsNotNull(result);
            Assert.AreEqual(31, result.People.Count);              // 31 people is setup in the json file
            Assert.AreEqual(6, result.PersonNodes.ToList().Count); //6 family tree is setup in the json file

            Assert.AreEqual(string.Empty, result.UserRequest.FirstName);
            Assert.AreEqual(_fileName, result.UserRequest.InputFile);
            Assert.AreEqual(sortParam, result.UserRequest.SortParam);
        }
    }
}
