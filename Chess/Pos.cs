namespace Chess
{
    public class Pos
    {
        public int row;
        public int col;

        public Pos(int row, int column)
            => (this.row, this.col) = (row, column);

        public Pos() : this(-1, -1)
        { }


        public static bool operator ==(Pos a, Pos b)
            => a.row == b.row && a.col == b.col;

        public static bool operator !=(Pos a, Pos b)
            => !(a == b);


        public bool is_None()
            => this.col == -1 || this.row == -1;

        public bool is_on(int row, int column)
            => this.row == row && this.col == column;

        public Pos copy()
            => new(this.row, this.col);

        public bool isin_board_range()
            => this.row >= 0 && this.row <= 7 &&
               this.col >= 0 && this.col <= 7;

        public override string ToString()
            => $"({this.row},{this.col})";

        public override bool Equals(object? obj)
            => obj is Pos other && this == other;

        public override int GetHashCode()
            => HashCode.Combine(this.row, this.col);
    }
}
