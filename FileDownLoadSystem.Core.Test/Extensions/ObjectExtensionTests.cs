using FileDownLoadSystem.Core.Extensions;
using System;
using System.Collections.Generic;
using Xunit;

namespace FileDownLoadSystem.Core.Test.Extensions
{
    public class ObjectExtensionTests
    {
        [Fact]
        public void DicToEnumerable_ValidDictionary_ReturnsCorrectEnumerable()
        {
            // Arrange
            var dictionaries = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "Id", 1 },
                    { "Name", "Test" }
                }
            };

            // Act
            var result = dictionaries.DicToEnumerable<TestClass>();

            // Assert
            Assert.Single(result);
            var firstItem = result.GetEnumerator();
            firstItem.MoveNext();
            Assert.Equal(1, firstItem.Current.Id);
            Assert.Equal("Test", firstItem.Current.Name);
        }

        [Fact]
        public void DicToList_ValidDictionary_ReturnsCorrectList()
        {
            // Arrange
            var dictionaries = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "Id", 1 },
                    { "Name", "Test" }
                }
            };

            // Act
            var result = dictionaries.DicToList<TestClass>();

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result[0].Id);
            Assert.Equal("Test", result[0].Name);
        }

        [Fact]
        public void DicToEntity_ValidDictionary_ReturnsCorrectEntity()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>
            {
                { "Id", 1 },
                { "Name", "Test" }
            };

            // Act
            var result = dictionary.DicToEntity<TestClass>();

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("Test", result.Name);
        }

        [Fact]
        public void ChangeType_ValidGuidString_ReturnsGuid()
        {
            // Arrange
            var guidString = Guid.NewGuid().ToString();

            // Act
            var result = guidString.ChangeType(typeof(Guid));

            // Assert
            Assert.IsType<Guid>(result);
            Assert.Equal(Guid.Parse(guidString), result);
        }

        [Fact]
        public void ChangeType_NullableBooleanString_ReturnsBoolean()
        {
            // Arrange
            var booleanString = "1";

            // Act
            var result = booleanString.ChangeType(typeof(bool?));

            // Assert
            Assert.IsType<bool>(result);
            Assert.True((bool)result);
        }

        [Fact]
        public void ChangeType_InvalidType_ReturnsNull()
        {
            // Arrange
            var invalidString = "invalid";

            // Act
            var result = invalidString.ChangeType(typeof(int));

            // Assert
            Assert.Null(result);
        }

        private class TestClass
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
