using System;
using System.Threading.Tasks;
using Audacia.Azure.BlobStorage.Config;
using Audacia.Azure.BlobStorage.Exceptions;
using Audacia.Azure.BlobStorage.Services;
using Audacia.Azure.BlobStorage.Services.Interfaces;
using Microsoft.Extensions.Options;
using Xunit;

namespace Azure.Azure.Tests.BlobStorage
{
    public class SetupAzureBlobStorageTests
    {
        private IAddAzureBlobStorageService _addAzureBlobStorageService;

        [Fact]
        public async Task Should_throw_exception_if_option_value_is_null()
        {
            // Arrange
            var blobStorageOptions = Options.Create<BlobStorageOption>(null);

            // Act
            var expectedException = BlobStorageConfigurationException.OptionsNotConfigured();
            Exception thrownException = null;
            try
            {
                _addAzureBlobStorageService = new AddAzureBlobStorageService(blobStorageOptions);
            }
            catch (Exception exception)
            {
                thrownException = exception;
            }

            // Assert
            Assert.NotNull(thrownException);
            Assert.Equal(expectedException.GetType(), thrownException.GetType());
            Assert.Equal(expectedException.Message, thrownException.Message);
        }

        [Fact]
        public async Task Should_throw_exception_if_option_value_is_empty()
        {
            // Arrange
            var blobStorageOption = new BlobStorageOption();

            var blobStorageOptions = Options.Create(blobStorageOption);

            // Act
            var expectedException = BlobStorageConfigurationException.AccountNameNotConfigured();
            Exception thrownException = null;
            try
            {
                _addAzureBlobStorageService = new AddAzureBlobStorageService(blobStorageOptions);
            }
            catch (Exception exception)
            {
                thrownException = exception;
            }

            // Assert
            Assert.NotNull(thrownException);
            Assert.Equal(expectedException.GetType(), thrownException.GetType());
            Assert.Equal(expectedException.Message, thrownException.Message);
        }

        [Theory]
        [InlineData("Photos-dev", null)]
        [InlineData("Files", "")]
        public async Task Should_throw_exception_if_account_key_option_value_is_null_or_empty(string accountName,
            string accountKey)
        {
            // Arrange
            var blobStorageOption = new BlobStorageOption
            {
                AccountName = accountName,
                AccountKey = accountKey
            };

            var blobStorageOptions = Options.Create(blobStorageOption);

            // Act
            var expectedException = BlobStorageConfigurationException.AccountKeyNotConfigured();
            Exception thrownException = null;
            try
            {
                _addAzureBlobStorageService = new AddAzureBlobStorageService(blobStorageOptions);
            }
            catch (Exception exception)
            {
                thrownException = exception;
            }

            // Assert
            Assert.NotNull(thrownException);
            Assert.Equal(expectedException.GetType(), thrownException.GetType());
            Assert.Equal(expectedException.Message, thrownException.Message);
        }

        [Theory]
        [InlineData(null, "123685924")]
        [InlineData("", "968574125")]
        public async Task Should_throw_exception_if_account_name_option_value_is_null_or_empty(string accountName,
            string accountKey)
        {
            // Arrange
            var blobStorageOption = new BlobStorageOption
            {
                AccountName = accountName,
                AccountKey = accountKey
            };

            var blobStorageOptions = Options.Create(blobStorageOption);

            // Act
            var expectedException = BlobStorageConfigurationException.AccountNameNotConfigured();
            Exception thrownException = null;
            try
            {
                _addAzureBlobStorageService = new AddAzureBlobStorageService(blobStorageOptions);
            }
            catch (Exception exception)
            {
                thrownException = exception;
            }

            // Assert
            Assert.NotNull(thrownException);
            Assert.Equal(expectedException.GetType(), thrownException.GetType());
            Assert.Equal(expectedException.Message, thrownException.Message);
        }
    }
}