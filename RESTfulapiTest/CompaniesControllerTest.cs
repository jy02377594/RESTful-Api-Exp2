using RESTful_Api_Exp2.Entities;
using RESTful_Api_Exp2.Models;
using RESTful_Api_Exp2.XunitTest;
using System;
using Xunit;

namespace RESTfulapiTest
{
    public class CalculatorTest
    {
        [Fact]
        public void ShouldAdd()
        {
            // Arrange
            var sut = new DemoXUnitTest();//system under test

            //Act
            var result = sut.Add(2, 3);

            //Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void HaveCorrectEmployeeName()
        {
            var employee = new Employee
            {
                FirstName = "Xiaopeng",
                LastName = "Luo"
            };

            var fullname = employee.LastName + employee.FirstName;

            Assert.Equal("LuoXiaopeng", fullname);
        }
    }
}
