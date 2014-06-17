namespace EdeaDay.GestaoDeIdeias.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Comentarios : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comentarios",
                c => new
                    {
                        ComentarioID = c.Int(nullable: false, identity: true),
                        Texto = c.String(),
                        Data = c.DateTime(nullable: false),
                        Usuario_UsuarioID = c.String(maxLength: 128),
                        Ideia_IdeiaID = c.Int(),
                    })
                .PrimaryKey(t => t.ComentarioID)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_UsuarioID)
                .ForeignKey("dbo.Ideias", t => t.Ideia_IdeiaID)
                .Index(t => t.Usuario_UsuarioID)
                .Index(t => t.Ideia_IdeiaID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Comentarios", new[] { "Ideia_IdeiaID" });
            DropIndex("dbo.Comentarios", new[] { "Usuario_UsuarioID" });
            DropForeignKey("dbo.Comentarios", "Ideia_IdeiaID", "dbo.Ideias");
            DropForeignKey("dbo.Comentarios", "Usuario_UsuarioID", "dbo.Usuarios");
            DropTable("dbo.Comentarios");
        }
    }
}
