namespace DelayedValidation.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    [TestClass]
    public class DelayedValidationTest
    {
        #region Public Methods and Operators

        [TestMethod]
        public void Create_With_Invalid_Age_But_Make_Valid_Before_Read()
        {
            var s = new SomeDomainObject("first", "last", 101);

            s.Age = 25;

            Assert.AreEqual("first", s.FirstName);
            Assert.AreEqual("last", s.LastName);
            Assert.AreEqual(25, s.Age);
        }

        [TestMethod]
        public void Create_With_Invalid_First_Name_But_Make_Valid_Before_Read()
        {
            var s = new SomeDomainObject("", "", 101);

            s.FirstName = "first";
            s.LastName = "last";
            s.Age = 25;

            Assert.AreEqual("first", s.FirstName);
            Assert.AreEqual("last", s.LastName);
            Assert.AreEqual(25, s.Age);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Read_With_Invalid_Age_And_Draft_Mode_Off()
        {
            var s = new SomeDomainObject("first", "last", 101);

            Assert.AreEqual("first", s.FirstName);
            Assert.AreEqual("last", s.LastName);
            Assert.AreEqual(101, s.Age);
        }

        [TestMethod]
        public void Read_With_Invalid_Age_And_Draft_Mode_On()
        {
            var s = new SomeDomainObject("first", "last", 101);

            s.IsDraft = true;
            Assert.AreEqual("first", s.FirstName);
            Assert.AreEqual("last", s.LastName);
            Assert.AreEqual(101, s.Age);
            Assert.AreEqual(1, s.GetValidationErrors().Count);
        }

        [TestMethod]
        public void Read_Without_Changes()
        {
            var s = new SomeDomainObject("first", "last", 25);

            Assert.AreEqual("first", s.FirstName);
            Assert.AreEqual("last", s.LastName);
            Assert.AreEqual(25, s.Age);
        }

        [TestMethod]
        [ExpectedException(typeof(JsonSerializationException))]
        public void Serialize_Invalid_Object()
        {
            var s = new SomeDomainObject("first", "last", 101);

            var j = JsonConvert.SerializeObject(s);
        }

        [TestMethod]
        public void Serialize_Invalid_Draftable_Object()
        {
            var s = new SomeDomainObject("first", "last", 101);

            s.IsDraft = true;

            var j = JsonConvert.SerializeObject(s);
        }

        [TestMethod]
        public void DeSerialize_Invalid_Draftable_Object()
        {
            var s =
                JsonConvert.DeserializeObject<SomeDomainObject>(
                    "{\"Age\":101,\"FirstName\":\"first\",\"IsDraft\":true,\"LastName\":\"last\"}");

            Assert.AreEqual("first", s.FirstName);
            Assert.AreEqual("last", s.LastName);
            Assert.AreEqual(101, s.Age);
            Assert.AreEqual(1, s.GetValidationErrors().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void DeSerialize_Invalid__Object()
        {
            var s =
                JsonConvert.DeserializeObject<SomeDomainObject>(
                    "{\"Age\":101,\"FirstName\":\"first\",\"IsDraft\":false,\"LastName\":\"last\"}");

            Assert.AreEqual("first", s.FirstName);
            Assert.AreEqual("last", s.LastName);
            Assert.AreEqual(101, s.Age);
            Assert.AreEqual(1, s.GetValidationErrors().Count);
        }

        #endregion
    }
}