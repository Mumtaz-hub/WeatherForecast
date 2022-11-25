using System.ComponentModel;

namespace Common.Enums
{
    public enum UnitsType : byte
    {
        [Description("K")]
        Default = 0,

        [Description("C")]
        Metric,

        [Description("F")]
        Imperial
    }
}
