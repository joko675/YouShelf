using Dapper;
using System.Data;

namespace YouShelf.Services;

public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override DateOnly Parse(object value)
    {
        return value switch
        {
            string s => DateOnly.Parse(s),
            DateTime dt => DateOnly.FromDateTime(dt),
            _ => throw new DataException($"Cannot convert {value.GetType()} to DateOnly")
        };
    }

    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.Value = value.ToString("yyyy-MM-dd");
        parameter.DbType = DbType.String;
    }

}
