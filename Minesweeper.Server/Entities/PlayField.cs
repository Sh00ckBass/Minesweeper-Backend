using Minesweeper.Server.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Minesweeper.Server.Entities;

public class PlayField
{
    [BsonId] public Guid Id { get; set; }
    public int BombCount { get; set; } = 10;
    public int Size { get; set; } = 9;
    private int _tempBombCount;

    public PlayField(Guid id, PlayFieldSize playFieldType)
    {
        Id = id;
        GenerateFields(playFieldType);
    }

    public List<Field> Fields { get; set; } = new();
    [BsonElement] public List<Field> Bombs { get; set; } = new();
    [BsonElement] public List<Field> EmptyFields { get; set; } = new();

    public Guid GetId()
    {
        return Id;
    }

    private List<ClearedField> _clearedFields;

    public List<ClearedField> OnClick(Field field)
    {
        field.Visible = true;
        _clearedFields = new();
        FindAllFields(field);
        return _clearedFields;
    }

    [BsonIgnore]
    public bool ClearedCompleteField
    {
        get
        {
            foreach (var field in Fields)
            {
                if (!field.Visible && !field.Bomb)
                {
                    return false;
                }
            }

            return true;
        }
    }

    private void FindAllFields(Field currentField)
    {
        var x = currentField.Position.X;
        var y = currentField.Position.Y;

        if (
            EmptyFields.Contains(currentField) ||
            currentField.Bomb ||
            currentField.BombCount != 0
        )
        {
            if (!EmptyFields.Contains(currentField))
            {
                EmptyFields.Add(currentField);
            }
            return;
        }

        EmptyFields.Add(currentField);

        var topY = y - 1;
        if (topY >= 0 && topY < Size)
        {
            var pos = new Position(x, topY);
            var field = GetField(pos);
            if (field.BombCount == 0)
            {
                if (!field.Bomb)
                {
                    field.Visible = true;
                    _clearedFields.Add(new ClearedField(pos, 0));
                    FindAllFields(field);
                }
            }
            else
            {
                EmptyFields.Add(field);
                field.Visible = true;
                _clearedFields.Add(new ClearedField(pos, field.BombCount));
            }
        }

        var bottomY = y + 1;
        if (bottomY >= 0 && bottomY < Size)
        {
            var pos = new Position(x, bottomY);
            var field = GetField(pos);
            if (field.BombCount == 0)
            {
                if (!field.Bomb)
                {
                    field.Visible = true;
                    _clearedFields.Add(new ClearedField(pos, 0));
                    FindAllFields(field);
                }
            }
            else
            {
                EmptyFields.Add(field);
                field.Visible = true;
                _clearedFields.Add(new ClearedField(pos, field.BombCount));
            }
        }

        var leftX = x - 1;
        if (leftX >= 0 && leftX < Size)
        {
            var pos = new Position(leftX, y);
            var field = GetField(pos);
            if (field.BombCount == 0)
            {
                if (!field.Bomb)
                {
                    field.Visible = true;
                    _clearedFields.Add(new ClearedField(pos, 0));
                    FindAllFields(field);
                }
            }
            else
            {
                EmptyFields.Add(field);
                field.Visible = true;
                _clearedFields.Add(new ClearedField(pos, field.BombCount));
            }
        }

        var rightX = x + 1;
        if (rightX >= 0 && rightX < Size)
        {
            var pos = new Position(rightX, y);
            var field = GetField(pos);
            if (field.BombCount == 0)
            {
                if (!field.Bomb)
                {
                    field.Visible = true;
                    _clearedFields.Add(new ClearedField(pos, 0));
                    FindAllFields(field);
                }
            }
            else
            {
                EmptyFields.Add(field);
                field.Visible = true;
                _clearedFields.Add(new ClearedField(pos, field.BombCount));
            }
        }
    }

    private void GenerateFields(PlayFieldSize playFieldType)
    {
        var size = 9;
        var bombCount = 10;

        switch (playFieldType)
        {
            case PlayFieldSize.Small:
            {
                bombCount = 10;
                size = 9;
                break;
            }
            case PlayFieldSize.Medium:
            {
                bombCount = 40;
                size = 16;
                break;
            }
            case PlayFieldSize.Large:
            {
                bombCount = 100;
                size = 22;
                break;
            }
        }

        BombCount = bombCount;
        Size = size;
        for (var y = 0; y < size; y++)
        for (var x = 0; x < size; x++)
        {
            var position = new Position(x, y);
            var field = new Field(position);
            Fields.Add(field);
        }

        GenerateBombs();
        CalculateFieldType();
    }

    private void GenerateBombs()
    {
        if (_tempBombCount == BombCount)
        {
            return;
        }

        var pos = new Position(Random.Shared.Next(Size), Random.Shared.Next(Size));
        var field = GetField(pos);
        if (!field.Bomb)
        {
            field.Bomb = true;
            _tempBombCount++;
            Bombs.Add(field);
        }

        GenerateBombs();
    }

    private void CalculateFieldType()
    {
        for (var y = 0; y < Size; y++)
        {
            for (var x = 0; x < Size; x++)
            {
                var pos = new Position(x, y);
                var field = Fields.First(f => f.Position.Equals(pos));
                if (!field.Bomb)
                {
                    GetBombCount(field);
                }
            }
        }
    }

    private void GetBombCount(Field currentField)
    {
        var xCf = currentField.Position.X;
        var yCf = currentField.Position.Y;
        for (var y = yCf - 1; y <= yCf + 1; y++)
        {
            if (y < 0 || y >= Size)
            {
                continue;
            }

            for (var x = xCf - 1; x <= xCf + 1; x++)
            {
                if (x < 0 || x >= Size)
                {
                    continue;
                }

                var pos = new Position(x, y);
                var field = GetField(pos);
                if (field.Bomb)
                {
                    currentField.BombCount++;
                }
            }
        }
    }

    public List<Position> RevealBombs()
    {
        List<Position> positions = new();

        ShowAllBombs();
        foreach (var field in Bombs)
        {
            positions.Add(field.Position);
        }

        return positions;
    }

    private void ShowAllBombs()
    {
        foreach (var field in Bombs)
        {
            field.Visible = true;
        }
    }

    public Field GetField(Position position)
    {
        return Fields.First(f => f.Position.Equals(position));
    }
}