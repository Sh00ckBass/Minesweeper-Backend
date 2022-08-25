using Minesweeper.Server.Enums;

namespace Minesweeper.Server.Entities;

public class PlayField
{
    private readonly Guid _id;
    private int _bombCount = 10;
    private int _size = 9;
    private int _tempBombCount;

    public PlayField(Guid id, PlayFieldSize playFieldType)
    {
        _id = id;
        GenerateFields(playFieldType);
    }

    public Dictionary<Position, Field> Fields { get; } = new();
    private List<Field> Bombs { get; } = new();
    private List<Field> EmptyFields { get; } = new();

    public Guid GetId()
    {
        return _id;
    }

    private List<ClearedField> _clearedFields;

    public List<ClearedField>? OnClick(Field field)
    {
        field.Visible = true;
        _clearedFields = new();
        FindAllFields(field);
        return _clearedFields;
    }

    public bool ClearedCompleteField()
    {
        foreach (var (key, value) in Fields)
        {
            if (!value.Visible && !value.Bomb)
            {
                return false;
            }
        }

        return true;
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
            return;
        }

        EmptyFields.Add(currentField);

        var topY = y - 1;
        if (topY >= 0 && topY < _size)
        {
            var pos = new Position(x, topY);
            var field = Fields[pos];
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
                field.Visible = true;
                _clearedFields.Add(new ClearedField(pos, field.BombCount));
            }
        }

        var bottomY = y + 1;
        if (bottomY >= 0 && bottomY < _size)
        {
            var pos = new Position(x, bottomY);
            var field = Fields[pos];
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
                field.Visible = true;
                _clearedFields.Add(new ClearedField(pos, field.BombCount));
            }
        }

        var leftX = x - 1;
        if (leftX >= 0 && leftX < _size)
        {
            var pos = new Position(leftX, y);
            var field = Fields[pos];
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
                field.Visible = true;
                _clearedFields.Add(new ClearedField(pos, field.BombCount));
            }
        }

        var rightX = x + 1;
        if (rightX >= 0 && rightX < _size)
        {
            var pos = new Position(rightX, y);
            var field = Fields[pos];
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

        _bombCount = bombCount;
        _size = size;
        for (var y = 0; y < size; y++)
        for (var x = 0; x < size; x++)
        {
            var position = new Position(x, y);
            var field = new Field(position);
            Fields.Add(position, field);
        }

        GenerateBombs();
        CalculateFieldType();
    }

    private void GenerateBombs()
    {
        if (_tempBombCount == _bombCount) return;

        var pos = new Position(Random.Shared.Next(_size), Random.Shared.Next(_size));
        var field = Fields[pos];
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
        for (var y = 0; y < _size; y++)
        {
            for (var x = 0; x < _size; x++)
            {
                var pos = new Position(x, y);
                var field = Fields[pos];
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
            if (y < 0 || y >= _size)
            {
                continue;
            }

            for (var x = xCf - 1; x <= xCf + 1; x++)
            {
                if (x < 0 || x >= _size)
                {
                    continue;
                }

                var pos = new Position(x, y);
                var field = Fields[pos];
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
}