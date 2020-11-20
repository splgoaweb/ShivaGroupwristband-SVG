using FluentMigrator;
using Nop.Data.Migrations;
using Nop.Plugin.NopFeatures.ShipRocket.Domain;

namespace Nop.Plugin.NopFeatures.ShipRocket.Data
{
    /// <summary>
    /// Table Migration Class
    /// </summary>
    [SkipMigrationOnUpdate]
    [NopMigration("2020/09/21 09:27:23:6455460", "NopFeatures.ShipRocket base schema")]    
    public class SchemaMigration : AutoReversingMigration
    {
        protected IMigrationManager _migrationManager;

        public SchemaMigration(IMigrationManager migrationManager)
        {
            _migrationManager = migrationManager;
        }

        public override void Up()
        {
            _migrationManager.BuildTable<NopShiprocketOrder>(Create);
        }
    }
}