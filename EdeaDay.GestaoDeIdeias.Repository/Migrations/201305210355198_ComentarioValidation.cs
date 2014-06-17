namespace EdeaDay.GestaoDeIdeias.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComentarioValidation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comentarios", "Texto", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comentarios", "Texto", c => c.String());
        }
    }
}
