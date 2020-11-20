using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.SVG;

namespace Nop.Data.Mapping.Builders.SVG
{
    class GetSVGContentBuilder : NopEntityBuilder<SVGContent>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(SVGContent.pID)).AsInt32().NotNullable()
                .WithColumn(nameof(SVGContent.SvgContent)).AsString().NotNullable();
        }

        #endregion
    }
}
