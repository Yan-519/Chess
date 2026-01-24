namespace Chess.Chess_code;

using static Chess_tools;

public class Chess_cell
{
    private None_piece action = new();

    public Turns color;
    private Piece_name _name;
    public Piece_name name
    {
        get => _name;
        set
        {
            action = value switch
            {
                Piece_name.None   => new None_piece(),
                Piece_name.pawn   => new Pawn(),
                Piece_name.knight => new Knight(),
                Piece_name.bishop => new Bishop(),
                Piece_name.rook   => new Rook(),
                Piece_name.queen  => new Queen(),
                Piece_name.king   => new King(),
                _ => throw new ArgumentOutOfRangeException(nameof(value), "Invalid piece name")
            };

            cost = action.cost;

            _name = value;
        }
    }

    public Pos pos;

    public bool is_pawn_double_moved = false;
    public int cost { get; private set; }

    public Chess_cell(Piece_name name, Turns color, Pos pos)
    {
        this.name = name;
        this.color = color;
        this.pos = pos.copy();
    }

    public void move_to(ref Chess_cell to)
    {
        to.name = this.name;
        to.color = this.color;

        this.name = Piece_name.None;
    }

    public HashSet<Move> get_moves(Chess_cell[,] board) => action.get_list_of_moves(board, this.pos, this.color, false);

    public HashSet<Move> get_range_attack(Chess_cell[,] board) => action.get_list_of_moves(board, this.pos, this.color, true);

    public Chess_cell copy() => new(this.name, this.color, this.pos);

    public bool is_None() => this.name == Piece_name.None;

    public override bool Equals(object? obj)
        => obj is Chess_cell other && this.name == other.name && this.color == other.color && this.pos == other.pos;

    public override int GetHashCode() => HashCode.Combine(this.color, this.name, this.pos);
}
