using AnagramSolver.Contracts.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Tests
{
    public class GenericsTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(Gender.Male, 1)]
        [TestCase(Gender.Female, 2)]
        [TestCase(Gender.Other, 3)]
        public void TestMapIntToGender(Gender gender, int identifier)
        {
            var result = Generics.Generics.MapIntToGender(identifier);
            Assert.AreEqual(gender, result);
        }

        [TestCase(Gender.Male, "Male")]
        [TestCase(Gender.Female, "Female")]
        [TestCase(Gender.Other, "Other")]
        public void TestMapStringToGender(Gender gender, string identifier)
        {
            var result = Generics.Generics.MapStringToGender(identifier);
            Assert.AreEqual(gender, result);
        }

        [TestCase(Weekday.Monday, "Monday")]
        [TestCase(Weekday.Tuesday, "Tuesday")]
        [TestCase(Weekday.Wednesday, "Wednesday")]
        [TestCase(Weekday.Thursday, "Thursday")]
        [TestCase(Weekday.Friday, "Friday")]
        [TestCase(Weekday.Saturday, "Saturday")]
        [TestCase(Weekday.Sunday, "Sunday")]
        public void TestMapStringToWeekday(Weekday weekday, string identifier)
        {
            var result = Generics.Generics.MapStringToWeekday(identifier);
            Assert.AreEqual(weekday, result);
        }

        [TestCase(Gender.Male, "Male")]
        [TestCase(Gender.Female, "Female")]
        [TestCase(Gender.Other, "Other")]
        public void TestMapEnumToValueOnGenderWithString(Gender gender, string stringIdentifier)
        {
            var result = Generics.Generics.MapValueToEnum<Gender, string>(stringIdentifier);
            Assert.AreEqual(gender, result);
        }

        [TestCase(Gender.Male, 1)]
        [TestCase(Gender.Female, 2)]
        [TestCase(Gender.Other, 3)]
        public void TestMapEnumToValueOnGenderWithInt(Gender gender, int intIdentifier)
        {
            var result = Generics.Generics.MapValueToEnum<Gender, int>(intIdentifier);
            Assert.AreEqual(gender, result);
        }

        [TestCase(Weekday.Monday, "Monday")]
        [TestCase(Weekday.Tuesday, "Tuesday")]
        [TestCase(Weekday.Wednesday, "Wednesday")]
        [TestCase(Weekday.Thursday, "Thursday")]
        [TestCase(Weekday.Friday, "Friday")]
        [TestCase(Weekday.Saturday, "Saturday")]
        [TestCase(Weekday.Sunday, "Sunday")]
        public void TestMapEnumToValueOnWeekday(Weekday weekday, string identifier)
        {
            var result = Generics.Generics.MapValueToEnum<Weekday, string>(identifier);
            Assert.AreEqual(weekday, result);
        }

        [TestCase("Female")]
        public void TestComparisonOfGenderStringAndEnum(string identifier)
        {
            var genderString = Generics.Generics.MapStringToGender(identifier);
            var enumToValue = Generics.Generics.MapValueToEnum<Gender, string>(identifier);
            Assert.AreEqual(enumToValue, genderString);
        }

        [TestCase(3)]
        public void TestComparisonOfGenderIntAndEnum(int identifier)
        {
            var genderInt = Generics.Generics.MapIntToGender(identifier);
            var enumToValue = Generics.Generics.MapValueToEnum<Gender, int>(identifier);
            Assert.AreEqual(enumToValue, genderInt);
        }

        [TestCase("Wednesday")]
        public void TestComparisonOfWeekdayAndEnum(string identifier)
        {
            var weekday = Generics.Generics.MapStringToWeekday(identifier);
            var enumToValue = Generics.Generics.MapValueToEnum<Weekday, string>(identifier);
            Assert.AreEqual(enumToValue, weekday);
        }
    }
}
