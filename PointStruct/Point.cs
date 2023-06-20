namespace PointStruct
{
    /// <summary>
    /// Represents a point in the Cartesian coordinate system.
    /// </summary>
    public struct Point : IEquatable<Point>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> structure with the specified <paramref name="x"/> and <paramref name="y"/>.
        /// </summary>
        /// <param name="x">An x-coordinate of this <see cref="Point"/> structure.</param>
        /// <param name="y">An y-coordinate of this <see cref="Point"/> structure.</param>
        public Point(int x, int y)
            : this((long)x, (long)y)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> structure with the specified <paramref name="x"/> and <paramref name="y"/>.
        /// </summary>
        /// <param name="x">An x-coordinate of this <see cref="Point"/> structure.</param>
        /// <param name="y">An y-coordinate of this <see cref="Point"/> structure.</param>
        public Point(long x, long y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Gets the x-coordinate of this <see cref="Point"/> structure.
        /// </summary>
        public long X { get; private set; }

        /// <summary>
        /// Gets the y-coordinate of this <see cref="Point"/> structure.
        /// </summary>
        public long Y { get; private set; }

        /// <summary>
        /// Compares the <paramref name="left"/> and <paramref name="right"/> objects. Returns true if the left <see cref="Point"/> is equal to the right <see cref="Point"/>; otherwise, false.
        /// </summary>
        /// <param name="left">A left <see cref="Point"/>.</param>
        /// <param name="right">A right <see cref="Point"/>.</param>
        /// <returns>true if the left <see cref="Point"/> is equal to the right <see cref="Point"/>; otherwise, false.</returns>
        public static bool operator ==(Point left, Point right) => left.X == right.X && left.Y == right.Y;

        /// <summary>
        /// Compares the <paramref name="left"/> and <paramref name="right"/> objects. Returns true if the left <see cref="Point"/> is not equal to the right <see cref="Point"/>; otherwise, false.
        /// </summary>
        /// <param name="left">A left <see cref="Point"/>.</param>
        /// <param name="right">A right <see cref="Point"/>.</param>
        /// <returns>true if the left <see cref="Point"/> is not equal to the right <see cref="Point"/>; otherwise, false.</returns>
        public static bool operator !=(Point left, Point right) => !(left == right);

        /// <summary>
        /// Converts the string representation of a point to its <see cref="Point"/> equivalent.
        /// </summary>
        /// <param name="pointString">A string containing a point to convert.</param>
        /// <returns>A <see cref="Point"/> equivalent to the point contained in <paramref name="pointString"/>.</returns>
        public static Point Parse(string pointString)
        {
            if (string.IsNullOrWhiteSpace(pointString))
            {
                throw new ArgumentException("Point string cannot be null or empty.", nameof(pointString));
            }

            var pointParts = pointString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (pointParts.Length != 2)
            {
                throw new ArgumentException("Point string must contain two parts separated by comma.", nameof(pointString));
            }

            if (!long.TryParse(pointParts[0], out long x))
            {
                throw new ArgumentException("Point string must contain two parts separated by comma.", nameof(pointString));
            }

            if (!long.TryParse(pointParts[1], out long y))
            {
                throw new ArgumentException("Point string must contain two parts separated by comma.", nameof(pointString));
            }

            return new Point(x, y);
        }

        /// <summary>
        /// Converts the string representation of a point to its <see cref="Point"/> equivalent. A return value indicates whether the operation succeeded.
        /// </summary>
        /// <param name="pointString">A string containing a point to convert.</param>
        /// <param name="point">A <see cref="Point"/> equivalent to the point contained in <paramref name="pointString"/>.</param>
        /// <returns>true if <paramref name="rgbString"/> was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string pointString, out Point point)
        {
            point = default;
            if (string.IsNullOrWhiteSpace(pointString))
            {
                return false;
            }

            var pointParts = pointString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (pointParts.Length != 2)
            {
                return false;
            }

            if (!long.TryParse(pointParts[0], out long x))
            {
                return false;
            }

            if (!long.TryParse(pointParts[1], out long y))
            {
                return false;
            }

            point = new Point(x, y);
            return true;
        }

        /// <summary>
        /// Returns the number of points that have exact same coordinates as the <see cref="Point"/>.
        /// </summary>
        /// <param name="points">A collection of points.</param>
        /// <returns>The number of points that have exact same coordinates as the <see cref="Point"/>.</returns>
        public readonly int CountPointsInExactSameLocation(IEnumerable<Point> points)
        {
            int numberOfPoints = 0;
            foreach (Point p in points)
            {
                if (p == this)
                {
                    numberOfPoints++;
                }
            }

            return numberOfPoints;
        }

        /// <summary>
        /// Returns a string with points that are collinear to the <see cref="Point"/>.
        /// </summary>
        /// <param name="points">A collection of points.</param>
        /// <returns>A string with points that are collinear to the <see cref="Point"/>.</returns>
        public readonly string GetCollinearPointCoordinates(IEnumerable<Point> points)
        {
            List<string> collinearPoints = new List<string>();
            foreach (Point point in points)
            {
                if (point.X == this.X && point.Y == this.Y)
                {
                    collinearPoints.Add("(" + point.ToString() + ",\"SAME\"" + ")");
                }
                else if (point.X == this.X)
                {
                    collinearPoints.Add("(" + point.ToString() + ",\"X\"" + ")");
                }
                else if (point.Y == this.Y)
                {
                    collinearPoints.Add("(" + point.ToString() + ",\"Y\"" + ")");
                }
            }

            return string.Join(",", collinearPoints);
        }

        /// <summary>
        /// Returns a collection of points that are distance-neighbors for the <see cref="Point"/>.
        /// </summary>
        /// <param name="distance">Distance around a given point.</param>
        /// <param name="points">A list of points.</param>
        /// <returns>A collection of points that are distance-neighbors.</returns>
        public readonly ICollection<Point> GetNeighbors(long distance, ICollection<Point> points)
        {
            if (distance <= 0)
            {
                throw new ArgumentException("Distance cannot be negative.", nameof(distance));
            }

            if (points == null)
            {
                throw new ArgumentNullException(nameof(points));
            }

            List<Point> neighbors = new List<Point>();
            foreach (Point point in points)
            {
                if (point == this)
                {
                    continue;
                }

                if (this.IsNeighbor(distance, point))
                {
                    neighbors.Add(point);
                }
            }

            return neighbors;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Point"/> is equal to the current <see cref="Point"/>.
        /// </summary>
        /// <param name="other">The <see cref="Point"/> to compare with the current <see cref="Point"/>.</param>
        /// <returns>true if the specified <see cref="Point"/> is equal to the current <see cref="Point"/>; otherwise, false.</returns>
        public readonly bool Equals(Point other) => this == other;

        /// <summary>
        /// Determines whether the specified <see cref="Point"/> is equal to the current <see cref="Point"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current <see cref="Point"/>.</param>
        /// <returns>true if the specified <see cref="Point"/> is equal to the current <see cref="Point"/>; otherwise, false.</returns>
        public override readonly bool Equals(object? obj) => base.Equals(obj);

        /// <summary>
        /// Returns a string that represents the current <see cref="Point"/>.
        /// </summary>
        /// <returns>A string that represents the current <see cref="Point"/>.</returns>
        public override readonly string ToString() => $"{this.X},{this.Y}";

        /// <summary>
        /// Gets a hash code of the current <see cref="Point"/>.
        /// </summary>
        /// <returns>A hash code of the current <see cref="Point"/>.</returns>
        public override readonly int GetHashCode() => (int)(((this.X >> 32) ^ this.X) ^ ((this.Y >> 32) ^ this.Y));

        /// <summary>
        /// Checks if the point is a neighbor of the current point.
        /// </summary>
        /// <param name="distance">Distance.</param>
        /// <param name="point">Point.</param>
        /// <returns>True or falce.</returns>
        private readonly bool IsNeighbor(long distance, Point point) => Math.Abs(point.X - this.X) <= distance && Math.Abs(point.Y - this.Y) <= distance;
    }
}
