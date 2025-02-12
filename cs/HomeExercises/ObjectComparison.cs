﻿using FluentAssertions;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace HomeExercises
{
    [TestFixture]	
	public class ObjectComparison
	{
		// Мое решение теперь также рекурсивное, как и CheckCurrentTsar_WithCustomEquality
		// Но мое решение лучше, т.к. при добавлении новых полей/свойств в класс Person
		// в CheckCurrentTsar_WithCustomEquality нужно будет дописывать новые условия
		// для равенства двух Person.
		// Мое решение использует метод BeEquivalentTo, который сравнивает объекты
		// Через рефлексию клаасов, который автоматически сравнивает все поля/свойства объектов,
		// Кроме Id, которые должны отличаться (очевидно).

		[Test]
		[Description("Проверка текущего царя")]
		[Category("ToRefactor")]
		public void CheckCurrentTsar()
        {
			var actualTsar = TsarRegistry.GetCurrentTsar();
            var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
                new Person("Vasili III of Russia", 28, 170, 60, null));

			actualTsar.Should().BeEquivalentTo(expectedTsar,
				options => options.Excluding(
					person => Regex.IsMatch(person.SelectedMemberPath, @"^(Parent\.)*Id$")));
        }

		[Test]
		[Description("Альтернативное решение. Какие у него недостатки?")]
		public void CheckCurrentTsar_WithCustomEquality()
		{
			var actualTsar = TsarRegistry.GetCurrentTsar();
			var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
				new Person("Vasili III of Russia", 28, 170, 60, null));

			// Какие недостатки у такого подхода? 
			Assert.True(AreEqual(actualTsar, expectedTsar));
		}

		private bool AreEqual(Person? actual, Person? expected)
		{
			if (actual == expected) return true;
			if (actual == null || expected == null) return false;
			return
				actual.Name == expected.Name
				&& actual.Age == expected.Age
				&& actual.Height == expected.Height
				&& actual.Weight == expected.Weight
				&& AreEqual(actual.Parent, expected.Parent);
		}
	}

	public class TsarRegistry
	{
		public static Person GetCurrentTsar()
		{
			return new Person(
				"Ivan IV The Terrible", 54, 170, 70,
				new Person("Vasili III of Russia", 28, 170, 60, null));
		}
	}

	public class Person
	{
		public static int IdCounter = 0;
		public int Age, Height, Weight;
		public string Name;
		public Person? Parent;
		public int Id;

		public Person(string name, int age, int height, int weight, Person? parent)
		{
			Id = IdCounter++;
			Name = name;
			Age = age;
			Height = height;
			Weight = weight;
			Parent = parent;
		}
	}
}