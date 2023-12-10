using Application.Services;
using Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace VeeztaUnitTest.MockData
{
    public class AdminServiceTest
    {
        private readonly Mock<IAdminRepository> _adminRepositoryMock;
        private readonly AdminServices _adminServices;

        public AdminServiceTest(Mock<IAdminRepository> adminRepositoryMock, AdminServices adminServices)
        {
            _adminRepositoryMock=  new Mock<IAdminRepository>();

            var expectedNumberOfDoctors = 20;
            _adminRepositoryMock.Setup(repo => repo.NumOfDoctors()).Returns(expectedNumberOfDoctors);

            _adminServices = adminServices;


        [Fact]
        public void GetTotalNumOfDoctors_ReturnsCorrectNumber()
        {


            // Arrange


            // Act
            var actualNumberOfDoctors = _adminServices.GetTotalNumOfDoctors();

            // Assert
            Assert.Equal(20, actualNumberOfDoctors);
        }
    }
}
