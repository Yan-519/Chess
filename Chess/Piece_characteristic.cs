namespace Chess; 

using Piece_name = Chess_game.Piece_name;
using Turns = Chess_game.Turns;

public class Piece_characteristic
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

    public Piece_characteristic(Piece_name name, Turns color, Pos pos)
    {
        this.name = name;
        this.color = color;
        this.pos = pos.copy();
    }

    public void move_to(ref Piece_characteristic to)
    {
        to.name = this.name;
        to.color = this.color;

        this.name = Piece_name.None;
    }

    public HashSet<Move> get_moves(Piece_characteristic[,] board) => action.get_list_of_moves(board, this.pos, this.color, false);

    public HashSet<Move> get_range_attack(Piece_characteristic[,] board) => action.get_list_of_moves(board, this.pos, this.color, true);

    public Piece_characteristic copy() => new(this.name, this.color, this.pos);

    public bool is_None() => this.name == Piece_name.None;

    public override bool Equals(object? obj)
        => obj is Piece_characteristic other && this.name == other.name && this.color == other.color && this.pos == other.pos;

    public override int GetHashCode() => HashCode.Combine(this.color, this.name, this.pos);
}
