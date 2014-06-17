namespace EdeaDay.GestaoDeIdeias.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdeiaComentarios : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comentarios", "ComentarioId", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Comentarios", new[] { "ComentarioID" });
            AddPrimaryKey("dbo.Comentarios", "ComentarioId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Comentarios", new[] { "ComentarioId" });
            AddPrimaryKey("dbo.Comentarios", "ComentarioID");
            AlterColumn("dbo.Comentarios", "ComentarioID", c => c.Int(nullable: false, identity: true));
        }
    }
}
