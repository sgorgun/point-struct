using System.Reflection;
using NUnit.Framework;

namespace PointStruct.Tests
{
    [TestFixture]
    public class PointTests
    {
        private static readonly object[][] ConstructorData =
        {
            new object[]
            {
                new[] { typeof(int), typeof(int) },
            },
            new object[]
            {
                new[] { typeof(long), typeof(long) },
            },
        };

        private static readonly object[][] HasMethodData =
        {
            new object[]
            {
                "Equals", false, true, true, typeof(bool), new Type[] { typeof(Point) },
            },
            new object[]
            {
                "Equals", false, true, true, typeof(bool), new Type[] { typeof(object) },
            },
            new object[]
            {
                "ToString", false, true, true, typeof(string), Array.Empty<Type>(),
            },
            new object[]
            {
                "GetHashCode", false, true, true, typeof(int), Array.Empty<Type>(),
            },
            new object[]
            {
                "Parse", true, true, false, typeof(Point), new Type[] { typeof(string) },
            },
            new object[]
            {
                "TryParse", true, true, false, typeof(bool), new Type[] { typeof(string), typeof(Point).MakeByRefType() },
            },
            new object[]
            {
                "CountPointsInExactSameLocation", false, true, false, typeof(int), new Type[] { typeof(IEnumerable<Point>) },
            },
            new object[]
            {
                "GetCollinearPointCoordinates", false, true, false, typeof(string), new Type[] { typeof(ICollection<Point>) },
            },
            new object[]
            {
                "GetNeighbors", false, true, false, typeof(ICollection<Point>), new Type[] { typeof(int), typeof(IList<Point>) },
            },
            new object[]
            {
                "IsNeighbor", false, false, false, typeof(bool), new Type[] { typeof(int), typeof(Point) },
            },
            new object[]
            {
                "op_Equality", true, true, false, typeof(bool), new Type[] { typeof(Point), typeof(Point) },
            },
            new object[]
            {
                "op_Inequality", true, true, false, typeof(bool), new Type[] { typeof(Point), typeof(Point) },
            },
        };

        private static readonly object[][] EqualsWithPointParameterData =
        {
            new object[] { new Point(0, 0), new Point(0, 0), true },
            new object[] { new Point(0, long.MinValue), new Point(0, long.MinValue), true },
            new object[] { new Point(0, long.MaxValue), new Point(0, long.MaxValue), true },
            new object[] { new Point(long.MinValue, 0), new Point(long.MinValue, 0), true },
            new object[] { new Point(long.MaxValue, 0), new Point(long.MaxValue, 0), true },
            new object[] { new Point(long.MaxValue, long.MinValue), new Point(long.MaxValue, long.MinValue), true },
            new object[] { new Point(0, long.MinValue), new Point(long.MinValue, 0), false },
            new object[] { new Point(0, long.MaxValue), new Point(long.MaxValue, 0), false },
            new object[] { new Point(long.MinValue, 0), new Point(0, long.MinValue), false },
            new object[] { new Point(long.MaxValue, 0), new Point(0, long.MaxValue), false },
            new object[] { new Point(long.MaxValue, long.MinValue), new Point(long.MinValue, long.MaxValue), false },
        };

        private static readonly object?[][] EqualsWithObjectParameterData =
        {
            new object[] { new Point(0, 0), new Point(0, 0), true },
            new object[] { new Point(0, long.MinValue), new Point(0, long.MinValue), true },
            new object[] { new Point(0, long.MaxValue), new Point(0, long.MaxValue), true },
            new object[] { new Point(long.MinValue, 0), new Point(long.MinValue, 0), true },
            new object[] { new Point(long.MaxValue, 0), new Point(long.MaxValue, 0), true },
            new object[] { new Point(long.MaxValue, long.MinValue), new Point(long.MaxValue, long.MinValue), true },
            new object[] { new Point(0, long.MinValue), new Point(long.MinValue, 0), false },
            new object[] { new Point(0, long.MaxValue), new Point(long.MaxValue, 0), false },
            new object[] { new Point(long.MinValue, 0), new Point(0, long.MinValue), false },
            new object[] { new Point(long.MaxValue, 0), new Point(0, long.MaxValue), false },
            new object[] { new Point(long.MaxValue, long.MinValue), new Point(long.MinValue, long.MaxValue), false },
            new object?[] { new Point(long.MaxValue, long.MinValue), null, false },
            new object[] { new Point(long.MaxValue, long.MinValue), new object(), false },
        };

        private static readonly object[][] FindNeighborsData =
        {
            new object[] { 3, 3, 2, new Point[] { new Point(3, 4), new Point(4, 4), new Point(2, 3), new Point(2, 2), new Point(5, 2) } },
            new object[] { 3, 3, 1, new Point[] { new Point(3, 4), new Point(4, 4), new Point(2, 3), new Point(2, 2) } },
            new object[] { -2, -4, 3, new Point[] { new Point(-2, -5), new Point(0, -3) } },
            new object[] { -1, 10, 6, new Point[] { new Point(-2, 5), new Point(3, 4), new Point(4, 4), new Point(0, 16) } },
        };

        private static readonly object[][] GetCollinearPointCoordinatesData =
        {
            new object[] { new Point(0, 0), new Point[] { new Point(1, 1), new Point(-1, 1), new Point(1, -1), new Point(-1, -1) }, string.Empty },
            new object[] { new Point(0, 0), new Point[] { new Point(1, 1), new Point(-1, 1), new Point(1, -1), new Point(-1, -1), new Point(0, 0) }, "(0,0,\"SAME\")" },
            new object[] { new Point(0, 0), new Point[] { new Point(0, 1), new Point(0, -1), new Point(1, 0), new Point(-1, 0), new Point(0, 0) }, "(0,1,\"X\"),(0,-1,\"X\"),(1,0,\"Y\"),(-1,0,\"Y\"),(0,0,\"SAME\")" },
            new object[] { new Point(5, -7), new Point[] { new Point(0, 0), new Point(5, -6), new Point(-5, 7), new Point(-5, -7), new Point(5, 7) }, "(5,-6,\"X\"),(-5,-7,\"Y\"),(5,7,\"X\")" },
        };

        private static readonly object[][] CountPointsInExactSameLocationData =
        {
            new object[] { new Point(0, 0), new Point[] { new Point(1, 1), new Point(-1, 1), new Point(1, -1), new Point(-1, -1) }, 0 },
            new object[] { new Point(0, 0), new Point[] { new Point(1, 1), new Point(-1, 1), new Point(1, -1), new Point(-1, -1), new Point(0, 0) }, 1 },
            new object[] { new Point(1, 1), new Point[] { new Point(0, 0), new Point(1, 1), new Point(-1, 1), new Point(1, -1), new Point(-1, -1), new Point(1, 1) }, 2 },
            new object[] { new Point(5, -7), new Point[] { new Point(0, 0), new Point(5, -7), new Point(1, 1), new Point(-5, 7), new Point(-5, -7), new Point(5, -7), new Point(5, 7) }, 2 },
        };

        private readonly Point[] points = new Point[]
        {
            new Point(-2, 5),
            new Point(6, 6),
            new Point(3, 4),
            new Point(4, 4),
            new Point(2, 3),
            new Point(2, 2),
            new Point(5, 2),
            new Point(3, -2),
            new Point(-2, -5),
            new Point(0, 16),
            new Point(31, 14),
            new Point(-12, 44),
            new Point(20, 13),
            new Point(22, -2),
            new Point(-5, 2),
            new Point(3, -2),
            new Point(-2, -50),
            new Point(-76, -6),
            new Point(-30, 04),
            new Point(40, 4),
            new Point(0, -3),
            new Point(-2, 0),
            new Point(0, 0),
            new Point(12, -2),
        };

        private Type? classType;

        [SetUp]
        public void SetUp()
        {
            this.classType = typeof(Point);
        }

        [TestCase(0, 0)]
        [TestCase(1, -1)]
        [TestCase(-1, 1)]
        public void Point_ConstructorWithLongParameters_ReturnsNewObject(long x, long y)
        {
            // Act
            var point = new Point(x: x, y: y);

            // Assert
            Assert.That(point.X, Is.EqualTo(x));
            Assert.That(point.Y, Is.EqualTo(y));
        }

        [TestCase(0, 0)]
        [TestCase(1, -1)]
        [TestCase(-1, 1)]
        public void Point_ConstructorWithIntParameters_ReturnsNewObject(int x, int y)
        {
            // Act
            var point = new Point(x: x, y: y);

            // Assert
            Assert.That(point.X, Is.EqualTo(x));
            Assert.That(point.Y, Is.EqualTo(y));
        }

        [TestCaseSource(nameof(CountPointsInExactSameLocationData))]
        public void CountPointsInExactSameLocation_CorrectParameters_ReturnsString(Point point, IEnumerable<Point> points, int expectedResult)
        {
            // Act
            int actualCount = point.CountPointsInExactSameLocation(points);

            // Assert
            Assert.That(actualCount, Is.EqualTo(expectedResult));
        }

        [TestCaseSource(nameof(GetCollinearPointCoordinatesData))]
        public void GetCollinearPointCoordinates_CorrectParameters_ReturnsString(Point point, ICollection<Point> points, string expectedResult)
        {
            // Act
            string actualResult = point.GetCollinearPointCoordinates(points);

            // Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void GetNeighbors_WithIncorrectDistance_ThrowsArgumentException(int distance)
        {
            // Arrange
            var point = new Point(x: 0, y: 0);

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                try
                {
                    // Act
                    point.GetNeighbors(distance, Array.Empty<Point>());
                }
                catch (ArgumentException e)
                {
                    Assert.That(e.ParamName, Is.EqualTo(nameof(distance)));
                    throw;
                }
            });
        }

        [TestCaseSource(nameof(FindNeighborsData))]
        public void GetNeighbors_CorrectParameters_ReturnsCollection(int x, int y, int distance, ICollection<Point> expectedResult)
        {
            // Arrange
            var point = new Point(x: x, y: y);

            // Act
            var actualResult = point.GetNeighbors(distance, this.points);

            // Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [TestCase("")]
        [TestCase("      ")]
        [TestCase("abc,1")]
        [TestCase("1,abc")]
        [TestCase(",1")]
        [TestCase("1,")]
        [TestCase(",")]
        public void Parse_ThrowsArgumentException(string pointString)
        {
            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                try
                {
                    // Act
                    Point.Parse(pointString);
                }
                catch (ArgumentException e)
                {
                    Assert.That(e.ParamName, Is.EqualTo(nameof(pointString)));
                    throw;
                }
            });
        }

        [TestCase("0,0", 0, 0)]
        [TestCase("000000000000000,000000000000000", 0, 0)]
        [TestCase("123,-456", 123, -456)]
        [TestCase("-123456789,987654321", -123456789, 987654321)]
        [TestCase("9223372036854775807,-9223372036854775808", long.MaxValue, long.MinValue)]
        public void Parse_ReturnsPoint(string pointString, long x, long y)
        {
            // Act
            Point point = Point.Parse(pointString: pointString);

            // Assert
            Assert.That(point.X, Is.EqualTo(x));
            Assert.That(point.Y, Is.EqualTo(y));
        }

        [TestCase("", false, 0, 0)]
        [TestCase("      ", false, 0, 0)]
        [TestCase("abc,1", false, 0, 0)]
        [TestCase("1,abc", false, 0, 0)]
        [TestCase(",1", false, 0, 0)]
        [TestCase("1,", false, 0, 0)]
        [TestCase(",", false, 0, 0)]
        [TestCase("0,0", true, 0, 0)]
        [TestCase("000000000000000,000000000000000", true, 0, 0)]
        [TestCase("123,-456", true, 123, -456)]
        [TestCase("-123456789,987654321", true, -123456789, 987654321)]
        [TestCase("9223372036854775807,-9223372036854775808", true, long.MaxValue, long.MinValue)]
        public void TryParse_ReturnsRgbColor(string pointString, bool expectedResult, long x, long y)
        {
            // Act
            bool actualResult = Point.TryParse(pointString: pointString, point: out Point point);

            // Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
            Assert.That(point.X, Is.EqualTo(x));
            Assert.That(point.Y, Is.EqualTo(y));
        }

        [TestCaseSource(nameof(EqualsWithPointParameterData))]
        public void Equals_WithPointParameter_ReturnsBoolean(Point point, Point other, bool expectedResult)
        {
            // Act
            bool actualResult = point.Equals(other: other);

            // Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [TestCaseSource(nameof(EqualsWithObjectParameterData))]
        public void Equals_WithObjectParameter_ReturnsBoolean(object rgbColor, object? obj, bool expectedResult)
        {
            // Act
            bool actualResult = rgbColor.Equals(obj: obj);

            // Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [TestCaseSource(nameof(EqualsWithPointParameterData))]
        public void EqualityOperators_ReturnsBoolean(Point left, Point right, bool expectedResult)
        {
            // Act
            bool isEqual = left == right;
            bool isUnequal = left != right;

            // Assert
            Assert.That(isEqual, Is.EqualTo(expectedResult));
            Assert.That(isUnequal, Is.EqualTo(!expectedResult));
        }

        [TestCase(0, 0, ExpectedResult = "0,0")]
        [TestCase(1, 1, ExpectedResult = "1,1")]
        [TestCase(long.MinValue, long.MinValue, ExpectedResult = "-9223372036854775808,-9223372036854775808")]
        [TestCase(long.MaxValue, long.MaxValue, ExpectedResult = "9223372036854775807,9223372036854775807")]
        public string ToString_ReturnsString(long x, long y)
        {
            // Arrange
            var point = new Point(x: x, y: y);

            // Act
            return point.ToString();
        }

        [TestCase(0x0000000000000000, 0x0000000000000000, ExpectedResult = 0x00000000)]
        [TestCase(0x0000000000000001, 0x0000000000000000, ExpectedResult = 0x00000001)]
        [TestCase(0x0000000000000000, 0x0000000000000001, ExpectedResult = 0x0000001)]
        [TestCase(0x0000000000000001, 0x0000000000000001, ExpectedResult = 0x00000000)]
        [TestCase(0x0102030405060708, 0x0102030405060708, ExpectedResult = 0x00000000)]
        [TestCase(0x0102030405060708, 0x1020304050607080, ExpectedResult = 0x444444cc)]
        [TestCase(0x123456789ABCDEFF, 0x123456789ABCDEFF, ExpectedResult = 0x00000000)]
        [TestCase(0x123456789ABCDEFF, 0x21436587A9CBEDFF, ExpectedResult = 0x000000ff)]
        [TestCase(0x1122334455667788, 0x1877665544332211, ExpectedResult = 0x18000088)]
        [TestCase(0x0807060504030201, 0x2D2C2B2A1D1C1B1A, ExpectedResult = 0x3c343434)]
        public int GetHashCode_ReturnsHashCode(long x, long y)
        {
            // Arrange
            var point = new Point(x: x, y: y);

            // Act
            return point.GetHashCode();
        }

        [TestCase(3L, 3L, 2L, 3L, 4L, ExpectedResult = true)]
        [TestCase(3L, 3L, 2L, 4L, 4L, ExpectedResult = true)]
        [TestCase(3L, 3L, 2L, 2L, 3L, ExpectedResult = true)]
        [TestCase(3L, 3L, 2L, 2L, 2L, ExpectedResult = true)]
        [TestCase(3L, 3L, 2L, 5L, 2L, ExpectedResult = true)]
        [TestCase(3L, 3L, 2L, -2L, 5L, ExpectedResult = false)]
        [TestCase(3L, 3L, 2L, 3L, -2L, ExpectedResult = false)]
        [TestCase(3L, 3L, 2L, 6L, 6L, ExpectedResult = false)]
        public bool IsNeighbor_ReturnsBoolean(long x, long y, long distance, long pointX, long pointY)
        {
            // Arrange
            var point = new Point(x: x, y: y);
            var methodInfo = point.GetType().GetMethod("IsNeighbor", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            Assert.That(methodInfo, Is.Not.Null);
            var actualResult = methodInfo!.Invoke(point, new object[] { distance, new Point(pointX, pointY) });
            Assert.That(actualResult, Is.Not.Null);

            return (bool)actualResult;
        }

        [Test]
        public void IsPublicStruct()
        {
            this.AssertThatStructIsPublic();
        }

        [Test]
        public void InheritsValuteType()
        {
            this.AssertThatStructInheritsValueType();
        }

        [Test]
        public void ImplementsEquitable()
        {
            this.AssertThatTypeImplementsInterface(typeof(IEquatable<Point>));
        }

        [Test]
        public void HasRequiredMembers()
        {
            Assert.AreEqual(0, this.classType!.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length);
            Assert.AreEqual(0, this.classType!.GetFields(BindingFlags.Instance | BindingFlags.Public).Length);
            Assert.AreEqual(2, this.classType!.GetFields(BindingFlags.Instance | BindingFlags.NonPublic).Length);

            Assert.AreEqual(0, this.classType!.GetConstructors(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length);
            Assert.AreEqual(2, this.classType!.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length);
            Assert.AreEqual(0, this.classType!.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).Length);

            Assert.AreEqual(0, this.classType!.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length);
            Assert.AreEqual(2, this.classType!.GetProperties(BindingFlags.Instance | BindingFlags.Public).Length);
            Assert.AreEqual(0, this.classType!.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic).Length);

            Assert.AreEqual(4, this.classType!.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly).Length);
            Assert.AreEqual(0, this.classType!.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).Length);

            Assert.AreEqual(9, this.classType!.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Length);
            Assert.AreEqual(3, this.classType!.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).Length);

            Assert.AreEqual(0, this.classType!.GetEvents(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length);
        }

        [TestCaseSource(nameof(ConstructorData))]
        public void HasPublicInstanceConstructor(Type[] parameterTypes)
        {
            this.AssertThatTypeHasPublicConstructor(parameterTypes);
        }

        [TestCase("X", typeof(long), true, true, true, false)]
        [TestCase("Y", typeof(long), true, true, true, false)]
        public void HasProperty(string propertyName, Type propertyType, bool hasGet, bool isGetPublic, bool hasSet, bool isSetPublic)
        {
            this.AssertThatTypeHasProperty(propertyName, propertyType, hasGet, isGetPublic, hasSet, isSetPublic);
        }

        [TestCaseSource(nameof(HasMethodData))]
        public void HasMethod(string methodName, bool isStatic, bool isPublic, bool isVirtual, Type returnType, Type[] parameters)
        {
            this.AssertThatTypeHasMethod(methodName, isStatic, isPublic, isVirtual, returnType, parameters);
        }

        private void AssertThatStructIsPublic()
        {
            Assert.That(this.classType!.IsValueType, Is.True);
            Assert.That(this.classType!.IsPublic, Is.True);
            Assert.That(this.classType!.IsAbstract, Is.False);
        }

        private void AssertThatStructInheritsValueType()
        {
            Assert.That(this.classType!.BaseType, Is.EqualTo(typeof(ValueType)));
        }

        private void AssertThatTypeHasPublicConstructor(Type[] parameterTypes)
        {
            var constructorInfo = this.classType!.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, parameterTypes, null);
            Assert.That(constructorInfo, Is.Not.Null);
        }

        private void AssertThatTypeHasProperty(string propertyName, Type expectedPropertyType, bool hasGet, bool isGetPublic, bool hasSet, bool isSetPublic)
        {
            var propertyInfo = this.classType!.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);

            Assert.That(propertyInfo, Is.Not.Null);
            Assert.That(propertyInfo!.PropertyType, Is.EqualTo(expectedPropertyType));

            if (hasGet)
            {
                Assert.That(propertyInfo!.GetMethod!.IsPublic, isGetPublic ? Is.True : Is.False);
            }

            if (hasSet)
            {
                Assert.That(propertyInfo!.SetMethod!.IsPublic, isSetPublic ? Is.True : Is.False);
            }
        }

        private void AssertThatTypeHasMethod(string methodName, bool isStatic, bool isPublic, bool isVirtual, Type returnType, Type[] parameters)
        {
            var methodInfo = this.classType!.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly, parameters);

            Assert.That(methodInfo, Is.Not.Null);
            Assert.That(methodInfo!.IsStatic, isStatic ? Is.True : Is.False);
            Assert.That(methodInfo!.IsPublic, isPublic ? Is.True : Is.False);
            Assert.That(methodInfo!.IsVirtual, isVirtual ? Is.True : Is.False);
            Assert.That(methodInfo!.ReturnType, Is.EqualTo(returnType));
        }

        private void AssertThatTypeImplementsInterface(Type interfaceType)
        {
            var @interface = this.classType!.GetInterface(interfaceType.Name);

            Assert.That(@interface, Is.Not.Null);
        }
    }
}
